using System;

// Token: 0x0200049C RID: 1180
public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
	// Token: 0x06001A9E RID: 6814 RVA: 0x0008E414 File Offset: 0x0008C614
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (FixedCapturableMonitor.Instance smi) => smi.ShouldGoGetCaptured(), null).Enter(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Add(smi);
		}).Exit(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Remove(smi);
		});
	}

	// Token: 0x02001138 RID: 4408
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001139 RID: 4409
	public new class Instance : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
	{
		// Token: 0x060078C1 RID: 30913 RVA: 0x002D7799 File Offset: 0x002D5999
		public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060078C2 RID: 30914 RVA: 0x002D77A3 File Offset: 0x002D59A3
		public bool ShouldGoGetCaptured()
		{
			return this.targetCapturePoint != null && this.targetCapturePoint.IsRunning() && this.targetCapturePoint.shouldCreatureGoGetCaptured;
		}

		// Token: 0x04005BAF RID: 23471
		public FixedCapturePoint.Instance targetCapturePoint;
	}
}
