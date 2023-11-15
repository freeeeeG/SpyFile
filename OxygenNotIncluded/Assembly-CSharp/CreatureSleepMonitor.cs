using System;

// Token: 0x02000712 RID: 1810
public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
	// Token: 0x060031C3 RID: 12739 RVA: 0x00108701 File Offset: 0x00106901
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep), null);
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x00108729 File Offset: 0x00106929
	public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
	{
		return GameClock.Instance.IsNighttime();
	}

	// Token: 0x02001473 RID: 5235
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001474 RID: 5236
	public new class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
	{
		// Token: 0x060084AE RID: 33966 RVA: 0x00303046 File Offset: 0x00301246
		public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
