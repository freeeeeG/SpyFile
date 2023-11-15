using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AC7 RID: 2759
public class FrontendVersusTabOptions : FrontendMenuBehaviour
{
	// Token: 0x0600377C RID: 14204 RVA: 0x00105903 File Offset: 0x00103D03
	protected override void Awake()
	{
		base.Awake();
		this.m_OfflineAgentPrivilegeChecksCompleteCallback = new GenericVoid<IConnectionModeSwitchStatus>(this.OnPrivilegeChecksComplete);
		this.m_OfflineAgentCouchPlayCompleteCallback = new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestConnectionStateOfflineForCouchPlayComplete);
		this.SetupForConnectionMode();
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x00105935 File Offset: 0x00103D35
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ConnectionModeSwitcher.InvalidateCallback(this.m_OfflineAgentPrivilegeChecksCompleteCallback);
		ConnectionModeSwitcher.InvalidateCallback(this.m_OfflineAgentCouchPlayCompleteCallback);
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x00105953 File Offset: 0x00103D53
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x0010595C File Offset: 0x00103D5C
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
		FastList<User> users = ServerUserSystem.m_Users;
		if (users.Count == 1)
		{
			NetworkDialogHelper.ShowMoreUsersRequiredDialog();
			return;
		}
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerGameSetup.Mode = GameMode.Versus;
			this.m_desiredConnectionMode = OnlineMultiplayerConnectionMode.eNone;
			this.SetupLobbyInfo(OnlineMultiplayerSessionVisibility.eClosed, this.m_desiredConnectionMode);
			this.SetupGameSession();
			ServerMessenger.LoadLevel("Lobbies", GameState.VSLobby, false, GameState.NotSet);
		}
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x001059F4 File Offset: 0x00103DF4
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

	// Token: 0x06003781 RID: 14209 RVA: 0x00105A55 File Offset: 0x00103E55
	private void OnRequestConnectionStateOfflineForCouchPlayComplete(IConnectionModeSwitchStatus status)
	{
		this.HideProgressSpinnerDialog();
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.OnCouchPlayClicked();
		}
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x00105A6F File Offset: 0x00103E6F
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

	// Token: 0x06003783 RID: 14211 RVA: 0x00105AB0 File Offset: 0x00103EB0
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

	// Token: 0x06003784 RID: 14212 RVA: 0x00105B1C File Offset: 0x00103F1C
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

	// Token: 0x06003785 RID: 14213 RVA: 0x00105B84 File Offset: 0x00103F84
	private void OnNotEnoughUsersDialogDismissed()
	{
		T17FrontendFlow instance = T17FrontendFlow.Instance;
		if (instance != null)
		{
			instance.FocusOnMultiplayerKitchen(false);
		}
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x00105BAC File Offset: 0x00103FAC
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

	// Token: 0x06003787 RID: 14215 RVA: 0x00105C02 File Offset: 0x00104002
	public void OnLocalPlayClicked()
	{
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x00105C04 File Offset: 0x00104004
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

	// Token: 0x06003789 RID: 14217 RVA: 0x00105C64 File Offset: 0x00104064
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

	// Token: 0x0600378A RID: 14218 RVA: 0x00105CDA File Offset: 0x001040DA
	private void LoadLobby(OnlineMultiplayerSessionVisibility _visiblity)
	{
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerGameSetup.Mode = GameMode.Versus;
		}
		this.SetupLobbyInfo(_visiblity, this.m_desiredConnectionMode);
		this.SetupGameSession();
		ServerMessenger.LoadLevel("Lobbies", GameState.VSLobby, false, GameState.NotSet);
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x00105D18 File Offset: 0x00104118
	protected override void Update()
	{
		base.Update();
		if (this.m_progressBox != null)
		{
			string localisedProgressDescription = ConnectionModeSwitcher.GetStatus().GetLocalisedProgressDescription();
			this.m_progressBox.SetMessage(localisedProgressDescription, false);
		}
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x00105D54 File Offset: 0x00104154
	private void SetupForConnectionMode()
	{
		this.m_localPlayButton.gameObject.SetActive(false);
		this.m_onlinePublicButton.gameObject.SetActive(true);
		this.m_onlinePrivateButton.gameObject.SetActive(true);
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x00105D8C File Offset: 0x0010418C
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

	// Token: 0x0600378E RID: 14222 RVA: 0x00105DEE File Offset: 0x001041EE
	private void HideProgressSpinnerDialog()
	{
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x00105E14 File Offset: 0x00104214
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
		lobbySetupInfo.m_gameType = GameSession.GameType.Competitive;
		lobbySetupInfo.m_originalConnectionState = ConnectionModeSwitcher.GetRequestedConnectionState();
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x00105E70 File Offset: 0x00104270
	protected void SetupGameSession()
	{
		GameSession gameSession = T17FrontendFlow.Instance.StartEmptySession(GameSession.GameType.Competitive, -1);
		gameSession.TypeSettings.WorldMapScene = "Lobbies";
	}

	// Token: 0x04002C89 RID: 11401
	public T17Button m_couchPlayButton;

	// Token: 0x04002C8A RID: 11402
	public T17Button m_onlinePublicButton;

	// Token: 0x04002C8B RID: 11403
	public T17Button m_onlinePrivateButton;

	// Token: 0x04002C8C RID: 11404
	public T17Button m_localPlayButton;

	// Token: 0x04002C8D RID: 11405
	private T17DialogBox m_progressBox;

	// Token: 0x04002C8E RID: 11406
	private OnlineMultiplayerSessionVisibility m_PendingVisibility = OnlineMultiplayerSessionVisibility.eClosed;

	// Token: 0x04002C8F RID: 11407
	private GenericVoid<IConnectionModeSwitchStatus> m_OfflineAgentPrivilegeChecksCompleteCallback;

	// Token: 0x04002C90 RID: 11408
	private GenericVoid<IConnectionModeSwitchStatus> m_OfflineAgentCouchPlayCompleteCallback;

	// Token: 0x04002C91 RID: 11409
	private OnlineMultiplayerConnectionMode m_desiredConnectionMode;
}
