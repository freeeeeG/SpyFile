using System;
using Team17.Online;
using Team17.Online.Multiplayer;

// Token: 0x0200083F RID: 2111
public class AcceptInviteAgent : ConnectionModeAgent
{
	// Token: 0x060028B4 RID: 10420 RVA: 0x000BF385 File Offset: 0x000BD785
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_CurrentAction.GetStatus();
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000BF394 File Offset: 0x000BD794
	public virtual bool Start(Server server, Client client, object data, GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		GameUtils.RequireManagerInterface<OvercookedEngagementController>().IsClientMode = true;
		AcceptInviteData acceptInviteData = data as AcceptInviteData;
		OnlineMultiplayerSessionInvite invite = acceptInviteData.Invite;
		ServerMessenger.OnServerStopped();
		this.m_LocalServer = server;
		this.m_LocalClient = client;
		this.m_LocalServer.Reset();
		this.m_LocalClient.Reset();
		PrivilegeCheckAllUsersForInviteTask privilegeCheckAllUsersForInviteTask = this.m_Tasks[1] as PrivilegeCheckAllUsersForInviteTask;
		privilegeCheckAllUsersForInviteTask.Initialise();
		AcceptInviteTask acceptInviteTask = this.m_Tasks[2] as AcceptInviteTask;
		acceptInviteTask.Initialise(invite);
		this.m_CurrentAction = new MultiplayerOperation();
		this.m_CurrentAction.Start(this.m_Tasks);
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.m_ProcessedCompletion = false;
		this.m_Callback = callback;
		return true;
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x000BF44C File Offset: 0x000BD84C
	public void Stop()
	{
		this.m_CurrentAction.Stop();
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x000BF459 File Offset: 0x000BD859
	public void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		if (this.m_Callback == callback)
		{
			this.m_Callback = null;
		}
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x000BF473 File Offset: 0x000BD873
	public object GetAgentData()
	{
		return null;
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x000BF476 File Offset: 0x000BD876
	public virtual void OnDisconnected()
	{
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x000BF478 File Offset: 0x000BD878
	public virtual void Update()
	{
		if (this.m_CurrentAction != null && !this.m_ProcessedCompletion)
		{
			this.m_CurrentAction.Update();
			if (this.m_CurrentAction.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
			{
				this.m_ProcessedCompletion = true;
				if (this.m_CurrentAction.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
				{
					NetworkSystemConfigurator.Client(this.m_LocalClient, this.m_CurrentAction.GetTaskData() as JoinData, this.m_iOnlineMultiplayerSessionCoordinator);
				}
				if (this.m_Callback != null)
				{
					this.m_Callback(this.GetStatus());
				}
			}
		}
	}

	// Token: 0x04002026 RID: 8230
	private IMultiplayerTask[] m_Tasks = new IMultiplayerTask[]
	{
		new LeaveSessionTask(),
		new PrivilegeCheckAllUsersForInviteTask(),
		new AcceptInviteTask()
	};

	// Token: 0x04002027 RID: 8231
	private Server m_LocalServer;

	// Token: 0x04002028 RID: 8232
	private Client m_LocalClient;

	// Token: 0x04002029 RID: 8233
	private MultiplayerOperation m_CurrentAction;

	// Token: 0x0400202A RID: 8234
	private bool m_ProcessedCompletion;

	// Token: 0x0400202B RID: 8235
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x0400202C RID: 8236
	private GenericVoid<IConnectionModeSwitchStatus> m_Callback;
}
