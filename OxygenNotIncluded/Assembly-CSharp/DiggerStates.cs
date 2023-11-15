using System;

// Token: 0x020000BA RID: 186
public class DiggerStates : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>
{
	// Token: 0x06000359 RID: 857 RVA: 0x0001A902 File Offset: 0x00018B02
	private static bool ShouldStopHiding(DiggerStates.Instance smi)
	{
		return !GameplayEventManager.Instance.IsGameplayEventRunningWithTag(GameTags.SpaceDanger);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0001A918 File Offset: 0x00018B18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		this.move.MoveTo((DiggerStates.Instance smi) => smi.GetTunnelCell(), this.hide, this.behaviourcomplete, false);
		this.hide.Transition(this.behaviourcomplete, new StateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.Transition.ConditionCallback(DiggerStates.ShouldStopHiding), UpdateRate.SIM_4000ms);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Tunnel, false);
	}

	// Token: 0x0400023F RID: 575
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State move;

	// Token: 0x04000240 RID: 576
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State hide;

	// Token: 0x04000241 RID: 577
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State behaviourcomplete;

	// Token: 0x02000EB3 RID: 3763
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EB4 RID: 3764
	public new class Instance : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.GameInstance
	{
		// Token: 0x06007018 RID: 28696 RVA: 0x002BA46F File Offset: 0x002B866F
		public Instance(Chore<DiggerStates.Instance> chore, DiggerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Tunnel);
		}

		// Token: 0x06007019 RID: 28697 RVA: 0x002BA494 File Offset: 0x002B8694
		public int GetTunnelCell()
		{
			DiggerMonitor.Instance smi = base.smi.GetSMI<DiggerMonitor.Instance>();
			if (smi != null)
			{
				return smi.lastDigCell;
			}
			return -1;
		}
	}
}
