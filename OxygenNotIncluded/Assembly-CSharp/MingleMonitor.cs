using System;

// Token: 0x02000886 RID: 2182
public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
	// Token: 0x06003F7C RID: 16252 RVA: 0x00162DF1 File Offset: 0x00160FF1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.mingle;
		base.serializable = StateMachine.SerializeType.Never;
		this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore), null);
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x00162E1B File Offset: 0x0016101B
	private Chore CreateMingleChore(MingleMonitor.Instance smi)
	{
		return new MingleChore(smi.master);
	}

	// Token: 0x0400293D RID: 10557
	public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

	// Token: 0x02001694 RID: 5780
	public new class Instance : GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B7A RID: 35706 RVA: 0x0031693D File Offset: 0x00314B3D
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
