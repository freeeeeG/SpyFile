using System;

// Token: 0x02000052 RID: 82
public class VentController : GameStateMachine<VentController, VentController.Instance>
{
	// Token: 0x06000176 RID: 374 RVA: 0x0000A50C File Offset: 0x0000870C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.VentClosed, this.closed, (VentController.Instance smi) => smi.GetComponent<Vent>().Closed()).EventTransition(GameHashes.VentOpen, this.off, (VentController.Instance smi) => !smi.GetComponent<Vent>().Closed());
		this.off.PlayAnim("off").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, (VentController.Instance smi) => smi.GetComponent<Exhaust>().IsAnimating());
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.VentAnimatingChanged, this.working_pst, (VentController.Instance smi) => !smi.GetComponent<Exhaust>().IsAnimating());
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.closed.PlayAnim("closed").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, (VentController.Instance smi) => smi.GetComponent<Exhaust>().IsAnimating());
	}

	// Token: 0x040000E8 RID: 232
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000E9 RID: 233
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x040000EA RID: 234
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x040000EB RID: 235
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x040000EC RID: 236
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State closed;

	// Token: 0x040000ED RID: 237
	public StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.BoolParameter isAnimating;

	// Token: 0x02000E4B RID: 3659
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E4C RID: 3660
	public new class Instance : GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006F2C RID: 28460 RVA: 0x002B8E58 File Offset: 0x002B7058
		public Instance(IStateMachineTarget master, VentController.Def def) : base(master, def)
		{
		}
	}
}
