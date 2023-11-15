using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000895 RID: 2197
public class FrontendInviteHandler : InviteHandler
{
	// Token: 0x06002ACC RID: 10956 RVA: 0x000C86E8 File Offset: 0x000C6AE8
	public void Start()
	{
		this.m_PlayTogetherData = InviteMonitor.GetPlayTogetherHost();
		if (this.m_PlayTogetherData != null)
		{
			this.HandlePlayTogetherHost(this.m_PlayTogetherData);
		}
		else
		{
			this.HandleAcceptedInvite(InviteMonitor.GetAcceptedInvite());
		}
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		playerManager.EngagementChangeCallback += this.OnEngagementChanged;
	}

	// Token: 0x06002ACD RID: 10957 RVA: 0x000C8740 File Offset: 0x000C6B40
	private void LoadIISScreen(IConnectionModeSwitchStatus status)
	{
		ServerUserSystem.UnlockEngagement();
		UserSystemUtils.RemoveAllSplitPadGuestUsers();
		IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		for (int i = 0; i < 4; i++)
		{
			GamepadUser user = playerManager.GetUser((EngagementSlot)i);
			if (null != user)
			{
				playerManager.DisengagePad((EngagementSlot)i);
			}
		}
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x000C878A File Offset: 0x000C6B8A
	private void OnEngagementChanged(EngagementSlot _e, GamepadUser _prev, GamepadUser _new)
	{
		if (_e == EngagementSlot.One && _new != null && this.m_InviteData != null)
		{
			this.HandleAcceptedInvite(this.m_InviteData);
		}
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x000C87B8 File Offset: 0x000C6BB8
	public void Stop()
	{
		if (this.m_bBusy)
		{
			this.m_bBusy = false;
			this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eIdle;
			this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eIdle;
			this.m_PlayTogetherData = null;
			this.m_InviteData = null;
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, null);
		}
		if (null != this.m_dialogBox && this.m_dialogBox.IsActive)
		{
			this.m_dialogBox.Hide();
			this.m_dialogBox = null;
		}
		if (null != this.m_progressBox && this.m_progressBox.IsActive)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x000C887C File Offset: 0x000C6C7C
	public void Update()
	{
		FrontendInviteHandler.InviteProcessingState inviteProcessingState = this.m_inviteProcessingState;
		if (inviteProcessingState != FrontendInviteHandler.InviteProcessingState.eDisengageNonRequiredUsers)
		{
			if (inviteProcessingState == FrontendInviteHandler.InviteProcessingState.eStartJoinProcess)
			{
				if ((long)ServerUserSystem.m_Users.Count < (long)((ulong)OnlineMultiplayerConfig.MaxPlayers))
				{
					this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eJoining;
					ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.AcceptInvite, this.m_InviteData, new GenericVoid<IConnectionModeSwitchStatus>(this.OnAcceptInviteConnectionStateComplete));
					this.m_InviteData = null;
				}
				else
				{
					this.m_InviteData = null;
					this.m_bBusy = false;
					this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eIdle;
					if (null != this.m_progressBox)
					{
						this.m_progressBox.Hide();
						this.m_progressBox = null;
					}
					NetworkDialogHelper.ShowTooManyLocalUsersForJoining();
				}
			}
		}
		else
		{
			this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eStartJoinProcess;
			UserSystemUtils.DisengageNonRequiredUsersForOnline(this.m_InviteRequiresAllActiveLocalUsers);
		}
		FrontendInviteHandler.PlayTogetherHostingState playTogetherHostingState = this.m_playTogetherHostingState;
		if (playTogetherHostingState != FrontendInviteHandler.PlayTogetherHostingState.eDisengageNonRequiredUsers)
		{
			if (playTogetherHostingState == FrontendInviteHandler.PlayTogetherHostingState.eStartHosting)
			{
				this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eHosting;
				ServerOptions serverOptions = default(ServerOptions);
				serverOptions.gameMode = GameMode.OnlineKitchen;
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
				serverOptions.hostUser = GameUtils.RequireManagerInterface<IPlayerManager>().GetUser(EngagementSlot.One);
				serverOptions.connectionMode = OnlineMultiplayerConnectionMode.eInternet;
				serverOptions.playTogetherHost = this.m_PlayTogetherData;
				this.m_PlayTogetherData = null;
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnPlayTogetherHostComplete));
			}
		}
		else
		{
			this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eStartHosting;
			UserSystemUtils.DisengageNonRequiredUsersForOnline(true);
		}
		if (this.m_progressBox != null)
		{
			string localisedProgressDescription = ConnectionModeSwitcher.GetStatus().GetLocalisedProgressDescription();
			this.m_progressBox.SetMessage(localisedProgressDescription, false);
		}
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x000C8A00 File Offset: 0x000C6E00
	public void HandleAcceptedInvite(AcceptInviteData invite)
	{
		if (null == this.m_dialogBox)
		{
			this.m_InviteData = invite;
			this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eIdle;
			GamepadUser user = GameUtils.RequireManager<PlayerManager>().GetUser(EngagementSlot.One);
			if (null != user && this.m_InviteData != null)
			{
				bool flag = this.m_InviteData.Invite.WasAcceptedBy(user);
				if (flag)
				{
					this.m_InviteData.User = user;
					switch (this.m_InviteData.JoinLocalUsersChoice)
					{
					case AcceptInviteData.LocalUsersChoice.eNotChosenYet:
						if (UserSystemUtils.LocalUserCount(ClientUserSystem.m_Users, false) > 1U)
						{
							this.m_dialogBox = T17DialogBoxManager.GetDialog(false);
							if (null != this.m_dialogBox)
							{
								FastList<User> users = ClientUserSystem.m_Users;
								User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
								User user2 = UserSystemUtils.FindUser(users, null, s_LocalMachineId, EngagementSlot.One, TeamID.Count, User.SplitStatus.Count);
								string message = Localization.Get("Text.Menu.MultiLocalUserInviteQuestion", new LocToken[]
								{
									new LocToken("[NAME]", (user2 == null) ? string.Empty : user2.DisplayName)
								});
								this.m_dialogBox.Initialize("StartScreen.AreYouSure", message, "Text.Button.InvitedUserOnly", "Text.Button.AllLocalUsers", "Text.Button.Cancel", T17DialogBox.Symbols.Warning, true, false, false);
								T17DialogBox dialogBox = this.m_dialogBox;
								dialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.OnInviteStartPrimaryUserOnly));
								T17DialogBox dialogBox2 = this.m_dialogBox;
								dialogBox2.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox2.OnDecline, new T17DialogBox.DialogEvent(this.OnInviteStartAllLocalUsers));
								T17DialogBox dialogBox3 = this.m_dialogBox;
								dialogBox3.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox3.OnCancel, new T17DialogBox.DialogEvent(this.OnInviteCancelled));
								this.m_dialogBox.Show();
							}
							else
							{
								this.OnInviteCancelled();
							}
						}
						else if (UserSystemUtils.AnySplitPadUsers() || UserSystemUtils.AnyRemoteUsers())
						{
							this.m_dialogBox = T17DialogBoxManager.GetDialog(false);
							if (null != this.m_dialogBox)
							{
								FastList<User> users = ClientUserSystem.m_Users;
								User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
								User user3 = UserSystemUtils.FindUser(users, null, s_LocalMachineId, EngagementSlot.One, TeamID.Count, User.SplitStatus.Count);
								string message2 = Localization.Get("Text.Menu.InviteRestriction", new LocToken[]
								{
									new LocToken("[NAME]", (user3 == null) ? string.Empty : user3.DisplayName)
								});
								this.m_dialogBox.Initialize("StartScreen.AreYouSure", message2, "Text.Button.Confirm", "Text.Button.Cancel", null, T17DialogBox.Symbols.Warning, true, false, false);
								T17DialogBox dialogBox4 = this.m_dialogBox;
								dialogBox4.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox4.OnConfirm, new T17DialogBox.DialogEvent(this.OnInviteStartPrimaryUserOnly));
								T17DialogBox dialogBox5 = this.m_dialogBox;
								dialogBox5.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox5.OnDecline, new T17DialogBox.DialogEvent(this.OnInviteCancelled));
								this.m_dialogBox.Show();
							}
							else
							{
								this.OnInviteCancelled();
							}
						}
						else
						{
							this.OnInviteStartPrimaryUserOnly();
						}
						break;
					case AcceptInviteData.LocalUsersChoice.ePrimary:
						this.OnInviteStartPrimaryUserOnly();
						break;
					case AcceptInviteData.LocalUsersChoice.eAll:
						this.OnInviteStartAllLocalUsers();
						break;
					default:
						this.OnInviteCancelled();
						break;
					}
				}
				else if (this.m_InviteData.FromIIS)
				{
					InviteMonitor.ClearInvite();
				}
				else
				{
					this.m_InviteData.FromIIS = true;
					this.m_InviteData.User = null;
					if (InviteMonitor.InviteAccepted != null)
					{
						InviteMonitor.InviteAccepted();
					}
					ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.LoadIISScreen));
				}
			}
		}
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000C8D78 File Offset: 0x000C7178
	public void HandlePlayTogetherHost(OnlineMultiplayerSessionPlayTogetherHosting host)
	{
		this.m_PlayTogetherData = host;
		this.m_bBusy = true;
		this.m_dialogBox = null;
		InviteMonitor.ClearPlayTogetherHost();
		if (null == this.m_progressBox)
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (null != this.m_progressBox)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", string.Empty, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
		}
		this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eGoingOffline;
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnOfflineToPlayTogetherHost));
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x000C8E0F File Offset: 0x000C720F
	public bool IsBusy()
	{
		return this.m_bBusy || null != this.m_dialogBox || null != this.m_progressBox;
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x000C8E41 File Offset: 0x000C7241
	public bool IsAwaitingUserInput()
	{
		return null != this.m_dialogBox;
	}

	// Token: 0x06002AD5 RID: 10965 RVA: 0x000C8E50 File Offset: 0x000C7250
	private void OnOfflineToPlayTogetherHost(IConnectionModeSwitchStatus status)
	{
		if (this.m_playTogetherHostingState == FrontendInviteHandler.PlayTogetherHostingState.eGoingOffline)
		{
			if (status.GetResult() == eConnectionModeSwitchResult.Success)
			{
				this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eDisengageNonRequiredUsers;
			}
			else
			{
				this.m_bBusy = false;
				this.m_PlayTogetherData = null;
				this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eIdle;
				if (null != this.m_progressBox)
				{
					this.m_progressBox.Hide();
					this.m_progressBox = null;
				}
				NetworkErrorDialog.ShowDialog(status);
			}
		}
	}

	// Token: 0x06002AD6 RID: 10966 RVA: 0x000C8EBF File Offset: 0x000C72BF
	private void OnInviteStartPrimaryUserOnly()
	{
		this.StartJoin(false);
	}

	// Token: 0x06002AD7 RID: 10967 RVA: 0x000C8EC8 File Offset: 0x000C72C8
	private void OnInviteStartAllLocalUsers()
	{
		this.StartJoin(true);
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x000C8ED1 File Offset: 0x000C72D1
	private void OnInviteCancelled()
	{
		InviteMonitor.ClearInvite();
		InviteMonitor.ClearPlayTogetherHost();
		this.m_InviteData = null;
		this.m_dialogBox = null;
		this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eIdle;
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x000C8EF4 File Offset: 0x000C72F4
	private void StartJoin(bool allowAllLocalUsers)
	{
		this.m_bBusy = true;
		this.m_dialogBox = null;
		InviteMonitor.ClearInvite();
		if (InviteMonitor.InviteAccepted != null)
		{
			InviteMonitor.InviteAccepted();
		}
		this.m_InviteRequiresAllActiveLocalUsers = allowAllLocalUsers;
		if (null == this.m_progressBox)
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (null != this.m_progressBox)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", ConnectionModeSwitcher.GetStatus().GetLocalisedProgressDescription(), null, null, null, T17DialogBox.Symbols.Spinner, true, false, false);
				this.m_progressBox.Show();
			}
		}
		this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eGoingOffline;
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnOfflineToJoinInvite));
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x000C8FA4 File Offset: 0x000C73A4
	private void OnOfflineToJoinInvite(IConnectionModeSwitchStatus status)
	{
		if (this.m_inviteProcessingState == FrontendInviteHandler.InviteProcessingState.eGoingOffline)
		{
			if (status.GetResult() == eConnectionModeSwitchResult.Success)
			{
				this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eDisengageNonRequiredUsers;
			}
			else
			{
				this.m_InviteData = null;
				this.m_bBusy = false;
				this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eIdle;
				if (null != this.m_progressBox)
				{
					this.m_progressBox.Hide();
					this.m_progressBox = null;
				}
				NetworkErrorDialog.ShowDialog(status);
			}
		}
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x000C9014 File Offset: 0x000C7414
	private void OnAcceptInviteConnectionStateComplete(IConnectionModeSwitchStatus status)
	{
		this.m_inviteProcessingState = FrontendInviteHandler.InviteProcessingState.eIdle;
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_bBusy = false;
			if (null != this.m_progressBox)
			{
				this.m_progressBox.Hide();
				this.m_progressBox = null;
			}
			if (InviteMonitor.InviteJoinComplete != null)
			{
				InviteMonitor.InviteJoinComplete();
			}
			GameUtils.SendDiagnosticEvent("AcceptInvite:Success");
		}
		else
		{
			CompositeStatus compositeStatus = status as CompositeStatus;
			ConnectionModeStatus connectionModeStatus = null;
			if (compositeStatus != null)
			{
				connectionModeStatus = (compositeStatus.m_TaskSubStatus as ConnectionModeStatus);
			}
			this.m_FailedStatus = status.Clone();
			if (connectionModeStatus != null)
			{
				OnlineMultiplayerConnectionModeConnectResult returnCode = connectionModeStatus.m_Result.m_returnCode;
				if (returnCode != OnlineMultiplayerConnectionModeConnectResult.eCancelledByUser)
				{
					if (returnCode != OnlineMultiplayerConnectionModeConnectResult.eGenericFailure)
					{
						GameUtils.SendDiagnosticEvent("AcceptInvite:Failure:Unknown");
					}
					else
					{
						GameUtils.SendDiagnosticEvent("AcceptInvite:Failure:eGenericFailure");
					}
				}
				else
				{
					GameUtils.SendDiagnosticEvent("AcceptInvite:Failure:eCancelledByUser");
				}
			}
			else
			{
				GameUtils.SendDiagnosticEvent("AcceptInvite:Failure:NotSpecified");
			}
			if (connectionModeStatus != null && connectionModeStatus.m_Result.m_returnCode == OnlineMultiplayerConnectionModeConnectResult.eCancelledByUser)
			{
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.JoinCancelled_OnOfflineComplete));
			}
			else
			{
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.JoinFailed_OnOfflineComplete));
			}
		}
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x000C9150 File Offset: 0x000C7550
	private void OnPlayTogetherHostComplete(IConnectionModeSwitchStatus status)
	{
		this.m_playTogetherHostingState = FrontendInviteHandler.PlayTogetherHostingState.eIdle;
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_bBusy = false;
			if (null != this.m_progressBox)
			{
				this.m_progressBox.Hide();
				this.m_progressBox = null;
			}
		}
		else
		{
			this.m_FailedStatus = status.Clone();
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.JoinFailed_OnOfflineComplete));
		}
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x000C91BF File Offset: 0x000C75BF
	private void JoinCancelled_OnOfflineComplete(IConnectionModeSwitchStatus status)
	{
		this.m_bBusy = false;
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x000C91EB File Offset: 0x000C75EB
	private void JoinFailed_OnOfflineComplete(IConnectionModeSwitchStatus status)
	{
		this.JoinCancelled_OnOfflineComplete(status);
		this.ShowError(this.m_FailedStatus);
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x000C9200 File Offset: 0x000C7600
	private void ShowError(IConnectionModeSwitchStatus status)
	{
		if (!status.DisplayPlatformDialog())
		{
			T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
			if (null != dialog)
			{
				dialog.Initialize("Text.Warning", status.GetLocalisedResultDescription(), "Text.Button.Confirm", null, null, T17DialogBox.Symbols.Warning, true, false, false);
				dialog.Show();
			}
		}
	}

	// Token: 0x040021C3 RID: 8643
	private bool m_bBusy;

	// Token: 0x040021C4 RID: 8644
	private T17DialogBox m_progressBox;

	// Token: 0x040021C5 RID: 8645
	private AcceptInviteData m_InviteData = new AcceptInviteData();

	// Token: 0x040021C6 RID: 8646
	private bool m_InviteRequiresAllActiveLocalUsers;

	// Token: 0x040021C7 RID: 8647
	private FrontendInviteHandler.InviteProcessingState m_inviteProcessingState;

	// Token: 0x040021C8 RID: 8648
	private FrontendInviteHandler.PlayTogetherHostingState m_playTogetherHostingState;

	// Token: 0x040021C9 RID: 8649
	private OnlineMultiplayerSessionPlayTogetherHosting m_PlayTogetherData;

	// Token: 0x040021CA RID: 8650
	private T17DialogBox m_dialogBox;

	// Token: 0x040021CB RID: 8651
	private IConnectionModeSwitchStatus m_FailedStatus;

	// Token: 0x02000896 RID: 2198
	private enum InviteProcessingState
	{
		// Token: 0x040021CD RID: 8653
		eIdle,
		// Token: 0x040021CE RID: 8654
		eGoingOffline,
		// Token: 0x040021CF RID: 8655
		eDisengageNonRequiredUsers,
		// Token: 0x040021D0 RID: 8656
		eStartJoinProcess,
		// Token: 0x040021D1 RID: 8657
		eJoining
	}

	// Token: 0x02000897 RID: 2199
	private enum PlayTogetherHostingState
	{
		// Token: 0x040021D3 RID: 8659
		eIdle,
		// Token: 0x040021D4 RID: 8660
		eGoingOffline,
		// Token: 0x040021D5 RID: 8661
		eDisengageNonRequiredUsers,
		// Token: 0x040021D6 RID: 8662
		eStartHosting,
		// Token: 0x040021D7 RID: 8663
		eHosting
	}
}
