using System;
using OrderController;

namespace GameModes
{
	// Token: 0x020006B4 RID: 1716
	public class ServerPracticeMode : ServerGameModeBase
	{
		// Token: 0x0600207A RID: 8314 RVA: 0x0009D013 File Offset: 0x0009B413
		public ServerPracticeMode(Config config) : base(config)
		{
			this.m_config = (PracticeModeConfig)config;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0009D028 File Offset: 0x0009B428
		private ServerOrderControllerBase ServerOrderControllerBuilder(VoidGeneric<OrderID> addedCallback, VoidGeneric<OrderID> timeoutCallback)
		{
			KitchenLevelConfigBase levelConfig = this.m_context.m_levelConfig;
			ServerFixedTimeOrderController.OrdersConfig ordersConfig = new ServerFixedTimeOrderController.OrdersConfig
			{
				m_maxActiveOrders = 5,
				m_roundData = levelConfig.GetRoundData(),
				m_orderLifetime = levelConfig.m_orderLifetime,
				m_timeBetweenOrders = levelConfig.m_timeBetweenOrders
			};
			ServerFixedTimeOrderController serverFixedTimeOrderController = new ServerFixedTimeOrderController(ref ordersConfig, addedCallback, timeoutCallback);
			serverFixedTimeOrderController.SetRoundTimer(this.m_roundTimer);
			return serverFixedTimeOrderController;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x0009D094 File Offset: 0x0009B494
		public override void Setup(ServerContext context, SessionConfig sessionConfig, ref ServerSetupData outputData)
		{
			this.m_context = context;
			this.m_sessionConfig = sessionConfig;
			this.m_roundTimer = new ServerUnlimitedRoundTimer();
			outputData.m_orderControllerBuilder = new ServerOrderControllerBuilder(this.ServerOrderControllerBuilder);
			outputData.m_roundTimer = this.m_roundTimer;
			outputData.m_onSessionConfigChangedCallback = new OnSessionConfigChanged(this.OnSessionConfigChanged);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x0009D0EC File Offset: 0x0009B4EC
		public override void Begin()
		{
			GameSession gameSession = GameUtils.GetGameSession();
			for (int i = 0; i < this.m_context.m_teamCount; i++)
			{
				ServerTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam((TeamID)i);
				monitorForTeam.OrdersController.EnableOrderExpiration = this.m_sessionConfig.m_settings[2];
			}
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x0009D148 File Offset: 0x0009B548
		private void OnSessionConfigChanged(SessionConfig sessionConfig)
		{
			this.m_sessionConfig = sessionConfig;
			for (int i = 0; i < this.m_context.m_teamCount; i++)
			{
				ServerTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam((TeamID)i);
				monitorForTeam.OrdersController.EnableOrderExpiration = this.m_sessionConfig.m_settings[2];
			}
		}

		// Token: 0x040018ED RID: 6381
		private ServerContext m_context;

		// Token: 0x040018EE RID: 6382
		private PracticeModeConfig m_config;

		// Token: 0x040018EF RID: 6383
		private SessionConfig m_sessionConfig;

		// Token: 0x040018F0 RID: 6384
		private ServerUnlimitedRoundTimer m_roundTimer;
	}
}
