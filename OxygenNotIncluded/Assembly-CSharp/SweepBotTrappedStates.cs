using System;

// Token: 0x02000311 RID: 785
public class SweepBotTrappedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>
{
	// Token: 0x06000FE6 RID: 4070 RVA: 0x00055BBC File Offset: 0x00053DBC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.blockedStates.evaluating;
		this.blockedStates.ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, (SweepBotTrappedStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Robots.Behaviours.TrappedBehaviour, this.behaviourcomplete, true);
		this.blockedStates.evaluating.Enter(delegate(SweepBotTrappedStates.Instance smi)
		{
			if (smi.sm.GetSweepLocker(smi) == null)
			{
				smi.GoTo(this.blockedStates.noHome);
				return;
			}
			smi.GoTo(this.blockedStates.blocked);
		});
		this.blockedStates.blocked.ToggleChore((SweepBotTrappedStates.Instance smi) => new RescueSweepBotChore(smi.master, smi.master.gameObject, smi.sm.GetSweepLocker(smi).gameObject), this.behaviourcomplete, this.blockedStates.evaluating).PlayAnim("react_stuck", KAnim.PlayMode.Loop);
		this.blockedStates.noHome.PlayAnim("react_stuck", KAnim.PlayMode.Once).OnAnimQueueComplete(this.blockedStates.evaluating);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.TrappedBehaviour, false);
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x00055CD4 File Offset: 0x00053ED4
	public Storage GetSweepLocker(SweepBotTrappedStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x040008BD RID: 2237
	public SweepBotTrappedStates.BlockedStates blockedStates;

	// Token: 0x040008BE RID: 2238
	public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State behaviourcomplete;

	// Token: 0x02000F8C RID: 3980
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F8D RID: 3981
	public new class Instance : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.GameInstance
	{
		// Token: 0x06007257 RID: 29271 RVA: 0x002BF2C6 File Offset: 0x002BD4C6
		public Instance(Chore<SweepBotTrappedStates.Instance> chore, SweepBotTrappedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.TrappedBehaviour);
		}
	}

	// Token: 0x02000F8E RID: 3982
	public class BlockedStates : GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State
	{
		// Token: 0x04005629 RID: 22057
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State evaluating;

		// Token: 0x0400562A RID: 22058
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State blocked;

		// Token: 0x0400562B RID: 22059
		public GameStateMachine<SweepBotTrappedStates, SweepBotTrappedStates.Instance, IStateMachineTarget, SweepBotTrappedStates.Def>.State noHome;
	}
}
