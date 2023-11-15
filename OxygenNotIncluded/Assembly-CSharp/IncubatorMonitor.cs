using System;

// Token: 0x020004A0 RID: 1184
public class IncubatorMonitor : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>
{
	// Token: 0x06001AAE RID: 6830 RVA: 0x0008E9D0 File Offset: 0x0008CBD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.not;
		this.not.EventTransition(GameHashes.OnStore, this.in_incubator, new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator));
		this.in_incubator.ToggleTag(GameTags.Creatures.InIncubator).EventTransition(GameHashes.OnStore, this.not, GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Not(new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator)));
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x0008EA3A File Offset: 0x0008CC3A
	public static bool InIncubator(IncubatorMonitor.Instance smi)
	{
		return smi.gameObject.transform.parent && smi.gameObject.transform.parent.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x04000ED6 RID: 3798
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not;

	// Token: 0x04000ED7 RID: 3799
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State in_incubator;

	// Token: 0x02001148 RID: 4424
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001149 RID: 4425
	public new class Instance : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.GameInstance
	{
		// Token: 0x060078FA RID: 30970 RVA: 0x002D81C4 File Offset: 0x002D63C4
		public Instance(IStateMachineTarget master, IncubatorMonitor.Def def) : base(master, def)
		{
		}
	}
}
