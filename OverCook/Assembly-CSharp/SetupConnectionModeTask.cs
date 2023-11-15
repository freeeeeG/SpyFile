using System;
using Team17.Online;

// Token: 0x0200087A RID: 2170
public class SetupConnectionModeTask : IMultiplayerTask
{
	// Token: 0x060029F3 RID: 10739 RVA: 0x000C44C3 File Offset: 0x000C28C3
	public void Initialise(GamepadUser user, OnlineMultiplayerConnectionMode mode)
	{
		this.m_user = user;
		this.m_connectionMode = mode;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000C44EC File Offset: 0x000C28EC
	public void Start(object startData)
	{
		this.m_PassthroughObject = startData;
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_connectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
		if (this.m_connectionModeCoordinator == null)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
			return;
		}
		OnlineMultiplayerConnectionMode onlineMultiplayerConnectionMode = this.m_connectionModeCoordinator.Mode();
		if (onlineMultiplayerConnectionMode == this.m_connectionMode)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
			return;
		}
		if (onlineMultiplayerConnectionMode != OnlineMultiplayerConnectionMode.eNone)
		{
			this.m_connectionModeCoordinator.Disconnect();
		}
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		this.TryStart();
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x000C4596 File Offset: 0x000C2996
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000C45B0 File Offset: 0x000C29B0
	private void TryStart()
	{
		if (this.m_connectionModeCoordinator.IsIdle())
		{
			if (this.m_connectionModeCoordinator.Connect(this.m_user, this.m_connectionMode, new OnlineMultiplayerConnectionModeConnectCallback(this.OnlineMultiplayerConnectionModeConnectCallback)))
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
				this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			}
			else
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
		}
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x000C462C File Offset: 0x000C2A2C
	public void Update()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.NotStarted && this.m_Status.Result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			this.TryStart();
		}
		if (this.m_connectionMode == OnlineMultiplayerConnectionMode.eNone && this.m_connectionModeCoordinator.Mode() == this.m_connectionMode)
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x000C4698 File Offset: 0x000C2A98
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000C46A0 File Offset: 0x000C2AA0
	public object GetData()
	{
		return this.m_PassthroughObject;
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000C46A8 File Offset: 0x000C2AA8
	public GamepadUser GetCurrentUser()
	{
		return this.m_user;
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000C46B0 File Offset: 0x000C2AB0
	private void OnlineMultiplayerConnectionModeConnectCallback(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeConnectResult> result)
	{
		this.m_Status.m_Result = result;
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		if (result.m_returnCode == OnlineMultiplayerConnectionModeConnectResult.eSuccess)
		{
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
	}

	// Token: 0x0400210B RID: 8459
	private OnlineMultiplayerConnectionMode m_connectionMode;

	// Token: 0x0400210C RID: 8460
	private IOnlineMultiplayerConnectionModeCoordinator m_connectionModeCoordinator;

	// Token: 0x0400210D RID: 8461
	private ConnectionModeStatus m_Status = new ConnectionModeStatus();

	// Token: 0x0400210E RID: 8462
	private GamepadUser m_user;

	// Token: 0x0400210F RID: 8463
	private object m_PassthroughObject;
}
