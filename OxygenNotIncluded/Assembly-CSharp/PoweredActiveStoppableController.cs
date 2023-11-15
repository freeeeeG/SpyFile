using System;

// Token: 0x0200004E RID: 78
public class PoweredActiveStoppableController : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance>
{
	// Token: 0x0600016E RID: 366 RVA: 0x0000A0B8 File Offset: 0x000082B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (PoweredActiveStoppableController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.stop, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working_pst, (PoweredActiveStoppableController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.stop.PlayAnim("stop").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000D9 RID: 217
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000DA RID: 218
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000DB RID: 219
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000DC RID: 220
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x040000DD RID: 221
	public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State stop;

	// Token: 0x02000E3F RID: 3647
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E40 RID: 3648
	public new class Instance : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006F0E RID: 28430 RVA: 0x002B8CBA File Offset: 0x002B6EBA
		public Instance(IStateMachineTarget master, PoweredActiveStoppableController.Def def) : base(master, def)
		{
		}
	}
}
