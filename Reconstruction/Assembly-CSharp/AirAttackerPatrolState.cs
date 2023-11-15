using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class AirAttackerPatrolState : FSMState
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x0000D578 File Offset: 0x0000B778
	public AirAttackerPatrolState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Patrol;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
	public override void Act(Aircraft agent)
	{
		agent.MovingToTarget(Destination.Random);
		this.directionWaitingTime += Time.deltaTime;
		if (this.directionWaitingTime > this.directionCD)
		{
			agent.PickRandomDes();
			this.directionWaitingTime = 0f;
		}
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0000D610 File Offset: 0x0000B810
	public override void Reason(Aircraft agent)
	{
		this.attackWaitingTime += Time.deltaTime;
		if (this.attackWaitingTime > this.searchTargetCD)
		{
			agent.SearchTarget();
			this.attackWaitingTime = 0f;
		}
		if (agent.targetTurret != null)
		{
			if (Random.Range(0f, 1f) < this.LureProb)
			{
				this.fsm.PerformTransition(Transition.LureTarget);
				return;
			}
			this.fsm.PerformTransition(Transition.AttackTarget);
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x0000D68C File Offset: 0x0000B88C
	public override void DoBeforeEntering()
	{
		base.DoBeforeEntering();
		this.searchTargetCD = 2f + Random.Range(-1f, 1f);
		this.directionCD = 1.5f + Random.Range(-1f, 1f);
	}

	// Token: 0x040001E2 RID: 482
	private float LureProb = 0.4f;

	// Token: 0x040001E3 RID: 483
	private float attackWaitingTime;

	// Token: 0x040001E4 RID: 484
	private float directionWaitingTime;

	// Token: 0x040001E5 RID: 485
	private float searchTargetCD = 2f + Random.Range(-0.5f, 0.5f);

	// Token: 0x040001E6 RID: 486
	private float directionCD = 1.5f + Random.Range(-0.5f, 0.5f);
}
