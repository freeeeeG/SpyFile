using System;

// Token: 0x020004F3 RID: 1267
public class ReachabilityMonitor : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable>
{
	// Token: 0x06001DB1 RID: 7601 RVA: 0x0009DE70 File Offset: 0x0009C070
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unreachable;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.FastUpdate("UpdateReachability", ReachabilityMonitor.updateReachabilityCB, UpdateRate.SIM_1000ms, true);
		this.reachable.ToggleTag(GameTags.Reachable).Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.unreachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsFalse);
		this.unreachable.Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.reachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsTrue);
	}

	// Token: 0x04001083 RID: 4227
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State reachable;

	// Token: 0x04001084 RID: 4228
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State unreachable;

	// Token: 0x04001085 RID: 4229
	public StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter isReachable = new StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter(false);

	// Token: 0x04001086 RID: 4230
	private static ReachabilityMonitor.UpdateReachabilityCB updateReachabilityCB = new ReachabilityMonitor.UpdateReachabilityCB();

	// Token: 0x02001193 RID: 4499
	private class UpdateReachabilityCB : UpdateBucketWithUpdater<ReachabilityMonitor.Instance>.IUpdater
	{
		// Token: 0x06007A06 RID: 31238 RVA: 0x002DAAA7 File Offset: 0x002D8CA7
		public void Update(ReachabilityMonitor.Instance smi, float dt)
		{
			smi.UpdateReachability();
		}
	}

	// Token: 0x02001194 RID: 4500
	public new class Instance : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.GameInstance
	{
		// Token: 0x06007A08 RID: 31240 RVA: 0x002DAAB7 File Offset: 0x002D8CB7
		public Instance(Workable workable) : base(workable)
		{
			this.UpdateReachability();
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x002DAAC8 File Offset: 0x002D8CC8
		public void TriggerEvent()
		{
			bool flag = base.sm.isReachable.Get(base.smi);
			base.Trigger(-1432940121, flag);
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x002DAB00 File Offset: 0x002D8D00
		public void UpdateReachability()
		{
			if (base.master == null)
			{
				return;
			}
			int cell = Grid.PosToCell(base.master);
			base.sm.isReachable.Set(MinionGroupProber.Get().IsAllReachable(cell, base.master.GetOffsets(cell)), base.smi, false);
		}
	}
}
