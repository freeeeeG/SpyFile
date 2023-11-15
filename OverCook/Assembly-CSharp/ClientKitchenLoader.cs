using System;
using System.Collections.Generic;
using System.Diagnostics;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200088E RID: 2190
public class ClientKitchenLoader : MonoBehaviour
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06002A92 RID: 10898 RVA: 0x000C75AC File Offset: 0x000C59AC
	// (remove) Token: 0x06002A93 RID: 10899 RVA: 0x000C75E0 File Offset: 0x000C59E0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event GenericVoid OnChefsSetupComplete;

	// Token: 0x06002A94 RID: 10900 RVA: 0x000C7614 File Offset: 0x000C5A14
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		Mailbox.Client.RegisterForMessageType(MessageType.ChefOwnership, new OrderedMessageReceivedCallback(this.AssignChefOwnershipFromNetwork));
		this.m_NetworkErrorDialog.Enable(new T17DialogBox.DialogEvent(this.OnNetworkDisconnectionConfirmed));
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000C7667 File Offset: 0x000C5A67
	private void Start()
	{
		this.m_PlayerSwitchingManager = GameUtils.RequireManager<PlayerSwitchingManager>();
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x000C7674 File Offset: 0x000C5A74
	private void Update()
	{
		GameState gameState = this.m_GameState;
		if (gameState != GameState.ScanNetworkEntities)
		{
			if (gameState == GameState.StartEntities)
			{
				if (this.CheckStarted())
				{
					this.StartEntities();
				}
				else
				{
					this.m_fSynchroniserStartupTimer += Time.deltaTime;
					if (this.m_fSynchroniserStartupTimer >= 10f)
					{
						this.StartEntities();
					}
				}
			}
		}
		else if (this.CheckScanned())
		{
			this.ScannedEntities();
		}
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000C76FA File Offset: 0x000C5AFA
	private void ScannedEntities()
	{
		UserSystemUtils.BuildGameInputConfig();
		ClientMessenger.GameState(GameState.ScannedNetworkEntities);
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000C7707 File Offset: 0x000C5B07
	private bool CheckScanned()
	{
		return !this.m_Controller.ScanActive && !ComponentCacheRegistry.ScanActive;
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000C7726 File Offset: 0x000C5B26
	private void StartEntities()
	{
		ClientMessenger.GameState(GameState.StartedEntities);
		this.m_GameState = GameState.NotSet;
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x000C7738 File Offset: 0x000C5B38
	private bool CheckStarted()
	{
		bool flag = true;
		FastList<EntitySerialisationEntry> entitiesList = EntitySerialisationRegistry.m_EntitiesList;
		int num = 0;
		while (flag && num < entitiesList.Count)
		{
			EntitySerialisationEntry entitySerialisationEntry = entitiesList._items[num];
			int num2 = 0;
			while (flag && num2 < entitySerialisationEntry.m_ClientSynchronisedComponents.Count)
			{
				ClientSynchroniser clientSynchroniser = entitySerialisationEntry.m_ClientSynchronisedComponents._items[num2];
				if (clientSynchroniser.GetEntityType() == EntityType.WorldObject || clientSynchroniser.GetEntityType() == EntityType.Chef)
				{
					ClientWorldObjectSynchroniser clientWorldObjectSynchroniser = clientSynchroniser as ClientWorldObjectSynchroniser;
					if (null != clientWorldObjectSynchroniser && !clientWorldObjectSynchroniser.m_bHasEverReceived)
					{
						flag = false;
						break;
					}
				}
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x000C77ED File Offset: 0x000C5BED
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		Mailbox.Client.UnregisterForMessageType(MessageType.ChefOwnership, new OrderedMessageReceivedCallback(this.AssignChefOwnershipFromNetwork));
		this.m_NetworkErrorDialog.Disable();
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x000C7829 File Offset: 0x000C5C29
	public void Initialise(Client client, MultiplayerController controller)
	{
		this.m_Client = client;
		this.m_Controller = controller;
		ClientMessenger.GameState(GameState.LoadedKitchen);
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000C7840 File Offset: 0x000C5C40
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		this.m_GameState = gameStateMessage.m_State;
		GameState state = gameStateMessage.m_State;
		switch (state)
		{
		case GameState.ScanNetworkEntities:
			this.m_Controller.ScanEntities(delegate
			{
				ComponentCacheRegistry.ScanForInitialObjects(null);
			});
			if (this.CheckScanned())
			{
				this.ScannedEntities();
			}
			break;
		default:
			switch (state)
			{
			case GameState.StartEntities:
				if (this.CheckStarted())
				{
					this.StartEntities();
				}
				break;
			case GameState.RunLevelIntro:
			{
				MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
				multiplayerController.SetLatencyMeasurePaused(false);
				if (!LoadingScreenFlow.IsLoadingStartScreen())
				{
					InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.Gameplay);
				}
				break;
			}
			}
			break;
		case GameState.StartSynchronising:
			this.m_Controller.StartSynchronisation();
			this.m_fSynchroniserStartupTimer = 0f;
			ClientMessenger.GameState(GameState.StartedSyncronising);
			break;
		}
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x000C7931 File Offset: 0x000C5D31
	private void OnNetworkDisconnectionConfirmed()
	{
		ServerGameSetup.Mode = GameMode.OnlineKitchen;
		LoadingScreenFlow.LoadScene("StartScreen", GameState.NotSet);
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000C7944 File Offset: 0x000C5D44
	private void AssignChefOwnershipFromNetwork(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		this.m_Client.GetUserSystem().OnUsersChanged(sessionUserId, message);
		int num = 0;
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user = ClientUserSystem.m_Users._items[i];
			if (user.IsLocal)
			{
				PlayerInputLookup.Player currentPlayer = (PlayerInputLookup.Player)num;
				ClientKitchenLoader.SetupMyChef(user.EntityID, currentPlayer);
				ClientKitchenLoader.SetupMyChef(user.Entity2ID, currentPlayer);
				num++;
			}
			else
			{
				ClientKitchenLoader.SetupOtherChef(user.EntityID, user.Machine, PlayerInputLookup.Player.Count);
				ClientKitchenLoader.SetupOtherChef(user.Entity2ID, user.Machine, PlayerInputLookup.Player.Count);
			}
		}
		if (ClientKitchenLoader.OnChefsSetupComplete != null)
		{
			ClientKitchenLoader.OnChefsSetupComplete();
		}
		this.m_PlayerSwitchingManager.InitialiseAvatars();
		FastList<GameObject> fastList = new FastList<GameObject>();
		int count = ClientUserSystem.m_Users.Count;
		for (int j = 0; j < count; j++)
		{
			User user2 = ClientUserSystem.m_Users._items[j];
			GameObject gameObject = this.ReplaceMesh(user2.EntityID, user2.SelectedChefData);
			if (null != gameObject)
			{
				fastList.Add(gameObject);
			}
			gameObject = this.ReplaceMesh(user2.Entity2ID, user2.SelectedChefData);
			if (null != gameObject)
			{
				fastList.Add(gameObject);
			}
		}
		FastList<GameObject> fastList2 = new FastList<GameObject>();
		count = PlayerIDProvider.s_AllProviders.Count;
		for (int k = 0; k < count; k++)
		{
			GameObject gameObject2 = PlayerIDProvider.s_AllProviders._items[k].gameObject;
			if (!fastList.Contains(gameObject2))
			{
				fastList2.Add(gameObject2);
			}
		}
		count = fastList2.Count;
		for (int l = 0; l < count; l++)
		{
			GameObject gameObject3 = fastList2._items[l];
			EntitySerialisationRegistry.UnregisterObject(gameObject3);
			UnityEngine.Object.Destroy(gameObject3);
		}
		fastList2.Clear();
		count = PlayerIDProvider.s_AllProviders.Count;
		for (int m = 0; m < count; m++)
		{
			PlayerIDProvider playerIDProvider = PlayerIDProvider.s_AllProviders._items[m];
			ClientInputTransmitter clientInputTransmitter = playerIDProvider.gameObject.RequireComponent<ClientInputTransmitter>();
			clientInputTransmitter.Setup();
		}
		ClientMessenger.GameState(GameState.AssignedChefsToUsers);
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000C7B78 File Offset: 0x000C5F78
	private GameObject ReplaceMesh(uint uEntityID, GameSession.SelectedChefData selectedChef)
	{
		if (selectedChef != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(uEntityID);
			if (entry != null && null != entry.m_GameObject)
			{
				ChefMeshReplacer component = entry.m_GameObject.GetComponent<ChefMeshReplacer>();
				if (null != component)
				{
					component.SetChefData(selectedChef, true);
					return component.gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000C7BD4 File Offset: 0x000C5FD4
	private static void SetupMyChef(uint entityID, PlayerInputLookup.Player currentPlayer)
	{
		if (entityID != 0U)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(entityID);
			if (entry != null)
			{
				GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
				PlayerIDProvider component = entry.m_GameObject.GetComponent<PlayerIDProvider>();
				GameInputConfig.ConfigEntry configEntry = Array.Find<GameInputConfig.ConfigEntry>(baseInputConfig.m_playerConfigs, (GameInputConfig.ConfigEntry x) => x.Player == currentPlayer);
				if (configEntry != null)
				{
					configEntry.MachineId = ClientUserSystem.s_LocalMachineId;
				}
				component.OverridePlayerId(currentPlayer);
			}
		}
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000C7C4C File Offset: 0x000C604C
	private static void SetupOtherChef(uint entityID, User.MachineID machineID, PlayerInputLookup.Player currentPlayer)
	{
		if (entityID != 0U)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(entityID);
			if (entry != null)
			{
				GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
				PlayerIDProvider component = entry.m_GameObject.GetComponent<PlayerIDProvider>();
				GameInputConfig.ConfigEntry configEntry = Array.Find<GameInputConfig.ConfigEntry>(baseInputConfig.m_playerConfigs, (GameInputConfig.ConfigEntry x) => x.Player == currentPlayer);
				if (configEntry != null)
				{
					configEntry.MachineId = machineID;
				}
				component.OverridePlayerId(PlayerInputLookup.Player.Count);
			}
		}
	}

	// Token: 0x0400218C RID: 8588
	private MultiplayerController m_Controller;

	// Token: 0x0400218D RID: 8589
	private Client m_Client;

	// Token: 0x0400218E RID: 8590
	private PlayerSwitchingManager m_PlayerSwitchingManager;

	// Token: 0x0400218F RID: 8591
	private NetworkErrorDialog m_NetworkErrorDialog = new NetworkErrorDialog();

	// Token: 0x04002190 RID: 8592
	private GameState m_GameState;

	// Token: 0x04002191 RID: 8593
	private const float m_fSynchroniserStartupTimerDuration = 10f;

	// Token: 0x04002192 RID: 8594
	private float m_fSynchroniserStartupTimer;
}
