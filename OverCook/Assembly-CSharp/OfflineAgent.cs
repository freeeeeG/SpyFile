using System;
using Team17.Online;
using Team17.Online.Multiplayer;

// Token: 0x0200084B RID: 2123
public class OfflineAgent : ConnectionModeAgent
{
	// Token: 0x060028DF RID: 10463 RVA: 0x000BFD20 File Offset: 0x000BE120
	public virtual bool Start(Server server, Client client, object data, GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		GameUtils.RequireManagerInterface<OvercookedEngagementController>().IsClientMode = false;
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_Callback = callback;
		bool flag = false;
		OnlineMultiplayerConnectionMode onlineMultiplayerConnectionMode = OnlineMultiplayerConnectionMode.eNone;
		OnlineMultiplayerConnectionMode onlineMultiplayerConnectionMode2 = OnlineMultiplayerConnectionMode.eNone;
		IOnlineMultiplayerConnectionModeCoordinator onlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
		if (onlineMultiplayerConnectionModeCoordinator != null)
		{
			onlineMultiplayerConnectionMode = onlineMultiplayerConnectionModeCoordinator.Mode();
			onlineMultiplayerConnectionMode2 = onlineMultiplayerConnectionMode;
			flag = true;
		}
		if (data != null)
		{
			OfflineOptions offlineOptions = (OfflineOptions)data;
			if (offlineOptions.connectionMode != null)
			{
				onlineMultiplayerConnectionMode2 = offlineOptions.connectionMode.Value;
			}
		}
		bool flag2 = false;
		if (flag)
		{
			flag2 = (onlineMultiplayerConnectionMode2 == onlineMultiplayerConnectionMode);
		}
		if (flag2 && this.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && this.GetStatus().GetResult() == eConnectionModeSwitchResult.Success && !ConnectionStatus.IsInSession() && (data == null || ((OfflineOptions)data).eAdditionalAction == OfflineOptions.AdditionalAction.None))
		{
			if (callback != null)
			{
				callback(this.GetStatus());
			}
			return true;
		}
		if (this.GetStatus().GetProgress() == eConnectionModeSwitchProgress.InProgress && this.GetStatus().GetResult() == eConnectionModeSwitchResult.NotAvailableYet)
		{
			return true;
		}
		this.m_bCalledBack = false;
		this.m_LocalServer = server;
		this.m_LocalClient = client;
		ServerMessenger.OnServerStopped();
		this.m_LocalServer.Reset();
		this.m_LocalClient.Reset();
		this.m_LocalServer.Initialise(false);
		this.m_LocalClient.Initialise(false);
		this.m_LeaveSessionTask.Initialise(false);
		PrivilegeCheckCache.Clear();
		if (data != null)
		{
			OfflineOptions offlineOptions2 = (OfflineOptions)data;
			OfflineOptions.AdditionalAction eAdditionalAction = offlineOptions2.eAdditionalAction;
			if (eAdditionalAction != OfflineOptions.AdditionalAction.PrivilegeCheckAllUsersAndSearchForGames)
			{
				if (eAdditionalAction != OfflineOptions.AdditionalAction.PrivilegeCheckAllUsers)
				{
					if (eAdditionalAction == OfflineOptions.AdditionalAction.None)
					{
						this.m_Tasks = new IMultiplayerTask[]
						{
							this.m_LeaveSessionTask,
							this.m_SetupConnectionModeTask,
							this.m_OfflineTask
						};
					}
				}
				else
				{
					this.m_Tasks = new IMultiplayerTask[]
					{
						this.m_LeaveSessionTask,
						this.m_SetupConnectionModeTask,
						this.m_OfflineTask,
						this.m_PrivilegeCheckAllUsersTask
					};
					this.m_PrivilegeCheckAllUsersTask.Initialise(offlineOptions2.hostUser);
				}
			}
			else
			{
				this.m_Tasks = new IMultiplayerTask[]
				{
					this.m_LeaveSessionTask,
					this.m_SetupConnectionModeTask,
					this.m_OfflineTask,
					this.m_PrivilegeCheckAllUsersTask,
					this.m_SearchTask
				};
				this.m_PrivilegeCheckAllUsersTask.Initialise(offlineOptions2.hostUser);
				SearchSessionPropertyValuesProvider.SetGameMode(offlineOptions2.searchGameMode);
				this.m_SearchTask.Initialise(SearchSessionPropertyValuesProvider.GetValues());
			}
			this.m_SetupConnectionModeTask.Initialise(offlineOptions2.hostUser, onlineMultiplayerConnectionMode2);
		}
		else
		{
			this.m_Tasks = new IMultiplayerTask[]
			{
				this.m_LeaveSessionTask,
				this.m_SetupConnectionModeTask,
				this.m_OfflineTask
			};
			this.m_SetupConnectionModeTask.Initialise(this.m_SetupConnectionModeTask.GetCurrentUser(), onlineMultiplayerConnectionMode2);
		}
		this.m_OfflineTask.Initialise(server, client);
		this.m_CurrentAction.Start(this.m_Tasks);
		return true;
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000C0008 File Offset: 0x000BE408
	public void Stop()
	{
		this.m_CurrentAction.Stop();
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x000C0015 File Offset: 0x000BE415
	public void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		if (this.m_Callback == callback)
		{
			this.m_Callback = null;
		}
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000C002F File Offset: 0x000BE42F
	public object GetAgentData()
	{
		return this.m_CurrentAction.GetTaskData();
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000C003C File Offset: 0x000BE43C
	public virtual void OnDisconnected()
	{
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x000C0040 File Offset: 0x000BE440
	public virtual void Update()
	{
		IConnectionModeSwitchStatus status = this.GetStatus();
		if (this.m_CurrentAction != null && !this.m_bCalledBack)
		{
			this.m_CurrentAction.Update();
			if (status.GetProgress() == eConnectionModeSwitchProgress.Complete)
			{
				this.m_bCalledBack = true;
				if (status.GetResult() == eConnectionModeSwitchResult.Success)
				{
				}
				if (this.m_Callback != null)
				{
					this.m_Callback(this.GetStatus());
				}
			}
		}
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x000C00B0 File Offset: 0x000BE4B0
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_CurrentAction.GetStatus();
	}

	// Token: 0x0400205F RID: 8287
	private IMultiplayerTask[] m_Tasks;

	// Token: 0x04002060 RID: 8288
	private Server m_LocalServer;

	// Token: 0x04002061 RID: 8289
	private Client m_LocalClient;

	// Token: 0x04002062 RID: 8290
	private GenericVoid<IConnectionModeSwitchStatus> m_Callback;

	// Token: 0x04002063 RID: 8291
	private bool m_bCalledBack;

	// Token: 0x04002064 RID: 8292
	private MultiplayerOperation m_CurrentAction = new MultiplayerOperation();

	// Token: 0x04002065 RID: 8293
	private LeaveSessionTask m_LeaveSessionTask = new LeaveSessionTask();

	// Token: 0x04002066 RID: 8294
	private SetupConnectionModeTask m_SetupConnectionModeTask = new SetupConnectionModeTask();

	// Token: 0x04002067 RID: 8295
	private OfflineTask m_OfflineTask = new OfflineTask();

	// Token: 0x04002068 RID: 8296
	private PrivilegeCheckAllUsersTask m_PrivilegeCheckAllUsersTask = new PrivilegeCheckAllUsersTask();

	// Token: 0x04002069 RID: 8297
	private SearchTask m_SearchTask = new SearchTask();
}
