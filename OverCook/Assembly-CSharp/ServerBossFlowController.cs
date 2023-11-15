using System;
using System.Collections;
using GameModes;
using OrderController;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class ServerBossFlowController : ServerDynamicFlowController
{
	// Token: 0x06001E7A RID: 7802 RVA: 0x00094480 File Offset: 0x00092880
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_bossFlowController = (BossFlowController)synchronisedObject;
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameModeKind = gameSession.GameModeKind;
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000944B2 File Offset: 0x000928B2
	private void SendSuccessMessage()
	{
		ServerMessenger.BossLevelMessage(this.m_data);
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x000944C0 File Offset: 0x000928C0
	protected override ServerOrderControllerBase BuildOrderController(VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
	{
		ServerFixedTimeOrderController.OrdersConfig ordersConfig = base.BuildOrderConfig();
		ServerOrderControllerBase serverOrderControllerBase = new ServerBossOrderController(ref ordersConfig, _addedCallback, _timeoutCallback);
		serverOrderControllerBase.SetRoundTimer(base.RoundTimer);
		return serverOrderControllerBase;
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x000944EC File Offset: 0x000928EC
	protected override IEnumerator BuildPhaseRoutine(int _phase)
	{
		ServerBossOrderController orderController = this.GetOrderController();
		while (!orderController.CurrentPhaseComplete())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x00094508 File Offset: 0x00092908
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		if (this.m_gameModeKind != Kind.Practice && !this.m_bHasFinished)
		{
			ServerBossOrderController orderController = this.GetOrderController();
			if (orderController.HasFinished())
			{
				this.m_bHasFinished = true;
				this.SendSuccessMessage();
			}
		}
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x00094554 File Offset: 0x00092954
	protected override bool HasFinished()
	{
		ServerBossOrderController orderController = this.GetOrderController();
		return (orderController.HasFinished() || base.HasFinished()) && this.m_gameModeKind != Kind.Practice;
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x0009458D File Offset: 0x0009298D
	private ServerBossOrderController GetOrderController()
	{
		return this.GetMonitorForTeam(TeamID.One).OrdersController as ServerBossOrderController;
	}

	// Token: 0x0400176E RID: 5998
	private BossFlowController m_bossFlowController;

	// Token: 0x0400176F RID: 5999
	private BossLevelMessage m_data = new BossLevelMessage();

	// Token: 0x04001770 RID: 6000
	private bool m_bHasFinished;

	// Token: 0x04001771 RID: 6001
	private Kind m_gameModeKind;
}
