using System;

// Token: 0x02000880 RID: 2176
public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
	// Token: 0x06003F65 RID: 16229 RVA: 0x0016230C File Offset: 0x0016050C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.TagTransition(GameTags.Dying, this.stopped, false).ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore), null);
		this.stopped.DoNothing();
	}

	// Token: 0x06003F66 RID: 16230 RVA: 0x0016234C File Offset: 0x0016054C
	private Chore CreateIdleChore(IdleMonitor.Instance smi)
	{
		return new IdleChore(smi.master);
	}

	// Token: 0x04002925 RID: 10533
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002926 RID: 10534
	public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State stopped;

	// Token: 0x02001684 RID: 5764
	public new class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B39 RID: 35641 RVA: 0x003160B7 File Offset: 0x003142B7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
