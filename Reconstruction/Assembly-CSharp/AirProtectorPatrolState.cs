using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class AirProtectorPatrolState : FSMState
{
	// Token: 0x0600050F RID: 1295 RVA: 0x0000DF70 File Offset: 0x0000C170
	public AirProtectorPatrolState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Patrol;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0000DFC1 File Offset: 0x0000C1C1
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

	// Token: 0x06000511 RID: 1297 RVA: 0x0000DFFB File Offset: 0x0000C1FB
	public override void Reason(Aircraft agent)
	{
		this.protectWaitingTime += Time.deltaTime;
		if (this.protectWaitingTime > this.protectBossCD)
		{
			this.fsm.PerformTransition(Transition.ProtectBoss);
			this.protectWaitingTime = 0f;
		}
	}

	// Token: 0x04000208 RID: 520
	private float protectWaitingTime;

	// Token: 0x04000209 RID: 521
	private float directionWaitingTime;

	// Token: 0x0400020A RID: 522
	private float protectBossCD = 2.5f + Random.Range(-0.5f, 0.5f);

	// Token: 0x0400020B RID: 523
	private float directionCD = 1.5f + Random.Range(-0.5f, 0.5f);
}
