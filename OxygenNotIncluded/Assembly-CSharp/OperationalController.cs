using System;

// Token: 0x0200004C RID: 76
public class OperationalController : GameStateMachine<OperationalController, OperationalController.Instance>
{
	// Token: 0x0600016A RID: 362 RVA: 0x00009DC4 File Offset: 0x00007FC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OperationalChanged, this.off, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working_pre, (OperationalController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.working_pst, (OperationalController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000D2 RID: 210
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000D3 RID: 211
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000D4 RID: 212
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000D5 RID: 213
	public GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x02000E38 RID: 3640
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E39 RID: 3641
	public new class Instance : GameStateMachine<OperationalController, OperationalController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006EFB RID: 28411 RVA: 0x002B8B99 File Offset: 0x002B6D99
		public Instance(IStateMachineTarget master, OperationalController.Def def) : base(master, def)
		{
		}
	}
}
