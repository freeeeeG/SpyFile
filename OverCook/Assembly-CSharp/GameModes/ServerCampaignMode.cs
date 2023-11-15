using System;
using OrderController;

namespace GameModes
{
	// Token: 0x02000691 RID: 1681
	public class ServerCampaignMode : ServerGameModeBase
	{
		// Token: 0x0600201B RID: 8219 RVA: 0x0009C75F File Offset: 0x0009AB5F
		public ServerCampaignMode(Config config) : base(config)
		{
			this.m_config = (CampaignModeConfig)config;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0009C774 File Offset: 0x0009AB74
		private ServerOrderControllerBase ServerOrderControllerBuilder(VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
		{
			ServerFixedTimeOrderController.OrdersConfig ordersConfig = this.BuildOrderConfig();
			ServerOrderControllerBase serverOrderControllerBase = new ServerFixedTimeOrderController(ref ordersConfig, _addedCallback, _timeoutCallback);
			serverOrderControllerBase.SetRoundTimer(this.m_roundTimer);
			return serverOrderControllerBase;
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0009C7A0 File Offset: 0x0009ABA0
		private ServerFixedTimeOrderController.OrdersConfig BuildOrderConfig()
		{
			KitchenLevelConfigBase levelConfig = this.m_context.m_levelConfig;
			return new ServerFixedTimeOrderController.OrdersConfig
			{
				m_maxActiveOrders = 5,
				m_roundData = levelConfig.GetRoundData(),
				m_orderLifetime = levelConfig.m_orderLifetime,
				m_timeBetweenOrders = levelConfig.m_timeBetweenOrders
			};
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0009C7F4 File Offset: 0x0009ABF4
		public override void Setup(ServerContext context, SessionConfig config, ref ServerSetupData outputData)
		{
			this.m_context = context;
			this.m_roundTimer = new ServerRoundTimer();
			outputData.m_orderControllerBuilder = new ServerOrderControllerBuilder(this.ServerOrderControllerBuilder);
			outputData.m_roundTimer = this.m_roundTimer;
			outputData.m_onSuccessfulDelivery = new OnSuccessfulDeliveryServer(this.OnSuccessfulDelivery);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0009C844 File Offset: 0x0009AC44
		public override void Begin()
		{
			if (ClientGameSetup.Mode == GameMode.Campaign && this.m_context.m_levelConfig.m_recipesBeforeTimerStarts > 0)
			{
				this.m_recipeDeliverySuppressor = this.m_roundTimer.Suppressor.AddSuppressor(this.m_context.m_gameObject);
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0009C894 File Offset: 0x0009AC94
		private void OnSuccessfulDelivery(OrderID orderID, RecipeList.Entry entry, float timePropRemainingPercentage, bool wasCombo, ServerPlateStation station)
		{
			if (this.m_recipeDeliverySuppressor != null)
			{
				this.m_recipesDelivered++;
				if (this.m_recipesDelivered >= this.m_context.m_levelConfig.m_recipesBeforeTimerStarts)
				{
					this.m_recipeDeliverySuppressor.Release();
					this.m_recipeDeliverySuppressor = null;
				}
			}
		}

		// Token: 0x04001899 RID: 6297
		private ServerContext m_context;

		// Token: 0x0400189A RID: 6298
		private CampaignModeConfig m_config;

		// Token: 0x0400189B RID: 6299
		private Suppressor m_recipeDeliverySuppressor;

		// Token: 0x0400189C RID: 6300
		private int m_recipesDelivered;

		// Token: 0x0400189D RID: 6301
		private IServerRoundTimer m_roundTimer;
	}
}
