using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class AirAttackerLureState : FSMState
{
	// Token: 0x060004DF RID: 1247 RVA: 0x0000D504 File Offset: 0x0000B704
	public AirAttackerLureState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Lure;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0000D52F File Offset: 0x0000B72F
	public override void Act(Aircraft agent)
	{
		agent.Lure();
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0000D537 File Offset: 0x0000B737
	public override void Reason(Aircraft agent)
	{
		this.waitingTime += Time.deltaTime;
		if (this.waitingTime > this.lureTime)
		{
			this.fsm.PerformTransition(Transition.Attacked);
			agent.targetTurret = null;
			this.waitingTime = 0f;
		}
	}

	// Token: 0x040001E0 RID: 480
	private float waitingTime;

	// Token: 0x040001E1 RID: 481
	private float lureTime = 4f + Random.Range(0f, 2f);
}
