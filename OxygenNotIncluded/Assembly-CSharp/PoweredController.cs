using System;

// Token: 0x02000050 RID: 80
public class PoweredController : GameStateMachine<PoweredController, PoweredController.Instance>
{
	// Token: 0x06000172 RID: 370 RVA: 0x0000A398 File Offset: 0x00008598
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (PoweredController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (PoweredController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x040000E3 RID: 227
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000E4 RID: 228
	public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000E45 RID: 3653
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E46 RID: 3654
	public new class Instance : GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006F20 RID: 28448 RVA: 0x002B8DD3 File Offset: 0x002B6FD3
		public Instance(IStateMachineTarget master, PoweredController.Def def) : base(master, def)
		{
		}

		// Token: 0x04005343 RID: 21315
		public bool ShowWorkingStatus;
	}
}
