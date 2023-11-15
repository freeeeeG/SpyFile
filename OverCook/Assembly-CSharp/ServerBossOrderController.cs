using System;
using OrderController;

// Token: 0x02000645 RID: 1605
public class ServerBossOrderController : ServerDynamicOrderController
{
	// Token: 0x06001E89 RID: 7817 RVA: 0x00096311 File Offset: 0x00094711
	public ServerBossOrderController(ref ServerFixedTimeOrderController.OrdersConfig _ordersConfig, VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback) : base(ref _ordersConfig, _addedCallback, _timeoutCallback)
	{
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x0009631C File Offset: 0x0009471C
	public bool CurrentPhaseComplete()
	{
		BossRoundData bossRoundData = base.AccessOrdersConfig.m_roundData as BossRoundData;
		bool flag = bossRoundData.NoMoreRecipesToIssueInPhase(base.AccessRoundInstanceData);
		return flag && base.IsEmpty();
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x00096359 File Offset: 0x00094759
	public bool HasFinished()
	{
		return this.CurrentPhaseComplete() && !base.HasMorePhases();
	}
}
