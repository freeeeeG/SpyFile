using System;
using OrderController;

// Token: 0x02000675 RID: 1653
public class ServerDynamicOrderController : ServerFixedTimeOrderController
{
	// Token: 0x06001F98 RID: 8088 RVA: 0x00096275 File Offset: 0x00094675
	public ServerDynamicOrderController(ref ServerFixedTimeOrderController.OrdersConfig _ordersConfig, VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback) : base(ref _ordersConfig, _addedCallback, _timeoutCallback)
	{
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x00096280 File Offset: 0x00094680
	public void MoveToNextPhase()
	{
		DynamicRoundData dynamicRoundData = base.AccessOrdersConfig.m_roundData as DynamicRoundData;
		dynamicRoundData.MoveToNextPhase(base.AccessRoundInstanceData);
		base.ResetOrderTimer();
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x000962B4 File Offset: 0x000946B4
	public bool HasMorePhases()
	{
		DynamicRoundData dynamicRoundData = base.AccessOrdersConfig.m_roundData as DynamicRoundData;
		return dynamicRoundData.GetRemainingPhases(base.AccessRoundInstanceData) > 0;
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x000962E4 File Offset: 0x000946E4
	public float GetCurrentPhaseDuration()
	{
		DynamicRoundData dynamicRoundData = base.AccessOrdersConfig.m_roundData as DynamicRoundData;
		return dynamicRoundData.GetCurrentPhaseDuration(base.AccessRoundInstanceData);
	}
}
