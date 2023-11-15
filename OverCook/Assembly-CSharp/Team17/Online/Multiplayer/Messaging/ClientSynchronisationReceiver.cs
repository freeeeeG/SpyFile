using System;
using System.Collections.Generic;
using GameModes;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x0200090E RID: 2318
	public class ClientSynchronisationReceiver
	{
		// Token: 0x06002D6D RID: 11629 RVA: 0x000D7528 File Offset: 0x000D5928
		public void OnEntitySynchronisationMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message, uint uSequence)
		{
			EntitySynchronisationMessage entitySynchronisationMessage = (EntitySynchronisationMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(entitySynchronisationMessage.m_Header.m_uEntityID);
			if (entry != null)
			{
				float num = ClientTime.Time();
				for (int i = 0; i < entry.m_ClientSynchronisedComponents.Count; i++)
				{
					if (entitySynchronisationMessage.m_Payloads._items[i] != null)
					{
						bool flag = false;
						ClientSynchroniser clientSynchroniser = entry.m_ClientSynchronisedComponents._items[i];
						if (clientSynchroniser.IsValidServerUpdateSequenceNumber(uSequence))
						{
							flag = true;
						}
						else if (clientSynchroniser.IsValidLastUpdateTimeStamp(num, 15f))
						{
							flag = true;
						}
						if (flag)
						{
							clientSynchroniser.SetLastServerUpdateSequenceNumber(uSequence);
							clientSynchroniser.SetLastUpdateTimeStamp(num);
							clientSynchroniser.ApplyServerUpdate(entitySynchronisationMessage.m_Payloads._items[i]);
							if (this.m_Tracker != null)
							{
								this.m_Tracker.TrackReceivedEntityUpdate(clientSynchroniser.GetEntityType());
							}
						}
					}
				}
			}
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000D7610 File Offset: 0x000D5A10
		public void OnEntityEventMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			EntityEventMessage entityEventMessage = (EntityEventMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(entityEventMessage.m_Header.m_uEntityID);
			if (entry != null)
			{
				ClientSynchroniser clientSynchroniser = entry.m_ClientSynchronisedComponents._items[(int)entityEventMessage.m_ComponentId];
				clientSynchroniser.ApplyServerEvent(entityEventMessage.m_Payload);
				if (this.m_Tracker != null)
				{
					this.m_Tracker.TrackReceivedEntityEvent(clientSynchroniser.GetEntityType());
				}
			}
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000D7678 File Offset: 0x000D5A78
		public void OnSpawnEntityMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			SpawnEntityMessage spawnEntityMessage = (SpawnEntityMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(spawnEntityMessage.m_SpawnerHeader.m_uEntityID);
			if (entry != null && null != entry.m_GameObject)
			{
				INetworkEntitySpawner networkEntitySpawner = entry.m_GameObject.RequireInterface<INetworkEntitySpawner>();
				if (EntitySerialisationRegistry.GetEntry(spawnEntityMessage.m_DesiredHeader.m_uEntityID) == null)
				{
					List<VoidGeneric<GameObject>> list = new List<VoidGeneric<GameObject>>();
					GameObject gameObject = networkEntitySpawner.SpawnEntity(spawnEntityMessage.m_SpawnableID, spawnEntityMessage.m_Position, spawnEntityMessage.m_Rotation, ref list);
					EntitySerialisationRegistry.RegisterObject(gameObject, spawnEntityMessage.m_DesiredHeader.m_uEntityID);
					ComponentCacheRegistry.UpdateObject(gameObject);
					if (list != null && list.Count > 0)
					{
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i] != null)
							{
								list[i](gameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000D7764 File Offset: 0x000D5B64
		public void OnSpawnPhysicalAttachmentMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			SpawnPhysicalAttachmentMessage spawnPhysicalAttachmentMessage = (SpawnPhysicalAttachmentMessage)message;
			SpawnEntityMessage spawnEntityData = spawnPhysicalAttachmentMessage.m_SpawnEntityData;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(spawnEntityData.m_SpawnerHeader.m_uEntityID);
			if (entry != null && null != entry.m_GameObject)
			{
				INetworkEntitySpawner networkEntitySpawner = entry.m_GameObject.RequireInterface<INetworkEntitySpawner>();
				if (EntitySerialisationRegistry.GetEntry(spawnEntityData.m_DesiredHeader.m_uEntityID) == null)
				{
					List<VoidGeneric<GameObject>> list = new List<VoidGeneric<GameObject>>();
					GameObject gameObject = networkEntitySpawner.SpawnEntity(spawnEntityData.m_SpawnableID, spawnEntityData.m_Position, spawnEntityData.m_Rotation, ref list);
					EntitySerialisationRegistry.RegisterObject(gameObject, spawnEntityData.m_DesiredHeader.m_uEntityID);
					ComponentCacheRegistry.UpdateObject(gameObject);
					PhysicalAttachment component = gameObject.GetComponent<PhysicalAttachment>();
					if (null != component)
					{
						EntitySerialisationRegistry.RegisterObject(component.m_container.gameObject, spawnPhysicalAttachmentMessage.m_ContainerHeader.m_uEntityID);
						ComponentCacheRegistry.UpdateObject(component.m_container.gameObject);
					}
					EntitySerialisationRegistry.RegisterObject(gameObject, spawnEntityData.m_DesiredHeader.m_uEntityID);
					ComponentCacheRegistry.UpdateObject(gameObject);
					if (list != null && list.Count > 0)
					{
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i] != null)
							{
								list[i](gameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000D78B4 File Offset: 0x000D5CB4
		public void OnDestroyEntityMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			DestroyEntityMessage destroyEntityMessage = (DestroyEntityMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(destroyEntityMessage.m_Header.m_uEntityID);
			if (entry != null && entry.m_GameObject != null)
			{
				this.DestroyObject(entry.m_GameObject);
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000D78FC File Offset: 0x000D5CFC
		private static void FindSynchroniserEntitiesRecursive(GameObject go, FastList<uint> ids, ref FastList<ClientWorldObjectSynchroniser> list)
		{
			if (go == null)
			{
				return;
			}
			for (int i = 0; i < go.transform.childCount; i++)
			{
				GameObject gameObject = go.transform.GetChild(i).gameObject;
				if (gameObject != null && !ids.Contains(EntitySerialisationRegistry.GetId(gameObject)))
				{
					ClientWorldObjectSynchroniser clientWorldObjectSynchroniser = gameObject.RequestComponent<ClientWorldObjectSynchroniser>();
					if (clientWorldObjectSynchroniser != null)
					{
						list.Add(clientWorldObjectSynchroniser);
					}
				}
				ClientSynchronisationReceiver.FindSynchroniserEntitiesRecursive(gameObject, ids, ref list);
			}
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000D7984 File Offset: 0x000D5D84
		public void OnDestroyEntitiesMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			DestroyEntitiesMessage destroyEntitiesMessage = (DestroyEntitiesMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(destroyEntitiesMessage.m_rootId);
			if (entry != null && entry.m_GameObject != null)
			{
				GameObject gameObject = entry.m_GameObject;
				ClientSynchronisationReceiver.FindSynchroniserEntitiesRecursive(gameObject, destroyEntitiesMessage.m_ids, ref this.m_entitiesToResume);
				for (int i = 0; i < this.m_entitiesToResume.Count; i++)
				{
					this.m_entitiesToResume._items[i].Pause();
					GameObject gameObject2 = this.m_entitiesToResume._items[i].gameObject;
					gameObject2.transform.SetParent(null);
					ClientMessenger.SendResumeEntitySync(gameObject2);
				}
				this.DestroyObject(gameObject);
			}
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000D7A34 File Offset: 0x000D5E34
		public void OnDestroyChefMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			DestroyChefMessage destroyChefMessage = (DestroyChefMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(destroyChefMessage.m_Chef.m_Header.m_uEntityID);
			if (entry != null && entry.m_GameObject != null)
			{
				IParentable chef = entry.m_GameObject.RequireInterface<IParentable>();
				int num = this.CanDeleteChef(chef, entry.m_GameObject);
				if (num == 0)
				{
					this.DestroyObject(entry.m_GameObject);
				}
				else
				{
					this.m_ChefsToDelete.Add(new ClientSynchronisationReceiver.ChefToDestroyInfo
					{
						chef = chef,
						chefGO = entry.m_GameObject
					});
					if (MaskUtils.HasFlag<ClientSynchronisationReceiver.ChefToDestroyInfo.BlockedBy>(num, ClientSynchronisationReceiver.ChefToDestroyInfo.BlockedBy.Interacting))
					{
						PlayerControls playerControls = entry.m_GameObject.RequestComponent<PlayerControls>();
						object obj = (!(playerControls != null)) ? null : playerControls.GetActiveControlsImpl();
						if (playerControls != null)
						{
							playerControls.enabled = false;
						}
					}
				}
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000D7B1C File Offset: 0x000D5F1C
		private void DestroyObject(GameObject entity)
		{
			ClientLimitedQuantityItem component = entity.GetComponent<ClientLimitedQuantityItem>();
			if (null != component)
			{
				component.PlayDestructionPFX();
				component.NotifyOfImpendingDestruction();
			}
			EntitySerialisationRegistry.UnregisterObject(entity);
			UnityEngine.Object.Destroy(entity);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000D7B54 File Offset: 0x000D5F54
		private int CanDeleteChef(IParentable chef, GameObject _chefGO)
		{
			int num = 0;
			if (chef != null)
			{
				IPlayerCarrier playerCarrier = _chefGO.RequireInterface<IPlayerCarrier>();
				for (int i = 0; i < 2; i++)
				{
					if (playerCarrier.InspectCarriedItem((PlayerAttachTarget)i) != null || playerCarrier.HasAttachment((PlayerAttachTarget)i))
					{
						num |= 2;
					}
				}
				PlayerControls playerControls = _chefGO.RequestComponent<PlayerControls>();
				if (playerControls != null && playerControls.GetCurrentlyInteracting() != null)
				{
					num |= 4;
				}
			}
			IClientTeleportable clientTeleportable = _chefGO.RequestInterface<IClientTeleportable>();
			if (clientTeleportable != null && clientTeleportable.IsTeleporting())
			{
				num |= 8;
			}
			return num;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000D7BEC File Offset: 0x000D5FEC
		public void Update()
		{
			for (int i = this.m_ChefsToDelete.Count - 1; i >= 0; i--)
			{
				ClientSynchronisationReceiver.ChefToDestroyInfo item = this.m_ChefsToDelete._items[i];
				if (this.CanDeleteChef(item.chef, item.chefGO) == 0)
				{
					this.m_ChefsToDelete.Remove(item);
					this.DestroyObject(item.chefGO);
				}
			}
			for (int j = this.m_entitiesToResume.Count - 1; j >= 0; j--)
			{
				ClientWorldObjectSynchroniser clientWorldObjectSynchroniser = this.m_entitiesToResume._items[j];
				if (clientWorldObjectSynchroniser != null && clientWorldObjectSynchroniser.IsReadyToResume())
				{
					clientWorldObjectSynchroniser.Resume();
					this.m_entitiesToResume.RemoveAt(j);
				}
				else if (clientWorldObjectSynchroniser == null)
				{
					this.m_entitiesToResume.RemoveAt(j);
				}
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000D7CDC File Offset: 0x000D60DC
		public void OnResumeObjectMessageReceived<T>(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message) where T : Serialisable, new()
		{
			ResumeObjectSyncMessage<T> resumeObjectSyncMessage = (ResumeObjectSyncMessage<T>)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(resumeObjectSyncMessage.EntityID);
			if (entry != null && null != entry.m_GameObject)
			{
				ClientWorldObjectSynchroniser clientWorldObjectSynchroniser = entry.m_GameObject.RequireComponent<ClientWorldObjectSynchroniser>();
				clientWorldObjectSynchroniser.OnResumeDataReceived(resumeObjectSyncMessage.Data);
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000D7D30 File Offset: 0x000D6130
		public void OnTriggerAudioMessageReceived(IOnlineMultiplayerSessionUserId _sender, Serialisable _message)
		{
			TriggerAudioMessage triggerAudioMessage = _message as TriggerAudioMessage;
			AudioManager audioManager = GameUtils.RequireManager<AudioManager>();
			audioManager.TriggerAudio(triggerAudioMessage.AudioTag, triggerAudioMessage.Layer);
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000D7D60 File Offset: 0x000D6160
		public void OnSessionConfigReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			SessionConfigSyncMessage sessionConfigSyncMessage = (SessionConfigSyncMessage)message;
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.GameModeSessionConfig = sessionConfigSyncMessage.m_config;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000D7D86 File Offset: 0x000D6186
		public void SetTracker(NetworkMessageTracker tracker)
		{
			this.m_Tracker = tracker;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000D7D8F File Offset: 0x000D618F
		public void CleanUp()
		{
			this.m_ChefsToDelete.Clear();
		}

		// Token: 0x0400247C RID: 9340
		private FastList<ClientSynchronisationReceiver.ChefToDestroyInfo> m_ChefsToDelete = new FastList<ClientSynchronisationReceiver.ChefToDestroyInfo>();

		// Token: 0x0400247D RID: 9341
		private FastList<ClientWorldObjectSynchroniser> m_entitiesToResume = new FastList<ClientWorldObjectSynchroniser>(16);

		// Token: 0x0400247E RID: 9342
		private NetworkMessageTracker m_Tracker;

		// Token: 0x0200090F RID: 2319
		private struct ChefToDestroyInfo
		{
			// Token: 0x0400247F RID: 9343
			public IParentable chef;

			// Token: 0x04002480 RID: 9344
			public GameObject chefGO;

			// Token: 0x02000910 RID: 2320
			public enum BlockedBy
			{
				// Token: 0x04002482 RID: 9346
				None,
				// Token: 0x04002483 RID: 9347
				HeldItem,
				// Token: 0x04002484 RID: 9348
				Interacting,
				// Token: 0x04002485 RID: 9349
				Teleporting
			}
		}
	}
}
