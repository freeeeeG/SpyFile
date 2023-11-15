using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000898 RID: 2200
public class GameplayInviteHandler : InviteHandler
{
	// Token: 0x06002AE1 RID: 10977 RVA: 0x000C9255 File Offset: 0x000C7655
	public void Start()
	{
		this.m_playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		if (InviteMonitor.GetAcceptedInvite() != null)
		{
			this.HandleAcceptedInvite(InviteMonitor.GetAcceptedInvite());
		}
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x000C9278 File Offset: 0x000C7678
	public void Stop()
	{
		if (null != this.m_dialogBox && this.m_dialogBox.IsActive)
		{
			this.m_dialogBox.Hide();
			this.m_Invite = null;
		}
		this.m_dialogBox = null;
		this.m_busy = false;
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x000C92C6 File Offset: 0x000C76C6
	public void Update()
	{
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x000C92C8 File Offset: 0x000C76C8
	public void HandleAcceptedInvite(AcceptInviteData invite)
	{
		if (this.m_dialogBox == null)
		{
			this.m_Invite = invite;
			this.m_busy = true;
			GamepadUser user = this.m_playerManager.GetUser(EngagementSlot.One);
			bool flag = invite.Invite.WasAcceptedBy(user);
			if (flag)
			{
				invite.User = user;
				if (UserSystemUtils.LocalUserCount(ClientUserSystem.m_Users, false) > 1U)
				{
					this.m_dialogBox = T17DialogBoxManager.GetDialog(false);
					if (this.m_dialogBox != null)
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
						dialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.OnConfirmedPrimaryUserOnly));
						T17DialogBox dialogBox2 = this.m_dialogBox;
						dialogBox2.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox2.OnDecline, new T17DialogBox.DialogEvent(this.OnConfirmedAllLocalUsers));
						T17DialogBox dialogBox3 = this.m_dialogBox;
						dialogBox3.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox3.OnCancel, new T17DialogBox.DialogEvent(this.OnDeclined));
						this.m_dialogBox.Show();
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
						dialogBox4.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox4.OnConfirm, new T17DialogBox.DialogEvent(this.OnConfirmedPrimaryUserOnly));
						T17DialogBox dialogBox5 = this.m_dialogBox;
						dialogBox5.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox5.OnDecline, new T17DialogBox.DialogEvent(this.OnDeclined));
						this.m_dialogBox.Show();
					}
					else
					{
						this.OnDeclined();
					}
				}
				else
				{
					this.OnConfirmedPrimaryUserOnly();
				}
			}
			else
			{
				this.OnConfirmed();
			}
		}
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x000C9572 File Offset: 0x000C7972
	public void HandlePlayTogetherHost(OnlineMultiplayerSessionPlayTogetherHosting host)
	{
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnOfflineComplete));
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x000C9588 File Offset: 0x000C7988
	public bool IsBusy()
	{
		return this.m_busy;
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x000C9590 File Offset: 0x000C7990
	public bool IsAwaitingUserInput()
	{
		return null != this.m_dialogBox;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000C95A0 File Offset: 0x000C79A0
	private void OnOfflineComplete(IConnectionModeSwitchStatus status)
	{
		this.m_dialogBox = null;
		ServerUserSystem.UnlockEngagement();
		if (this.m_playerManager == null)
		{
			this.m_playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		}
		if (this.m_Invite != null && !this.m_Invite.Invite.WasAcceptedBy(this.m_playerManager.GetUser(EngagementSlot.One)))
		{
			UserSystemUtils.RemoveAllSplitPadGuestUsers();
			for (int i = 0; i < 4; i++)
			{
				GamepadUser user = this.m_playerManager.GetUser((EngagementSlot)i);
				if (null != user)
				{
					this.m_playerManager.DisengagePad((EngagementSlot)i);
				}
			}
			this.m_Invite.FromIIS = true;
		}
		if (this.m_Invite == null || !this.m_Invite.FromIIS)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
			ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
		}
		this.m_busy = false;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000C9678 File Offset: 0x000C7A78
	private void OnConfirmedPrimaryUserOnly()
	{
		if (this.m_busy)
		{
			this.m_Invite.JoinLocalUsersChoice = AcceptInviteData.LocalUsersChoice.ePrimary;
			this.OnConfirmed();
		}
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x000C9697 File Offset: 0x000C7A97
	private void OnConfirmedAllLocalUsers()
	{
		if (this.m_busy)
		{
			this.m_Invite.JoinLocalUsersChoice = AcceptInviteData.LocalUsersChoice.eAll;
			this.OnConfirmed();
		}
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x000C96B6 File Offset: 0x000C7AB6
	private void OnConfirmed()
	{
		if (this.m_busy)
		{
			this.m_dialogBox = null;
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnOfflineComplete));
		}
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x000C96DE File Offset: 0x000C7ADE
	private void OnDeclined()
	{
		InviteMonitor.ClearInvite();
		this.m_Invite = null;
		this.m_dialogBox = null;
		this.m_busy = false;
	}

	// Token: 0x040021D8 RID: 8664
	private AcceptInviteData m_Invite;

	// Token: 0x040021D9 RID: 8665
	private T17DialogBox m_dialogBox;

	// Token: 0x040021DA RID: 8666
	private IPlayerManager m_playerManager;

	// Token: 0x040021DB RID: 8667
	private bool m_busy;
}
