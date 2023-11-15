using System;
using Team17.Online;
using Team17.Online.Multiplayer;

// Token: 0x02000847 RID: 2119
public class MatchmakingAgent : ConnectionModeAgent
{
	// Token: 0x060028D4 RID: 10452 RVA: 0x000BF8C4 File Offset: 0x000BDCC4
	public virtual bool Start(Server server, Client client, object data, GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		this.m_Callback = callback;
		if (this.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && this.GetStatus().GetResult() == eConnectionModeSwitchResult.Success && ConnectionStatus.IsInSession())
		{
			if (callback != null)
			{
				callback(this.GetStatus());
			}
			this.m_bCalledBack = true;
			return true;
		}
		this.m_Data = (MatchmakeData)data;
		this.m_LocalServer = server;
		this.m_LocalClient = client;
		OfflineTask offlineTask = this.m_Tasks[1] as OfflineTask;
		offlineTask.Initialise(this.m_LocalServer, this.m_LocalClient);
		SetupConnectionModeTask setupConnectionModeTask = this.m_Tasks[2] as SetupConnectionModeTask;
		setupConnectionModeTask.Initialise(this.m_Data.User, this.m_Data.connectionMode);
		PrivilegeCheckTask privilegeCheckTask = this.m_Tasks[3] as PrivilegeCheckTask;
		privilegeCheckTask.Initialise(this.m_Data.User);
		SearchSessionPropertyValuesProvider.SetGameMode(this.m_Data.gameMode);
		SearchTask searchTask = this.m_Tasks[4] as SearchTask;
		searchTask.Initialise(SearchSessionPropertyValuesProvider.GetValues());
		ServerMessenger.OnServerStopped();
		this.m_CurrentAction.Start(this.m_Tasks);
		this.m_bCalledBack = false;
		return true;
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000BF9EC File Offset: 0x000BDDEC
	public void Stop()
	{
		this.m_CurrentAction.Stop();
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x000BF9F9 File Offset: 0x000BDDF9
	public void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		if (this.m_Callback == callback)
		{
			this.m_Callback = null;
		}
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x000BFA13 File Offset: 0x000BDE13
	public object GetAgentData()
	{
		return this.m_Data;
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x000BFA20 File Offset: 0x000BDE20
	public virtual void Update()
	{
		if (!this.m_bCalledBack)
		{
			this.m_CurrentAction.Update();
			IConnectionModeSwitchStatus status = this.GetStatus();
			if (status.GetProgress() == eConnectionModeSwitchProgress.Complete)
			{
				if (status.GetResult() == eConnectionModeSwitchResult.Success)
				{
					IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
					IOnlineMultiplayerSessionCoordinator onlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
					IOnlineMultiplayerSessionUserId sessionHostUser = UserSystemUtils.GetSessionHostUser();
					JoinData joinData = this.m_CurrentAction.GetTaskData() as JoinData;
					this.m_LocalServer.Reset();
					this.m_LocalClient.Reset();
					if (ConnectionStatus.IsHost())
					{
						if (sessionHostUser != null && onlineMultiplayerSessionCoordinator != null)
						{
							this.m_LocalServer.Initialise(true);
							this.m_LocalClient.Initialise(false);
							NetworkSystemConfigurator.Server(this.m_LocalServer, this.m_LocalClient, PrivilegeCheckCache.GetAllowedUser(this.m_Data.User));
							ServerMessenger.TimeSync(0f);
						}
					}
					else if (sessionHostUser != null && joinData != null && onlineMultiplayerSessionCoordinator != null)
					{
						NetworkSystemConfigurator.Client(this.m_LocalClient, joinData, onlineMultiplayerSessionCoordinator);
					}
				}
				if (this.m_Callback != null)
				{
					this.m_Callback(this.GetStatus());
				}
				this.m_bCalledBack = true;
			}
		}
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x000BFB45 File Offset: 0x000BDF45
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_CurrentAction.GetStatus();
	}

	// Token: 0x04002050 RID: 8272
	private IMultiplayerTask[] m_Tasks = new IMultiplayerTask[]
	{
		new LeaveSessionTask(),
		new OfflineTask(),
		new SetupConnectionModeTask(),
		new PrivilegeCheckTask(),
		new SearchTask(),
		new JoinRandomRoomTask()
	};

	// Token: 0x04002051 RID: 8273
	private bool m_bCalledBack;

	// Token: 0x04002052 RID: 8274
	private GenericVoid<IConnectionModeSwitchStatus> m_Callback;

	// Token: 0x04002053 RID: 8275
	private MultiplayerOperation m_CurrentAction = new MultiplayerOperation();

	// Token: 0x04002054 RID: 8276
	private MatchmakeData m_Data;

	// Token: 0x04002055 RID: 8277
	private Server m_LocalServer;

	// Token: 0x04002056 RID: 8278
	private Client m_LocalClient;
}
