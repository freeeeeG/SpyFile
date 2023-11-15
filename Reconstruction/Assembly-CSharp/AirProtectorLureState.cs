using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class AirProtectorLureState : FSMState
{
	// Token: 0x0600050C RID: 1292 RVA: 0x0000DEFB File Offset: 0x0000C0FB
	public AirProtectorLureState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Lure;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0000DF26 File Offset: 0x0000C126
	public override void Act(Aircraft agent)
	{
		agent.Lure();
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0000DF2E File Offset: 0x0000C12E
	public override void Reason(Aircraft agent)
	{
		this.waitingTime += Time.deltaTime;
		if (this.waitingTime > this.lureTime)
		{
			this.fsm.PerformTransition(Transition.ProtectBoss);
			agent.targetTurret = null;
			this.waitingTime = 0f;
		}
	}

	// Token: 0x04000206 RID: 518
	private float waitingTime;

	// Token: 0x04000207 RID: 519
	private float lureTime = 4f + Random.Range(0f, 2f);
}
