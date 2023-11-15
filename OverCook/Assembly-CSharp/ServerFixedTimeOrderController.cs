using System;
using OrderController;

// Token: 0x0200075B RID: 1883
public class ServerFixedTimeOrderController : ServerOrderControllerBase
{
	// Token: 0x06002448 RID: 9288 RVA: 0x00096231 File Offset: 0x00094631
	public ServerFixedTimeOrderController(ref ServerFixedTimeOrderController.OrdersConfig _ordersConfig, VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback) : base(_ordersConfig.m_roundData, _ordersConfig.m_maxActiveOrders, _addedCallback, _timeoutCallback)
	{
		this.m_ordersConfig = _ordersConfig;
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06002449 RID: 9289 RVA: 0x00096253 File Offset: 0x00094653
	protected ServerFixedTimeOrderController.OrdersConfig AccessOrdersConfig
	{
		get
		{
			return this.m_ordersConfig;
		}
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x0009625B File Offset: 0x0009465B
	protected override float GetNextOrderLifetime()
	{
		return this.m_ordersConfig.m_orderLifetime;
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x00096268 File Offset: 0x00094668
	protected override float GetNextTimeBetweenOrders()
	{
		return this.m_ordersConfig.m_timeBetweenOrders;
	}

	// Token: 0x04001BB4 RID: 7092
	private ServerFixedTimeOrderController.OrdersConfig m_ordersConfig;

	// Token: 0x0200075C RID: 1884
	public struct OrdersConfig
	{
		// Token: 0x04001BB5 RID: 7093
		public int m_maxActiveOrders;

		// Token: 0x04001BB6 RID: 7094
		public RoundDataBase m_roundData;

		// Token: 0x04001BB7 RID: 7095
		public float m_orderLifetime;

		// Token: 0x04001BB8 RID: 7096
		public float m_timeBetweenOrders;
	}
}
