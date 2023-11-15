using System;
using Team17.Online;

// Token: 0x02000867 RID: 2151
public class DropInLocalUserTask : IMultiplayerTask
{
	// Token: 0x0600297C RID: 10620 RVA: 0x000C292C File Offset: 0x000C0D2C
	public void Initialise(GamepadUser userToDropIn)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.m_PlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_UserToCheck = userToDropIn;
		this.m_Status.currentUser = this.m_LocalUserId;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x000C2986 File Offset: 0x000C0D86
	public void Reset()
	{
		this.m_LocalUserId = null;
		this.m_UserToCheck = null;
		this.m_Status.currentUser = null;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x000C29BC File Offset: 0x000C0DBC
	public void Start(object startData)
	{
		this.m_LocalUserId = (OnlineMultiplayerLocalUserId)startData;
		this.m_Status.currentUser = this.m_LocalUserId;
		if (this.m_iOnlineMultiplayerSessionCoordinator == null || LocalDroppedInUserCache.HasBeenDroppedIn(this.m_LocalUserId))
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
			this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
			{
				m_returnCode = OnlineMultiplayerSessionJoinResult.eSuccess
			};
		}
		else
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
			this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			this.TryStart();
		}
		this.m_PlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
	}

	// Token: 0x0600297F RID: 10623 RVA: 0x000C2A6C File Offset: 0x000C0E6C
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		this.m_PlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x000C2AA0 File Offset: 0x000C0EA0
	public void OnEngagementChanged(EngagementSlot _e, GamepadUser _prev, GamepadUser _new)
	{
		if (_new == null && _prev != null && _prev == this.m_UserToCheck && this.m_Status.GetProgress() != eConnectionModeSwitchProgress.Complete && this.m_iOnlineMultiplayerSessionCoordinator != null)
		{
			this.m_iOnlineMultiplayerSessionCoordinator.RemoveNonPrimaryLocalUser(this.m_LocalUserId);
			this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
			{
				m_returnCode = OnlineMultiplayerSessionJoinResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000C2B36 File Offset: 0x000C0F36
	public void Update()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.NotStarted && this.m_Status.Result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			this.TryStart();
		}
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x000C2B5E File Offset: 0x000C0F5E
	public object GetData()
	{
		return null;
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x000C2B61 File Offset: 0x000C0F61
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x06002984 RID: 10628 RVA: 0x000C2B6C File Offset: 0x000C0F6C
	public void TryStart()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.NotStarted)
		{
			if (this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
			else
			{
				switch (this.m_iOnlineMultiplayerSessionCoordinator.AddNonPrimaryLocalUser(this.m_LocalUserId, new OnlineMultiplayerSessionAddNonPrimaryLocalUserCallback(this.OnlineMultiplayerSessionAddNonPrimaryLocalUserCallback)))
				{
				case OnlineMultiplayerNonPrimaryLocalUserChangeResult.eStarted:
					this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
					this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
					break;
				case OnlineMultiplayerNonPrimaryLocalUserChangeResult.eNotPossible:
					this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
					this.m_Status.Result = eConnectionModeSwitchResult.Failure;
					this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
					{
						m_returnCode = OnlineMultiplayerSessionJoinResult.eGenericFailure
					};
					break;
				case OnlineMultiplayerNonPrimaryLocalUserChangeResult.eComplete:
					LocalDroppedInUserCache.AddDroppedInUser(this.m_LocalUserId);
					this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
					this.m_Status.Result = eConnectionModeSwitchResult.Success;
					this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
					{
						m_returnCode = OnlineMultiplayerSessionJoinResult.eSuccess
					};
					break;
				}
			}
		}
	}

	// Token: 0x06002985 RID: 10629 RVA: 0x000C2C8C File Offset: 0x000C108C
	private void OnlineMultiplayerSessionAddNonPrimaryLocalUserCallback(OnlineMultiplayerLocalUserId localUserId, OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult> result)
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_Status.sessionJoinResult = result;
		if (result != null && result.m_returnCode == OnlineMultiplayerSessionJoinResult.eSuccess && localUserId != null)
		{
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
			LocalDroppedInUserCache.AddDroppedInUser(this.m_LocalUserId);
		}
		else
		{
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
	}

	// Token: 0x040020C7 RID: 8391
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040020C8 RID: 8392
	private OnlineMultiplayerLocalUserId m_LocalUserId;

	// Token: 0x040020C9 RID: 8393
	private GamepadUser m_UserToCheck;

	// Token: 0x040020CA RID: 8394
	private JoinSessionStatus m_Status = new JoinSessionStatus();

	// Token: 0x040020CB RID: 8395
	private IPlayerManager m_PlayerManager;
}
