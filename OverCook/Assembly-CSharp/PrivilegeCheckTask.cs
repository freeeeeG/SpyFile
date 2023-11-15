using System;
using Team17.Online;

// Token: 0x02000876 RID: 2166
public class PrivilegeCheckTask : IMultiplayerTask
{
	// Token: 0x060029D6 RID: 10710 RVA: 0x000C3D90 File Offset: 0x000C2190
	public void Initialise(GamepadUser user)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlinePrivlegeCheckCoordinator = onlinePlatformManager.OnlineMultiplayerPrivilegeChecksCoordinator();
		this.m_PlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_UserToCheck = user;
		this.m_Status.currentUser = this.m_UserToCheck;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		if (this.m_UserToCheck != null)
		{
		}
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x000C3DFC File Offset: 0x000C21FC
	public void Reset()
	{
		this.m_UserToCheck = null;
		this.m_Status.currentUser = null;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		if (this.m_iOnlinePrivlegeCheckCoordinator != null && !this.m_iOnlinePrivlegeCheckCoordinator.IsIdle())
		{
			this.m_iOnlinePrivlegeCheckCoordinator.Cancel();
		}
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000C3E5C File Offset: 0x000C225C
	public void Start(object startData)
	{
		if (this.m_UserToCheck == null)
		{
			this.m_Status.privilegeCheckResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult>
			{
				m_returnCode = OnlineMultiplayerPrivilegeCheckResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			return;
		}
		if (this.m_iOnlinePrivlegeCheckCoordinator == null)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
			return;
		}
		this.m_LocalUserId = PrivilegeCheckCache.GetAllowedUser(this.m_UserToCheck);
		this.m_Status.currentUser = this.m_UserToCheck;
		if (this.m_LocalUserId != null)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
			this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			this.TryStart();
		}
		this.m_PlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000C3F50 File Offset: 0x000C2350
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		if (this.m_PlayerManager != null)
		{
			this.m_PlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		}
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x000C3F8C File Offset: 0x000C238C
	public void OnEngagementChanged(EngagementSlot _e, GamepadUser _prev, GamepadUser _new)
	{
		if (_new == null && _prev != null && _prev == this.m_UserToCheck && this.m_Status.GetProgress() != eConnectionModeSwitchProgress.Complete)
		{
			this.m_iOnlinePrivlegeCheckCoordinator.Cancel();
			this.m_Status.privilegeCheckResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult>
			{
				m_returnCode = OnlineMultiplayerPrivilegeCheckResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x000C4010 File Offset: 0x000C2410
	private void TryStart()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.NotStarted && this.m_iOnlinePrivlegeCheckCoordinator.IsIdle())
		{
			if (this.m_iOnlinePrivlegeCheckCoordinator.Start(this.m_UserToCheck, new OnlineMultiplayerPrivilegeCheckCallback(this.OnlineMultiplayerPrivilegeCheckCallback)))
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
				this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			}
			else
			{
				this.m_Status.privilegeCheckResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult>
				{
					m_returnCode = OnlineMultiplayerPrivilegeCheckResult.eGenericFailure
				};
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
		}
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x000C40AE File Offset: 0x000C24AE
	public void Update()
	{
		this.TryStart();
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x000C40B6 File Offset: 0x000C24B6
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x000C40BE File Offset: 0x000C24BE
	public object GetData()
	{
		return this.m_LocalUserId;
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000C40C8 File Offset: 0x000C24C8
	private void OnlineMultiplayerPrivilegeCheckCallback(OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult> result, OnlineMultiplayerLocalUserId localOnlineUser)
	{
		if (this.m_Status.Progress != eConnectionModeSwitchProgress.Complete)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.privilegeCheckResult = result;
			if (result != null && result.m_returnCode == OnlineMultiplayerPrivilegeCheckResult.eSuccess && localOnlineUser != null)
			{
				this.m_LocalUserId = localOnlineUser;
				this.m_Status.Result = eConnectionModeSwitchResult.Success;
				PrivilegeCheckCache.AddAllowedUser(this.m_UserToCheck, localOnlineUser);
			}
			else
			{
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
		}
	}

	// Token: 0x040020FC RID: 8444
	private GamepadUser m_UserToCheck;

	// Token: 0x040020FD RID: 8445
	private IOnlineMultiplayerPrivilegeChecksCoordinator m_iOnlinePrivlegeCheckCoordinator;

	// Token: 0x040020FE RID: 8446
	private OnlineMultiplayerLocalUserId m_LocalUserId;

	// Token: 0x040020FF RID: 8447
	private PrivilegeStatus m_Status = new PrivilegeStatus();

	// Token: 0x04002100 RID: 8448
	private IPlayerManager m_PlayerManager;
}
