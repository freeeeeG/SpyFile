using System;

// Token: 0x02000049 RID: 73
public class ActiveController : GameStateMachine<ActiveController, ActiveController.Instance>
{
	// Token: 0x06000162 RID: 354 RVA: 0x00009A78 File Offset: 0x00007C78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (ActiveController.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.working_pst, (ActiveController.Instance smi) => !smi.GetComponent<Operational>().IsActive);
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000C9 RID: 201
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000CA RID: 202
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000CB RID: 203
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000CC RID: 204
	public GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x02000E2E RID: 3630
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E2F RID: 3631
	public new class Instance : GameStateMachine<ActiveController, ActiveController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006EE3 RID: 28387 RVA: 0x002B8A08 File Offset: 0x002B6C08
		public Instance(IStateMachineTarget master, ActiveController.Def def) : base(master, def)
		{
		}
	}
}
