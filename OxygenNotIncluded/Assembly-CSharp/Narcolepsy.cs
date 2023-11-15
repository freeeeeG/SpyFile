using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020008AB RID: 2219
[SkipSaveFileSerialization]
public class Narcolepsy : StateMachineComponent<Narcolepsy.StatesInstance>
{
	// Token: 0x0600405A RID: 16474 RVA: 0x001688F0 File Offset: 0x00166AF0
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600405B RID: 16475 RVA: 0x001688FD File Offset: 0x00166AFD
	public bool IsNarcolepsing()
	{
		return base.smi.IsNarcolepsing();
	}

	// Token: 0x04002A02 RID: 10754
	public static readonly Chore.Precondition IsNarcolepsingPrecondition = new Chore.Precondition
	{
		id = "IsNarcolepsingPrecondition",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NARCOLEPSING,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			return component != null && component.IsNarcolepsing();
		}
	};

	// Token: 0x020016EB RID: 5867
	public class StatesInstance : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.GameInstance
	{
		// Token: 0x06008CF9 RID: 36089 RVA: 0x0031A51C File Offset: 0x0031871C
		public StatesInstance(Narcolepsy master) : base(master)
		{
		}

		// Token: 0x06008CFA RID: 36090 RVA: 0x0031A528 File Offset: 0x00318728
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06008CFB RID: 36091 RVA: 0x0031A54C File Offset: 0x0031874C
		public bool IsNarcolepsing()
		{
			return this.GetCurrentState() == base.sm.sleepy;
		}

		// Token: 0x06008CFC RID: 36092 RVA: 0x0031A561 File Offset: 0x00318761
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "NarcolepticSleep";
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}
	}

	// Token: 0x020016EC RID: 5868
	public class States : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy>
	{
		// Token: 0x06008CFD RID: 36093 RVA: 0x0031A58C File Offset: 0x0031878C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextSleep", delegate(Narcolepsy.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_INTERVAL_MIN, TRAITS.NARCOLEPSY_INTERVAL_MAX), this.sleepy);
			});
			this.sleepy.Enter("Is Already Sleeping Check", delegate(Narcolepsy.StatesInstance smi)
			{
				if (smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping())
				{
					smi.GoTo(this.idle);
					return;
				}
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_SLEEPDURATION_MIN, TRAITS.NARCOLEPSY_SLEEPDURATION_MAX), this.idle);
			}).ToggleUrge(Db.Get().Urges.Narcolepsy).ToggleChore(new Func<Narcolepsy.StatesInstance, Chore>(this.CreateNarcolepsyChore), this.idle);
		}

		// Token: 0x06008CFE RID: 36094 RVA: 0x0031A61C File Offset: 0x0031881C
		private Chore CreateNarcolepsyChore(Narcolepsy.StatesInstance smi)
		{
			GameObject bed = smi.CreateFloorLocator();
			SleepChore sleepChore = new SleepChore(Db.Get().ChoreTypes.Narcolepsy, smi.master, bed, true, false);
			sleepChore.AddPrecondition(Narcolepsy.IsNarcolepsingPrecondition, null);
			return sleepChore;
		}

		// Token: 0x06008CFF RID: 36095 RVA: 0x0031A659 File Offset: 0x00318859
		private float GetNewInterval(float min, float max)
		{
			Mathf.Min(Mathf.Max(Util.GaussianRandom(max - min, 1f), min), max);
			return UnityEngine.Random.Range(min, max);
		}

		// Token: 0x04006D72 RID: 28018
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State idle;

		// Token: 0x04006D73 RID: 28019
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State sleepy;
	}
}
