using System;
using Team17.Online;
using Team17.Online.Multiplayer;
using UnityEngine;

// Token: 0x02000853 RID: 2131
public class ServerAgent : ConnectionModeAgent
{
	// Token: 0x0600290F RID: 10511 RVA: 0x000C08EE File Offset: 0x000BECEE
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_CurrentAction.GetStatus();
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x000C08FC File Offset: 0x000BECFC
	private void Setup()
	{
		if (!this.m_bSetup)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
			this.m_iOnlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
			this.m_StartServerTasks = new IMultiplayerTask[]
			{
				this.m_LeaveSessionTask,
				this.m_ConnectionModeTask,
				this.m_ResetServerUsers,
				this.m_PrivilegeCheckAllUsersTask,
				this.m_CreateSessionTask,
				this.m_AddLocalUsersTask
			};
			this.m_ModifyServerTasks = new IMultiplayerTask[]
			{
				this.m_ModifySessionTask
			};
			this.m_bSetup = true;
			this.m_bCalledBack = false;
		}
		this.m_bCalledBack = false;
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000C09A0 File Offset: 0x000BEDA0
	public virtual bool Start(Server server, Client client, object data, GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		this.Setup();
		ServerOptions options = (ServerOptions)data;
		try
		{
		}
		catch (UnityException ex)
		{
			if (ex != null)
			{
			}
		}
		bool flag = this.m_iOnlineMultiplayerSessionCoordinator.IsHost() && (this.m_iOnlineMultiplayerConnectionModeCoordinator == null || this.m_iOnlineMultiplayerConnectionModeCoordinator.Mode() == options.connectionMode);
		if (options.gameMode == GameMode.COUNT || (!flag && options.hostUser == null))
		{
			if (callback != null)
			{
				OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult> privilegeCheckResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult>
				{
					m_returnCode = OnlineMultiplayerPrivilegeCheckResult.eGenericFailure,
					m_usePlatformError = false
				};
				callback(new PrivilegeStatus
				{
					Result = eConnectionModeSwitchResult.Failure,
					privilegeCheckResult = privilegeCheckResult
				});
			}
			this.m_bCalledBack = true;
			return false;
		}
		this.m_Callback = callback;
		IConnectionModeSwitchStatus status = this.GetStatus();
		bool flag2 = status.GetProgress() == eConnectionModeSwitchProgress.Complete && status.GetResult() == eConnectionModeSwitchResult.Success;
		if (options.gameMode == this.m_ServerOptions.gameMode && options.visibility == this.m_ServerOptions.visibility && options.hostUser == this.m_ServerOptions.hostUser && options.connectionMode == this.m_ServerOptions.connectionMode && options.playTogetherHost == this.m_ServerOptions.playTogetherHost && ConnectionStatus.IsInSession() && ConnectionStatus.IsHost())
		{
			if (flag2)
			{
				if (this.m_Callback != null)
				{
					this.m_Callback(this.GetStatus());
				}
				return true;
			}
			if (status.GetProgress() == eConnectionModeSwitchProgress.InProgress && status.GetResult() == eConnectionModeSwitchResult.NotAvailableYet)
			{
				return true;
			}
		}
		this.m_ServerOptions.gameMode = options.gameMode;
		this.m_ServerOptions.visibility = options.visibility;
		this.m_ServerOptions.hostUser = options.hostUser;
		this.m_ServerOptions.connectionMode = options.connectionMode;
		this.m_ServerOptions.playTogetherHost = options.playTogetherHost;
		ServerSessionPropertyValuesProvider.SetGameMode(this.m_ServerOptions.gameMode);
		OnlineMultiplayerSessionVisibility visibility = options.visibility;
		if (flag)
		{
			this.StartModify(options);
		}
		else
		{
			this.StartCreate(server, client, options);
		}
		if (this.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && this.m_Callback != null)
		{
			this.m_Callback(this.GetStatus());
		}
		return true;
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000C0C38 File Offset: 0x000BF038
	public void Stop()
	{
		this.m_CurrentAction.Stop();
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000C0C45 File Offset: 0x000BF045
	public void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		if (this.m_Callback == callback)
		{
			this.m_Callback = null;
		}
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000C0C5F File Offset: 0x000BF05F
	public object GetAgentData()
	{
		return this.m_ServerOptions;
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000C0C6C File Offset: 0x000BF06C
	private void StartModify(ServerOptions options)
	{
		OnlineMultiplayerSessionVisibility visibility = options.visibility;
		this.m_ModifySessionTask.Initialise(this.m_iOnlineMultiplayerSessionCoordinator, visibility, ServerSessionPropertyValuesProvider.GetValues());
		this.m_CurrentAction.Start(this.m_ModifyServerTasks);
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000C0CAC File Offset: 0x000BF0AC
	private void StartCreate(Server server, Client client, ServerOptions options)
	{
		ServerMessenger.OnServerStopped();
		this.m_LocalServer = server;
		this.m_LocalClient = client;
		this.m_LocalServer.Reset();
		this.m_LocalClient.Reset();
		this.m_LocalServer.Initialise(true);
		this.m_LocalClient.Initialise(false);
		this.m_PrivilegeCheckAllUsersTask.Initialise(options.hostUser);
		this.m_ConnectionModeTask.Initialise(options.hostUser, options.connectionMode);
		this.m_ResetServerUsers.Initialise(server);
		this.m_AddLocalUsersTask.Initialise(null);
		this.m_CreateSessionTask.Initialise(server, client, options.visibility, options.playTogetherHost, ServerSessionPropertyValuesProvider.GetValues());
		options.playTogetherHost = null;
		this.m_ServerOptions.playTogetherHost = null;
		this.m_CurrentAction.Start(this.m_StartServerTasks);
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x000C0D82 File Offset: 0x000BF182
	public virtual void OnDisconnected()
	{
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x000C0D84 File Offset: 0x000BF184
	public virtual void Update()
	{
		if (this.m_CurrentAction != null && !this.m_bCalledBack)
		{
			this.m_CurrentAction.Update();
			if (this.m_CurrentAction.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
			{
				if (this.m_Callback != null)
				{
					this.m_Callback(this.GetStatus());
				}
				this.m_bCalledBack = true;
			}
		}
	}

	// Token: 0x04002083 RID: 8323
	private ServerOptions m_ServerOptions = default(ServerOptions);

	// Token: 0x04002084 RID: 8324
	private LeaveSessionTask m_LeaveSessionTask = new LeaveSessionTask();

	// Token: 0x04002085 RID: 8325
	private SetupConnectionModeTask m_ConnectionModeTask = new SetupConnectionModeTask();

	// Token: 0x04002086 RID: 8326
	private ResetServerUsersTask m_ResetServerUsers = new ResetServerUsersTask();

	// Token: 0x04002087 RID: 8327
	private PrivilegeCheckAllUsersTask m_PrivilegeCheckAllUsersTask = new PrivilegeCheckAllUsersTask();

	// Token: 0x04002088 RID: 8328
	private CreateSessionTask m_CreateSessionTask = new CreateSessionTask();

	// Token: 0x04002089 RID: 8329
	private CheckPrivilegesAndDropInAllLocalUsersTask m_AddLocalUsersTask = new CheckPrivilegesAndDropInAllLocalUsersTask();

	// Token: 0x0400208A RID: 8330
	private IMultiplayerTask[] m_StartServerTasks;

	// Token: 0x0400208B RID: 8331
	private ModifySessionTask m_ModifySessionTask = new ModifySessionTask();

	// Token: 0x0400208C RID: 8332
	private IMultiplayerTask[] m_ModifyServerTasks;

	// Token: 0x0400208D RID: 8333
	private Server m_LocalServer;

	// Token: 0x0400208E RID: 8334
	private Client m_LocalClient;

	// Token: 0x0400208F RID: 8335
	private MultiplayerOperation m_CurrentAction = new MultiplayerOperation();

	// Token: 0x04002090 RID: 8336
	private GenericVoid<IConnectionModeSwitchStatus> m_Callback;

	// Token: 0x04002091 RID: 8337
	private IOnlineMultiplayerConnectionModeCoordinator m_iOnlineMultiplayerConnectionModeCoordinator;

	// Token: 0x04002092 RID: 8338
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x04002093 RID: 8339
	private bool m_bSetup;

	// Token: 0x04002094 RID: 8340
	private bool m_bCalledBack;
}
