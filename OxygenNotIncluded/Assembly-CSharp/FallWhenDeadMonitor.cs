using System;

// Token: 0x020004B3 RID: 1203
public class FallWhenDeadMonitor : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance>
{
	// Token: 0x06001B54 RID: 6996 RVA: 0x00092F7C File Offset: 0x0009117C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.standing.Transition(this.entombed, (FallWhenDeadMonitor.Instance smi) => smi.IsEntombed(), UpdateRate.SIM_200ms).Transition(this.falling, (FallWhenDeadMonitor.Instance smi) => smi.IsFalling(), UpdateRate.SIM_200ms);
		this.falling.ToggleGravity(this.standing);
		this.entombed.Transition(this.standing, (FallWhenDeadMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04000F39 RID: 3897
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x04000F3A RID: 3898
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x04000F3B RID: 3899
	public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State entombed;

	// Token: 0x02001166 RID: 4454
	public new class Instance : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007970 RID: 31088 RVA: 0x002D93D7 File Offset: 0x002D75D7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x002D93E0 File Offset: 0x002D75E0
		public bool IsEntombed()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			return component != null && component.IsEntombed;
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x002D9408 File Offset: 0x002D7608
		public bool IsFalling()
		{
			int num = Grid.CellBelow(Grid.PosToCell(base.master.transform.GetPosition()));
			return Grid.IsValidCell(num) && !Grid.Solid[num];
		}
	}
}
