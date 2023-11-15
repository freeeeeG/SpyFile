using System;
using STRINGS;

// Token: 0x020000B5 RID: 181
public class CreatureSleepStates : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>
{
	// Token: 0x0600033D RID: 829 RVA: 0x00019C9C File Offset: 0x00017E9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.SLEEPING.NAME, CREATURES.STATUSITEMS.SLEEPING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.pre.QueueAnim("sleep_pre", false, null).OnAnimQueueComplete(this.loop);
		this.loop.QueueAnim("sleep_loop", true, null).Transition(this.pst, new StateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.Transition.ConditionCallback(CreatureSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
		this.pst.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.SleepBehaviour, false);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00019D71 File Offset: 0x00017F71
	public static bool ShouldWakeUp(CreatureSleepStates.Instance smi)
	{
		return !GameClock.Instance.IsNighttime();
	}

	// Token: 0x0400022C RID: 556
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pre;

	// Token: 0x0400022D RID: 557
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State loop;

	// Token: 0x0400022E RID: 558
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pst;

	// Token: 0x0400022F RID: 559
	public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State behaviourcomplete;

	// Token: 0x02000EA2 RID: 3746
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EA3 RID: 3747
	public new class Instance : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.GameInstance
	{
		// Token: 0x06006FF6 RID: 28662 RVA: 0x002BA0C6 File Offset: 0x002B82C6
		public Instance(Chore<CreatureSleepStates.Instance> chore, CreatureSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.SleepBehaviour);
		}
	}
}
