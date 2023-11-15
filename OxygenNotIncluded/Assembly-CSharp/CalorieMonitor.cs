using System;
using Klei.AI;

// Token: 0x0200086B RID: 2155
public class CalorieMonitor : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance>
{
	// Token: 0x06003F1A RID: 16154 RVA: 0x0015FF74 File Offset: 0x0015E174
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Transition(this.hungry, (CalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_200ms);
		this.hungry.DefaultState(this.hungry.normal).Transition(this.satisfied, (CalorieMonitor.Instance smi) => smi.IsSatisfied(), UpdateRate.SIM_200ms).EventTransition(GameHashes.BeginChore, this.eating, (CalorieMonitor.Instance smi) => smi.IsEating());
		this.hungry.working.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.normal, (CalorieMonitor.Instance smi) => smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null);
		this.hungry.normal.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.working, (CalorieMonitor.Instance smi) => !smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.hungry.starving.Transition(this.hungry.normal, (CalorieMonitor.Instance smi) => !smi.IsStarving(), UpdateRate.SIM_200ms).Transition(this.depleted, (CalorieMonitor.Instance smi) => smi.IsDepleted(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Starving, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.eating.EventTransition(GameHashes.EndChore, this.satisfied, (CalorieMonitor.Instance smi) => !smi.IsEating());
		this.depleted.ToggleTag(GameTags.CaloriesDepleted).Enter(delegate(CalorieMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x040028D5 RID: 10453
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040028D6 RID: 10454
	public CalorieMonitor.HungryState hungry;

	// Token: 0x040028D7 RID: 10455
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State eating;

	// Token: 0x040028D8 RID: 10456
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x040028D9 RID: 10457
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State depleted;

	// Token: 0x0200164C RID: 5708
	public class HungryState : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B38 RID: 27448
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x04006B39 RID: 27449
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04006B3A RID: 27450
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State starving;
	}

	// Token: 0x0200164D RID: 5709
	public new class Instance : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008A3A RID: 35386 RVA: 0x0031368A File Offset: 0x0031188A
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
		}

		// Token: 0x06008A3B RID: 35387 RVA: 0x003136B3 File Offset: 0x003118B3
		private float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06008A3C RID: 35388 RVA: 0x003136CC File Offset: 0x003118CC
		public bool IsEatTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
		}

		// Token: 0x06008A3D RID: 35389 RVA: 0x003136ED File Offset: 0x003118ED
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.825f;
		}

		// Token: 0x06008A3E RID: 35390 RVA: 0x003136FC File Offset: 0x003118FC
		public bool IsStarving()
		{
			return this.GetCalories0to1() < 0.25f;
		}

		// Token: 0x06008A3F RID: 35391 RVA: 0x0031370B File Offset: 0x0031190B
		public bool IsSatisfied()
		{
			return this.GetCalories0to1() > 0.95f;
		}

		// Token: 0x06008A40 RID: 35392 RVA: 0x0031371C File Offset: 0x0031191C
		public bool IsEating()
		{
			ChoreDriver component = base.master.GetComponent<ChoreDriver>();
			return component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x06008A41 RID: 35393 RVA: 0x00313760 File Offset: 0x00311960
		public bool IsDepleted()
		{
			return this.calories.value <= 0f;
		}

		// Token: 0x06008A42 RID: 35394 RVA: 0x00313777 File Offset: 0x00311977
		public bool ShouldExitInfirmary()
		{
			return !this.IsStarving();
		}

		// Token: 0x06008A43 RID: 35395 RVA: 0x00313782 File Offset: 0x00311982
		public void Kill()
		{
			if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
			{
				base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
			}
		}

		// Token: 0x04006B3B RID: 27451
		public AmountInstance calories;
	}
}
