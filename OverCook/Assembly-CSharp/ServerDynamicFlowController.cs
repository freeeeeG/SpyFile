using System;
using System.Collections;
using OrderController;
using UnityEngine;

// Token: 0x0200066B RID: 1643
public class ServerDynamicFlowController : ServerCampaignFlowController
{
	// Token: 0x06001F54 RID: 8020 RVA: 0x0009423C File Offset: 0x0009263C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_dynamicFlowController = (DynamicFlowController)synchronisedObject;
		DynamicCampaignLevelConfigBase dynamicCampaignLevelConfigBase = base.LevelConfig as DynamicCampaignLevelConfigBase;
		DynamicRoundData dynamicRoundData = (DynamicRoundData)dynamicCampaignLevelConfigBase.GetRoundData();
		this.m_runLevel = this.BuildRunLevelRoutine();
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x00094280 File Offset: 0x00092680
	private void SendPhaseMessage(int _phase)
	{
		this.m_data.Initialise(_phase);
		ServerMessenger.DynamicLevelMessage(this.m_data);
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x0009429C File Offset: 0x0009269C
	protected override ServerOrderControllerBase BuildOrderController(VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
	{
		ServerFixedTimeOrderController.OrdersConfig ordersConfig = base.BuildOrderConfig();
		ServerOrderControllerBase serverOrderControllerBase = new ServerDynamicOrderController(ref ordersConfig, _addedCallback, _timeoutCallback);
		serverOrderControllerBase.SetRoundTimer(base.RoundTimer);
		return serverOrderControllerBase;
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x000942C8 File Offset: 0x000926C8
	protected virtual IEnumerator BuildRunLevelRoutine()
	{
		int phaseNumber = 0;
		for (;;)
		{
			IEnumerator phaseRoutine = this.BuildPhaseRoutine(phaseNumber);
			while (phaseRoutine.MoveNext())
			{
				yield return null;
			}
			ServerDynamicOrderController orderController = this.GetOrderController();
			if (!orderController.HasMorePhases())
			{
				break;
			}
			phaseNumber++;
			this.SendPhaseMessage(phaseNumber);
			orderController.MoveToNextPhase();
		}
		for (;;)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x000942E4 File Offset: 0x000926E4
	protected virtual IEnumerator BuildPhaseRoutine(int _phase)
	{
		ServerDynamicOrderController orderController = this.GetOrderController();
		float currentPhaseDuration = orderController.GetCurrentPhaseDuration();
		return CoroutineUtils.TimerRoutine(currentPhaseDuration, LayerMask.NameToLayer("Default"));
	}

	// Token: 0x06001F59 RID: 8025 RVA: 0x0009430F File Offset: 0x0009270F
	protected override void OnUpdateInRound()
	{
		this.m_runLevel.MoveNext();
		base.OnUpdateInRound();
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x00094323 File Offset: 0x00092723
	private ServerDynamicOrderController GetOrderController()
	{
		return this.GetMonitorForTeam(TeamID.One).OrdersController as ServerDynamicOrderController;
	}

	// Token: 0x040017E8 RID: 6120
	private DynamicFlowController m_dynamicFlowController;

	// Token: 0x040017E9 RID: 6121
	private DynamicLevelMessage m_data = new DynamicLevelMessage();

	// Token: 0x040017EA RID: 6122
	private IEnumerator m_runLevel;
}
