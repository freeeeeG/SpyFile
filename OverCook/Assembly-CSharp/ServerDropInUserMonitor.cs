using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000995 RID: 2453
public class ServerDropInUserMonitor
{
	// Token: 0x06002FDC RID: 12252 RVA: 0x000E0F50 File Offset: 0x000DF350
	public void Initialise(GenericVoid<IConnectionModeSwitchStatus> onStarted, GenericVoid<IConnectionModeSwitchStatus> onCompleted)
	{
		this.m_Checker.Initialise(new GenericVoid<GamepadUser, EngagementSlot, IConnectionModeSwitchStatus>(this.OnUserCheckCompleted));
		this.m_PlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.OnEngagementPrivilegeCheckStarted = onStarted;
		this.OnEngagementPrivilegeCheckCompleted = onCompleted;
		ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Combine(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x000E0FBF File Offset: 0x000DF3BF
	public void Shutdown()
	{
		ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Remove(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x000E0FE1 File Offset: 0x000DF3E1
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Checker.GetStatus();
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x000E0FF0 File Offset: 0x000DF3F0
	public void Update()
	{
		if (ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && ConnectionModeSwitcher.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_Checker.Update();
			ServerDropInUserMonitor.DropInStatus dropInStatus = this.m_DropInStatus;
			if (dropInStatus != ServerDropInUserMonitor.DropInStatus.Idle)
			{
				if (dropInStatus != ServerDropInUserMonitor.DropInStatus.Triggered)
				{
					if (dropInStatus == ServerDropInUserMonitor.DropInStatus.Busy)
					{
						this.UpdateBusy();
					}
				}
				else
				{
					this.UpdateTriggered();
				}
			}
		}
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x000E1063 File Offset: 0x000DF463
	public void TriggerDropIn()
	{
		if (this.m_DropInStatus == ServerDropInUserMonitor.DropInStatus.Idle && ConnectionModeSwitcher.GetRequestedConnectionState() != NetConnectionState.Offline && ConnectionStatus.IsHost())
		{
			this.m_DropInStatus = ServerDropInUserMonitor.DropInStatus.Triggered;
		}
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x000E108B File Offset: 0x000DF48B
	private void SetDropInStatus(ServerDropInUserMonitor.DropInStatus status)
	{
		this.m_DropInStatus = status;
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x000E1094 File Offset: 0x000DF494
	private void UpdateTriggered()
	{
		if (this.m_DropInStatus != ServerDropInUserMonitor.DropInStatus.Busy && this.m_Checker.GetStatus().GetProgress() != eConnectionModeSwitchProgress.InProgress)
		{
			this.m_Checker.Start(null);
			eConnectionModeSwitchProgress progress = this.m_Checker.GetStatus().GetProgress();
			if (progress != eConnectionModeSwitchProgress.NotStarted)
			{
				if (progress != eConnectionModeSwitchProgress.InProgress)
				{
					if (progress == eConnectionModeSwitchProgress.Complete)
					{
						this.SetDropInStatus(ServerDropInUserMonitor.DropInStatus.Idle);
					}
				}
				else
				{
					if (this.OnEngagementPrivilegeCheckStarted != null)
					{
						this.OnEngagementPrivilegeCheckStarted(this.GetStatus());
					}
					this.SetDropInStatus(ServerDropInUserMonitor.DropInStatus.Busy);
				}
			}
		}
	}

	// Token: 0x06002FE3 RID: 12259 RVA: 0x000E1132 File Offset: 0x000DF532
	private void UpdateBusy()
	{
		if (this.m_Checker.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
		{
			if (this.OnEngagementPrivilegeCheckCompleted != null)
			{
				this.OnEngagementPrivilegeCheckCompleted(this.GetStatus());
			}
			this.SetDropInStatus(ServerDropInUserMonitor.DropInStatus.Idle);
		}
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x000E1170 File Offset: 0x000DF570
	private void OnUserCheckCompleted(GamepadUser user, EngagementSlot slot, IConnectionModeSwitchStatus status)
	{
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			FastList<User> users = ServerUserSystem.m_Users;
			User.MachineID s_LocalMachineId = ServerUserSystem.s_LocalMachineId;
			if (UserSystemUtils.FindUser(users, null, s_LocalMachineId, slot, TeamID.Count, User.SplitStatus.Count) == null)
			{
				bool bLocal = true;
				s_LocalMachineId = ServerUserSystem.s_LocalMachineId;
				ServerUserSystem.AddUser(bLocal, s_LocalMachineId, null, 0U, 0U, slot, PadSide.Both, TeamID.None, User.PartyPersistance.Remain, null, 127U, null, User.SplitStatus.NotSplit);
			}
		}
		else
		{
			this.m_PlayerManager.DisengagePad(slot);
			PrivilegeCheckCache.RemoveAllowedUser(user);
			this.TriggerDropIn();
		}
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000E11E4 File Offset: 0x000DF5E4
	private void OnUserRemoved(User user)
	{
		if (user.IsLocal)
		{
			OnlineMultiplayerLocalUserId allowedUser = PrivilegeCheckCache.GetAllowedUser(user.GamepadUser);
			if (LocalDroppedInUserCache.HasBeenDroppedIn(allowedUser))
			{
				this.m_iOnlineMultiplayerSessionCoordinator.RemoveNonPrimaryLocalUser(allowedUser);
			}
			LocalDroppedInUserCache.RemoveDroppedInUser(allowedUser);
			PrivilegeCheckCache.RemoveAllowedUser(user.GamepadUser);
		}
	}

	// Token: 0x0400266D RID: 9837
	private GenericVoid<IConnectionModeSwitchStatus> OnEngagementPrivilegeCheckStarted;

	// Token: 0x0400266E RID: 9838
	private GenericVoid<IConnectionModeSwitchStatus> OnEngagementPrivilegeCheckCompleted;

	// Token: 0x0400266F RID: 9839
	private CheckPrivilegesAndDropInAllLocalUsersTask m_Checker = new CheckPrivilegesAndDropInAllLocalUsersTask();

	// Token: 0x04002670 RID: 9840
	private IPlayerManager m_PlayerManager;

	// Token: 0x04002671 RID: 9841
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x04002672 RID: 9842
	private ServerDropInUserMonitor.DropInStatus m_DropInStatus;

	// Token: 0x02000996 RID: 2454
	private enum DropInStatus
	{
		// Token: 0x04002674 RID: 9844
		Idle,
		// Token: 0x04002675 RID: 9845
		Triggered,
		// Token: 0x04002676 RID: 9846
		Busy
	}
}
