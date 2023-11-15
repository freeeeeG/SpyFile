using System;
using OrderController;

namespace GameModes
{
	// Token: 0x020006B9 RID: 1721
	public class ServerSurvivalMode : ServerGameModeBase
	{
		// Token: 0x0600208F RID: 8335 RVA: 0x0009D5AD File Offset: 0x0009B9AD
		public ServerSurvivalMode(Config config) : base(config)
		{
			this.m_config = (SurvivalModeConfig)config;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x0009D5C4 File Offset: 0x0009B9C4
		private ServerOrderControllerBase ServerOrderControllerBuilder(VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
		{
			ServerFixedTimeOrderController.OrdersConfig ordersConfig = this.BuildOrderConfig();
			ServerOrderControllerBase serverOrderControllerBase = new ServerFixedTimeOrderController(ref ordersConfig, _addedCallback, _timeoutCallback);
			serverOrderControllerBase.SetRoundTimer(this.m_roundTimer);
			return serverOrderControllerBase;
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x0009D5F0 File Offset: 0x0009B9F0
		private ServerFixedTimeOrderController.OrdersConfig BuildOrderConfig()
		{
			KitchenLevelConfigBase kitchenLevelConfigBase = this.m_levelConfig as KitchenLevelConfigBase;
			return new ServerFixedTimeOrderController.OrdersConfig
			{
				m_maxActiveOrders = 5,
				m_roundData = kitchenLevelConfigBase.GetRoundData(),
				m_orderLifetime = kitchenLevelConfigBase.m_orderLifetime,
				m_timeBetweenOrders = kitchenLevelConfigBase.m_timeBetweenOrders
			};
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x0009D644 File Offset: 0x0009BA44
		public override void Setup(ServerContext context, SessionConfig config, ref ServerSetupData setupData)
		{
			this.m_levelConfig = context.m_levelConfig;
			this.m_roundTimer = new ServerModifiableRoundTimer();
			setupData.m_orderControllerBuilder = new ServerOrderControllerBuilder(this.ServerOrderControllerBuilder);
			setupData.m_roundTimer = this.m_roundTimer;
			setupData.m_onSuccessfulDelivery = new OnSuccessfulDeliveryServer(this.OnSuccessfulDelivery);
			setupData.m_onOrderExpired = new OnOrderExpiredServer(this.OnOrderExpired);
			setupData.m_onOutroScene = new OnOutroSceneServer(this.OnOutroScene);
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x0009D6BC File Offset: 0x0009BABC
		private void OnSuccessfulDelivery(OrderID orderID, RecipeList.Entry entry, float timePropRemainingPercentage, bool wasCombo, ServerPlateStation station)
		{
			float num = (float)this.m_config.m_recipeTimes.Get(entry.m_order);
			int num2 = SurvivalModeUtil.CalculateDeliveryBonus(this.m_config, timePropRemainingPercentage);
			this.m_roundTimer.AddTime((int)(this.m_config.m_timeMultiplier * num) + num2);
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x0009D709 File Offset: 0x0009BB09
		private void OnOrderExpired(TeamID teamID, OrderID order)
		{
			this.m_roundTimer.AddTime(-Math.Abs(this.m_config.m_recipeTimes.RecipeFailedPenalty));
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x0009D72C File Offset: 0x0009BB2C
		private CampaignFlowController.IOutroFlowSceneProvider OnOutroScene()
		{
			return new ServerSurvivalMode.SurvivalModeOutroProvider();
		}

		// Token: 0x040018FC RID: 6396
		private SurvivalModeConfig m_config;

		// Token: 0x040018FD RID: 6397
		private LevelConfigBase m_levelConfig;

		// Token: 0x040018FE RID: 6398
		private ServerModifiableRoundTimer m_roundTimer;

		// Token: 0x020006BA RID: 1722
		private class SurvivalModeOutroProvider : CampaignFlowController.IOutroFlowSceneProvider
		{
			// Token: 0x06002097 RID: 8343 RVA: 0x0009D73B File Offset: 0x0009BB3B
			public string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen)
			{
				o_loadState = GameState.CampaignMap;
				o_loadEndState = GameState.RunMapUnfoldRoutine;
				o_useLoadingScreen = true;
				return GameUtils.GetGameSession().TypeSettings.WorldMapScene;
			}
		}
	}
}
