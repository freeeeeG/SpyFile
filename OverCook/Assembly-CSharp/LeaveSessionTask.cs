using System;
using Team17.Online;

// Token: 0x0200086D RID: 2157
public class LeaveSessionTask : IMultiplayerTask
{
	// Token: 0x060029A3 RID: 10659 RVA: 0x000C3222 File Offset: 0x000C1622
	public void Initialise(bool bWait)
	{
		this.m_bWait = bWait;
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000C322C File Offset: 0x000C162C
	public void Start(object startData)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		if (this.m_iOnlineMultiplayerSessionCoordinator == null)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_iOnlineMultiplayerSessionCoordinator.Leave();
			if (!this.m_bWait || this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Success;
			}
			else
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
				this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			}
		}
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000C32CE File Offset: 0x000C16CE
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000C32E8 File Offset: 0x000C16E8
	public void Update()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.InProgress && this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000C3323 File Offset: 0x000C1723
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000C332B File Offset: 0x000C172B
	public object GetData()
	{
		return null;
	}

	// Token: 0x040020DE RID: 8414
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040020DF RID: 8415
	private LeaveSessionStatus m_Status = new LeaveSessionStatus();

	// Token: 0x040020E0 RID: 8416
	private bool m_bWait = true;
}
