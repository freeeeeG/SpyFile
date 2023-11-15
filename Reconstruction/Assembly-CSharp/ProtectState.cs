using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class ProtectState : FSMState
{
	// Token: 0x06000516 RID: 1302 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
	public ProtectState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Protect;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0000E0BF File Offset: 0x0000C2BF
	public override void Act(Aircraft agent)
	{
		agent.Protect();
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0000E0C7 File Offset: 0x0000C2C7
	public override void Reason(Aircraft agent)
	{
		this.waitingTime += Time.deltaTime;
		if (this.waitingTime > this.protectTime)
		{
			this.fsm.PerformTransition(Transition.Patrol);
			this.waitingTime = 0f;
		}
	}

	// Token: 0x0400020C RID: 524
	private float waitingTime;

	// Token: 0x0400020D RID: 525
	private float protectTime = 5f;
}
