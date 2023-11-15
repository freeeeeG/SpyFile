using System;

// Token: 0x020000C7 RID: 199
public class BackState : FSMState
{
	// Token: 0x06000512 RID: 1298 RVA: 0x0000E034 File Offset: 0x0000C234
	public BackState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Back;
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0000E044 File Offset: 0x0000C244
	public override void Act(Aircraft agent)
	{
		agent.MovingToTarget(Destination.boss);
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0000E050 File Offset: 0x0000C250
	public override void Reason(Aircraft agent)
	{
		if ((agent.transform.position - agent.boss.transform.position).magnitude < 1f)
		{
			this.fsm.PerformTransition(Transition.Patrol);
		}
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0000E0A2 File Offset: 0x0000C2A2
	public override void DoBeforeEntering()
	{
	}
}
