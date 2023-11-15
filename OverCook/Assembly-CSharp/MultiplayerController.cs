using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GameModes;
using GameModes.Horde;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020008EA RID: 2282
public class MultiplayerController : Manager
{
	// Token: 0x06002C4E RID: 11342 RVA: 0x000CE490 File Offset: 0x000CC890
	private void Awake()
	{
		SerialisationRegistry<MessageType>.Initialise(new MessageTypeComparer());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.EntitySynchronisation, new EntitySynchronisationMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.EntityEvent, new EntityEventMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.SpawnEntity, new SpawnEntityMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.DestroyEntity, new DestroyEntityMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.Input, new ControllerStateMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ChefOwnership, new UsersChangedMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.UsersChanged, new UsersChangedMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.UsersAdded, new UserAddedMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.LevelLoadByIndex, new LevelLoadByIndexMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.LevelLoadByName, new LevelLoadByNameMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.GameState, new GameStateMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ChefAvatar, new ChefAvatarMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.LobbyServer, new LobbyServerMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.LobbyClient, new LobbyClientMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.DynamicLevel, new DynamicLevelMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ChefEvent, new ChefEventMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ChefEffect, new ChefEffectMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.LatencyMeasure, new LatencyMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.TimeSync, new TimeSyncMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ControllerSettings, new ControllerSettingsMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.MapAvatar, new MapAvatarControlsMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.MapAvatarHorn, new MapAvatarHornMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.GameSetup, new GameSetupMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.GameProgressData, new GameProgressDataNetworkMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.EmoteWheel, new EmoteWheelMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.SetupCoopSession, new SetupCoopSessionNetworkMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.Achievement, new AchievementMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.TriggerAudio, new TriggerAudioMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.SpawnPhysicalAttachment, new SpawnPhysicalAttachmentMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ResumeWorldObjectSync, new ResumeObjectSyncMessage<WorldObjectMessage>());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ResumeChefPositionSync, new ResumeObjectSyncMessage<ChefPositionMessage>());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ResumePhysicsObjectSync, new ResumeObjectSyncMessage<PhysicsObjectMessage>());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.BossLevel, new BossLevelMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.DestroyChef, new DestroyChefMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.HighScores, new HighScoresMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.DestroyEntities, new DestroyEntitiesMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.ResumeEntitySync, new ResumeEntitySyncMessage());
		SerialisationRegistry<MessageType>.RegisterMessageType(MessageType.SessionConfigSync, new SessionConfigSyncMessage());
		SerialisationRegistry<EntityType>.Initialise(new EntityTypeComparer());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.WorldObject, new WorldObjectMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.WorldPopup, new WorldMapInfoPopupMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PhysicsObject, new PhysicsObjectMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Chef, new ChefPositionMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.SprayingUtensil, new SprayingUtensilMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.RespawnBehaviour, new RespawnMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Flammable, new FlammableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Workstation, new WorkstationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Workable, new WorkableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PlateStation, new PlateStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PlateStack, new PlateStackMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.WashingStation, new WashingStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PhysicalAttach, new PhysicalAttachMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.IngredientContainer, new IngredientContainerMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.CookingState, new CookingStateMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.MixingState, new MixingStateMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.AttachStation, new AttachStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.CookingStation, new CookingStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.ConveyorStation, new ConveyorStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.ConveyorAnimator, new ConveyorAnimationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TimedQueue, new TimedQueueMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerZone, new TriggerZoneMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerDisable, new TriggerDisableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerOnAnimator, new TriggerOnAnimatorMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerToggleOnAnimator, new TriggerToggleOnAnimatorMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerMoveSpawn, new MoveSpawnMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerDialogue, new TriggerDialogueMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.AnimatorVariable, new TriggerAnimatorVariableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.ChefCarry, new ChefCarryMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.InputEvent, new InputEventMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.FlowController, new KitchenFlowMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.ThrowableItem, new ThrowableItemMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Teleportal, new TeleportalMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Cutscene, new CutsceneStateMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Dialogue, new DialogueStateMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TutorialPopup, new TutorialDismissMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.AttachCatcher, new AttachmentCatcherMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.SessionInteractable, new SessionInteractableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.WorldMapVanControls, new MapAvatarControlsMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.WorldMapVanAvatar, new AvatarPositionMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.SwitchMapNode, new SwitchMapNodeMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.LevelPortalMapNode, new LevelPortalMapNodeMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.RubbishBin, new RubbishBinMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.MixingStation, new MixingStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Washable, new WashableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.HeatedStation, new HeatedStationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.RespawnCollider, new RespawnColliderMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.AutoWorkstation, new AutoWorkstationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PlacementItemSpawner, new PlacementItemSpawnerMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.HordeFlowController, default(HordeFlowMessage));
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.HordeTarget, default(HordeTargetMessage));
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.HordeEnemy, default(HordeEnemyMessage));
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.HordeLockable, new HordeLockableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PickupItemSwitcher, new PickupItemSwitcherMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.Cannon, new CannonMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.PilotRotation, new PilotRotationMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.TriggerColourCycle, new TriggerColourCycleMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.MultiTriggerDisable, new TriggerDisableMessage());
		SerialisationRegistry<EntityType>.RegisterMessageType(EntityType.FireHazardSpawner, new FireHazardSpawnerMessage());
		Mailbox.Server.Initialise(this.m_LocalServer);
		Mailbox.Client.Initialise(this.m_LocalClient);
		ConnectionModeSwitcher.Initialise(this.m_LocalServer, this.m_LocalClient);
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnOfflineMode));
		Mailbox server = Mailbox.Server;
		MessageType type = MessageType.HighScores;
		if (MultiplayerController.<>f__mg$cache0 == null)
		{
			MultiplayerController.<>f__mg$cache0 = new OrderedMessageReceivedCallback(NetworkUtils.RepeatHighScores);
		}
		server.RegisterForMessageType(type, MultiplayerController.<>f__mg$cache0);
		Mailbox server2 = Mailbox.Server;
		MessageType type2 = MessageType.ResumeEntitySync;
		if (MultiplayerController.<>f__mg$cache1 == null)
		{
			MultiplayerController.<>f__mg$cache1 = new OrderedMessageReceivedCallback(NetworkUtils.OnResumeEntitySyncMessageReceived);
		}
		server2.RegisterForMessageType(type2, MultiplayerController.<>f__mg$cache1);
		Mailbox.Client.RegisterForMessageType(MessageType.Input, new OrderedMessageReceivedCallback(this.m_ClientSync.OnEntityEventMessageReceived));
		Mailbox client = Mailbox.Client;
		MessageType type3 = MessageType.LevelLoadByIndex;
		if (MultiplayerController.<>f__mg$cache2 == null)
		{
			MultiplayerController.<>f__mg$cache2 = new OrderedMessageReceivedCallback(NetworkUtils.LevelLoadByIndex);
		}
		client.RegisterForMessageType(type3, MultiplayerController.<>f__mg$cache2);
		Mailbox client2 = Mailbox.Client;
		MessageType type4 = MessageType.LevelLoadByName;
		if (MultiplayerController.<>f__mg$cache3 == null)
		{
			MultiplayerController.<>f__mg$cache3 = new OrderedMessageReceivedCallback(NetworkUtils.LevelLoadByName);
		}
		client2.RegisterForMessageType(type4, MultiplayerController.<>f__mg$cache3);
		Mailbox client3 = Mailbox.Client;
		MessageType type5 = MessageType.GameProgressData;
		if (MultiplayerController.<>f__mg$cache4 == null)
		{
			MultiplayerController.<>f__mg$cache4 = new OrderedMessageReceivedCallback(NetworkUtils.LoadGameProgressData);
		}
		client3.RegisterForMessageType(type5, MultiplayerController.<>f__mg$cache4);
		Mailbox client4 = Mailbox.Client;
		MessageType type6 = MessageType.SetupCoopSession;
		if (MultiplayerController.<>f__mg$cache5 == null)
		{
			MultiplayerController.<>f__mg$cache5 = new OrderedMessageReceivedCallback(NetworkUtils.SetupCoopSession);
		}
		client4.RegisterForMessageType(type6, MultiplayerController.<>f__mg$cache5);
		Mailbox.Client.RegisterForMessageType(MessageType.TriggerAudio, new OrderedMessageReceivedCallback(this.m_ClientSync.OnTriggerAudioMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.ResumeWorldObjectSync, new OrderedMessageReceivedCallback(this.m_ClientSync.OnResumeObjectMessageReceived<WorldObjectMessage>));
		Mailbox.Client.RegisterForMessageType(MessageType.ResumeChefPositionSync, new OrderedMessageReceivedCallback(this.m_ClientSync.OnResumeObjectMessageReceived<ChefPositionMessage>));
		Mailbox.Client.RegisterForMessageType(MessageType.ResumePhysicsObjectSync, new OrderedMessageReceivedCallback(this.m_ClientSync.OnResumeObjectMessageReceived<PhysicsObjectMessage>));
		Mailbox.Client.RegisterForMessageType(MessageType.SessionConfigSync, new OrderedMessageReceivedCallback(this.m_ClientSync.OnSessionConfigReceived));
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x000CEB30 File Offset: 0x000CCF30
	private void OnOfflineMode(IConnectionModeSwitchStatus status)
	{
		UserSystemUtils.Initialise();
		this.m_ConnectionStatus.Initialise();
		this.m_DisconnectionHandler.Initialise();
		ClientGameSetup.Initialise();
		ServerGameSetup.Mode = GameMode.OnlineKitchen;
		this.m_LocalClient.OnLeftSession += this.m_DisconnectionHandler.HandleSessionConnectionLost;
		Delegate usersChanged = ClientUserSystem.usersChanged;
		if (MultiplayerController.<>f__mg$cache6 == null)
		{
			MultiplayerController.<>f__mg$cache6 = new GenericVoid(NetworkUtils.SelectRandomAvatar);
		}
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(usersChanged, MultiplayerController.<>f__mg$cache6);
	}

	// Token: 0x06002C50 RID: 11344 RVA: 0x000CEBB0 File Offset: 0x000CCFB0
	[Conditional("DEBUG")]
	private void CheckBitCount()
	{
		int num = 0;
		IEnumerator enumerator = Enum.GetValues(typeof(GameState)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				GameState gameState = (GameState)obj;
				if (gameState > (GameState)num)
				{
					num = (int)gameState;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06002C51 RID: 11345 RVA: 0x000CEC24 File Offset: 0x000CD024
	private void Start()
	{
		this.m_InviteMonitor.Initialise();
	}

	// Token: 0x06002C52 RID: 11346 RVA: 0x000CEC34 File Offset: 0x000CD034
	private void OnDestroy()
	{
		Delegate usersChanged = ClientUserSystem.usersChanged;
		if (MultiplayerController.<>f__mg$cache7 == null)
		{
			MultiplayerController.<>f__mg$cache7 = new GenericVoid(NetworkUtils.SelectRandomAvatar);
		}
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(usersChanged, MultiplayerController.<>f__mg$cache7);
		this.m_EntitySerialiser.Clear();
		this.m_scanCoroutine = null;
		this.m_ServerSync.StopSynchronising();
		Mailbox server = Mailbox.Server;
		MessageType type = MessageType.HighScores;
		if (MultiplayerController.<>f__mg$cache8 == null)
		{
			MultiplayerController.<>f__mg$cache8 = new OrderedMessageReceivedCallback(NetworkUtils.RepeatHighScores);
		}
		server.UnregisterForMessageType(type, MultiplayerController.<>f__mg$cache8);
		Mailbox server2 = Mailbox.Server;
		MessageType type2 = MessageType.ResumeEntitySync;
		if (MultiplayerController.<>f__mg$cache9 == null)
		{
			MultiplayerController.<>f__mg$cache9 = new OrderedMessageReceivedCallback(NetworkUtils.OnResumeEntitySyncMessageReceived);
		}
		server2.UnregisterForMessageType(type2, MultiplayerController.<>f__mg$cache9);
		Mailbox.Client.UnregisterForMessageType(MessageType.Input, new OrderedMessageReceivedCallback(this.m_ClientSync.OnEntityEventMessageReceived));
		Mailbox client = Mailbox.Client;
		MessageType type3 = MessageType.LevelLoadByIndex;
		if (MultiplayerController.<>f__mg$cacheA == null)
		{
			MultiplayerController.<>f__mg$cacheA = new OrderedMessageReceivedCallback(NetworkUtils.LevelLoadByIndex);
		}
		client.UnregisterForMessageType(type3, MultiplayerController.<>f__mg$cacheA);
		Mailbox client2 = Mailbox.Client;
		MessageType type4 = MessageType.LevelLoadByName;
		if (MultiplayerController.<>f__mg$cacheB == null)
		{
			MultiplayerController.<>f__mg$cacheB = new OrderedMessageReceivedCallback(NetworkUtils.LevelLoadByName);
		}
		client2.UnregisterForMessageType(type4, MultiplayerController.<>f__mg$cacheB);
		Mailbox client3 = Mailbox.Client;
		MessageType type5 = MessageType.GameProgressData;
		if (MultiplayerController.<>f__mg$cacheC == null)
		{
			MultiplayerController.<>f__mg$cacheC = new OrderedMessageReceivedCallback(NetworkUtils.LoadGameProgressData);
		}
		client3.UnregisterForMessageType(type5, MultiplayerController.<>f__mg$cacheC);
		Mailbox client4 = Mailbox.Client;
		MessageType type6 = MessageType.SetupCoopSession;
		if (MultiplayerController.<>f__mg$cacheD == null)
		{
			MultiplayerController.<>f__mg$cacheD = new OrderedMessageReceivedCallback(NetworkUtils.SetupCoopSession);
		}
		client4.UnregisterForMessageType(type6, MultiplayerController.<>f__mg$cacheD);
		Mailbox.Server.Clear();
		Mailbox.Client.Clear();
		Mailbox.Server.Shutdown();
		Mailbox.Client.Shutdown();
		this.m_ConnectionStatus.Shutdown();
		this.m_LocalClient.OnLeftSession -= this.m_DisconnectionHandler.HandleSessionConnectionLost;
		this.m_DisconnectionHandler.Shutdown();
		ClientGameSetup.Shutdown();
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x000CEE00 File Offset: 0x000CD200
	public void StartWorldMap()
	{
		if (this.IsServer())
		{
			base.gameObject.AddComponent<ServerMapLoader>();
		}
		ClientMapLoader clientMapLoader = base.gameObject.AddComponent<ClientMapLoader>();
		clientMapLoader.Initialise(this.m_LocalClient, this);
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x000CEE3D File Offset: 0x000CD23D
	public void StopWorldMap()
	{
		this.TryRemoveComponent<ServerMapLoader>();
		this.TryRemoveComponent<ClientMapLoader>();
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000CEE4C File Offset: 0x000CD24C
	public void StartKitchen()
	{
		if (this.IsServer())
		{
			base.gameObject.AddComponent<ServerKitchenLoader>();
		}
		ClientKitchenLoader clientKitchenLoader = base.gameObject.AddComponent<ClientKitchenLoader>();
		clientKitchenLoader.Initialise(this.m_LocalClient, this);
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000CEE89 File Offset: 0x000CD289
	public void StopKitchen()
	{
		this.TryRemoveComponent<ServerKitchenLoader>();
		this.TryRemoveComponent<ClientKitchenLoader>();
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06002C57 RID: 11351 RVA: 0x000CEE97 File Offset: 0x000CD297
	public bool ScanActive
	{
		get
		{
			return this.m_scanCoroutine != null;
		}
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x000CEEA5 File Offset: 0x000CD2A5
	public void ScanEntities(Action onComplete = null)
	{
		this.m_scanCoroutine = this.AsyncScanEntities(onComplete);
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x000CEEB4 File Offset: 0x000CD2B4
	private IEnumerator AsyncScanEntities(Action onComplete)
	{
		GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		for (int i = 0; i < rootObjects.Length; i++)
		{
			foreach (PhysicalAttachment physicalAttachment in rootObjects[i].GetComponentsInChildren(typeof(PhysicalAttachment), true))
			{
				physicalAttachment.InactiveSetup();
			}
		}
		this.m_EntitySerialiser.Clear();
		this.m_ServerSync.StopSynchronising();
		if (this.IsServer())
		{
			this.m_EntitySerialiser.AddSynchronisedType(typeof(PlayerAnimationDecisions), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerChefSynchroniser), typeof(ClientOnTheServerChefSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(EmptyLerp), typeof(EmptyLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(MapAvatarControls), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMapAvatarControls), typeof(ClientMapAvatarControls)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(EmptyLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(PhysicsObjectSynchroniser), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPhysicsObjectSynchroniser), typeof(ServerClientPhysicsObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(EmptyLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(RigidbodyMotion), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(EmptyLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(PhysicalAttachment), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPhysicalAttachment), typeof(ClientPhysicalAttachment)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(RendererSceneInfo)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(EmptyLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(Projectile), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerProjectile), typeof(ClientProjectile)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(EmptyLerp))
			});
		}
		else
		{
			this.m_EntitySerialiser.AddSynchronisedType(typeof(PlayerAnimationDecisions), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerChefSynchroniser), typeof(ClientChefSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(EmptyLerp), typeof(EmptyLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(MapAvatarControls), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMapAvatarControls), typeof(ClientMapAvatarControls)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(BasicLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(PhysicsObjectSynchroniser), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPhysicsObjectSynchroniser), typeof(ClientPhysicsObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(PhysicsObjectLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(RigidbodyMotion), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(BasicLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(PhysicalAttachment), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPhysicalAttachment), typeof(ClientPhysicalAttachment)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(RendererSceneInfo)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(SnapLerp))
			});
			this.m_EntitySerialiser.AddSynchronisedType(typeof(Projectile), new SynchroniserConfig[]
			{
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldObjectSynchroniser), typeof(ClientWorldObjectSynchroniser)),
				new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerProjectile), typeof(ClientProjectile)),
				new SynchroniserConfig(InstancesPerGameObject.Single, null, typeof(BasicLerp))
			});
		}
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DummySynchroniserComponent), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDummySynchroniserComponent), typeof(ClientDummySynchroniserComponent)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DynamicLandscapeParenting), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDynamicLandscapeParenting), typeof(ClientDynamicLandscapeParenting)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(StoryLevelFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerStoryLevelFlowController), typeof(ClientStoryLevelFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TutorialIconController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTutorialIconController), typeof(ClientTutorialIconController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IconTutorialBase), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIconTutorialBase), typeof(ClientIconTutorialBase)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BossFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBossFlowController), typeof(ClientBossFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DynamicFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDynamicFlowController), typeof(ClientDynamicFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CampaignFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCampaignFlowController), typeof(ClientCampaignFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CompetitiveFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCompetitiveFlowController), typeof(ClientCompetitiveFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WorldMapFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldMapFlowController), typeof(ClientWorldMapFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MapAvatarDynamicLandscapeParenting), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMapAvatarDynamicLandscapeParenting), typeof(ClientMapAvatarDynamicLandscapeParenting)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WaterGunSpray), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWaterGunSpray), typeof(ClientWaterGunSpray)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Washable), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWashable), typeof(ClientWashable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BellowsSpray), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBellowsSpray), typeof(ClientBellowsSpray)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IngredientSpray), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIngredientSpray), typeof(ClientIngredientSpray)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Backpack), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBackpack), typeof(ClientBackpack)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Backpack), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBackpackDispenser), typeof(ClientBackpackDispenser)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(LimitedFromOrderComplexity), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerLimitedFromOrderComplexity), typeof(ClientLimitedFromOrderComplexity)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(LimitedQuantityItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerLimitedQuantityItem), typeof(ClientLimitedQuantityItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(LimitedQuantityItemManager), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerLimitedQuantityItemManager), typeof(ClientLimitedQuantityItemManager)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachmentWindReceiver), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentWindReceiver), typeof(ClientAttachmentWindReceiver)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FireExtinguishSpray), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFireExtinguishSpray), typeof(ClientFireExtinguishSpray)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FlamethrowerSpray), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFlamethrowerSpray), typeof(ClientFlamethrowerSpray)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Flammable), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFlammable), typeof(ClientFlammable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlayerControls), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerInputReceiver), typeof(ClientInputTransmitter)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlayerAttachmentCarrier), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlayerAttachmentCarrier), typeof(ClientPlayerAttachmentCarrier)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachStation), new SynchroniserConfig[]
		{
			new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachStation), typeof(ClientAttachStation)),
			new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentCatchingProxy), typeof(ClientAttachmentCatchingProxy))
		});
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ObjectContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerSynchroniserBase), typeof(ClientSynchroniserBase)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerAttachedSpawn), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerAttachedSpawn), typeof(ClientTriggerAttachedSpawn)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(RubbishBin), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerRubbishBin), typeof(ClientRubbishBin)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Workstation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorkstation), typeof(ClientWorkstation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WorkableItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorkableItem), typeof(ClientWorkableItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlateReturnStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlateReturnStation), typeof(ClientPlateReturnStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlateStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlateStation), typeof(ClientPlateStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CleanPlateStack), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCleanPlateStack), typeof(ClientCleanPlateStack)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DirtyPlateStack), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDirtyPlateStack), typeof(ClientDirtyPlateStack)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Tray), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTray), typeof(ClientTray)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Plate), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlate), typeof(ClientPlate)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WashingStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWashingStation), typeof(ClientWashingStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HandlePlacementReferral), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHandlePlacementReferral), typeof(ClientHandlePlacementReferral)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HandlePickupReferral), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHandlePickupReferral), typeof(ClientHandlePickupReferral)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TrayIngredientContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTrayIngredientContainer), typeof(ClientTrayIngredientContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IngredientContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIngredientContainer), typeof(ClientIngredientContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookableContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookableContainer), typeof(ClientCookableContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookablePreparationContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookablePreparationContainer), typeof(ClientCookablePreparationContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PreparationContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPreparationContainer), typeof(ClientPreparationContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MixableContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMixableContainer), typeof(ClientMixableContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ItemContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerItemContainer), typeof(ClientItemContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(LadleContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerLadleContainer), typeof(ClientLadleContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WokEffectsCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWokEffectsCosmeticDecisions), typeof(ClientWokEffectsCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ContentsDisposalBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerContentsDisposalBehaviour), typeof(ClientContentsDisposalBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IngredientDisposalBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIngredientDisposalBehaviour), typeof(ClientIngredientDisposalBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TrayPlacementContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTrayPlacementContainer), typeof(ClientTrayPlacementContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlacementContainer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlacementContainer), typeof(ClientPlacementContainer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IngredientContentGUI), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIngredientContentGUI), typeof(ClientIngredientContentGUI)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ProjectileSpawner), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerProjectileSpawner), typeof(ClientProjectileSpawner)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FireHazardSpawner), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFireHazardSpawner), typeof(ClientFireHazardSpawner)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FireHazard), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFireHazard), typeof(ClientFireHazard)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AnticipateInteractionHighlight), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAnticipateInteractionHighlight), typeof(ClientAnticipateInteractionHighlight)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookingEffectsCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookingEffectsCosmeticDecisions), typeof(ClientCookingEffectsCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookingHandler), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookingHandler), typeof(ClientCookingHandler)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ContentsCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerContentsCosmeticDecisions), typeof(ClientContentsCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CakeTinContentsCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCakeTinContentsCosmeticDecisions), typeof(ClientCakeTinContentsCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FryingContentsCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFryingContentsCosmeticDecisions), typeof(ClientFryingContentsCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FlambeCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFlambeCosmeticDecisions), typeof(ClientFlambeCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookableIngredient), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookableIngredient), typeof(ClientCookableIngredient)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HeatedStation), new SynchroniserConfig[]
		{
			new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHeatedStation), typeof(ClientHeatedStation)),
			new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentCatchingProxy), typeof(ClientAttachmentCatchingProxy))
		});
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HeatedCookingStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHeatedCookingStation), typeof(ClientHeatedCookingStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HeatedStationGUI), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHeatedStationGUI), typeof(ClientHeatedStationGUI)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BarbequeCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBarbequeCosmeticDecisions), typeof(ClientBarbequeCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BellowsCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBellowsCosmeticDecisions), typeof(ClientBellowsCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WaterGunCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWaterGunCosmeticDecisions), typeof(ClientWaterGunCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BlenderCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBlenderCosmeticDecisions), typeof(ClientBlenderCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CampfireCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCampfireCosmeticDecisions), typeof(ClientCampfireCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ToastingForkCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerToastingForkCosmeticDecisions), typeof(ClientToastingForkCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookingStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookingStation), typeof(ClientCookingStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MixingStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMixingStation), typeof(ClientMixingStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MixingHandler), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMixingHandler), typeof(ClientMixingHandler)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(OvenCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerOvenCosmeticDecisions), typeof(ClientOvenCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ChoppingStationCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerChoppingStationCosmeticDecisions), typeof(ClientChoppingStationCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ConveyorStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerConveyorStation), typeof(ClientConveyorStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TabletopConveyenceReceiver), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTabletopConveyenceReceiver), typeof(ClientTabletopConveyenceReceiver)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalConveyenceReceiver), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalConveyenceReceiver), typeof(ClientTeleportalConveyenceReceiver)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ConveyorBeltCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerConveyorBeltCosmeticDecisions), typeof(ClientConveyorBeltCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerAnimationOnConveyor), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTriggerAnimationOnConveyor), typeof(ClientTriggerAnimationOnConveyor)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachedOrderCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachedOrderCosmeticDecisions), typeof(ClientAttachedOrderCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PickupItemSpawner), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPickupItemSpawner), typeof(ClientPickupItemSpawner)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlacementItemSpawner), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlacementItemSpawner), typeof(ClientPlacementItemSpawner)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ItemCrateCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerItemCrateCosmeticDecisions), typeof(ClientItemCrateCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Teleportal), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportal), typeof(ClientTeleportal)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalPlayerSender), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalPlayerSender), typeof(ClientTeleportalPlayerSender)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalPlayerReceiver), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalPlayerReceiver), typeof(ClientTeleportalPlayerReceiver)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalAttachmentSender), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalAttachmentSender), typeof(ClientTeleportalAttachmentSender)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalAttachmentReceiver), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalAttachmentReceiver), typeof(ClientTeleportalAttachmentReceiver)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalCosmeticDecisions), typeof(ClientTeleportalCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportablePlayer), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportablePlayer), typeof(ClientTeleportablePlayer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportableItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportableItem), typeof(ClientTeleportableItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Terminal), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTerminal), typeof(ClientTerminal)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TerminalCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTerminalCosmeticDecisions), typeof(ClientTerminalCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PilotRotation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPilotRotation), typeof(ClientPilotRotation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PilotMovement), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPilotMovement), typeof(ClientPilotMovement)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MovingPlatformCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMovingPlatformCosmeticDecisions), typeof(ClientMovingPlatformCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ExtinguishCollider), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerExtinguishCollider), typeof(ClientExtinguishCollider)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PushableObject), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPushableObject), typeof(ClientPushableObject)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookingRegion), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookingRegion), typeof(ClientCookingRegion)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerZone), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerZone), typeof(ClientTriggerZone)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerTimer), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerTimer), typeof(ClientTriggerTimer)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerAdapter), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerAdapter), typeof(ClientTriggerAdapter)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerCounter), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerCounter), typeof(ClientTriggerCounter)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MultiTriggerAdapter), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerMultiTriggerAdapter), typeof(ClientMultiTriggerAdapter)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerOnObject), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerOnObject), typeof(ClientTriggerOnObject)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerDisableScript), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerDisableScript), typeof(ClientTriggerDisableScript)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerDestroy), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerDestroy), typeof(ClientTriggerDestroy)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerQueue), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerQueue), typeof(ClientTriggerQueue)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerAnimationQueue), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerAnimationQueue), typeof(ClientTriggerAnimationQueue)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerAnimationCoordinator), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTriggerAnimationCoordinator), typeof(ClientTriggerAnimationCoordinator)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerConveyorAdjacentUpdate), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerConveyorAdjacentUpdate), typeof(ClientTriggerConveyorAdjacentUpdate)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerAnimatorSetVariable), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerAnimatorSetVariable), typeof(ClientTriggerAnimatorSetVariable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerOnAnimator), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerOnAnimator), typeof(ClientTriggerOnAnimator)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerToggleOnAnimator), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerToggleOnAnimator), typeof(ClientTriggerToggleOnAnimator)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerMoveSpawnPoints), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerMoveSpawnPoints), typeof(ClientTriggerMoveSpawnPoints)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerKillAttachments), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerKillAttachments), typeof(ClientTriggerKillAttachments)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerDialogue), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerDialogue), typeof(ClientTriggerDialogue)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CollisionTrigger), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerCollisionTrigger), typeof(ClientCollisionTrigger)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(SwitchCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerSwitchCosmeticDecisions), typeof(ClientSwitchCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PressureSwitchCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPressureSwitchCosmeticDecisions), typeof(ClientPressureSwitchCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ToggleSwitchCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerToggleSwitchCosmeticDecisions), typeof(ClientToggleSwitchCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(UsableItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerUsableItem), typeof(ClientUsableItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Interactable), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerInteractable), typeof(ClientInteractable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(SwitchStation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerSwitchStation), typeof(ClientSwitchStation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IngredientMeshVisibility), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIngredientMeshVisibility), typeof(ClientIngredientMeshVisibility)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HeldItemsMeshVisibility), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHeldItemsMeshVisibility), typeof(ClientHeldItemsMeshVisibility)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HeldItemMeshVisibility), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHeldItemMeshVisibility), typeof(ClientHeldItemsMeshVisibility)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HatMeshVisibility), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHatMeshVisibility), typeof(ClientHatMeshVisibility)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TailMeshVisibility), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTailMeshVisibility), typeof(ClientTailMeshVisibility)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(RespawnCollider), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerRespawnCollider), typeof(ClientRespawnCollider)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlayerRespawnBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlayerRespawnBehaviour), typeof(ClientPlayerRespawnBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DirtyPlateStackUtensilRespawnBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDirtyPlateStackUtensilRespawnBehaviour), typeof(ClientUtensilRespawnBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CleanPlateUtensilRespawnBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCleanPlateUtensilRespawnBehaviour), typeof(ClientUtensilRespawnBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CookingUtensilRespawnBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCookingUtensilRespawnBehaviour), typeof(ClientUtensilRespawnBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(UtensilRespawnBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerUtensilRespawnBehaviour), typeof(ClientUtensilRespawnBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AutoDestructEmptyPlateStack), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAutoDestructEmptyPlateStack), typeof(ClientAutoDestructEmptyPlateStack)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(EmoteWheel), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerEmoteWheel), typeof(ClientEmoteWheel)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlayerControlsImpl_Default), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlayerControlsImpl_Default), typeof(ClientPlayerControlsImpl_Default)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PreventPlacement), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPreventPlacement), typeof(ClientPreventPlacement)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(global::Stack), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerStack), typeof(ClientStack)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CarryableItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCarryableItem), typeof(ClientCarryableItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(ThrowableItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerThrowableItem), typeof(ClientThrowableItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CatchableItem), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCatchableItem), typeof(ClientCatchableItem)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachmentThrower), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentThrower), typeof(ClientAttachmentThrower)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachmentCatcher), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentCatcher), typeof(ClientAttachmentCatcher)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(IngredientCatcher), new SynchroniserConfig[]
		{
			new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerIngredientCatcher), typeof(ClientIngredientCatcher)),
			new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentCatchingProxy), typeof(ClientAttachmentCatchingProxy))
		});
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachmentCatchingProxy), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachmentCatchingProxy), typeof(ClientAttachmentCatchingProxy)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CutsceneController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCutsceneController), typeof(ClientCutsceneController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DialogueController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDialogueController), typeof(ClientDialogueController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TutorialPopupController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTutorialPopupController), typeof(ClientTutorialPopupController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(StoryOnionKingCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerStoryOnionKingCosmeticDecisions), typeof(ClientStoryOnionKingCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(StoryKevinCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerStoryKevinCosmeticDecisions), typeof(ClientStoryKevinCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MultiLevelMiniPortalMapNode), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMultiLevelMiniPortalMapNode), typeof(ClientMultiLevelMiniPortalMapNode)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MiniLevelPortalMapNode), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerMiniLevelPortalMapNode), typeof(ClientMiniLevelPortalMapNode)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(LevelPortalMapNode), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerLevelPortalMapNode), typeof(ClientLevelPortalMapNode)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(SwitchMapNode), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerSwitchMapNode), typeof(ClientSwitchMapNode)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WorldMapSwitch), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldMapSwitch), typeof(ClientWorldMapSwitch)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WorldMapTeleportal), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldMapTeleportal), typeof(ClientWorldMapTeleportal)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportableMapAvatar), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportableMapAvatar), typeof(ClientTeleportableMapAvatar)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalMapAvatarSender), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalMapAvatarSender), typeof(ClientTeleportalMapAvatarSender)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TeleportalMapAvatarReceiver), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTeleportalMapAvatarReceiver), typeof(ClientTeleportalMapAvatarReceiver)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(WorldMapInfoPopup), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerWorldMapInfoPopup), typeof(ClientWorldMapInfoPopup)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BackpackRespawnBehaviour), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBackpackRespawnBehaviour), typeof(ClientBackpackRespawnBehaviour)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BackpackCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBackpackCosmeticDecisions), typeof(ClientBackpackCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AutoWorkstation), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAutoWorkstation), typeof(ClientAutoWorkstation)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FurnaceCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFurnaceCosmeticDecisions), typeof(ClientFurnaceCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(FurnaceOvenCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerFurnaceOvenCosmeticDecisions), typeof(ClientFurnaceOvenCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CoalScuttleCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCoalScuttleCosmeticDecisions), typeof(ClientCoalScuttleCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeEnemy), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeEnemy), typeof(ClientHordeEnemy)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeEnemyCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeEnemyCosmeticDecisions), typeof(ClientHordeEnemyCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeTarget), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeTarget), typeof(ClientHordeTarget)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeTargetCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeTargetCosmeticDecisions), typeof(ClientHordeTargetCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeLockable), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeLockable), typeof(ClientHordeLockable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeLockableCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeLockableCosmeticDecisions), typeof(ClientHordeLockableCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(HordeFlowController), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerHordeFlowController), typeof(ClientHordeFlowController)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerOnAttach), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTriggerOnAttach), typeof(ClientTriggerOnAttach)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(AttachItemSpawner), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerAttachItemSpawner), typeof(ClientAttachItemSpawner)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PickupItemSwitcher), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPickupItemSwitcher), typeof(ClientPickupItemSwitcher)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlacementItemSwitcher), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlacementItemSwitcher), typeof(ClientPlacementItemSwitcher)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(DrinksMachineCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerDrinksMachineCosmeticDecisions), typeof(ClientDrinksMachineCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CondimentDispenserCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCondimentDispenserCosmeticDecisions), typeof(ClientCondimentDispenserCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CannonSessionInteractable), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCannonSessionInteractable), typeof(ClientCannonSessionInteractable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(Cannon), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCannon), typeof(ClientCannon)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CannonPlayerHandler), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCannonPlayerHandler), typeof(ClientCannonPlayerHandler)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TrayCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTrayCosmeticDecisions), typeof(ClientTrayCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(PlacementInteractable), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerPlacementInteractable), typeof(ClientPlacementInteractable)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BodyMeshVisibility), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerBodyMeshVisibility), typeof(ClientBodyMeshVisibility)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(CannonCosmeticDecisions), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerCannonCosmeticDecisions), typeof(ClientCannonCosmeticDecisions)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerColourCycle), new SynchroniserConfig(InstancesPerGameObject.Single, typeof(ServerTriggerColourCycle), typeof(ClientTriggerColourCycle)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(MultiTriggerDisableScript), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerMultiTriggerDisableScript), typeof(ClientMultiTriggerDisableScript)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(TriggerWithCooldown), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerTriggerWithCooldown), typeof(ClientTriggerWithCooldown)));
		this.m_EntitySerialiser.AddSynchronisedType(typeof(BoundContainer), new SynchroniserConfig(InstancesPerGameObject.Multiple, typeof(ServerBoundContainerBehaviour), typeof(ClientBoundContainerBehaviour)));
		if (this.IsServer())
		{
			base.gameObject.AddComponent<ServerPlayerRespawnManager>();
			this.m_ServerSync.Initialise();
		}
		IEnumerator setupRoutine = this.m_EntitySerialiser.SetupSynchronisation(this);
		while (setupRoutine.MoveNext())
		{
			yield return null;
		}
		if (this.IsServer())
		{
			ServerChefSynchroniser[] array = GameObjectUtils.FindComponentsOfTypeInScene<ServerChefSynchroniser>();
			for (int k = 0; k < array.Length; k++)
			{
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(array[k].gameObject);
				this.m_ServerSync.AddToFastList(entry);
			}
		}
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x000CEED8 File Offset: 0x000CD2D8
	public void StartSynchronisation()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.EntitySynchronisation, new UnorderedMessageReceivedCallback(this.m_ClientSync.OnEntitySynchronisationMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.EntityEvent, new OrderedMessageReceivedCallback(this.m_ClientSync.OnEntityEventMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.SpawnEntity, new OrderedMessageReceivedCallback(this.m_ClientSync.OnSpawnEntityMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.DestroyEntity, new OrderedMessageReceivedCallback(this.m_ClientSync.OnDestroyEntityMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.SpawnPhysicalAttachment, new OrderedMessageReceivedCallback(this.m_ClientSync.OnSpawnPhysicalAttachmentMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.m_ClientSync.OnDestroyChefMessageReceived));
		Mailbox.Client.RegisterForMessageType(MessageType.DestroyEntities, new OrderedMessageReceivedCallback(this.m_ClientSync.OnDestroyEntitiesMessageReceived));
		this.m_EntitySerialiser.StartSynchronisation();
		if (this.m_ServerSync != null && this.IsServer())
		{
			this.m_ServerSync.StartSynchronising();
		}
		MultiplayerController.m_bSyncActive = true;
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x000CEFDE File Offset: 0x000CD3DE
	public static bool IsSynchronisationActive()
	{
		return MultiplayerController.m_bSyncActive;
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x000CEFE8 File Offset: 0x000CD3E8
	public void StopSynchronisation()
	{
		MultiplayerController.m_bSyncActive = false;
		this.m_EntitySerialiser.StopSynchronisation();
		this.m_ServerSync.StopSynchronising();
		this.m_EntitySerialiser.Clear();
		this.m_scanCoroutine = null;
		this.TryRemoveComponent<ServerPlayerRespawnManager>();
		this.m_LocalServer.GetUserSystem().InvalidateEntities();
		this.m_ClientSync.CleanUp();
		Mailbox.Client.UnregisterForMessageType(MessageType.EntityEvent, new OrderedMessageReceivedCallback(this.m_ClientSync.OnEntityEventMessageReceived));
		Mailbox.Client.UnregisterForMessageType(MessageType.EntitySynchronisation, new UnorderedMessageReceivedCallback(this.m_ClientSync.OnEntitySynchronisationMessageReceived));
		Mailbox.Client.UnregisterForMessageType(MessageType.SpawnEntity, new OrderedMessageReceivedCallback(this.m_ClientSync.OnSpawnEntityMessageReceived));
		Mailbox.Client.UnregisterForMessageType(MessageType.DestroyEntity, new OrderedMessageReceivedCallback(this.m_ClientSync.OnDestroyEntityMessageReceived));
		Mailbox.Client.UnregisterForMessageType(MessageType.SpawnPhysicalAttachment, new OrderedMessageReceivedCallback(this.m_ClientSync.OnSpawnPhysicalAttachmentMessageReceived));
		Mailbox.Client.UnregisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.m_ClientSync.OnDestroyChefMessageReceived));
		Mailbox.Client.UnregisterForMessageType(MessageType.DestroyEntities, new OrderedMessageReceivedCallback(this.m_ClientSync.OnDestroyEntitiesMessageReceived));
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x000CF10B File Offset: 0x000CD50B
	public ConnectionStats GetClientConnectionStats(bool bReliable)
	{
		return this.m_LocalClient.GetConnectionStats(bReliable);
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000CF119 File Offset: 0x000CD519
	public FastList<ConnectionStats> GetServerConnectionStats(bool bReliable)
	{
		return this.m_LocalServer.GetAllConnectionStats(bReliable);
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000CF128 File Offset: 0x000CD528
	private void TryRemoveComponent<T>() where T : Component
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		Component component = base.gameObject.GetComponent<T>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000CF178 File Offset: 0x000CD578
	public void Update()
	{
		this.m_InviteMonitor.Update();
		ConnectionModeSwitcher.Update();
		ClientUserSystem.Update();
		if (this.IsServer())
		{
			ServerTime.Update();
			this.m_LocalServer.Update();
		}
		this.m_LocalClient.Update();
		if (MultiplayerController.m_bSyncActive)
		{
			FastList<EntitySerialisationEntry> entitiesList = EntitySerialisationRegistry.m_EntitiesList;
			int count = entitiesList.Count;
			int num = 0;
			while (num < count && MultiplayerController.m_bSyncActive)
			{
				EntitySerialisationEntry entitySerialisationEntry = entitiesList._items[num];
				FastList<ServerSynchroniser> serverSynchronisedComponents = entitySerialisationEntry.m_ServerSynchronisedComponents;
				int count2 = serverSynchronisedComponents.Count;
				int num2 = 0;
				while (num2 < count2 && MultiplayerController.m_bSyncActive)
				{
					ServerSynchroniser serverSynchroniser = serverSynchronisedComponents._items[num2];
					if (serverSynchroniser.IsSynchronising())
					{
						serverSynchronisedComponents._items[num2].UpdateSynchronising();
					}
					num2++;
				}
				if (MultiplayerController.m_bSyncActive)
				{
					FastList<ClientSynchroniser> clientSynchronisedComponents = entitySerialisationEntry.m_ClientSynchronisedComponents;
					int count3 = clientSynchronisedComponents.Count;
					int num3 = 0;
					while (num3 < count3 && MultiplayerController.m_bSyncActive)
					{
						ClientSynchroniser clientSynchroniser = clientSynchronisedComponents._items[num3];
						if (clientSynchroniser.IsSynchronising())
						{
							clientSynchronisedComponents._items[num3].UpdateSynchronising();
						}
						num3++;
					}
				}
				num++;
			}
			if (this.IsServer() && MultiplayerController.m_bSyncActive)
			{
				this.m_ServerSync.Update();
			}
			this.m_ClientSync.Update();
		}
		ServerMessenger.DeferredLevelLoad();
		if (this.m_scanCoroutine != null && !this.m_scanCoroutine.MoveNext())
		{
			this.m_scanCoroutine = null;
		}
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x000CF30A File Offset: 0x000CD70A
	public void LateUpdate()
	{
		if (this.IsServer())
		{
			this.m_LocalServer.Dispatch();
		}
		this.m_LocalClient.Dispatch();
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x000CF32D File Offset: 0x000CD72D
	public void SetTracker(NetworkMessageTracker tracker)
	{
		this.m_LocalServer.SetTracker(tracker);
		this.m_LocalClient.SetTracker(tracker);
		this.m_ServerSync.SetTracker(tracker);
		this.m_ClientSync.SetTracker(tracker);
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x000CF360 File Offset: 0x000CD760
	public void SwitchNodeType(MultiplayerController.NodeType type)
	{
		if (type != this.m_NodeType)
		{
			this.m_NodeType = type;
			if (this.m_NodeType == MultiplayerController.NodeType.Server)
			{
				this.m_LocalServer.GetUserSystem().Initialise();
			}
			else
			{
				this.m_LocalServer.GetUserSystem().Shutdown();
			}
		}
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000CF3B1 File Offset: 0x000CD7B1
	private bool IsServer()
	{
		return this.m_NodeType == MultiplayerController.NodeType.Server;
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000CF3BC File Offset: 0x000CD7BC
	public void SetLatencyMeasurePaused(bool bPaused)
	{
		if (this.m_LocalServer != null)
		{
			this.m_LocalServer.SetLatencyTestPaused(bPaused);
		}
		if (this.m_LocalClient != null)
		{
			this.m_LocalClient.SetLatencyTestPaused(bPaused);
		}
	}

	// Token: 0x040023A2 RID: 9122
	public const int kBitsPerEntityID = 10;

	// Token: 0x040023A3 RID: 9123
	private Server m_LocalServer = new Server();

	// Token: 0x040023A4 RID: 9124
	private Client m_LocalClient = new Client();

	// Token: 0x040023A5 RID: 9125
	private ServerSynchronisationScheduler m_ServerSync = new ServerSynchronisationScheduler();

	// Token: 0x040023A6 RID: 9126
	private ClientSynchronisationReceiver m_ClientSync = new ClientSynchronisationReceiver();

	// Token: 0x040023A7 RID: 9127
	private EntitySerialisationRegistry m_EntitySerialiser = new EntitySerialisationRegistry();

	// Token: 0x040023A8 RID: 9128
	private InviteMonitor m_InviteMonitor = new InviteMonitor();

	// Token: 0x040023A9 RID: 9129
	private MultiplayerController.NodeType m_NodeType;

	// Token: 0x040023AA RID: 9130
	private ConnectionStatus m_ConnectionStatus = new ConnectionStatus();

	// Token: 0x040023AB RID: 9131
	private DisconnectionHandler m_DisconnectionHandler = new DisconnectionHandler();

	// Token: 0x040023AC RID: 9132
	private static bool m_bSyncActive;

	// Token: 0x040023AD RID: 9133
	private IEnumerator m_scanCoroutine;

	// Token: 0x040023AE RID: 9134
	public NetworkPredictionTweekables m_NetworkPredictionTweekables;

	// Token: 0x040023AF RID: 9135
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache0;

	// Token: 0x040023B0 RID: 9136
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache1;

	// Token: 0x040023B1 RID: 9137
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache2;

	// Token: 0x040023B2 RID: 9138
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache3;

	// Token: 0x040023B3 RID: 9139
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache4;

	// Token: 0x040023B4 RID: 9140
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache5;

	// Token: 0x040023B5 RID: 9141
	[CompilerGenerated]
	private static GenericVoid <>f__mg$cache6;

	// Token: 0x040023B6 RID: 9142
	[CompilerGenerated]
	private static GenericVoid <>f__mg$cache7;

	// Token: 0x040023B7 RID: 9143
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache8;

	// Token: 0x040023B8 RID: 9144
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache9;

	// Token: 0x040023B9 RID: 9145
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cacheA;

	// Token: 0x040023BA RID: 9146
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cacheB;

	// Token: 0x040023BB RID: 9147
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cacheC;

	// Token: 0x040023BC RID: 9148
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cacheD;

	// Token: 0x020008EB RID: 2283
	public enum NodeType
	{
		// Token: 0x040023BE RID: 9150
		Uninitialised,
		// Token: 0x040023BF RID: 9151
		Server,
		// Token: 0x040023C0 RID: 9152
		Client
	}
}
