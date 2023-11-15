using System;
using System.Collections.Generic;
using GameModes;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008DB RID: 2267
internal class ServerMessenger
{
	// Token: 0x06002BED RID: 11245 RVA: 0x000CCD30 File Offset: 0x000CB130
	public static void DeferredLevelLoad()
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.DeferredLoadLevelState deferredLoadLevelState = ServerMessenger.m_deferredLoadLevelState;
			if (deferredLoadLevelState != ServerMessenger.DeferredLoadLevelState.eLoadByIndex)
			{
				if (deferredLoadLevelState == ServerMessenger.DeferredLoadLevelState.eLoadByName)
				{
					ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.LevelLoadByName, ServerMessenger.m_LevelLoadByName, true);
					ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eInvalid;
				}
			}
			else
			{
				ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.LevelLoadByIndex, ServerMessenger.m_LevelLoadByIndex, true);
				ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eInvalid;
			}
		}
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x000CCD98 File Offset: 0x000CB198
	public static void LoadLevel(uint uLevelIndex, uint players, GameState setAtLoadingBegin, GameState waitForHide = global::GameState.NotSet)
	{
		ServerUserSystem.LockEngagement();
		InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.None);
		ServerMessenger.m_LevelLoadByIndex.Initialise(setAtLoadingBegin, waitForHide, uLevelIndex, players, true);
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.LevelLoadByIndex, ServerMessenger.m_LevelLoadByIndex, true);
			ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eInvalid;
		}
		else
		{
			ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eLoadByIndex;
		}
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x000CCDEC File Offset: 0x000CB1EC
	public static void LoadLevel(string sceneName, GameState setAtLoadingBegin, bool bUseLoadingScreen, GameState waitForHide = global::GameState.NotSet)
	{
		int num = ClientUserSystem.m_Users.Count;
		ServerUserSystem.LockEngagement();
		GameSession gameSession = GameUtils.GetGameSession();
		InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.None);
		if (gameSession != null && gameSession.TypeSettings.Type == GameSession.GameType.Competitive)
		{
			num = 4;
		}
		if (null != gameSession && null != gameSession.Progress)
		{
			SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
			if (null != sceneDirectory && sceneDirectory.Scenes != null)
			{
				for (int i = 0; i < sceneDirectory.Scenes.Length; i++)
				{
					SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[i];
					SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient(num);
					if (sceneVarient != null && sceneVarient.SceneName.Equals(sceneName))
					{
						ServerMessenger.m_LevelLoadByIndex.Initialise(setAtLoadingBegin, waitForHide, (uint)i, (uint)num, bUseLoadingScreen);
						if (ServerMessenger.m_LocalServer != null)
						{
							ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.LevelLoadByIndex, ServerMessenger.m_LevelLoadByIndex, true);
							ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eInvalid;
						}
						else
						{
							ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eLoadByIndex;
						}
						return;
					}
				}
			}
		}
		ServerMessenger.m_LevelLoadByName.Initialise(setAtLoadingBegin, waitForHide, sceneName, bUseLoadingScreen);
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.LevelLoadByName, ServerMessenger.m_LevelLoadByName, true);
			ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eInvalid;
		}
		else
		{
			ServerMessenger.m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eLoadByName;
		}
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x000CCF30 File Offset: 0x000CB330
	public static bool Example(float fFloat, bool bBool)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_Example.Initialise(fFloat, bBool);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.Example, ServerMessenger.m_Example, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x000CCF5C File Offset: 0x000CB35C
	public static bool DestroyEntity(GameObject gameObject)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
			if (entry != null)
			{
				ServerMessenger.m_DestroyEntity.Initialise(entry.m_Header);
				ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.DestroyEntity, ServerMessenger.m_DestroyEntity, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x000CCFA4 File Offset: 0x000CB3A4
	private static void FindNetworkedEntitiesRecursive(GameObject root, ref FastList<uint> ids)
	{
		if (root == null)
		{
			return;
		}
		for (int i = 0; i < root.transform.childCount; i++)
		{
			GameObject gameObject = root.transform.GetChild(i).gameObject;
			if (gameObject != null)
			{
				uint id = EntitySerialisationRegistry.GetId(gameObject);
				if (id != 0U)
				{
					ids.Add(id);
				}
			}
			ServerMessenger.FindNetworkedEntitiesRecursive(gameObject, ref ids);
		}
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x000CD014 File Offset: 0x000CB414
	public static bool DestroyEntities(GameObject root)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			uint id = EntitySerialisationRegistry.GetId(root);
			ServerMessenger.m_destroyEntityIds.Clear();
			ServerMessenger.FindNetworkedEntitiesRecursive(root, ref ServerMessenger.m_destroyEntityIds);
			ServerMessenger.m_destroyEntities.Initialise(id, ServerMessenger.m_destroyEntityIds);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.DestroyEntities, ServerMessenger.m_destroyEntities, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x000CD06C File Offset: 0x000CB46C
	public static bool DestroyChef(GameObject gameObject)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
			if (entry != null)
			{
				ServerMessenger.m_DestroyChef.Initialise(entry.m_Header);
				ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.DestroyChef, ServerMessenger.m_DestroyChef, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x000CD0B8 File Offset: 0x000CB4B8
	public static bool SpawnPhysicalAttachment(INetworkEntitySpawner _spawner, int _spawnableID, EntityMessageHeader _desiredHeader, Vector3 _position, Quaternion _rotation, EntityMessageHeader _containerHeader)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_spawner.AccessGameObject());
			if (entry != null)
			{
				ServerMessenger.m_SpawnPhysicalAttachment.Initialise(entry.m_Header, _spawnableID, _desiredHeader, _position, _rotation, _containerHeader);
				ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.SpawnPhysicalAttachment, ServerMessenger.m_SpawnPhysicalAttachment, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x000CD110 File Offset: 0x000CB510
	public static bool SpawnEntity(INetworkEntitySpawner _spawner, int _spawnableID, EntityMessageHeader _desiredHeader, Vector3 _position, Quaternion _rotation)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_spawner.AccessGameObject());
			if (entry != null)
			{
				ServerMessenger.m_SpawnEntity.Initialise(entry.m_Header, _spawnableID, _desiredHeader, _position, _rotation);
				ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.SpawnEntity, ServerMessenger.m_SpawnEntity, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x000CD164 File Offset: 0x000CB564
	private static bool ResumeObjectSync<T>(MessageType _messageType, GameObject _object, T _resumeData, IOnlineMultiplayerSessionUserId sessionUserId) where T : Serialisable, new()
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_object);
			if (entry != null)
			{
				ResumeObjectSyncMessage<T> resumeObjectSyncMessage = new ResumeObjectSyncMessage<T>();
				resumeObjectSyncMessage.Initialise(entry.m_Header.m_uEntityID, _resumeData);
				ServerMessenger.m_LocalServer.SendMessageToClient(sessionUserId, _messageType, resumeObjectSyncMessage, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x000CD1B1 File Offset: 0x000CB5B1
	public static bool ResumeWorldObjectSync(GameObject _object, WorldObjectMessage _data, IOnlineMultiplayerSessionUserId sessionUserId)
	{
		return ServerMessenger.ResumeObjectSync<WorldObjectMessage>(MessageType.ResumeWorldObjectSync, _object, _data, sessionUserId);
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x000CD1BD File Offset: 0x000CB5BD
	public static bool ResumeChefPositionSync(GameObject _object, ChefPositionMessage _data, IOnlineMultiplayerSessionUserId sessionUserId)
	{
		return ServerMessenger.ResumeObjectSync<ChefPositionMessage>(MessageType.ResumeChefPositionSync, _object, _data, sessionUserId);
	}

	// Token: 0x06002BFA RID: 11258 RVA: 0x000CD1C9 File Offset: 0x000CB5C9
	public static bool ResumePhysicsObjectSync(GameObject _object, PhysicsObjectMessage _data, IOnlineMultiplayerSessionUserId sessionUserId)
	{
		return ServerMessenger.ResumeObjectSync<PhysicsObjectMessage>(MessageType.ResumePhysicsObjectSync, _object, _data, sessionUserId);
	}

	// Token: 0x06002BFB RID: 11259 RVA: 0x000CD1D5 File Offset: 0x000CB5D5
	public static bool EntitySynchronisation(EntityMessageHeader header, FastList<Serialisable> payloads)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_EntitySynchronisation.Initialise(header, payloads);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.EntitySynchronisation, ServerMessenger.m_EntitySynchronisation, false);
			return true;
		}
		return false;
	}

	// Token: 0x06002BFC RID: 11260 RVA: 0x000CD201 File Offset: 0x000CB601
	public static bool EntitySynchronisation(EntityMessageHeader header, IOnlineMultiplayerSessionUserId recipient, FastList<Serialisable> payloads)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_EntitySynchronisation.Initialise(header, payloads);
			ServerMessenger.m_LocalServer.SendMessageToClient(recipient, MessageType.EntitySynchronisation, ServerMessenger.m_EntitySynchronisation, false);
			return true;
		}
		return false;
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x000CD230 File Offset: 0x000CB630
	public static bool EntityEvent(ServerSynchroniserBase synchroniser, Serialisable payload)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntityMessageHeader entityMessageHeader = new EntityMessageHeader();
			entityMessageHeader.m_uEntityID = synchroniser.GetEntityId();
			NetworkMessageTracker tracker = ServerMessenger.m_LocalServer.GetTracker();
			if (tracker != null)
			{
				tracker.TrackSentEntityEvent(synchroniser.GetEntityType());
			}
			ServerMessenger.m_EntityEvent.Initialise(entityMessageHeader, synchroniser.GetComponentId(), payload);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.EntityEvent, ServerMessenger.m_EntityEvent, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002BFE RID: 11262 RVA: 0x000CD29C File Offset: 0x000CB69C
	public static bool EntityEvent(IOnlineMultiplayerSessionUserId recipient, ServerSynchroniserBase synchroniser, Serialisable payload)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntityMessageHeader entityMessageHeader = new EntityMessageHeader();
			entityMessageHeader.m_uEntityID = synchroniser.GetEntityId();
			NetworkMessageTracker tracker = ServerMessenger.m_LocalServer.GetTracker();
			if (tracker != null)
			{
				tracker.TrackSentEntityEvent(synchroniser.GetEntityType());
			}
			ServerMessenger.m_EntityEvent.Initialise(entityMessageHeader, synchroniser.GetComponentId(), payload);
			ServerMessenger.m_LocalServer.SendMessageToClient(recipient, MessageType.EntityEvent, ServerMessenger.m_EntityEvent, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x000CD309 File Offset: 0x000CB709
	public static bool UsersChanged()
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_UsersChanged.Initialise(ServerUserSystem.m_Users);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.UsersChanged, ServerMessenger.m_UsersChanged, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x000CD338 File Offset: 0x000CB738
	public static bool UserAdded(uint _idx, User _user)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_UserAdded.Initialise(_idx, _user);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.UsersAdded, ServerMessenger.m_UserAdded, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x000CD365 File Offset: 0x000CB765
	public static bool ChefOwnership()
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_ChefOwnership.Initialise(ServerUserSystem.m_Users);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.ChefOwnership, ServerMessenger.m_ChefOwnership, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x000CD394 File Offset: 0x000CB794
	public static bool GameState(GameState state, GameStateMessage.GameStatePayload payload = null)
	{
		ServerMessenger.m_GameState.Initialise(state, ServerUserSystem.s_LocalMachineId, payload);
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.GameState, ServerMessenger.m_GameState, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x000CD3C6 File Offset: 0x000CB7C6
	public static bool ChefAvatar(uint chefAvatar, User user)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_ChefAvatar.Initialise(chefAvatar, user.Machine, user.Engagement, user.Split);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.ChefAvatar, ServerMessenger.m_ChefAvatar, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x000CD404 File Offset: 0x000CB804
	public static bool LobbyMessage(LobbyServerMessage _message)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.LobbyServer, _message, _message.ToSendReliable());
			return true;
		}
		return false;
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x000CD426 File Offset: 0x000CB826
	public static bool EmoteWheelMessage(EmoteWheelMessage _message)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.EmoteWheel, _message, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x000CD443 File Offset: 0x000CB843
	public static bool DynamicLevelMessage(DynamicLevelMessage _message)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.DynamicLevel, _message, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x000CD460 File Offset: 0x000CB860
	public static bool BossLevelMessage(BossLevelMessage _message)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.BossLevel, _message, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x000CD47D File Offset: 0x000CB87D
	public static bool TimeSync(float fTime)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_TimeSyncMessage.Initialise(fTime);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.TimeSync, ServerMessenger.m_TimeSyncMessage, false);
			return true;
		}
		return false;
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x000CD4A9 File Offset: 0x000CB8A9
	public static bool GameSetup(GameMode mode)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_GameSetupMessage.Initialise(mode);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.GameSetup, ServerMessenger.m_GameSetupMessage, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x000CD4D5 File Offset: 0x000CB8D5
	public static bool GameProgressData(GameProgress.GameProgressData progressData, bool[] metaDialogShownStatus)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_GameProgressDataMessage.ProgressData = progressData;
			ServerMessenger.m_GameProgressDataMessage.MetaDialogsShownStatus = metaDialogShownStatus;
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.GameProgressData, ServerMessenger.m_GameProgressDataMessage, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x000CD50C File Offset: 0x000CB90C
	public static bool SetupCoopSession(int dlcID, GameProgress.GameProgressData progressData, bool[] _metaDialogShownStatus, SessionConfig sessionConfig)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_SetupCoopSessionDataMessage.m_sessionConfig.m_config = sessionConfig;
			ServerMessenger.m_SetupCoopSessionDataMessage.m_Progress.ProgressData = progressData;
			ServerMessenger.m_SetupCoopSessionDataMessage.m_Progress.MetaDialogsShownStatus = _metaDialogShownStatus;
			ServerMessenger.m_SetupCoopSessionDataMessage.m_DLCID = dlcID;
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.SetupCoopSession, ServerMessenger.m_SetupCoopSessionDataMessage, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x000CD574 File Offset: 0x000CB974
	public static bool Achievement(GameObject gameObject, int statId, int increment = 1)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
			if (entry != null)
			{
				ServerMessenger.m_AchievementMessage.Initialise(entry.m_Header, statId, increment);
				ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.Achievement, ServerMessenger.m_AchievementMessage, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x000CD5C0 File Offset: 0x000CB9C0
	public static void SendChefEffectMessage(GameObject _targetChef, ChefEffectMessage.EffectType _effectType, Vector3 _relativeEffectPosition)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_targetChef);
		ServerMessenger.SendChefEffectMessage(entry.m_Header.m_uEntityID, _effectType, _relativeEffectPosition);
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x000CD5E6 File Offset: 0x000CB9E6
	public static void SendChefEffectMessage(uint _targetChef, ChefEffectMessage.EffectType _effectType, Vector3 _relativeEffetPosition)
	{
		ServerMessenger.m_ChefEffectMessage.Initalise(_targetChef, _effectType, _relativeEffetPosition);
		ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.ChefEffect, ServerMessenger.m_ChefEffectMessage, true);
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x000CD607 File Offset: 0x000CBA07
	public static bool TriggerAudioMessage(GameOneShotAudioTag _audioTag, int _layer)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_triggerAudioMessage.Initialise(_audioTag, _layer);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.TriggerAudio, ServerMessenger.m_triggerAudioMessage, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x000CD634 File Offset: 0x000CBA34
	public static void HighScores(Serialisable message, IOnlineMultiplayerSessionUserId destination)
	{
		ServerMessenger.m_LocalServer.SendMessageToClient(destination, MessageType.HighScores, message, true);
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000CD645 File Offset: 0x000CBA45
	public static bool SendHostModeConfigChanged(SessionConfig config)
	{
		if (ServerMessenger.m_LocalServer != null)
		{
			ServerMessenger.m_hostModeConfigChangedMessage.Initialise(config);
			ServerMessenger.m_LocalServer.BroadcastMessageToAll(MessageType.SessionConfigSync, ServerMessenger.m_hostModeConfigChangedMessage, true);
			return true;
		}
		return false;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000CD671 File Offset: 0x000CBA71
	public static void OnServerStarted(Server server)
	{
		ServerMessenger.m_LocalServer = server;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x000CD679 File Offset: 0x000CBA79
	public static void OnServerStopped()
	{
		ServerMessenger.m_LocalServer = null;
	}

	// Token: 0x04002349 RID: 9033
	private static ServerMessenger.DeferredLoadLevelState m_deferredLoadLevelState = ServerMessenger.DeferredLoadLevelState.eInvalid;

	// Token: 0x0400234A RID: 9034
	private static LevelLoadByIndexMessage m_LevelLoadByIndex = new LevelLoadByIndexMessage();

	// Token: 0x0400234B RID: 9035
	private static LevelLoadByNameMessage m_LevelLoadByName = new LevelLoadByNameMessage();

	// Token: 0x0400234C RID: 9036
	private static ExampleNetworkMessage m_Example = new ExampleNetworkMessage();

	// Token: 0x0400234D RID: 9037
	private static DestroyEntityMessage m_DestroyEntity = new DestroyEntityMessage();

	// Token: 0x0400234E RID: 9038
	private static DestroyEntitiesMessage m_destroyEntities = new DestroyEntitiesMessage();

	// Token: 0x0400234F RID: 9039
	private static FastList<uint> m_destroyEntityIds = new FastList<uint>(16);

	// Token: 0x04002350 RID: 9040
	private static DestroyChefMessage m_DestroyChef = new DestroyChefMessage();

	// Token: 0x04002351 RID: 9041
	private static SpawnPhysicalAttachmentMessage m_SpawnPhysicalAttachment = new SpawnPhysicalAttachmentMessage();

	// Token: 0x04002352 RID: 9042
	private static SpawnEntityMessage m_SpawnEntity = new SpawnEntityMessage();

	// Token: 0x04002353 RID: 9043
	private static EntitySynchronisationMessage m_EntitySynchronisation = new EntitySynchronisationMessage();

	// Token: 0x04002354 RID: 9044
	private static EntityEventMessage m_EntityEvent = new EntityEventMessage();

	// Token: 0x04002355 RID: 9045
	private static UsersChangedMessage m_UsersChanged = new UsersChangedMessage();

	// Token: 0x04002356 RID: 9046
	private static UserAddedMessage m_UserAdded = new UserAddedMessage();

	// Token: 0x04002357 RID: 9047
	private static UsersChangedMessage m_ChefOwnership = new UsersChangedMessage();

	// Token: 0x04002358 RID: 9048
	private static GameStateMessage m_GameState = new GameStateMessage();

	// Token: 0x04002359 RID: 9049
	private static ChefAvatarMessage m_ChefAvatar = new ChefAvatarMessage();

	// Token: 0x0400235A RID: 9050
	private static TimeSyncMessage m_TimeSyncMessage = new TimeSyncMessage();

	// Token: 0x0400235B RID: 9051
	private static GameSetupMessage m_GameSetupMessage = new GameSetupMessage();

	// Token: 0x0400235C RID: 9052
	private static GameProgressDataNetworkMessage m_GameProgressDataMessage = new GameProgressDataNetworkMessage();

	// Token: 0x0400235D RID: 9053
	private static SetupCoopSessionNetworkMessage m_SetupCoopSessionDataMessage = new SetupCoopSessionNetworkMessage();

	// Token: 0x0400235E RID: 9054
	private static AchievementMessage m_AchievementMessage = new AchievementMessage();

	// Token: 0x0400235F RID: 9055
	private static ChefEffectMessage m_ChefEffectMessage = new ChefEffectMessage();

	// Token: 0x04002360 RID: 9056
	private static TriggerAudioMessage m_triggerAudioMessage = new TriggerAudioMessage();

	// Token: 0x04002361 RID: 9057
	private static SessionConfigSyncMessage m_hostModeConfigChangedMessage = new SessionConfigSyncMessage();

	// Token: 0x04002362 RID: 9058
	private static Server m_LocalServer = null;

	// Token: 0x020008DC RID: 2268
	private enum DeferredLoadLevelState
	{
		// Token: 0x04002364 RID: 9060
		eInvalid,
		// Token: 0x04002365 RID: 9061
		eLoadByIndex,
		// Token: 0x04002366 RID: 9062
		eLoadByName
	}
}
