using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AB2 RID: 2738
public class FrontendCoopTabOptions : FrontendMenuBehaviour
{
	// Token: 0x0600365D RID: 13917 RVA: 0x000FEE6E File Offset: 0x000FD26E
	protected override void Awake()
	{
		base.Awake();
		this.m_OfflineAgentPrivilegeChecksCompleteCallback = new GenericVoid<IConnectionModeSwitchStatus>(this.OnPrivilegeChecksComplete);
		this.m_OfflineAgentCouchPlayCompleteCallback = new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestConnectionStateOfflineForCouchPlayComplete);
		this.SetupForConnectionMode();
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x000FEEA0 File Offset: 0x000FD2A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ConnectionModeSwitcher.InvalidateCallback(this.m_OfflineAgentPrivilegeChecksCompleteCallback);
		ConnectionModeSwitcher.InvalidateCallback(this.m_OfflineAgentCouchPlayCompleteCallback);
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x000FEEBE File Offset: 0x000FD2BE
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x000FEEC8 File Offset: 0x000FD2C8
	public void OnCouchPlayClicked()
	{
		if (ConnectionStatus.IsInSession())
		{
			if (UserSystemUtils.AnyRemoteUsers())
			{
				NetworkDialogHelper.ShowGoingOfflineDialog(new T17DialogBox.DialogEvent(this.GoOfflineBeforeCouchPlay));
			}
			else
			{
				this.GoOfflineBeforeCouchPlay();
			}
			return;
		}
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerGameSetup.Mode = GameMode.Party;
			this.m_desiredConnectionMode = OnlineMultiplayerConnectionMode.eNone;
			this.SetupLobbyInfo(OnlineMultiplayerSessionVisibility.eClosed, this.m_desiredConnectionMode);
			this.SetupGameSession();
			ClientTime.Update();
			ServerMessenger.TimeSync(ClientTime.Time());
			ServerMessenger.LoadLevel("Lobbies", GameState.PartyLobby, false, GameState.NotSet);
		}
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x000FEF58 File Offset: 0x000FD358
	private void GoOfflineBeforeCouchPlay()
	{
		this.ShowProgressSpinnerDialog();
		OnlineMultiplayerConnectionMode value;
		if (ConnectionStatus.CurrentConnectionMode() == OnlineMultiplayerConnectionMode.eInternet)
		{
			value = OnlineMultiplayerConnectionMode.eInternet;
		}
		else
		{
			value = OnlineMultiplayerConnectionMode.eNone;
		}
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, new OfflineOptions
		{
			hostUser = GameUtils.RequireManagerInterface<IPlayerManager>().GetUser(EngagementSlot.One),
			connectionMode = new OnlineMultiplayerConnectionMode?(value)
		}, this.m_OfflineAgentCouchPlayCompleteCallback);
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x000FEFB9 File Offset: 0x000FD3B9
	private void OnRequestConnectionStateOfflineForCouchPlayComplete(IConnectionModeSwitchStatus status)
	{
		this.HideProgressSpinnerDialog();
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.OnCouchPlayClicked();
		}
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x000FEFD3 File Offset: 0x000FD3D3
	public void OnOnlinePublicClicked()
	{
		if (UserSystemUtils.AtMaxUserCount() && !UserSystemUtils.AnyRemoteUsers())
		{
			NetworkDialogHelper.ShowFullLobbyDialog();
			return;
		}
		if (UserSystemUtils.AnySplitPadUsers())
		{
			NetworkDialogHelper.ShowRemoveSplitPadUsersDialog(new T17DialogBox.DialogEvent(this.StartOnlinePublic), null);
			return;
		}
		this.StartOnlinePublic();
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x000FF014 File Offset: 0x000FD414
	private void StartOnlinePublic()
	{
		UserSystemUtils.RemoveAllSplitPadGuestUsers();
		this.m_desiredConnectionMode = OnlineMultiplayerConnectionMode.eInternet;
		if (!ConnectionStatus.IsInSession())
		{
			this.DoPrivilegeCheck(OnlineMultiplayerSessionVisibility.eMatchmaking);
		}
		else if (ConnectionStatus.IsHost())
		{
			if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Server)
			{
				ServerOptions serverOptions = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.eClosed;
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, null);
			}
			this.LoadLobby(OnlineMultiplayerSessionVisibility.eMatchmaking);
		}
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x000FF080 File Offset: 0x000FD480
	public void OnOnlinePrivateClicked()
	{
		if (UserSystemUtils.AtMaxUserCount() && !UserSystemUtils.AnyRemoteUsers())
		{
			NetworkDialogHelper.ShowFullLobbyDialog();
			return;
		}
		if (!UserSystemUtils.AnyRemoteUsers())
		{
			NetworkDialogHelper.ShowNoOnlineUsersDialog(new T17DialogBox.DialogEvent(this.OnNotEnoughUsersDialogDismissed));
			return;
		}
		if (UserSystemUtils.AnySplitPadUsers())
		{
			NetworkDialogHelper.ShowRemoveSplitPadUsersDialog(new T17DialogBox.DialogEvent(this.StartPrivateOnline), null);
			return;
		}
		this.StartPrivateOnline();
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x000FF0E8 File Offset: 0x000FD4E8
	private void OnNotEnoughUsersDialogDismissed()
	{
		T17FrontendFlow instance = T17FrontendFlow.Instance;
		if (instance != null)
		{
			instance.FocusOnMultiplayerKitchen(false);
		}
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x000FF110 File Offset: 0x000FD510
	private void StartPrivateOnline()
	{
		UserSystemUtils.RemoveAllSplitPadGuestUsers();
		this.m_desiredConnectionMode = OnlineMultiplayerConnectionMode.eInternet;
		if (ConnectionStatus.IsHost())
		{
			if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Server)
			{
				ServerOptions serverOptions = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.eClosed;
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, null);
			}
			this.LoadLobby(OnlineMultiplayerSessionVisibility.ePrivate);
		}
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x000FF166 File Offset: 0x000FD566
	public void OnLocalPlayClicked()
	{
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x000FF168 File Offset: 0x000FD568
	protected void DoPrivilegeCheck(OnlineMultiplayerSessionVisibility _visiblity)
	{
		this.m_PendingVisibility = _visiblity;
		this.ShowProgressSpinnerDialog();
		IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		GamepadUser user = playerManager.GetUser(EngagementSlot.One);
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, new OfflineOptions
		{
			hostUser = user,
			eAdditionalAction = OfflineOptions.AdditionalAction.PrivilegeCheckAllUsers,
			connectionMode = new OnlineMultiplayerConnectionMode?(OnlineMultiplayerConnectionMode.eInternet)
		}, this.m_OfflineAgentPrivilegeChecksCompleteCallback);
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x000FF1C8 File Offset: 0x000FD5C8
	private void OnPrivilegeChecksComplete(IConnectionModeSwitchStatus status)
	{
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.LoadLobby(this.m_PendingVisibility);
		}
		else
		{
			CompositeStatus compositeStatus = status as CompositeStatus;
			ConnectionModeStatus connectionModeStatus = null;
			if (compositeStatus != null)
			{
				connectionModeStatus = (compositeStatus.m_TaskSubStatus as ConnectionModeStatus);
			}
			if (connectionModeStatus == null || connectionModeStatus.m_Result.m_returnCode != OnlineMultiplayerConnectionModeConnectResult.eCancelledByUser)
			{
				NetworkErrorDialog.ShowDialog(status);
			}
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, null);
		}
		this.HideProgressSpinnerDialog();
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x000FF240 File Offset: 0x000FD640
	private void LoadLobby(OnlineMultiplayerSessionVisibility _visiblity)
	{
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerGameSetup.Mode = GameMode.Party;
		}
		this.SetupLobbyInfo(_visiblity, this.m_desiredConnectionMode);
		this.SetupGameSession();
		ClientTime.Update();
		ServerMessenger.TimeSync(ClientTime.Time());
		ServerMessenger.LoadLevel("Lobbies", GameState.PartyLobby, false, GameState.NotSet);
	}

	// Token: 0x0600366C RID: 13932 RVA: 0x000FF298 File Offset: 0x000FD698
	protected override void Update()
	{
		base.Update();
		if (this.m_progressBox != null)
		{
			string localisedProgressDescription = ConnectionModeSwitcher.GetStatus().GetLocalisedProgressDescription();
			this.m_progressBox.SetMessage(localisedProgressDescription, false);
		}
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x000FF2D4 File Offset: 0x000FD6D4
	private void SetupForConnectionMode()
	{
		this.m_localPlayButton.gameObject.SetActive(false);
		this.m_onlinePublicButton.gameObject.SetActive(true);
		this.m_onlinePrivateButton.gameObject.SetActive(true);
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x000FF30C File Offset: 0x000FD70C
	private void ShowProgressSpinnerDialog()
	{
		if (this.m_progressBox == null)
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", string.Empty, null, null, null, T17DialogBox.Symbols.Spinner, true, false, false);
				this.m_progressBox.Show();
			}
		}
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x000FF36E File Offset: 0x000FD76E
	private void HideProgressSpinnerDialog()
	{
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x000FF394 File Offset: 0x000FD794
	private void ShowErrorDialog(string strErrorString)
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.PleaseWait", strErrorString, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
			dialog.Show();
		}
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x000FF3D0 File Offset: 0x000FD7D0
	protected void SetupLobbyInfo(OnlineMultiplayerSessionVisibility _visibility, OnlineMultiplayerConnectionMode _connectionMode)
	{
		LobbySetupInfo lobbySetupInfo = LobbySetupInfo.Instance;
		if (lobbySetupInfo != null)
		{
			UnityEngine.Object.DestroyImmediate(lobbySetupInfo.gameObject);
		}
		GameObject gameObject = new GameObject("LobbySetupInfo");
		lobbySetupInfo = gameObject.AddComponent<LobbySetupInfo>();
		lobbySetupInfo.m_visiblity = _visibility;
		lobbySetupInfo.m_connectionMode = _connectionMode;
		lobbySetupInfo.m_gameType = GameSession.GameType.Cooperative;
		lobbySetupInfo.m_originalConnectionState = ConnectionModeSwitcher.GetRequestedConnectionState();
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x000FF42C File Offset: 0x000FD82C
	protected void SetupGameSession()
	{
		GameSession gameSession = T17FrontendFlow.Instance.StartEmptySession(GameSession.GameType.Cooperative, -1);
		gameSession.TypeSettings.WorldMapScene = "Lobbies";
	}

	// Token: 0x04002BBA RID: 11194
	public T17Button m_couchPlayButton;

	// Token: 0x04002BBB RID: 11195
	public T17Button m_onlinePublicButton;

	// Token: 0x04002BBC RID: 11196
	public T17Button m_onlinePrivateButton;

	// Token: 0x04002BBD RID: 11197
	public T17Button m_localPlayButton;

	// Token: 0x04002BBE RID: 11198
	private T17DialogBox m_progressBox;

	// Token: 0x04002BBF RID: 11199
	private OnlineMultiplayerSessionVisibility m_PendingVisibility = OnlineMultiplayerSessionVisibility.eClosed;

	// Token: 0x04002BC0 RID: 11200
	private GenericVoid<IConnectionModeSwitchStatus> m_OfflineAgentPrivilegeChecksCompleteCallback;

	// Token: 0x04002BC1 RID: 11201
	private GenericVoid<IConnectionModeSwitchStatus> m_OfflineAgentCouchPlayCompleteCallback;

	// Token: 0x04002BC2 RID: 11202
	private OnlineMultiplayerConnectionMode m_desiredConnectionMode;
}
