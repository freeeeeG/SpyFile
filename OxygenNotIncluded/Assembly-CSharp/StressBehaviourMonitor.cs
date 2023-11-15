using System;
using TUNING;

// Token: 0x02000898 RID: 2200
public class StressBehaviourMonitor : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance>
{
	// Token: 0x06003FEB RID: 16363 RVA: 0x00165924 File Offset: 0x00163B24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.satisfied.EventTransition(GameHashes.Stressed, this.stressed, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed());
		this.stressed.DefaultState(this.stressed.tierOne).ToggleExpression(Db.Get().Expressions.Unhappy, null).ToggleAnims((StressBehaviourMonitor.Instance smi) => smi.tierOneLocoAnim).Transition(this.satisfied, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && !smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed(), UpdateRate.SIM_200ms);
		this.stressed.tierOne.DefaultState(this.stressed.tierOne.actingOut).EventTransition(GameHashes.StressedHadEnough, this.stressed.tierTwo, null);
		this.stressed.tierOne.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierOneStressChore(), this.stressed.tierOne.reprieve);
		this.stressed.tierOne.reprieve.ScheduleGoTo(30f, this.stressed.tierOne.actingOut);
		this.stressed.tierTwo.DefaultState(this.stressed.tierTwo.actingOut).Update(delegate(StressBehaviourMonitor.Instance smi, float dt)
		{
			smi.sm.timeInTierTwoStressResponse.Set(smi.sm.timeInTierTwoStressResponse.Get(smi) + dt, smi, false);
		}, UpdateRate.SIM_200ms, false).Exit("ResetStress", delegate(StressBehaviourMonitor.Instance smi)
		{
			Db.Get().Amounts.Stress.Lookup(smi.gameObject).SetValue(STRESS.ACTING_OUT_RESET);
		});
		this.stressed.tierTwo.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierTwoStressChore(), this.stressed.tierTwo.reprieve);
		this.stressed.tierTwo.reprieve.ToggleChore((StressBehaviourMonitor.Instance smi) => new StressIdleChore(smi.master), null).Enter(delegate(StressBehaviourMonitor.Instance smi)
		{
			if (smi.sm.timeInTierTwoStressResponse.Get(smi) >= 150f)
			{
				smi.sm.timeInTierTwoStressResponse.Set(0f, smi, false);
				smi.GoTo(this.stressed);
			}
		}).ScheduleGoTo((StressBehaviourMonitor.Instance smi) => smi.tierTwoReprieveDuration, this.stressed.tierTwo);
	}

	// Token: 0x04002996 RID: 10646
	public StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeInTierTwoStressResponse;

	// Token: 0x04002997 RID: 10647
	public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002998 RID: 10648
	public StressBehaviourMonitor.StressedState stressed;

	// Token: 0x020016BA RID: 5818
	public class StressedState : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006CA0 RID: 27808
		public StressBehaviourMonitor.TierOneStates tierOne;

		// Token: 0x04006CA1 RID: 27809
		public StressBehaviourMonitor.TierTwoStates tierTwo;
	}

	// Token: 0x020016BB RID: 5819
	public class TierOneStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006CA2 RID: 27810
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x04006CA3 RID: 27811
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x020016BC RID: 5820
	public class TierTwoStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006CA4 RID: 27812
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x04006CA5 RID: 27813
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x020016BD RID: 5821
	public new class Instance : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C19 RID: 35865 RVA: 0x00317C9F File Offset: 0x00315E9F
		public Instance(IStateMachineTarget master, Func<ChoreProvider, Chore> tier_one_stress_chore_creator, Func<ChoreProvider, Chore> tier_two_stress_chore_creator, string tier_one_loco_anim, float tier_two_reprieve_duration = 3f) : base(master)
		{
			this.tierOneLocoAnim = tier_one_loco_anim;
			this.tierTwoReprieveDuration = tier_two_reprieve_duration;
			this.tierOneStressChoreCreator = tier_one_stress_chore_creator;
			this.tierTwoStressChoreCreator = tier_two_stress_chore_creator;
		}

		// Token: 0x06008C1A RID: 35866 RVA: 0x00317CD1 File Offset: 0x00315ED1
		public Chore CreateTierOneStressChore()
		{
			return this.tierOneStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x06008C1B RID: 35867 RVA: 0x00317CE4 File Offset: 0x00315EE4
		public Chore CreateTierTwoStressChore()
		{
			return this.tierTwoStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x04006CA6 RID: 27814
		public Func<ChoreProvider, Chore> tierOneStressChoreCreator;

		// Token: 0x04006CA7 RID: 27815
		public Func<ChoreProvider, Chore> tierTwoStressChoreCreator;

		// Token: 0x04006CA8 RID: 27816
		public string tierOneLocoAnim = "";

		// Token: 0x04006CA9 RID: 27817
		public float tierTwoReprieveDuration;
	}
}
