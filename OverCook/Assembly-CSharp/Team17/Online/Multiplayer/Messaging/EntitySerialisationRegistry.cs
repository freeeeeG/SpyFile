using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000917 RID: 2327
	public class EntitySerialisationRegistry
	{
		// Token: 0x06002D85 RID: 11653 RVA: 0x000D7E2E File Offset: 0x000D622E
		public static EntitySerialisationEntry GetEntry(uint uEntityID)
		{
			if (EntitySerialisationRegistry.m_Entities.ContainsKey(uEntityID))
			{
				return EntitySerialisationRegistry.m_Entities[uEntityID];
			}
			return null;
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000D7E4D File Offset: 0x000D624D
		public static EntitySerialisationEntry GetEntry(GameObject gameObject)
		{
			if (EntitySerialisationRegistry.m_EntitiesByGameObject.ContainsKey(gameObject))
			{
				return EntitySerialisationRegistry.m_EntitiesByGameObject[gameObject];
			}
			return null;
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000D7E6C File Offset: 0x000D626C
		public static uint GetId(GameObject gameObject)
		{
			if (EntitySerialisationRegistry.m_EntitiesByGameObject.ContainsKey(gameObject))
			{
				return EntitySerialisationRegistry.m_EntitiesByGameObject[gameObject].m_Header.m_uEntityID;
			}
			return 0U;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000D7E95 File Offset: 0x000D6295
		public void Clear()
		{
			EntitySerialisationRegistry.m_Entities.Clear();
			EntitySerialisationRegistry.m_EntitiesByGameObject.Clear();
			EntitySerialisationRegistry.m_EntitiesList.Clear();
			EntitySerialisationRegistry.m_ServerSerialisedTypes.Clear();
			EntitySerialisationRegistry.m_ClientSerialisedTypes.Clear();
			EntitySerialisationRegistry.m_ServerFreeEntityIDList.Clear();
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000D7ED3 File Offset: 0x000D62D3
		private void SetupSynchronisedType(ref Dictionary<Type, InstancesPerGameObject> instancesPerObjectForType, ref FastList<Type> validTypes, Type syncType, InstancesPerGameObject option)
		{
			if (syncType == null)
			{
				return;
			}
			if (instancesPerObjectForType.ContainsKey(syncType))
			{
				instancesPerObjectForType[syncType] = option;
			}
			else
			{
				instancesPerObjectForType.Add(syncType, option);
			}
			validTypes.Add(syncType);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000D7F0A File Offset: 0x000D630A
		public void AddSynchronisedType(Type gameType, SynchroniserConfig config)
		{
			this.AddSynchronisedType(gameType, new SynchroniserConfig[]
			{
				config
			});
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000D7F20 File Offset: 0x000D6320
		public void AddSynchronisedType(Type gameType, SynchroniserConfig[] configs)
		{
			FastList<Type> value = new FastList<Type>(configs.Length);
			FastList<Type> value2 = new FastList<Type>(configs.Length);
			bool flag = ConnectionStatus.IsHost() || false == ConnectionStatus.IsInSession();
			foreach (SynchroniserConfig synchroniserConfig in configs)
			{
				if (flag)
				{
					this.SetupSynchronisedType(ref EntitySerialisationRegistry.m_InstancesPerObjectForServerSynchronisedTypes, ref value, synchroniserConfig.m_ServerSynchroniserType, synchroniserConfig.m_InstancesAllowed);
				}
				this.SetupSynchronisedType(ref EntitySerialisationRegistry.m_InstancesPerObjectForClientSynchronisedTypes, ref value2, synchroniserConfig.m_ClientSynchroniserType, synchroniserConfig.m_InstancesAllowed);
			}
			if (flag)
			{
				EntitySerialisationRegistry.m_ServerSerialisedTypes.Add(new KeyValuePair<Type, FastList<Type>>(gameType, value));
			}
			EntitySerialisationRegistry.m_ClientSerialisedTypes.Add(new KeyValuePair<Type, FastList<Type>>(gameType, value2));
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000D7FD4 File Offset: 0x000D63D4
		public IEnumerator SetupSynchronisation(MultiplayerController multiplayerpController)
		{
			EntitySerialisationRegistry.m_ServerFreeEntityIDList.Clear();
			for (ushort num = 1; num < 1023; num += 1)
			{
				EntitySerialisationRegistry.m_ServerFreeEntityIDList.Enqueue(num);
			}
			IEnumerator linkRoutine = EntitySerialisationRegistry.LinkAllEntitiesToSynchronisationScripts();
			while (linkRoutine.MoveNext())
			{
				yield return null;
			}
			int iCount = EntitySerialisationRegistry.m_EntitiesList.Count;
			for (int i = 0; i < iCount; i++)
			{
				EntitySerialisationEntry entitySerialisationEntry = EntitySerialisationRegistry.m_EntitiesList._items[i];
				if (entitySerialisationEntry != null)
				{
				}
			}
			yield break;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000D7FF0 File Offset: 0x000D63F0
		public void StartSynchronisation()
		{
			int count = EntitySerialisationRegistry.m_EntitiesList.Count;
			for (int i = 0; i < count; i++)
			{
				EntitySerialisationRegistry.StartSynchronisingEntry(EntitySerialisationRegistry.m_EntitiesList._items[i]);
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000D802C File Offset: 0x000D642C
		public void StopSynchronisation()
		{
			int count = EntitySerialisationRegistry.m_EntitiesList.Count;
			for (int i = 0; i < count; i++)
			{
				EntitySerialisationRegistry.StopSynchronisingEntry(EntitySerialisationRegistry.m_EntitiesList._items[i]);
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000D8068 File Offset: 0x000D6468
		private static IEnumerator LinkAllEntitiesToSynchronisationScripts()
		{
			EntitySerialisationRegistry.s_bLinkingEntities = true;
			GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			float yieldTime = Time.realtimeSinceStartup + 0.1f;
			for (int i = 0; i < rootObjects.Length; i++)
			{
				if (!(rootObjects[i] != null) || !rootObjects[i].CompareTag("NetworkStatic"))
				{
					for (int iServerSync = 0; iServerSync < EntitySerialisationRegistry.m_ServerSerialisedTypes.Count; iServerSync++)
					{
						KeyValuePair<Type, FastList<Type>> serverSync = EntitySerialisationRegistry.m_ServerSerialisedTypes._items[iServerSync];
						if (rootObjects[i] != null)
						{
							Component[] componentsInChildren = rootObjects[i].GetComponentsInChildren(serverSync.Key, true);
							for (int j = 0; j < componentsInChildren.Length; j++)
							{
								GameObject gameObject = componentsInChildren[j].gameObject;
								EntitySerialisationEntry entitySerialisationEntry = EntitySerialisationRegistry.GetEntry(gameObject);
								for (int k = 0; k < serverSync.Value.Count; k++)
								{
									Type type = serverSync.Value._items[k];
									if (entitySerialisationEntry != null)
									{
										EntitySerialisationRegistry.TryAddServerSynchronisationComponent(entitySerialisationEntry, type, componentsInChildren[j]);
									}
									else
									{
										entitySerialisationEntry = EntitySerialisationRegistry.AddEntry(gameObject);
										EntitySerialisationRegistry.TryAddServerSynchronisationComponent(entitySerialisationEntry, type, componentsInChildren[j]);
									}
								}
							}
						}
						if (Time.realtimeSinceStartup >= yieldTime)
						{
							yield return null;
							yieldTime = Time.realtimeSinceStartup + 0.1f;
						}
					}
					FastList<Type> types = null;
					for (int iClientSync = 0; iClientSync < EntitySerialisationRegistry.m_ClientSerialisedTypes.Count; iClientSync++)
					{
						KeyValuePair<Type, FastList<Type>> clientSync = EntitySerialisationRegistry.m_ClientSerialisedTypes._items[iClientSync];
						if (rootObjects[i] != null)
						{
							Component[] componentsInChildren2 = rootObjects[i].GetComponentsInChildren(clientSync.Key, true);
							for (int l = 0; l < componentsInChildren2.Length; l++)
							{
								GameObject gameObject2 = componentsInChildren2[l].gameObject;
								EntitySerialisationEntry entitySerialisationEntry2 = EntitySerialisationRegistry.GetEntry(gameObject2);
								types = clientSync.Value;
								for (int m = 0; m < types.Count; m++)
								{
									Type type2 = types._items[m];
									if (entitySerialisationEntry2 != null)
									{
										EntitySerialisationRegistry.TryAddClientSynchronisationComponent(entitySerialisationEntry2, type2, componentsInChildren2[l]);
									}
									else
									{
										entitySerialisationEntry2 = EntitySerialisationRegistry.AddEntry(gameObject2);
										EntitySerialisationRegistry.TryAddClientSynchronisationComponent(entitySerialisationEntry2, type2, componentsInChildren2[l]);
									}
								}
							}
						}
						if (Time.realtimeSinceStartup >= yieldTime)
						{
							yield return null;
							yieldTime = Time.realtimeSinceStartup + 0.1f;
						}
					}
				}
			}
			EntitySerialisationRegistry.s_bLinkingEntities = false;
			yield break;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000D807C File Offset: 0x000D647C
		private static void StartSynchronisingEntry(EntitySerialisationEntry entry)
		{
			FastList<ServerSynchroniser> serverSynchronisedComponents = entry.m_ServerSynchronisedComponents;
			int count = serverSynchronisedComponents.Count;
			for (int i = 0; i < count; i++)
			{
				Synchroniser synchroniser = serverSynchronisedComponents._items[i];
				synchroniser.StartSynchronising(synchroniser.GetSynchronisedComponent());
			}
			FastList<ClientSynchroniser> clientSynchronisedComponents = entry.m_ClientSynchronisedComponents;
			int count2 = clientSynchronisedComponents.Count;
			for (int j = 0; j < count2; j++)
			{
				Synchroniser synchroniser2 = clientSynchronisedComponents._items[j];
				synchroniser2.StartSynchronising(synchroniser2.GetSynchronisedComponent());
			}
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000D8100 File Offset: 0x000D6500
		public static void ServerRegisterObject(GameObject gameObject)
		{
			EntitySerialisationRegistry.RegisterObject(gameObject, EntitySerialisationRegistry.ServerGenerateEntityID());
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000D8110 File Offset: 0x000D6510
		public static void RegisterObject(GameObject gameObject, uint uEntityID)
		{
			if (EntitySerialisationRegistry.GetEntry(gameObject) != null)
			{
				return;
			}
			if (EntitySerialisationRegistry.GetEntry(uEntityID) != null)
			{
				return;
			}
			EntitySerialisationEntry entitySerialisationEntry = EntitySerialisationRegistry.AddEntry(gameObject, uEntityID);
			for (int i = 0; i < EntitySerialisationRegistry.m_ServerSerialisedTypes.Count; i++)
			{
				KeyValuePair<Type, FastList<Type>> keyValuePair = EntitySerialisationRegistry.m_ServerSerialisedTypes._items[i];
				Component[] componentsInChildren = gameObject.GetComponentsInChildren(keyValuePair.Key, true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					for (int k = 0; k < keyValuePair.Value.Count; k++)
					{
						Type type = keyValuePair.Value._items[k];
						EntitySerialisationRegistry.TryAddServerSynchronisationComponent(entitySerialisationEntry, type, componentsInChildren[j]);
					}
				}
			}
			for (int l = 0; l < EntitySerialisationRegistry.m_ClientSerialisedTypes.Count; l++)
			{
				KeyValuePair<Type, FastList<Type>> keyValuePair2 = EntitySerialisationRegistry.m_ClientSerialisedTypes._items[l];
				Component[] componentsInChildren2 = gameObject.GetComponentsInChildren(keyValuePair2.Key, true);
				for (int m = 0; m < componentsInChildren2.Length; m++)
				{
					for (int n = 0; n < keyValuePair2.Value.Count; n++)
					{
						Type type2 = keyValuePair2.Value._items[n];
						EntitySerialisationRegistry.TryAddClientSynchronisationComponent(entitySerialisationEntry, type2, componentsInChildren2[m]);
					}
				}
			}
			if ((entitySerialisationEntry.m_ServerSynchronisedComponents == null || entitySerialisationEntry.m_ServerSynchronisedComponents.Count == 0) && (entitySerialisationEntry.m_ClientSynchronisedComponents == null || entitySerialisationEntry.m_ClientSynchronisedComponents.Count == 0))
			{
				return;
			}
			EntitySerialisationRegistry.StartSynchronisingEntry(entitySerialisationEntry);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000D82B0 File Offset: 0x000D66B0
		public static void UnregisterObject(uint uEntityID)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(uEntityID);
			EntitySerialisationRegistry.StopSynchronisingEntry(entry);
			EntitySerialisationRegistry.RemoveEntry(entry);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000D82D0 File Offset: 0x000D66D0
		public static void UnregisterObject(GameObject gameObject)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
			EntitySerialisationRegistry.StopSynchronisingEntry(entry);
			EntitySerialisationRegistry.RemoveEntry(entry);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000D82F0 File Offset: 0x000D66F0
		public static void StopSynchronisingEntry(EntitySerialisationEntry entry)
		{
			if (entry != null)
			{
				FastList<ServerSynchroniser> serverSynchronisedComponents = entry.m_ServerSynchronisedComponents;
				int count = serverSynchronisedComponents.Count;
				for (int i = 0; i < count; i++)
				{
					Synchroniser synchroniser = serverSynchronisedComponents._items[i];
					synchroniser.StopSynchronising();
				}
				FastList<ClientSynchroniser> clientSynchronisedComponents = entry.m_ClientSynchronisedComponents;
				int count2 = clientSynchronisedComponents.Count;
				for (int j = 0; j < count2; j++)
				{
					Synchroniser synchroniser2 = clientSynchronisedComponents._items[j];
					synchroniser2.StopSynchronising();
				}
			}
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000D836D File Offset: 0x000D676D
		private static EntitySerialisationEntry AddEntry(GameObject gameObject)
		{
			return EntitySerialisationRegistry.AddEntry(gameObject, EntitySerialisationRegistry.ServerGenerateEntityID());
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000D837C File Offset: 0x000D677C
		private static EntitySerialisationEntry AddEntry(GameObject gameObject, uint entityID)
		{
			EntitySerialisationEntry entitySerialisationEntry = new EntitySerialisationEntry();
			entitySerialisationEntry.m_Header = new EntityMessageHeader();
			entitySerialisationEntry.m_Header.m_uEntityID = entityID;
			entitySerialisationEntry.m_GameObject = gameObject;
			EntitySerialisationRegistry.m_Entities.Add(entityID, entitySerialisationEntry);
			EntitySerialisationRegistry.m_EntitiesByGameObject.Add(entitySerialisationEntry.m_GameObject, entitySerialisationEntry);
			EntitySerialisationRegistry.m_EntitiesList.Add(entitySerialisationEntry);
			if (EntitySerialisationRegistry.OnEntryAdded != null)
			{
				EntitySerialisationRegistry.OnEntryAdded(entitySerialisationEntry);
			}
			return entitySerialisationEntry;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000D83EC File Offset: 0x000D67EC
		private static void RemoveEntry(EntitySerialisationEntry entry)
		{
			if (entry != null)
			{
				EntitySerialisationRegistry.m_Entities.Remove(entry.m_Header.m_uEntityID);
				EntitySerialisationRegistry.m_EntitiesByGameObject.Remove(entry.m_GameObject);
				EntitySerialisationRegistry.m_EntitiesList.Remove(entry);
				EntitySerialisationRegistry.m_ServerFreeEntityIDList.Enqueue((ushort)entry.m_Header.m_uEntityID);
				if (EntitySerialisationRegistry.OnEntryRemoved != null)
				{
					EntitySerialisationRegistry.OnEntryRemoved(entry);
				}
			}
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000D845D File Offset: 0x000D685D
		private static void TryAddServerSynchronisationComponent(EntitySerialisationEntry entry, Type type, Component baseComponent)
		{
			if (EntitySerialisationRegistry.GetInstancesPerGameObjectForServerSynchronisedTypes(type) == InstancesPerGameObject.Multiple)
			{
				EntitySerialisationRegistry.AddServerSynchronisationComponent(entry, type, baseComponent);
			}
			else if (null == entry.m_GameObject.GetComponent(type))
			{
				EntitySerialisationRegistry.AddServerSynchronisationComponent(entry, type, baseComponent);
			}
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000D849C File Offset: 0x000D689C
		private static void AddServerSynchronisationComponent(EntitySerialisationEntry entry, Type type, Component baseComponent)
		{
			Component component = entry.m_GameObject.AddComponent(type);
			ServerSynchroniser serverSynchroniser = component as ServerSynchroniser;
			if (serverSynchroniser != null)
			{
				entry.m_ServerSynchronisedComponents.Add(serverSynchroniser);
				serverSynchroniser.SetSynchronisedComponent(baseComponent);
				serverSynchroniser.Initialise(entry.m_Header.m_uEntityID, (uint)(entry.m_ServerSynchronisedComponents.Count - 1));
			}
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000D84F4 File Offset: 0x000D68F4
		private static InstancesPerGameObject GetInstancesPerGameObjectForServerSynchronisedTypes(Type type)
		{
			if (EntitySerialisationRegistry.m_InstancesPerObjectForServerSynchronisedTypes.ContainsKey(type))
			{
				return EntitySerialisationRegistry.m_InstancesPerObjectForServerSynchronisedTypes[type];
			}
			return EntitySerialisationRegistry.m_InstancesPerObjectDefault;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000D8517 File Offset: 0x000D6917
		private static void TryAddClientSynchronisationComponent(EntitySerialisationEntry entry, Type type, Component baseComponent)
		{
			if (EntitySerialisationRegistry.GetInstancesPerGameObjectForClientSynchronisedTypes(type) == InstancesPerGameObject.Multiple)
			{
				EntitySerialisationRegistry.AddClientSynchronisationComponent(entry, type, baseComponent);
			}
			else if (null == entry.m_GameObject.GetComponent(type))
			{
				EntitySerialisationRegistry.AddClientSynchronisationComponent(entry, type, baseComponent);
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000D8558 File Offset: 0x000D6958
		private static void AddClientSynchronisationComponent(EntitySerialisationEntry entry, Type type, Component baseComponent)
		{
			Component component = entry.m_GameObject.AddComponent(type);
			ClientSynchroniser clientSynchroniser = component as ClientSynchroniser;
			if (clientSynchroniser != null)
			{
				entry.m_ClientSynchronisedComponents.Add(clientSynchroniser);
				clientSynchroniser.SetSynchronisedComponent(baseComponent);
				if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
				{
				}
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000D85A6 File Offset: 0x000D69A6
		private static InstancesPerGameObject GetInstancesPerGameObjectForClientSynchronisedTypes(Type type)
		{
			if (EntitySerialisationRegistry.m_InstancesPerObjectForClientSynchronisedTypes.ContainsKey(type))
			{
				return EntitySerialisationRegistry.m_InstancesPerObjectForClientSynchronisedTypes[type];
			}
			return EntitySerialisationRegistry.m_InstancesPerObjectDefault;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000D85C9 File Offset: 0x000D69C9
		private static uint ServerGenerateEntityID()
		{
			return (uint)EntitySerialisationRegistry.m_ServerFreeEntityIDList.Dequeue();
		}

		// Token: 0x04002492 RID: 9362
		public const uint kInvalidEntityID = 0U;

		// Token: 0x04002493 RID: 9363
		public const uint kMaxEntityID = 1023U;

		// Token: 0x04002494 RID: 9364
		public const string c_tagNetworkStatic = "NetworkStatic";

		// Token: 0x04002495 RID: 9365
		public static bool HasUrgentOutgoingUpdates = false;

		// Token: 0x04002496 RID: 9366
		public static Dictionary<uint, EntitySerialisationEntry> m_Entities = new Dictionary<uint, EntitySerialisationEntry>();

		// Token: 0x04002497 RID: 9367
		public static Dictionary<GameObject, EntitySerialisationEntry> m_EntitiesByGameObject = new Dictionary<GameObject, EntitySerialisationEntry>();

		// Token: 0x04002498 RID: 9368
		public static FastList<EntitySerialisationEntry> m_EntitiesList = new FastList<EntitySerialisationEntry>();

		// Token: 0x04002499 RID: 9369
		private static FastList<KeyValuePair<Type, FastList<Type>>> m_ServerSerialisedTypes = new FastList<KeyValuePair<Type, FastList<Type>>>();

		// Token: 0x0400249A RID: 9370
		private static FastList<KeyValuePair<Type, FastList<Type>>> m_ClientSerialisedTypes = new FastList<KeyValuePair<Type, FastList<Type>>>();

		// Token: 0x0400249B RID: 9371
		private static InstancesPerGameObject m_InstancesPerObjectDefault = InstancesPerGameObject.Single;

		// Token: 0x0400249C RID: 9372
		private static Dictionary<Type, InstancesPerGameObject> m_InstancesPerObjectForServerSynchronisedTypes = new Dictionary<Type, InstancesPerGameObject>();

		// Token: 0x0400249D RID: 9373
		private static Dictionary<Type, InstancesPerGameObject> m_InstancesPerObjectForClientSynchronisedTypes = new Dictionary<Type, InstancesPerGameObject>();

		// Token: 0x0400249E RID: 9374
		public static Queue<ushort> m_ServerFreeEntityIDList = new Queue<ushort>(1022);

		// Token: 0x0400249F RID: 9375
		public static GenericVoid<EntitySerialisationEntry> OnEntryAdded = null;

		// Token: 0x040024A0 RID: 9376
		public static GenericVoid<EntitySerialisationEntry> OnEntryRemoved = null;

		// Token: 0x040024A1 RID: 9377
		private static bool s_bLinkingEntities = false;
	}
}
