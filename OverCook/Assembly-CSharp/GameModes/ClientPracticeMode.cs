using System;
using OrderController;
using UnityEngine;

namespace GameModes
{
	// Token: 0x020006B5 RID: 1717
	public class ClientPracticeMode : ClientGameModeBase
	{
		// Token: 0x0600207F RID: 8319 RVA: 0x0009D1A2 File Offset: 0x0009B5A2
		public ClientPracticeMode(Config config) : base(config)
		{
			this.m_config = (PracticeModeConfig)config;
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x0009D1D0 File Offset: 0x0009B5D0
		private ClientOrderControllerBase ClientOrderControllerBuilder(RecipeFlowGUI recipeFlowGUI)
		{
			ClientFixedTimeOrderController clientFixedTimeOrderController = new ClientFixedTimeOrderController(recipeFlowGUI);
			clientFixedTimeOrderController.SetRoundTimer(this.m_roundTimer);
			return clientFixedTimeOrderController;
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x0009D1F4 File Offset: 0x0009B5F4
		public override void Setup(ClientContext context, SessionConfig config, ref ClientSetupData setupData)
		{
			this.m_context = context;
			this.m_sessionConfig = config;
			this.m_roundTimer = new ClientUnlimitedRoundTimer();
			setupData.m_orderControllerBuilder = new ClientOrderControllerBuilder(this.ClientOrderControllerBuilder);
			setupData.m_roundTimer = this.m_roundTimer;
			setupData.m_onSessionConfigChangedCallback = new OnSessionConfigChanged(this.OnSessionConfigChanged);
			setupData.m_onSuccessfulDelivery = new OnSuccessfulDeliveryClient(this.OnSuccessfulDelivery);
			setupData.m_onFailedDelivery = new OnFailedDeliveryClient(this.OnFailedDelivery);
			setupData.m_onOrderExpired = new OnOrderExpiredClient(this.OnOrderExpired);
			GameObject obj = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_config.m_uiPrefab);
			this.m_scoreUIControllers = obj.RequestComponentsRecursive<ScoreUIController>();
			this.m_levelTimerUIControllers = obj.RequestComponentsRecursive<DisplayTimeUIController>();
			this.m_dataStore = GameUtils.RequireManager<DataStore>();
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0009D2B4 File Offset: 0x0009B6B4
		public override void Begin()
		{
			this.SetUIControllersActive<ScoreUIController>(this.m_scoreUIControllers, this.m_sessionConfig.m_settings[1]);
			this.SetUIControllersActive<DisplayTimeUIController>(this.m_levelTimerUIControllers, this.m_sessionConfig.m_settings[0]);
			for (int i = 0; i < this.m_context.m_teamCount; i++)
			{
				ClientTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam((TeamID)i);
				monitorForTeam.OrdersController.EnableOrderExpiration = this.m_sessionConfig.m_settings[2];
			}
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x0009D33C File Offset: 0x0009B73C
		private void SetUIControllersActive<T>(T[] uiControllers, bool enable) where T : UIControllerBase
		{
			if (uiControllers != null && uiControllers.Length > 0)
			{
				for (int i = 0; i < uiControllers.Length; i++)
				{
					uiControllers[i].gameObject.SetActive(enable);
				}
			}
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x0009D388 File Offset: 0x0009B788
		private void OnSessionConfigChanged(SessionConfig config)
		{
			this.m_sessionConfig = config;
			this.SetUIControllersActive<ScoreUIController>(this.m_scoreUIControllers, this.m_sessionConfig.m_settings[1]);
			this.SetUIControllersActive<DisplayTimeUIController>(this.m_levelTimerUIControllers, this.m_sessionConfig.m_settings[0]);
			for (int i = 0; i < this.m_context.m_teamCount; i++)
			{
				ClientTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam((TeamID)i);
				monitorForTeam.OrdersController.EnableOrderExpiration = this.m_sessionConfig.m_settings[2];
			}
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x0009D414 File Offset: 0x0009B814
		private void OnSuccessfulDelivery(TeamID teamId, OrderID orderId, float timePropRemainingPercentage, int tip, bool wasCombo, ClientPlateStation station)
		{
			TeamTip teamTip = new TeamTip
			{
				m_team = teamId,
				m_tip = tip,
				m_station = station
			};
			this.m_dataStore.Write(ClientPracticeMode.k_scoreTipId, teamTip);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x0009D45C File Offset: 0x0009B85C
		private void OnFailedDelivery(TeamID teamId, OrderID orderId)
		{
			TeamTip teamTip = new TeamTip
			{
				m_team = teamId,
				m_tip = 0
			};
			this.m_dataStore.Write(ClientPracticeMode.k_scoreTipId, teamTip);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0009D49C File Offset: 0x0009B89C
		private void OnOrderExpired(TeamID teamId, OrderID orderId)
		{
			TeamTip teamTip = new TeamTip
			{
				m_team = teamId,
				m_tip = 0
			};
			this.m_dataStore.Write(ClientPracticeMode.k_scoreTipId, teamTip);
		}

		// Token: 0x040018F1 RID: 6385
		private ClientContext m_context;

		// Token: 0x040018F2 RID: 6386
		private PracticeModeConfig m_config;

		// Token: 0x040018F3 RID: 6387
		private SessionConfig m_sessionConfig;

		// Token: 0x040018F4 RID: 6388
		private ClientUnlimitedRoundTimer m_roundTimer;

		// Token: 0x040018F5 RID: 6389
		private ScoreUIController[] m_scoreUIControllers = new ScoreUIController[0];

		// Token: 0x040018F6 RID: 6390
		private DisplayTimeUIController[] m_levelTimerUIControllers = new DisplayTimeUIController[0];

		// Token: 0x040018F7 RID: 6391
		private DataStore m_dataStore;

		// Token: 0x040018F8 RID: 6392
		private static readonly DataStore.Id k_scoreTipId = new DataStore.Id("score.tip");
	}
}
