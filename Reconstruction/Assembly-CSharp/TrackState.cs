using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class TrackState : FSMState
{
	// Token: 0x06000519 RID: 1305 RVA: 0x0000E100 File Offset: 0x0000C300
	public TrackState(FSMSystem fsm) : base(fsm)
	{
		base.StateID = StateID.Track;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0000E11B File Offset: 0x0000C31B
	public override void Act(Aircraft agent)
	{
		agent.MovingToTarget(Destination.target);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0000E124 File Offset: 0x0000C324
	public override void Reason(Aircraft agent)
	{
		if ((agent.transform.position - agent.targetTurret.transform.position).magnitude < agent.minDistanceToDealDamage)
		{
			agent.Attack();
			this.fsm.PerformTransition(Transition.Attacked);
		}
		this.trackCounter += Time.deltaTime;
		if (this.trackCounter > this.maxTrackTime)
		{
			this.fsm.PerformTransition(Transition.Attacked);
			this.trackCounter = 0f;
		}
	}

	// Token: 0x0400020E RID: 526
	private float trackCounter;

	// Token: 0x0400020F RID: 527
	private float maxTrackTime = 4f;
}
