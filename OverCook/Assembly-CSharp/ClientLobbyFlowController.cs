using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
public class ClientLobbyFlowController : MonoBehaviour
{
	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06002161 RID: 8545 RVA: 0x000A065F File Offset: 0x0009EA5F
	public static ClientLobbyFlowController Instance
	{
		get
		{
			return ClientLobbyFlowController.s_instance;
		}
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000A0668 File Offset: 0x0009EA68
	public void Awake()
	{
		if (ClientLobbyFlowController.s_instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			ClientLobbyFlowController.s_instance = this;
		}
		this.m_lobbyFlow = LobbyFlowController.Instance;
		this.m_lobbyInfo = LobbySetupInfo.Instance;
		if (this.m_lobbyInfo == null)
		{
			GameObject gameObject = new GameObject("LobbySetupInfo");
			this.m_lobbyInfo = gameObject.AddComponent<LobbySetupInfo>();
			this.m_lobbyInfo.m_visiblity = OnlineMultiplayerSessionVisibility.eMatchmaking;
			this.m_lobbyInfo.m_gameType = GameSession.GameType.Cooperative;
		}
		this.m_lobbyFlow.m_themeSelectMenu.Hide(true, false);
		this.m_lobbyFlow.m_chosenThemes.SetActive(false);
		this.m_lobbyFlow.m_timerText.enabled = false;
		this.m_lobbyFlow.m_themeSelectMenu.CarouselButtonClicked += this.OnThemeButtonClicked;
		this.UpdateRequirementNotifications();
		this.m_userChoices = new LobbyFlowController.ThemeChoice[OnlineMultiplayerConfig.MaxPlayers];
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		this.m_dlcManager = GameUtils.RequireManager<DLCManager>();
		this.m_NetworkErrorDialog.Enable(new T17DialogBox.DialogEvent(this.OnNetworkErrorDismissed));
		Mailbox.Client.RegisterForMessageType(MessageType.LobbyServer, new OrderedMessageReceivedCallback(this.OnLobbyServerMessage));
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000A07D0 File Offset: 0x0009EBD0
	private void OnEngagementChanged(EngagementSlot slot, GamepadUser oldUser, GamepadUser newUser)
	{
		if (oldUser == null && newUser != null)
		{
			this.SetupUserInput();
		}
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000A07F0 File Offset: 0x0009EBF0
	private void Start()
	{
		this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.None);
		this.SetupUserInput();
		if (ClientGameSetup.PrevScene != "StartScreen")
		{
			SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
			saveManager.SaveMetaProgress(new SaveSystemCallback(this.OnMetaSaveStatus));
		}
		if (this.m_lobbyInfo != null)
		{
			this.m_bIsCoop = (this.m_lobbyInfo.m_gameType == GameSession.GameType.Cooperative);
			this.m_lobbyFlow.m_lobbyNames.Update(this.m_bIsCoop);
			this.m_lobbyFlow.UpdateLegend(this.m_bIsCoop);
			if (!string.IsNullOrEmpty(NetworkErrors.CachedErrorMessage))
			{
				T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
				if (null != dialog)
				{
					dialog.Initialize(NetworkErrors.CachedErrorTitle, NetworkErrors.CachedErrorMessage, "Text.Button.Confirm", null, null, T17DialogBox.Symbols.Warning, true, true, false);
					dialog.Show();
					NetworkErrors.CachedErrorMessage = null;
				}
			}
			if (this.m_lobbyInfo.m_connectionMode != OnlineMultiplayerConnectionMode.eNone && this.m_lobbyInfo.m_originalConnectionState != NetConnectionState.Offline && ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline)
			{
				return;
			}
			if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
			{
				if (this.m_lobbyInfo.m_connectionMode == OnlineMultiplayerConnectionMode.eNone)
				{
					if (base.gameObject.GetComponent<ServerLobbyFlowController>() == null)
					{
						base.gameObject.AddComponent<ServerLobbyFlowController>();
					}
				}
				else if (ClientUserSystem.m_Users.Count > 1)
				{
					this.HostGame();
				}
				else
				{
					this.TryJoinGame();
				}
			}
			else if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
			{
				this.SetState(LobbyFlowController.LobbyState.OnlineSetup);
				this.m_message.m_type = LobbyClientMessage.LobbyMessageType.StateRequest;
				ClientMessenger.LobbyMessage(this.m_message);
				this.UpdateUIColours();
			}
		}
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000A09A9 File Offset: 0x0009EDA9
	private void OnMetaSaveStatus(SaveSystemStatus _status)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (_status.Result != SaveLoadResult.Exists)
		{
			this.Leave();
		}
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000A09DC File Offset: 0x0009EDDC
	protected void OnDestroy()
	{
		if (ClientLobbyFlowController.s_instance == this)
		{
			ClientLobbyFlowController.s_instance = null;
		}
		base.StopAllCoroutines();
		this.m_NetworkErrorDialog.Disable();
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestConnectionStateJoinComplete));
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestConnectionStateServerComplete));
		Mailbox.Client.UnregisterForMessageType(MessageType.LobbyServer, new OrderedMessageReceivedCallback(this.OnLobbyServerMessage));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		this.HideLeaveDialog();
		this.m_lobbyFlow.m_themeSelectMenu.CarouselButtonClicked -= this.OnThemeButtonClicked;
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000A0AA4 File Offset: 0x0009EEA4
	private void OnThemeButtonClicked(CarouselButton _button)
	{
		ThemeSelectButton themeSelectButton = (ThemeSelectButton)_button;
		if (themeSelectButton != null)
		{
			DlcThemeSelectButton dlcThemeSelectButton = themeSelectButton as DlcThemeSelectButton;
			if (dlcThemeSelectButton != null && dlcThemeSelectButton.DLCData != null && !this.m_dlcManager.IsDLCAvailable(dlcThemeSelectButton.DLCData))
			{
				return;
			}
			this.SelectTheme(themeSelectButton.Theme);
		}
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000A0B0B File Offset: 0x0009EF0B
	protected void OnNetworkErrorDismissed()
	{
		ServerGameSetup.Mode = GameMode.OnlineKitchen;
		ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000A0B28 File Offset: 0x0009EF28
	protected void TryJoinGame()
	{
		this.SetState(LobbyFlowController.LobbyState.Matchmake);
		IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		IOnlineMultiplayerSessionCoordinator onlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		if (onlineMultiplayerSessionCoordinator != null)
		{
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Matchmake, new MatchmakeData
			{
				gameMode = ((!this.m_bIsCoop) ? GameMode.Versus : GameMode.Party),
				User = playerManager.GetUser(EngagementSlot.One),
				connectionMode = this.m_lobbyInfo.m_connectionMode
			}, new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestConnectionStateJoinComplete));
		}
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x000A0BB4 File Offset: 0x0009EFB4
	private void OnRequestConnectionStateJoinComplete(IConnectionModeSwitchStatus status)
	{
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			GameUtils.SendDiagnosticEvent("Automatchmake:Success");
			if (ConnectionStatus.IsHost())
			{
				this.HostGame();
			}
			else
			{
				this.SetState(LobbyFlowController.LobbyState.OnlineSetup);
				this.m_message.m_type = LobbyClientMessage.LobbyMessageType.StateRequest;
				ClientMessenger.LobbyMessage(this.m_message);
				this.m_lobbyFlow.RefreshUserColours(this.m_bIsCoop);
				this.UpdateUIColours();
			}
		}
		else if (status.DisplayPlatformDialog())
		{
			GameUtils.SendDiagnosticEvent("Automatchmake:Failure:PlatformError");
			this.Leave();
		}
		else
		{
			CompositeStatus compositeStatus = status as CompositeStatus;
			JoinSessionStatus joinSessionStatus = compositeStatus.m_TaskSubStatus as JoinSessionStatus;
			if (joinSessionStatus == null)
			{
				joinSessionStatus = (compositeStatus.m_TaskSubStatus as AutoMatchmakingStatus);
			}
			if (joinSessionStatus != null && (joinSessionStatus.sessionJoinResult.m_returnCode == OnlineMultiplayerSessionJoinResult.eLostNetwork || joinSessionStatus.sessionJoinResult.m_returnCode == OnlineMultiplayerSessionJoinResult.eApplicationSuspended || joinSessionStatus.sessionJoinResult.m_returnCode == OnlineMultiplayerSessionJoinResult.eGoneOffline || joinSessionStatus.sessionJoinResult.m_returnCode == OnlineMultiplayerSessionJoinResult.eLoggedOut))
			{
				switch (joinSessionStatus.sessionJoinResult.m_returnCode)
				{
				case OnlineMultiplayerSessionJoinResult.eLostNetwork:
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:eLostNetwork");
					break;
				case OnlineMultiplayerSessionJoinResult.eApplicationSuspended:
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:eApplicationSuspended");
					break;
				case OnlineMultiplayerSessionJoinResult.eGoneOffline:
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:eGoneOffline");
					break;
				case OnlineMultiplayerSessionJoinResult.eLoggedOut:
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:eLoggedOut");
					break;
				default:
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:Generic");
					break;
				}
				this.m_lastStatus = status.Clone();
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestOfflineStateFollowingFailureComplete));
			}
			else
			{
				if (joinSessionStatus != null)
				{
					switch (joinSessionStatus.sessionJoinResult.m_returnCode)
					{
					case OnlineMultiplayerSessionJoinResult.eClosed:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eClosed");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eFull:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eFull");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eNoLongerExists:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eNoLongerExists");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eNoHostConnection:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eNoHostConnection");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eLoggedOut:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eLoggedOut");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eCodeVersionMismatch:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eCodeVersionMismatch");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eGenericFailure:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eGenericFailure");
						goto IL_25F;
					case OnlineMultiplayerSessionJoinResult.eNotEnoughRoomForAllLocalUsers:
						GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_eNotEnoughRoomForAllLocalUsers");
						goto IL_25F;
					}
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_Unknown");
					IL_25F:;
				}
				else if (!GameUtils.s_RoomSearch_NoneAvailable)
				{
					GameUtils.SendDiagnosticEvent("Automatchmake:Failure:NonFatal_NotSpecified");
				}
				this.HostGame();
			}
		}
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000A0E40 File Offset: 0x0009F240
	protected void HostGame()
	{
		this.SetState(LobbyFlowController.LobbyState.Matchmake);
		IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		IOnlineMultiplayerSessionCoordinator onlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		if (onlineMultiplayerSessionCoordinator != null)
		{
			ServerOptions serverOptions = default(ServerOptions);
			serverOptions.gameMode = ((!this.m_bIsCoop) ? GameMode.Versus : GameMode.Party);
			if (this.m_lobbyInfo.m_visiblity == OnlineMultiplayerSessionVisibility.ePrivate)
			{
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.eClosed;
			}
			else
			{
				serverOptions.visibility = this.m_lobbyInfo.m_visiblity;
			}
			serverOptions.hostUser = playerManager.GetUser(EngagementSlot.One);
			serverOptions.connectionMode = this.m_lobbyInfo.m_connectionMode;
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestConnectionStateServerComplete));
		}
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000A0EFC File Offset: 0x0009F2FC
	private void OnRequestConnectionStateServerComplete(IConnectionModeSwitchStatus status)
	{
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			if (base.gameObject.GetComponent<ServerLobbyFlowController>() == null)
			{
				base.gameObject.AddComponent<ServerLobbyFlowController>();
			}
		}
		else
		{
			this.m_lastStatus = status.Clone();
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestOfflineStateFollowingFailureComplete));
		}
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000A0F5C File Offset: 0x0009F35C
	private void OnRequestOfflineStateFollowingFailureComplete(IConnectionModeSwitchStatus status)
	{
		NetworkErrorDialog.ShowDialog(this.m_lastStatus);
		this.m_lastStatus = null;
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000A0F70 File Offset: 0x0009F370
	protected void OnLobbyServerMessage(IOnlineMultiplayerSessionUserId _sender, Serialisable _data)
	{
		LobbyServerMessage lobbyServerMessage = (LobbyServerMessage)_data;
		if (lobbyServerMessage != null)
		{
			switch (lobbyServerMessage.m_type)
			{
			case LobbyServerMessage.LobbyMessageType.StateChange:
				this.m_bIsCoop = lobbyServerMessage.m_stateChange.m_bIsCoop;
				this.m_sessionVisibility = lobbyServerMessage.m_stateChange.m_sessionVisibility;
				this.m_connectionMode = lobbyServerMessage.m_stateChange.m_connectionMode;
				this.m_lobbyFlow.m_lobbyNames.Update(this.m_bIsCoop);
				if (this.m_bIsCoop)
				{
					this.m_lobbyInfo.m_gameType = GameSession.GameType.Cooperative;
				}
				else
				{
					this.m_lobbyInfo.m_gameType = GameSession.GameType.Competitive;
				}
				this.m_lobbyFlow.UpdateLegend(this.m_bIsCoop);
				this.SetState(lobbyServerMessage.m_stateChange.m_state);
				break;
			case LobbyServerMessage.LobbyMessageType.TimerUpdate:
				this.SetTimer(lobbyServerMessage.m_timerInfo.m_timerVal);
				break;
			case LobbyServerMessage.LobbyMessageType.ResetTimer:
				this.OnTimerReset(lobbyServerMessage.m_timerInfo.m_timerVal);
				break;
			case LobbyServerMessage.LobbyMessageType.SelectionUpdate:
				this.OnThemeSelected(lobbyServerMessage.m_selectionUpdate.m_theme, lobbyServerMessage.m_selectionUpdate.m_chefIndex);
				break;
			case LobbyServerMessage.LobbyMessageType.FinalSelection:
				Mailbox.Client.RegisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLoadLevel));
				Mailbox.Client.RegisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLoadLevel));
				base.StartCoroutine(this.AnimateThemeSelection(lobbyServerMessage.m_selectionUpdate.m_chefIndex));
				break;
			case LobbyServerMessage.LobbyMessageType.CreateGameSession:
				this.m_lobbyFlow.CreateLobbySession(this.m_lobbyInfo.m_gameType, lobbyServerMessage.m_dlcID);
				break;
			}
		}
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x000A110B File Offset: 0x0009F50B
	private void OnLoadLevel(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		UserSystemUtils.BuildGameInputConfig();
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLoadLevel));
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLoadLevel));
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000A1140 File Offset: 0x0009F540
	protected void OnUsersChanged()
	{
		this.UpdateRequirementNotifications();
		for (int i = 0; i < this.m_lobbyFlow.m_themeSelections.Length; i++)
		{
			User user = (i >= ClientUserSystem.m_Users.Count) ? null : ClientUserSystem.m_Users._items[i];
			if (user != null && (i == 0 || UserSystemUtils.AnyRemoteUsers()))
			{
				if (this.m_state != LobbyFlowController.LobbyState.OnlineThemeSelected && this.m_state != LobbyFlowController.LobbyState.LocalThemeSelected)
				{
					this.m_lobbyFlow.m_themeSelections[i].gameObject.SetActive(true);
				}
			}
			else
			{
				this.m_lobbyFlow.m_themeSelections[i].gameObject.SetActive(false);
			}
		}
		SceneDirectoryData.LevelTheme levelTheme = SceneDirectoryData.LevelTheme.Count;
		for (int j = 0; j < ClientUserSystem.m_Users.Count; j++)
		{
			if (this.m_userChoices[j] != null && ClientUserSystem.m_Users._items[j].IsLocal)
			{
				levelTheme = this.m_userChoices[j].m_theme;
			}
		}
		if (levelTheme != SceneDirectoryData.LevelTheme.Count)
		{
			for (int k = 0; k < ClientUserSystem.m_Users.Count; k++)
			{
				if (this.m_userChoices[k] != null && ClientUserSystem.m_Users._items[k].IsLocal)
				{
					this.m_message.m_type = LobbyClientMessage.LobbyMessageType.ThemeSelected;
					this.m_message.m_theme = levelTheme;
					this.m_message.m_chefIndex = k;
					ClientMessenger.LobbyMessage(this.m_message);
				}
			}
		}
		if (this.m_lobbyInfo.m_visiblity == OnlineMultiplayerSessionVisibility.ePrivate && this.m_lobbyFlow.m_uiPlayerRoot.UIPlayers.Count != ClientUserSystem.m_Users.Count)
		{
			this.m_lobbyFlow.m_uiPlayerRoot.ReCreateUIPlayers();
		}
		this.m_lobbyFlow.RefreshUserColours(this.m_bIsCoop);
		this.SetupUserInput();
		this.UpdateUIColours();
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x000A1324 File Offset: 0x0009F724
	private void UpdateUIColours()
	{
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user = ClientUserSystem.m_Users._items[i];
			UIPlayerMenuBehaviour uiplayerForUser = this.m_lobbyFlow.m_uiPlayerRoot.GetUIPlayerForUser(user);
			if (uiplayerForUser != null && user.SelectedChefData != null)
			{
				uiplayerForUser.UpdateColour();
			}
		}
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x000A1388 File Offset: 0x0009F788
	protected void SetState(LobbyFlowController.LobbyState _state)
	{
		this.m_lobbyFlow.m_lobbyNames.Update(this.m_bIsCoop);
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			this.m_lobbyInfo = LobbySetupInfo.Instance;
			bool flag = false;
			if (this.m_lobbyInfo == null)
			{
				GameObject gameObject = new GameObject("LobbySetupInfo");
				this.m_lobbyInfo = gameObject.AddComponent<LobbySetupInfo>();
				flag = true;
			}
			GameSession.GameType gameType = (!this.m_bIsCoop) ? GameSession.GameType.Competitive : GameSession.GameType.Cooperative;
			if (flag || this.m_lobbyInfo.m_gameType != gameType || this.m_lobbyInfo.m_visiblity != this.m_sessionVisibility || this.m_lobbyInfo.m_connectionMode != this.m_connectionMode)
			{
				this.m_lobbyInfo.m_gameType = gameType;
				this.m_lobbyInfo.m_visiblity = this.m_sessionVisibility;
				this.m_lobbyInfo.m_connectionMode = this.m_connectionMode;
				this.m_lobbyFlow.m_uiPlayerRoot.ReCreateUIPlayers();
			}
		}
		LobbyFlowController.LobbyState state = this.m_state;
		if (this.m_state == _state)
		{
			return;
		}
		this.m_state = _state;
		if (this.m_state == LobbyFlowController.LobbyState.Matchmake || (state != LobbyFlowController.LobbyState.Matchmake && (this.m_state == LobbyFlowController.LobbyState.LocalSetup || this.m_state == LobbyFlowController.LobbyState.OnlineSetup)))
		{
			GamepadUser primaryGamepad = this.GetPrimaryGamepad();
			this.m_lobbyFlow.m_themeSelectMenu.Show(primaryGamepad, null, null, true);
			this.m_lobbyFlow.m_uiPlayerRoot.Show(primaryGamepad, null, null, true);
			this.m_lobbyFlow.m_chosenThemes.SetActive(false);
		}
		switch (this.m_state)
		{
		case LobbyFlowController.LobbyState.Matchmake:
			this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.ChooseTheme);
			break;
		case LobbyFlowController.LobbyState.LocalSetup:
			this.m_userChoices = new LobbyFlowController.ThemeChoice[OnlineMultiplayerConfig.MaxPlayers];
			this.m_lobbyFlow.m_timerText.enabled = false;
			this.SetupUserInput();
			break;
		case LobbyFlowController.LobbyState.OnlineSetup:
			this.m_userChoices = new LobbyFlowController.ThemeChoice[OnlineMultiplayerConfig.MaxPlayers];
			this.m_lobbyFlow.m_timerText.enabled = (ClientUserSystem.m_Users.Count > 1);
			this.SetupUserInput();
			break;
		case LobbyFlowController.LobbyState.LocalThemeSelection:
			if (ServerUserSystem.m_Users.Count > 0)
			{
				TeamID team = ServerUserSystem.m_Users._items[0].Team;
				ServerUserSystem.m_Users._items[0].Team = ((team != TeamID.One) ? TeamID.One : TeamID.Two);
				ServerUserSystem.m_Users._items[0].Team = team;
			}
			this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.ChooseTheme);
			if (this.m_matchmakeSelection != SceneDirectoryData.LevelTheme.Count)
			{
				this.SelectTheme(this.m_matchmakeSelection);
			}
			break;
		case LobbyFlowController.LobbyState.OnlineThemeSelection:
			if (ServerUserSystem.m_Users.Count > 0)
			{
				TeamID team2 = ServerUserSystem.m_Users._items[0].Team;
				ServerUserSystem.m_Users._items[0].Team = ((team2 != TeamID.One) ? TeamID.One : TeamID.Two);
				ServerUserSystem.m_Users._items[0].Team = team2;
			}
			this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.ChooseTheme);
			if (this.m_matchmakeSelection != SceneDirectoryData.LevelTheme.Count)
			{
				this.SelectTheme(this.m_matchmakeSelection);
			}
			break;
		case LobbyFlowController.LobbyState.LocalThemeSelected:
			this.m_lobbyFlow.m_uiPlayerRoot.Show(this.GetPrimaryGamepad(), null, null, true);
			this.ShowSelectedThemes();
			this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.PickingLevel);
			break;
		case LobbyFlowController.LobbyState.OnlineThemeSelected:
			this.m_lobbyFlow.m_timerText.text = "00";
			this.m_lobbyFlow.m_uiPlayerRoot.Show(this.GetPrimaryGamepad(), null, null, true);
			this.ShowSelectedThemes();
			this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.PickingLevel);
			break;
		}
		this.UpdateRequirementNotifications();
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000A1754 File Offset: 0x0009FB54
	public void SelectTheme(SceneDirectoryData.LevelTheme _theme)
	{
		if (this.m_state == LobbyFlowController.LobbyState.Matchmake)
		{
			this.m_matchmakeSelection = _theme;
			ThemeSelectButton buttonForTheme = this.m_lobbyFlow.m_themeSelectMenu.GetButtonForTheme(_theme);
			this.m_lobbyFlow.m_themeSelections[0].Theme = buttonForTheme.Theme;
			this.m_lobbyFlow.m_themeSelections[0].Sprite = buttonForTheme.ThemeSprite;
		}
		else
		{
			List<int> localChefIndices = this.GetLocalChefIndices();
			for (int i = 0; i < localChefIndices.Count; i++)
			{
				this.m_message.m_type = LobbyClientMessage.LobbyMessageType.ThemeSelected;
				this.m_message.m_theme = _theme;
				this.m_message.m_chefIndex = localChefIndices[i];
				ClientMessenger.LobbyMessage(this.m_message);
			}
			Analytics.LogEvent("Theme Vote", "Theme " + Enum.GetName(typeof(SceneDirectoryData.LevelTheme), _theme), (long)_theme, (Analytics.Flags)0);
		}
		this.m_lobbyFlow.m_uiPlayerRoot.Show(this.GetPrimaryGamepad(), null, null, true);
		this.ShowSelectedThemes();
		this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.WaitingForOthers);
		this.UpdateRequirementNotifications();
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x000A1874 File Offset: 0x0009FC74
	protected void OnThemeSelected(SceneDirectoryData.LevelTheme _theme, int _chefIndex)
	{
		if (_theme != SceneDirectoryData.LevelTheme.Count)
		{
			ThemeSelectButton buttonForTheme = this.m_lobbyFlow.m_themeSelectMenu.GetButtonForTheme(_theme);
			if (buttonForTheme != null && _chefIndex > -1 && _chefIndex < this.m_userChoices.Length)
			{
				this.m_userChoices[_chefIndex] = new LobbyFlowController.ThemeChoice();
				this.m_userChoices[_chefIndex].m_theme = _theme;
				this.m_userChoices[_chefIndex].m_chefIndex = _chefIndex;
				this.m_lobbyFlow.m_themeSelections[_chefIndex].Theme = buttonForTheme.Theme;
				this.m_lobbyFlow.m_themeSelections[_chefIndex].Sprite = buttonForTheme.ThemeSprite;
			}
		}
		else if (_chefIndex > -1 && _chefIndex < this.m_userChoices.Length)
		{
			this.m_userChoices[_chefIndex] = null;
			this.m_lobbyFlow.m_themeSelections[_chefIndex].Theme = SceneDirectoryData.LevelTheme.Count;
			this.m_lobbyFlow.m_themeSelections[_chefIndex].Sprite = null;
		}
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x000A1960 File Offset: 0x0009FD60
	protected void ShowSelectedThemes()
	{
		T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
		if (eventSystemForEngagementSlot != null && eventSystemForEngagementSlot.currentSelectedGameObject != null && (eventSystemForEngagementSlot.currentSelectedGameObject == null || eventSystemForEngagementSlot.currentSelectedGameObject.IsInHierarchyOf(this.m_lobbyFlow.m_themeSelectMenu.gameObject)))
		{
			this.m_lobbyFlow.m_uiPlayerRoot.FocusOnFirstPlayer(true);
		}
		this.m_lobbyFlow.m_themeSelectMenu.Hide(true, false);
		if (UserSystemUtils.AnyRemoteUsers())
		{
			bool flag = true;
			if (this.m_lobbyFlow.UnanimousSelection(this.m_userChoices) && this.m_state == LobbyFlowController.LobbyState.OnlineThemeSelected)
			{
				flag = false;
			}
			this.m_lobbyFlow.m_chosenThemes.SetActive(true);
			for (int i = 0; i < this.m_lobbyFlow.m_themeSelections.Length; i++)
			{
				this.m_lobbyFlow.m_themeSelections[i].gameObject.SetActive((!flag) ? (i == 0) : (i < ClientUserSystem.m_Users.Count));
			}
		}
		else
		{
			this.m_lobbyFlow.m_chosenThemes.SetActive(true);
			for (int j = 0; j < this.m_lobbyFlow.m_themeSelections.Length; j++)
			{
				this.m_lobbyFlow.m_themeSelections[j].gameObject.SetActive(j == 0);
			}
		}
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000A1ACC File Offset: 0x0009FECC
	protected void UpdateRequirementNotifications()
	{
		if (this.m_state == LobbyFlowController.LobbyState.PreSetup)
		{
			this.m_lobbyFlow.m_localPlayerNotification.SetActive(false);
			this.m_lobbyFlow.m_netPlayerNotification.SetActive(false);
		}
		else if (this.m_lobbyFlow.IsLocalState(this.m_state))
		{
			this.m_lobbyFlow.m_localPlayerNotification.SetActive(!this.m_bIsCoop && ClientUserSystem.m_Users.Count < 2);
		}
		else
		{
			this.m_lobbyFlow.m_localPlayerNotification.SetActive(false);
			this.m_lobbyFlow.m_netPlayerNotification.SetActive(!UserSystemUtils.AnyRemoteUsers());
		}
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000A1B7C File Offset: 0x0009FF7C
	private IEnumerator AnimateThemeSelection(int _chefIndex)
	{
		this.ShowSelectedThemes();
		List<ThemeChoiceElement> activeThemeChoices = new List<ThemeChoiceElement>();
		for (int j = 0; j < this.m_userChoices.Length; j++)
		{
			if (j < this.m_lobbyFlow.m_themeSelections.Length && this.m_userChoices[j] != null)
			{
				ThemeChoiceElement themeChoiceElement = this.m_lobbyFlow.m_themeSelections[j];
				if (themeChoiceElement != null && this.m_userChoices[j].m_theme != SceneDirectoryData.LevelTheme.Count && ((!this.m_lobbyFlow.UnanimousSelection(this.m_userChoices) && UserSystemUtils.AnyRemoteUsers()) || activeThemeChoices.Count == 0))
				{
					activeThemeChoices.Add(themeChoiceElement);
				}
			}
		}
		if (activeThemeChoices.Count == 1)
		{
			activeThemeChoices[0].ShowAsSelected(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.UI_Roulette_Confirm, base.gameObject.layer);
		}
		else if (activeThemeChoices.Count > 1)
		{
			AnimationCurve selectionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
			int numIterations = 40 + _chefIndex + 1;
			for (int i = 0; i < numIterations; i++)
			{
				for (int k = 0; k < activeThemeChoices.Count; k++)
				{
					activeThemeChoices[k].ShowAsSelected(k == i % activeThemeChoices.Count);
				}
				GameUtils.TriggerAudio(GameOneShotAudioTag.UI_Roulette_Click, base.gameObject.layer);
				yield return new WaitForSeconds(2f * this.m_lobbyFlow.m_themeSelectionDuration / (float)numIterations * selectionCurve.Evaluate((float)i / (float)numIterations));
			}
			GameUtils.TriggerAudio(GameOneShotAudioTag.UI_Roulette_Confirm, base.gameObject.layer);
		}
		if (_chefIndex > -1 && _chefIndex < activeThemeChoices.Count)
		{
			this.m_lobbyFlow.m_stateStrings.Update(LobbyFlowController.StateStrings.State.None);
			ThemeChoiceElement pickedChoice = activeThemeChoices[_chefIndex];
			for (int l = 0; l < this.m_lobbyFlow.m_themeSelections.Length; l++)
			{
				this.m_lobbyFlow.m_themeSelections[l].gameObject.SetActive(l == _chefIndex);
			}
			Vector3 selectedEndScale = pickedChoice.transform.localScale * this.m_lobbyFlow.m_selectedEndScale;
			float progress = 0f;
			while (progress < 1f)
			{
				progress = Mathf.Min(1f, progress + TimeManager.GetDeltaTime(base.gameObject));
				pickedChoice.transform.localScale = Vector3.Lerp(pickedChoice.transform.localScale, selectedEndScale, progress);
				yield return null;
			}
		}
		else
		{
			string text = string.Empty;
			for (int m = 0; m < activeThemeChoices.Count; m++)
			{
				text += activeThemeChoices[m].Theme;
				if (m < activeThemeChoices.Count - 1)
				{
					text += ", ";
				}
			}
		}
		yield break;
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000A1BA0 File Offset: 0x0009FFA0
	private GamepadUser GetPrimaryGamepad()
	{
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		if (user != null && T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user) == null)
		{
			T17EventSystemsManager.Instance.AssignFreeEventSystemToGamepadUser(user);
		}
		return user;
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x000A1BEC File Offset: 0x0009FFEC
	protected void SetupUserInput()
	{
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user = ClientUserSystem.m_Users._items[i];
			if (user.IsLocal)
			{
				bool flag = true;
				for (int j = this.m_localUserInput.Count - 1; j >= 0; j--)
				{
					ClientLobbyFlowController.UserInput userInput = this.m_localUserInput[j];
					if (userInput.m_user == user)
					{
						if (userInput.m_gamepad == user.GamepadUser)
						{
							flag = false;
						}
						else
						{
							this.m_localUserInput.RemoveAt(j);
						}
						break;
					}
				}
				if (flag)
				{
					ClientLobbyFlowController.UserInput userInput2 = new ClientLobbyFlowController.UserInput();
					if (user.Split == User.SplitStatus.NotSplit || user.Split == User.SplitStatus.SplitPadHost)
					{
						userInput2.m_changeTeamButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.WorkstationInteract, (PlayerInputLookup.Player)user.Engagement);
					}
					else
					{
						userInput2.m_changeTeamButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.PlayerSwitch, (PlayerInputLookup.Player)user.Engagement);
					}
					userInput2.m_gamepad = user.GamepadUser;
					userInput2.m_user = user;
					this.m_localUserInput.Add(userInput2);
				}
			}
		}
		for (int k = this.m_localUserInput.Count - 1; k >= 0; k--)
		{
			ClientLobbyFlowController.UserInput userInput3 = this.m_localUserInput[k];
			if (!userInput3.m_user.IsLocal)
			{
				this.m_localUserInput.RemoveAt(k);
			}
		}
		GamepadUser gamepadUser = (this.m_IPlayerManager == null) ? null : this.m_IPlayerManager.GetUser(EngagementSlot.One);
		if (gamepadUser != this.m_lastPadForLeave)
		{
			this.m_leaveButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
			this.m_lastPadForLeave = gamepadUser;
		}
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x000A1DA0 File Offset: 0x000A01A0
	public void ShowLeaveDialog()
	{
		if (this.m_leaveDialog == null)
		{
			this.m_leaveDialog = T17DialogBoxManager.GetDialog(false);
			if (this.m_leaveDialog != null)
			{
				this.m_leaveDialog.Initialize("Text.Lobby.Leave.Title", "Text.Lobby.Leave.Message", "Text.Button.Leave", null, "Text.Button.Cancel", T17DialogBox.Symbols.Unassigned, true, true, false);
				T17DialogBox leaveDialog = this.m_leaveDialog;
				leaveDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(leaveDialog.OnConfirm, new T17DialogBox.DialogEvent(this.OnLeaveConfirmed));
				T17DialogBox leaveDialog2 = this.m_leaveDialog;
				leaveDialog2.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(leaveDialog2.OnCancel, new T17DialogBox.DialogEvent(this.HideLeaveDialog));
				this.m_leaveDialog.Show();
			}
		}
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x000A1E58 File Offset: 0x000A0258
	private void HideLeaveDialog()
	{
		if (this.m_leaveDialog != null)
		{
			if (this.m_leaveDialog.IsActive)
			{
				this.m_leaveDialog.Hide();
			}
			this.m_leaveDialog = null;
		}
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000A1E90 File Offset: 0x000A0290
	private void OnLeaveConfirmed()
	{
		if (this.OnLeave != null)
		{
			this.OnLeave();
		}
		if (ConnectionStatus.IsHost())
		{
			ServerUserSystem.RemoveMatchmadeUsers();
			bool flag = false;
			for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
			{
				if (!ServerUserSystem.m_Users._items[i].IsLocal)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
				ServerOptions serverOptions = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
				serverOptions.gameMode = GameMode.OnlineKitchen;
				serverOptions.hostUser = playerManager.GetUser(EngagementSlot.One);
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveConfirmedServerConnectionState));
			}
			else
			{
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveConfirmedOfflineConnectionState));
			}
		}
		else
		{
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveConfirmedOfflineConnectionState));
		}
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x000A1F7C File Offset: 0x000A037C
	private void OnLeaveConfirmedServerConnectionState(IConnectionModeSwitchStatus result)
	{
		if (result.GetProgress() == eConnectionModeSwitchProgress.Complete && result.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.Leave();
		}
		else
		{
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveConfirmedOfflineConnectionState));
		}
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x000A1FB5 File Offset: 0x000A03B5
	private void OnLeaveConfirmedOfflineConnectionState(IConnectionModeSwitchStatus result)
	{
		this.Leave();
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x000A1FBD File Offset: 0x000A03BD
	private void Leave()
	{
		ServerGameSetup.Mode = GameMode.OnlineKitchen;
		ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, false, GameState.NotSet);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x000A1FD8 File Offset: 0x000A03D8
	public void Update()
	{
		if (!T17DialogBoxManager.HasAnyOpenDialogs())
		{
			for (int i = 0; i < this.m_localUserInput.Count; i++)
			{
				if (this.m_localUserInput[i].m_changeTeamButton.JustPressed())
				{
					this.ChangeTeam(this.m_localUserInput[i].m_user);
				}
			}
		}
		if (this.m_leaveButton != null && this.m_leaveButton.JustPressed())
		{
			this.ShowLeaveDialog();
		}
		switch (this.m_state)
		{
		case LobbyFlowController.LobbyState.OnlineThemeSelection:
			if (UserSystemUtils.AnyRemoteUsers())
			{
				this.m_lobbyFlow.m_timerText.enabled = true;
				this.SetTimer(this.m_timeLeft - TimeManager.GetDeltaTime(base.gameObject));
			}
			else
			{
				this.m_lobbyFlow.m_timerText.enabled = false;
			}
			break;
		case LobbyFlowController.LobbyState.LocalThemeSelected:
			this.m_lobbyFlow.m_timerText.enabled = true;
			this.SetTimer(this.m_timeLeft - TimeManager.GetDeltaTime(base.gameObject));
			break;
		case LobbyFlowController.LobbyState.OnlineThemeSelected:
			this.m_lobbyFlow.m_timerText.enabled = true;
			this.SetTimer(this.m_timeLeft - TimeManager.GetDeltaTime(base.gameObject));
			break;
		}
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x000A2131 File Offset: 0x000A0531
	protected virtual void OnTimerReset(float _timerVal)
	{
		this.SetTimer(_timerVal);
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x000A213C File Offset: 0x000A053C
	protected void SetTimer(float _timerVal)
	{
		this.m_timeLeft = _timerVal;
		if (this.m_timeLeft > 0f)
		{
			string text = Mathf.RoundToInt(this.m_timeLeft).ToString();
			text = text.PadLeft(2, '0');
			this.m_lobbyFlow.m_timerText.text = text;
		}
		else
		{
			this.m_lobbyFlow.m_timerText.text = "00";
		}
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x000A21B0 File Offset: 0x000A05B0
	protected void ChangeTeam(User _user)
	{
		if (this.m_bIsCoop || (this.m_state != LobbyFlowController.LobbyState.LocalThemeSelection && this.m_state != LobbyFlowController.LobbyState.OnlineThemeSelection))
		{
			return;
		}
		int chefIndex = ClientUserSystem.m_Users.FindIndex((User x) => x == _user);
		this.ChangeTeam(chefIndex);
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x000A220C File Offset: 0x000A060C
	protected void ChangeTeam(int _chefIndex)
	{
		if (this.m_bIsCoop || (this.m_state != LobbyFlowController.LobbyState.LocalThemeSelection && this.m_state != LobbyFlowController.LobbyState.OnlineThemeSelection))
		{
			return;
		}
		if (_chefIndex > -1 && _chefIndex < ClientUserSystem.m_Users.Count)
		{
			this.m_message.m_type = LobbyClientMessage.LobbyMessageType.TeamChangeRequest;
			this.m_message.m_chefIndex = _chefIndex;
			ClientMessenger.LobbyMessage(this.m_message);
		}
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x000A2278 File Offset: 0x000A0678
	protected List<int> GetLocalChefIndices()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			if (ClientUserSystem.m_Users._items[i] != null && ClientUserSystem.m_Users._items[i].IsLocal)
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x04001984 RID: 6532
	private LobbyFlowController m_lobbyFlow;

	// Token: 0x04001985 RID: 6533
	private static ClientLobbyFlowController s_instance;

	// Token: 0x04001986 RID: 6534
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04001987 RID: 6535
	private LobbySetupInfo m_lobbyInfo;

	// Token: 0x04001988 RID: 6536
	private List<ClientLobbyFlowController.UserInput> m_localUserInput = new List<ClientLobbyFlowController.UserInput>();

	// Token: 0x04001989 RID: 6537
	private float m_timeLeft;

	// Token: 0x0400198A RID: 6538
	protected LobbyFlowController.LobbyState m_state;

	// Token: 0x0400198B RID: 6539
	protected LobbyFlowController.ThemeChoice[] m_userChoices;

	// Token: 0x0400198C RID: 6540
	protected bool m_bIsCoop = true;

	// Token: 0x0400198D RID: 6541
	protected OnlineMultiplayerSessionVisibility m_sessionVisibility;

	// Token: 0x0400198E RID: 6542
	protected OnlineMultiplayerConnectionMode m_connectionMode;

	// Token: 0x0400198F RID: 6543
	private NetworkErrorDialog m_NetworkErrorDialog = new NetworkErrorDialog();

	// Token: 0x04001990 RID: 6544
	public LobbyClientMessage m_message = new LobbyClientMessage();

	// Token: 0x04001991 RID: 6545
	private GamepadUser m_lastPadForLeave;

	// Token: 0x04001992 RID: 6546
	private ILogicalButton m_leaveButton;

	// Token: 0x04001993 RID: 6547
	public T17DialogBox m_leaveDialog;

	// Token: 0x04001994 RID: 6548
	private DLCManager m_dlcManager;

	// Token: 0x04001995 RID: 6549
	private SceneDirectoryData.LevelTheme m_matchmakeSelection = SceneDirectoryData.LevelTheme.Count;

	// Token: 0x04001996 RID: 6550
	public GenericVoid OnLeave;

	// Token: 0x04001997 RID: 6551
	private IConnectionModeSwitchStatus m_lastStatus;

	// Token: 0x020006E6 RID: 1766
	protected class UserInput
	{
		// Token: 0x04001998 RID: 6552
		public ILogicalButton m_changeTeamButton;

		// Token: 0x04001999 RID: 6553
		public GamepadUser m_gamepad;

		// Token: 0x0400199A RID: 6554
		public User m_user;
	}
}
