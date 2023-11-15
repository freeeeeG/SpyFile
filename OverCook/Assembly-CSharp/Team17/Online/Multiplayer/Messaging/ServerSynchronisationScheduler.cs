using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000921 RID: 2337
	public class ServerSynchronisationScheduler
	{
		// Token: 0x06002DE1 RID: 11745 RVA: 0x000DA8AC File Offset: 0x000D8CAC
		public void Initialise()
		{
			this.m_EntitiesList = new FastList<EntitySerialisationEntry>();
			this.m_FastEntitiesList = new FastList<EntitySerialisationEntry>();
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			this.m_SessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000DA8E1 File Offset: 0x000D8CE1
		public void AddToFastList(EntitySerialisationEntry entry)
		{
			this.m_FastEntitiesList.Add(entry);
			if (this.m_EntitiesList.Contains(entry))
			{
				this.m_EntitiesList.Remove(entry);
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000DA910 File Offset: 0x000D8D10
		public void StartSynchronising()
		{
			EntitySerialisationRegistry.OnEntryAdded = (GenericVoid<EntitySerialisationEntry>)Delegate.Combine(EntitySerialisationRegistry.OnEntryAdded, new GenericVoid<EntitySerialisationEntry>(this.OnEntryAdded));
			EntitySerialisationRegistry.OnEntryRemoved = (GenericVoid<EntitySerialisationEntry>)Delegate.Combine(EntitySerialisationRegistry.OnEntryRemoved, new GenericVoid<EntitySerialisationEntry>(this.OnEntryRemoved));
			DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnNetworkError));
			DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
			DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnNetworkError));
			DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Combine(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
			this.BuildEntityLists();
			this.m_bStarted = true;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000DA9EC File Offset: 0x000D8DEC
		public void StopSynchronising()
		{
			this.m_bStarted = false;
			this.m_EntitiesList = null;
			this.m_FastEntitiesList = null;
			DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnNetworkError));
			DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
			DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnNetworkError));
			DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Remove(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
			EntitySerialisationRegistry.OnEntryAdded = (GenericVoid<EntitySerialisationEntry>)Delegate.Remove(EntitySerialisationRegistry.OnEntryAdded, new GenericVoid<EntitySerialisationEntry>(this.OnEntryAdded));
			EntitySerialisationRegistry.OnEntryRemoved = (GenericVoid<EntitySerialisationEntry>)Delegate.Remove(EntitySerialisationRegistry.OnEntryRemoved, new GenericVoid<EntitySerialisationEntry>(this.OnEntryRemoved));
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000DAACE File Offset: 0x000D8ECE
		private void OnEntryAdded(EntitySerialisationEntry entry)
		{
			if (!this.m_EntitiesList.Contains(entry) && !this.m_FastEntitiesList.Contains(entry))
			{
				this.m_EntitiesList.Add(entry);
			}
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000DAAFE File Offset: 0x000D8EFE
		private void OnEntryRemoved(EntitySerialisationEntry entry)
		{
			this.m_EntitiesList.Remove(entry);
			this.m_FastEntitiesList.Remove(entry);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000DAB1A File Offset: 0x000D8F1A
		private void OnNetworkError()
		{
			this.StopSynchronising();
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000DAB22 File Offset: 0x000D8F22
		private void OnConnectionModeError(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
		{
			this.StopSynchronising();
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000DAB2A File Offset: 0x000D8F2A
		private void OnLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
		{
			this.StopSynchronising();
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000DAB34 File Offset: 0x000D8F34
		public void Update()
		{
			if (!this.m_bStarted)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			this.m_fNextUpdate += deltaTime;
			this.m_fNextFastUpdate += deltaTime;
			this.SynchroniseList(this.m_EntitiesList, 0.1f, ref this.m_fNextUpdate);
			if (DebugManager.Instance.GetOption("Fast NetworkChefs"))
			{
				this.SynchroniseList(this.m_FastEntitiesList, 0.033333335f, ref this.m_fNextFastUpdate);
			}
			else
			{
				this.SynchroniseList(this.m_FastEntitiesList, 0.1f, ref this.m_fNextFastUpdate);
			}
			EntitySerialisationRegistry.HasUrgentOutgoingUpdates = false;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000DABD4 File Offset: 0x000D8FD4
		private void SynchroniseList(FastList<EntitySerialisationEntry> entities, float fFrameDelay, ref float fNextUpdate)
		{
			if (entities != null)
			{
				if (fNextUpdate >= fFrameDelay)
				{
					int count = entities.Count;
					for (int i = 0; i < count; i++)
					{
						EntitySerialisationEntry entitySerialisationEntry = entities._items[i];
						this.SynchroniseEntity(entitySerialisationEntry);
						entitySerialisationEntry.SetRequiresUrgentUpdate(false);
					}
					fNextUpdate -= fFrameDelay;
				}
				else if (EntitySerialisationRegistry.HasUrgentOutgoingUpdates)
				{
					int count2 = entities.Count;
					for (int j = 0; j < count2; j++)
					{
						EntitySerialisationEntry entitySerialisationEntry2 = entities._items[j];
						if (entitySerialisationEntry2.HasUrgentUpdate())
						{
							this.SynchroniseEntity(entitySerialisationEntry2);
							entitySerialisationEntry2.SetRequiresUrgentUpdate(false);
						}
					}
				}
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000DAC78 File Offset: 0x000D9078
		private void BuildEntityLists()
		{
			FastList<EntitySerialisationEntry> entitiesList = EntitySerialisationRegistry.m_EntitiesList;
			for (int i = 0; i < entitiesList.Count; i++)
			{
				EntitySerialisationEntry item = entitiesList._items[i];
				if (!this.m_EntitiesList.Contains(item) && !this.m_FastEntitiesList.Contains(item))
				{
					this.m_EntitiesList.Add(item);
				}
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000DACDC File Offset: 0x000D90DC
		private void SynchroniseEntity(EntitySerialisationEntry entry)
		{
			bool flag = false;
			int num = 0;
			this.m_GlobalPayloadCache.Clear();
			for (int i = 0; i < entry.m_ServerSynchronisedComponents.Count; i++)
			{
				ServerSynchroniser serverSynchroniser = entry.m_ServerSynchronisedComponents._items[i];
				flag |= serverSynchroniser.HasTargetedServerUpdates();
				Serialisable serverUpdate = serverSynchroniser.GetServerUpdate();
				if (serverUpdate != null)
				{
					if (this.m_Tracker != null)
					{
						this.m_Tracker.TrackSentEntityUpdate(serverSynchroniser.GetEntityType());
					}
					num++;
				}
				this.m_GlobalPayloadCache.Add(serverUpdate);
			}
			if (!flag && num > 0)
			{
				ServerMessenger.EntitySynchronisation(entry.m_Header, this.m_GlobalPayloadCache);
			}
			if (flag)
			{
				this.SynchroniseForRecipient(entry, this.m_GlobalPayloadCache, null);
				if (this.m_SessionCoordinator != null)
				{
					IOnlineMultiplayerSessionUserId[] array = this.m_SessionCoordinator.Members();
					if (array != null)
					{
						foreach (IOnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId in array)
						{
							if (onlineMultiplayerSessionUserId != null && !onlineMultiplayerSessionUserId.IsHost)
							{
								this.SynchroniseForRecipient(entry, this.m_GlobalPayloadCache, onlineMultiplayerSessionUserId);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000DADFC File Offset: 0x000D91FC
		private void SynchroniseForRecipient(EntitySerialisationEntry entry, FastList<Serialisable> globalPayloads, IOnlineMultiplayerSessionUserId currentMember)
		{
			this.m_TargetPayloadCache.Clear();
			int num = 0;
			for (int i = 0; i < entry.m_ServerSynchronisedComponents.Count; i++)
			{
				ServerSynchroniser serverSynchroniser = entry.m_ServerSynchronisedComponents._items[i];
				Serialisable serverUpdateForRecipient = serverSynchroniser.GetServerUpdateForRecipient(currentMember);
				if (serverUpdateForRecipient != null)
				{
					if (this.m_Tracker != null)
					{
						this.m_Tracker.TrackSentEntityUpdate(serverSynchroniser.GetEntityType());
					}
					num++;
				}
				if (serverUpdateForRecipient != null)
				{
					this.m_TargetPayloadCache.Add(serverUpdateForRecipient);
				}
				else
				{
					this.m_TargetPayloadCache.Add(globalPayloads._items[i]);
				}
			}
			if (num > 0)
			{
				ServerMessenger.EntitySynchronisation(entry.m_Header, currentMember, this.m_TargetPayloadCache);
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000DAEB2 File Offset: 0x000D92B2
		public void SetTracker(NetworkMessageTracker tracker)
		{
			this.m_Tracker = tracker;
		}

		// Token: 0x040024FF RID: 9471
		private const float kFrameRate = 10f;

		// Token: 0x04002500 RID: 9472
		private const float kFrameDelay = 0.1f;

		// Token: 0x04002501 RID: 9473
		private const float kFastFrameRate = 30f;

		// Token: 0x04002502 RID: 9474
		private const float kFastFrameDelay = 0.033333335f;

		// Token: 0x04002503 RID: 9475
		private float m_fNextUpdate;

		// Token: 0x04002504 RID: 9476
		private float m_fNextFastUpdate;

		// Token: 0x04002505 RID: 9477
		private FastList<EntitySerialisationEntry> m_EntitiesList;

		// Token: 0x04002506 RID: 9478
		private FastList<EntitySerialisationEntry> m_FastEntitiesList;

		// Token: 0x04002507 RID: 9479
		private IOnlineMultiplayerSessionCoordinator m_SessionCoordinator;

		// Token: 0x04002508 RID: 9480
		private NetworkMessageTracker m_Tracker;

		// Token: 0x04002509 RID: 9481
		private bool m_bStarted;

		// Token: 0x0400250A RID: 9482
		private FastList<Serialisable> m_GlobalPayloadCache = new FastList<Serialisable>(16);

		// Token: 0x0400250B RID: 9483
		private FastList<Serialisable> m_TargetPayloadCache = new FastList<Serialisable>(16);
	}
}
