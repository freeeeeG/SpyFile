using System;

// Token: 0x02000733 RID: 1843
public class RobotIdleMonitor : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance>
{
	// Token: 0x060032AA RID: 12970 RVA: 0x0010D128 File Offset: 0x0010B328
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Transition(this.working, (RobotIdleMonitor.Instance smi) => !RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
		this.working.Transition(this.idle, (RobotIdleMonitor.Instance smi) => RobotIdleMonitor.CheckShouldIdle(smi), UpdateRate.SIM_200ms);
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x0010D1AC File Offset: 0x0010B3AC
	private static bool CheckShouldIdle(RobotIdleMonitor.Instance smi)
	{
		FallMonitor.Instance smi2 = smi.master.gameObject.GetSMI<FallMonitor.Instance>();
		return smi2 == null || (!smi.master.gameObject.GetComponent<ChoreConsumer>().choreDriver.HasChore() && smi2.GetCurrentState() == smi2.sm.standing);
	}

	// Token: 0x04001E6B RID: 7787
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04001E6C RID: 7788
	public GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x020014C2 RID: 5314
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014C3 RID: 5315
	public new class Instance : GameStateMachine<RobotIdleMonitor, RobotIdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060085CF RID: 34255 RVA: 0x00307562 File Offset: 0x00305762
		public Instance(IStateMachineTarget master, RobotIdleMonitor.Def def) : base(master)
		{
		}

		// Token: 0x0400665C RID: 26204
		public KBatchedAnimController eyes;
	}
}
