using System;
using Team17.Online;
using Team17.Online.Multiplayer;

// Token: 0x02000845 RID: 2117
public class JoinEnumeratedRoomAgent : ConnectionModeAgent
{
	// Token: 0x060028CC RID: 10444 RVA: 0x000BF6D2 File Offset: 0x000BDAD2
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_CurrentAction.GetStatus();
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x000BF6E0 File Offset: 0x000BDAE0
	public virtual bool Start(Server server, Client client, object data, GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		GameUtils.RequireManagerInterface<OvercookedEngagementController>().IsClientMode = true;
		JoinEnumeratedRoomOptions joinEnumeratedRoomOptions = data as JoinEnumeratedRoomOptions;
		ServerMessenger.OnServerStopped();
		this.m_LocalServer = server;
		this.m_LocalClient = client;
		this.m_LocalServer.Reset();
		this.m_LocalClient.Reset();
		PrivilegeCheckAllUsersForJoinEnumeratedRoomTask privilegeCheckAllUsersForJoinEnumeratedRoomTask = this.m_Tasks[1] as PrivilegeCheckAllUsersForJoinEnumeratedRoomTask;
		privilegeCheckAllUsersForJoinEnumeratedRoomTask.Initialise();
		JoinEnumeratedRoomTask joinEnumeratedRoomTask = this.m_Tasks[2] as JoinEnumeratedRoomTask;
		joinEnumeratedRoomTask.Initialise(joinEnumeratedRoomOptions.Room, NetConnectionState.JoinEnumeratedRoom);
		this.m_CurrentAction = new MultiplayerOperation();
		this.m_CurrentAction.Start(this.m_Tasks);
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.m_ProcessedCompletion = false;
		this.m_Callback = callback;
		return true;
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000BF795 File Offset: 0x000BDB95
	public void Stop()
	{
		this.m_CurrentAction.Stop();
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x000BF7A2 File Offset: 0x000BDBA2
	public void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		if (this.m_Callback == callback)
		{
			this.m_Callback = null;
		}
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x000BF7BC File Offset: 0x000BDBBC
	public object GetAgentData()
	{
		return null;
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x000BF7BF File Offset: 0x000BDBBF
	public virtual void OnDisconnected()
	{
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000BF7C4 File Offset: 0x000BDBC4
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

	// Token: 0x04002046 RID: 8262
	private IMultiplayerTask[] m_Tasks = new IMultiplayerTask[]
	{
		new LeaveSessionTask(),
		new PrivilegeCheckAllUsersForJoinEnumeratedRoomTask(),
		new JoinEnumeratedRoomTask()
	};

	// Token: 0x04002047 RID: 8263
	private Server m_LocalServer;

	// Token: 0x04002048 RID: 8264
	private Client m_LocalClient;

	// Token: 0x04002049 RID: 8265
	private MultiplayerOperation m_CurrentAction;

	// Token: 0x0400204A RID: 8266
	private bool m_ProcessedCompletion;

	// Token: 0x0400204B RID: 8267
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x0400204C RID: 8268
	private GenericVoid<IConnectionModeSwitchStatus> m_Callback;
}
