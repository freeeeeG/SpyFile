using System;
using Klei.AI;

// Token: 0x02000896 RID: 2198
public class StaminaMonitor : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance>
{
	// Token: 0x06003FDE RID: 16350 RVA: 0x0016540C File Offset: 0x0016360C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleStateMachine((StaminaMonitor.Instance smi) => new UrgeMonitor.Instance(smi.master, Db.Get().Urges.Sleep, Db.Get().Amounts.Stamina, Db.Get().ScheduleBlockTypes.Sleep, 100f, 0f, false)).ToggleStateMachine((StaminaMonitor.Instance smi) => new SleepChoreMonitor.Instance(smi.master));
		this.satisfied.Transition(this.sleepy, (StaminaMonitor.Instance smi) => smi.NeedsToSleep() || smi.WantsToSleep(), UpdateRate.SIM_200ms);
		this.sleepy.Update("Check Sleep State", delegate(StaminaMonitor.Instance smi, float dt)
		{
			smi.TryExitSleepState();
		}, UpdateRate.SIM_1000ms, false).DefaultState(this.sleepy.needssleep);
		this.sleepy.needssleep.Transition(this.sleepy.sleeping, (StaminaMonitor.Instance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Tired, null).ToggleThought(Db.Get().Thoughts.Sleepy, null);
		this.sleepy.sleeping.Enter(delegate(StaminaMonitor.Instance smi)
		{
			smi.CheckDebugFastWorkMode();
		}).Transition(this.satisfied, (StaminaMonitor.Instance smi) => !smi.IsSleeping(), UpdateRate.SIM_200ms);
	}

	// Token: 0x0400298F RID: 10639
	public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002990 RID: 10640
	public StaminaMonitor.SleepyState sleepy;

	// Token: 0x04002991 RID: 10641
	private const float OUTSIDE_SCHEDULE_STAMINA_THRESHOLD = 0f;

	// Token: 0x020016B6 RID: 5814
	public class SleepyState : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C92 RID: 27794
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State needssleep;

		// Token: 0x04006C93 RID: 27795
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State sleeping;
	}

	// Token: 0x020016B7 RID: 5815
	public new class Instance : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C05 RID: 35845 RVA: 0x00317A54 File Offset: 0x00315C54
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stamina = Db.Get().Amounts.Stamina.Lookup(base.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x06008C06 RID: 35846 RVA: 0x00317AA0 File Offset: 0x00315CA0
		public bool NeedsToSleep()
		{
			return this.stamina.value <= 0f;
		}

		// Token: 0x06008C07 RID: 35847 RVA: 0x00317AB7 File Offset: 0x00315CB7
		public bool WantsToSleep()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x06008C08 RID: 35848 RVA: 0x00317AE7 File Offset: 0x00315CE7
		public void TryExitSleepState()
		{
			if (!this.NeedsToSleep() && !this.WantsToSleep())
			{
				base.smi.GoTo(base.smi.sm.satisfied);
			}
		}

		// Token: 0x06008C09 RID: 35849 RVA: 0x00317B14 File Offset: 0x00315D14
		public bool IsSleeping()
		{
			bool result = false;
			if (this.WantsToSleep() && this.choreDriver.GetComponent<Worker>().workable != null)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06008C0A RID: 35850 RVA: 0x00317B46 File Offset: 0x00315D46
		public void CheckDebugFastWorkMode()
		{
			if (Game.Instance.FastWorkersModeActive)
			{
				this.stamina.value = this.stamina.GetMax();
			}
		}

		// Token: 0x06008C0B RID: 35851 RVA: 0x00317B6C File Offset: 0x00315D6C
		public bool ShouldExitSleep()
		{
			if (this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return false;
			}
			Narcolepsy component = base.GetComponent<Narcolepsy>();
			return (!(component != null) || !component.IsNarcolepsing()) && this.stamina.value >= this.stamina.GetMax();
		}

		// Token: 0x04006C94 RID: 27796
		private ChoreDriver choreDriver;

		// Token: 0x04006C95 RID: 27797
		private Schedulable schedulable;

		// Token: 0x04006C96 RID: 27798
		public AmountInstance stamina;
	}
}
