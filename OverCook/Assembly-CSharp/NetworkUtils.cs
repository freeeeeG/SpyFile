using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200092C RID: 2348
public static class NetworkUtils
{
	// Token: 0x06002E4E RID: 11854 RVA: 0x000DB124 File Offset: 0x000D9524
	public static Transform FindVisualRoot(GameObject _object)
	{
		MeshLerper meshLerper = _object.RequestComponentRecursive<MeshLerper>();
		if (meshLerper != null)
		{
			return meshLerper.transform;
		}
		return _object.transform;
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x000DB151 File Offset: 0x000D9551
	public static void RegisterSpawnablePrefab(GameObject _gameObject, GameObject _prefab)
	{
		NetworkUtils.RegisterSpawnablePrefab(_gameObject, _prefab, null);
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x000DB15C File Offset: 0x000D955C
	public static void RegisterSpawnablePrefab(GameObject _gameObject, GameObject _prefab, VoidGeneric<GameObject> _callback)
	{
		SpawnableEntityCollection spawnableEntityCollection = _gameObject.RequestComponent<SpawnableEntityCollection>();
		if (spawnableEntityCollection == null)
		{
			spawnableEntityCollection = _gameObject.AddComponent<SpawnableEntityCollection>();
		}
		spawnableEntityCollection.RegisterSpawnable(_prefab, _callback);
	}

	// Token: 0x06002E51 RID: 11857 RVA: 0x000DB18B File Offset: 0x000D958B
	public static GameObject ServerSpawnPrefab(GameObject _gameObject, GameObject _prefab)
	{
		return NetworkUtils.ServerSpawnPrefab(_gameObject, _prefab, _gameObject.transform.position, _gameObject.transform.rotation);
	}

	// Token: 0x06002E52 RID: 11858 RVA: 0x000DB1AC File Offset: 0x000D95AC
	public static GameObject ServerSpawnPrefab(GameObject _gameObject, GameObject _prefab, Vector3 _position, Quaternion _rotation)
	{
		INetworkEntitySpawner networkEntitySpawner = _gameObject.RequireInterface<INetworkEntitySpawner>();
		int spawnableID = networkEntitySpawner.GetSpawnableID(_prefab);
		List<VoidGeneric<GameObject>> list = new List<VoidGeneric<GameObject>>();
		GameObject gameObject = networkEntitySpawner.SpawnEntity(spawnableID, _position, _rotation, ref list);
		EntitySerialisationRegistry.ServerRegisterObject(gameObject);
		ComponentCacheRegistry.UpdateObject(gameObject);
		PhysicalAttachment component = gameObject.GetComponent<PhysicalAttachment>();
		if (null != component)
		{
			EntitySerialisationRegistry.ServerRegisterObject(component.m_container.gameObject);
			ComponentCacheRegistry.UpdateObject(component.m_container.gameObject);
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(component.m_container.gameObject);
			EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(gameObject);
			if (entry2 != null && entry != null)
			{
				ServerMessenger.SpawnPhysicalAttachment(networkEntitySpawner, spawnableID, entry2.m_Header, _position, _rotation, entry.m_Header);
			}
		}
		else
		{
			EntitySerialisationEntry entry3 = EntitySerialisationRegistry.GetEntry(gameObject);
			if (entry3 != null)
			{
				ServerMessenger.SpawnEntity(networkEntitySpawner, spawnableID, entry3.m_Header, _position, _rotation);
			}
		}
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
		return gameObject;
	}

	// Token: 0x06002E53 RID: 11859 RVA: 0x000DB2CC File Offset: 0x000D96CC
	public static void DestroyObject(GameObject _gameObject)
	{
		_gameObject.SetActive(false);
		ServerMessenger.DestroyEntity(_gameObject);
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x000DB2DC File Offset: 0x000D96DC
	public static void DestroyObjectsRecursive(GameObject root)
	{
		root.SetActive(false);
		ServerMessenger.DestroyEntities(root);
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x000DB2EC File Offset: 0x000D96EC
	public static void OnResumeEntitySyncMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		ResumeEntitySyncMessage resumeEntitySyncMessage = (ResumeEntitySyncMessage)message;
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(resumeEntitySyncMessage.m_header.m_uEntityID);
		ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = entry.m_GameObject.RequireComponent<ServerWorldObjectSynchroniser>();
		serverWorldObjectSynchroniser.ResumeClient(sessionUserId);
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x000DB324 File Offset: 0x000D9724
	public static void LevelLoadByIndex(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		ClientGameSetup.PrevScene = SceneManager.GetActiveScene().name;
		MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
		if (MultiplayerController.IsSynchronisationActive())
		{
			multiplayerController.StopSynchronisation();
		}
		multiplayerController.SetLatencyMeasurePaused(true);
		LevelLoadByIndexMessage levelLoadByIndexMessage = (LevelLoadByIndexMessage)message;
		GameSession gameSession = GameUtils.GetGameSession();
		SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[(int)((UIntPtr)levelLoadByIndexMessage.LevelIndex)];
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient((int)levelLoadByIndexMessage.Players);
		if (sceneVarient != null)
		{
			string sceneName = sceneVarient.SceneName;
			if (sceneName != string.Empty)
			{
				gameSession.LevelSettings = new GameSession.GameLevelSettings
				{
					SceneDirectoryVarientEntry = sceneVarient
				};
				gameSession.Progress.SetLastLevelEntered((int)levelLoadByIndexMessage.LevelIndex);
				GameUtils.GetMetaGameProgress().SetLastPlayedTheme(sceneDirectoryEntry.Theme);
				if (levelLoadByIndexMessage.UseLoadingScreen)
				{
					LoadingScreenFlow.LoadScene(sceneName, levelLoadByIndexMessage.m_HideLoadingScreenGameState);
				}
				else
				{
					GameUtils.LoadScene(sceneName, LoadSceneMode.Single);
				}
			}
		}
		else
		{
			Vector2 vector = new Vector2(0.5f * (float)Camera.main.pixelWidth, 0.5f * (float)Camera.main.pixelHeight);
		}
		ClientMessenger.GameState(levelLoadByIndexMessage.m_StartLoadGameState);
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x000DB454 File Offset: 0x000D9854
	public static void LevelLoadByName(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		ClientGameSetup.PrevScene = SceneManager.GetActiveScene().name;
		MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
		if (MultiplayerController.IsSynchronisationActive())
		{
			multiplayerController.StopSynchronisation();
		}
		multiplayerController.SetLatencyMeasurePaused(true);
		LevelLoadByNameMessage levelLoadByNameMessage = (LevelLoadByNameMessage)message;
		if (levelLoadByNameMessage.UseLoadingScreen)
		{
			LoadingScreenFlow.LoadScene(levelLoadByNameMessage.m_Scene, levelLoadByNameMessage.m_HideLoadingScreenGameState);
		}
		else
		{
			GameUtils.LoadScene(levelLoadByNameMessage.m_Scene, LoadSceneMode.Single);
		}
		ClientMessenger.GameState(levelLoadByNameMessage.m_StartLoadGameState);
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x000DB4D0 File Offset: 0x000D98D0
	public static void LoadGameProgressData(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		GameProgressDataNetworkMessage gameProgressDataNetworkMessage = (GameProgressDataNetworkMessage)message;
		gameSession.Progress.LoadFromNetwork(gameProgressDataNetworkMessage.ProgressData);
		gameSession.m_shownMetaDialogs = gameProgressDataNetworkMessage.MetaDialogsShownStatus;
		if (NetworkUtils.OnGameProgressLoadedFromNetwork != null)
		{
			NetworkUtils.OnGameProgressLoadedFromNetwork();
		}
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x000DB51C File Offset: 0x000D991C
	public static void SetupCoopSession(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		SetupCoopSessionNetworkMessage setupCoopSessionNetworkMessage = (SetupCoopSessionNetworkMessage)message;
		GameSession gameSession = GameUtils.GetGameSession();
		if ((gameSession == null || gameSession.TypeSettings.Type != GameSession.GameType.Cooperative || gameSession.DLC != setupCoopSessionNetworkMessage.m_DLCID || gameSession.TypeSettings.WorldMapScene == "Lobbies") && T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.StartEmptySession(GameSession.GameType.Cooperative, setupCoopSessionNetworkMessage.m_DLCID);
		}
		NetworkUtils.LoadGameProgressData(sessionUserId, setupCoopSessionNetworkMessage.m_Progress);
		GameProgress.GameProgressData progressData = setupCoopSessionNetworkMessage.m_Progress.ProgressData;
		GameProgress.HighScores highScores = new GameProgress.HighScores();
		for (int i = 0; i < progressData.Levels.Length; i++)
		{
			highScores.Scores.Add(new GameProgress.HighScores.Score
			{
				iLevelID = progressData.Levels[i].LevelId,
				iHighScore = progressData.Levels[i].HighScore,
				iSurvivalModeTime = progressData.Levels[i].SurvivalModeTime
			});
		}
		if (sessionUserId != null)
		{
			User user = UserSystemUtils.FindUser(ClientUserSystem.m_Users, sessionUserId, User.MachineID.Count, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
			if (user != null)
			{
				gameSession.HighScoreRepository.SetScoresForMachine(user.Machine, setupCoopSessionNetworkMessage.m_DLCID, highScores);
			}
		}
		gameSession.GameModeSessionConfig = setupCoopSessionNetworkMessage.m_sessionConfig.m_config;
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x000DB678 File Offset: 0x000D9A78
	public static void RepeatHighScores(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		if (sessionUserId != null)
		{
			ServerMessenger.HighScores(message, null);
		}
		IOnlineMultiplayerSessionCoordinator onlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		IOnlineMultiplayerSessionUserId[] array = onlineMultiplayerSessionCoordinator.Members();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].UniqueId != sessionUserId.UniqueId)
			{
				ServerMessenger.HighScores(message, array[i]);
			}
		}
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x000DB6D8 File Offset: 0x000D9AD8
	public static string GetNetworkMessageDescription(Message message)
	{
		string text = "Type: " + message.Type;
		if (message.Type == MessageType.EntitySynchronisation)
		{
			EntitySynchronisationMessage entitySynchronisationMessage = message.Payload as EntitySynchronisationMessage;
			if (entitySynchronisationMessage != null && entitySynchronisationMessage.m_Header != null)
			{
				text = text + ", Entity ID: " + entitySynchronisationMessage.m_Header.m_uEntityID;
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(entitySynchronisationMessage.m_Header.m_uEntityID);
				if (entry != null)
				{
					text += ", Entry found ";
					if (null != entry.m_GameObject)
					{
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							", Name: ",
							entry.m_GameObject.name,
							", ServerComponents: ",
							entry.m_ServerSynchronisedComponents.Count,
							", ClientComponents: ",
							entry.m_ClientSynchronisedComponents.Count
						});
					}
				}
				else
				{
					text += ", Entry not found";
				}
			}
		}
		if (message.Type == MessageType.EntityEvent)
		{
			EntityEventMessage entityEventMessage = message.Payload as EntityEventMessage;
			if (entityEventMessage != null && entityEventMessage.m_Header != null)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					", Entity ID: ",
					entityEventMessage.m_Header.m_uEntityID,
					", ComponentID: ",
					entityEventMessage.m_ComponentId
				});
				EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(entityEventMessage.m_Header.m_uEntityID);
				if (entry2 != null)
				{
					text += ", Entry found ";
					if (null != entry2.m_GameObject)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							", Name: ",
							entry2.m_GameObject.name,
							", ServerComponents: ",
							entry2.m_ServerSynchronisedComponents.Count,
							", ClientComponents: ",
							entry2.m_ClientSynchronisedComponents.Count
						});
					}
				}
				else
				{
					text += ", Entry not found";
				}
			}
		}
		return text;
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x000DB900 File Offset: 0x000D9D00
	public static void SelectRandomAvatar()
	{
		AvatarDirectoryData avatarDirectoryData = GameUtils.GetAvatarDirectoryData();
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		if (null != metaGameProgress)
		{
			ChefAvatarData[] array = null;
			for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
			{
				User user = ClientUserSystem.m_Users._items[i];
				if (user.IsLocal)
				{
					uint num = user.SelectedChefAvatar;
					if (num == 127U)
					{
						if (array == null)
						{
							array = metaGameProgress.GetUnlockedAvatars();
						}
						int num2 = UnityEngine.Random.Range(0, array.Length);
						ChefAvatarData avatarData = array[num2];
						num = (uint)avatarDirectoryData.Avatars.FindIndex_Predicate((ChefAvatarData x) => x == avatarData);
						ClientMessenger.ChefAvatar(num, user);
					}
				}
			}
		}
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x000DB9BC File Offset: 0x000D9DBC
	public static User.PartyPersistance GetRemoteUserPartyPersistanceForJoinState(NetConnectionState state)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		IOnlineMultiplayerConnectionModeCoordinator onlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
		if (onlineMultiplayerConnectionModeCoordinator == null)
		{
			return (state != NetConnectionState.AcceptInvite) ? User.PartyPersistance.Kick : User.PartyPersistance.Remain;
		}
		if (onlineMultiplayerConnectionModeCoordinator.Mode() == OnlineMultiplayerConnectionMode.eInternet && state == NetConnectionState.Matchmake)
		{
			return User.PartyPersistance.Kick;
		}
		return User.PartyPersistance.Remain;
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x000DBA04 File Offset: 0x000D9E04
	public static bool DeserialiseGameObject(out GameObject gameObject, BitStreamReader reader)
	{
		uint num = reader.ReadUInt32(10);
		gameObject = null;
		if (num == 0U)
		{
			return false;
		}
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(num);
		if (entry != null && entry.m_GameObject != null)
		{
			gameObject = entry.m_GameObject;
			return true;
		}
		return false;
	}

	// Token: 0x0400252A RID: 9514
	public static GenericVoid OnGameProgressLoadedFromNetwork = delegate()
	{
	};

	// Token: 0x0200092D RID: 2349
	public static class CompressFloat
	{
		// Token: 0x06002E61 RID: 11873 RVA: 0x000DBA63 File Offset: 0x000D9E63
		public static uint Pack(float val, float maxSize, int bits)
		{
			if (val > maxSize)
			{
				val = maxSize;
			}
			else if (val < -maxSize)
			{
				val = -maxSize;
			}
			return (uint)((val + maxSize) * (float)(1 << bits - 1) / (maxSize * 2f));
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000DBA96 File Offset: 0x000D9E96
		public static float UnPack(uint val, float maxSize, int bits)
		{
			if (bits < 32)
			{
				val &= (1U << bits) - 1U;
			}
			return val * maxSize * 2f / (float)(1 << bits - 1) - maxSize;
		}
	}
}
