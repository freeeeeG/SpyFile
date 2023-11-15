using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class SleepChore : Chore<SleepChore.StatesInstance>
{
	// Token: 0x06001400 RID: 5120 RVA: 0x0006AADC File Offset: 0x00068CDC
	public SleepChore(ChoreType choreType, IStateMachineTarget target, GameObject bed, bool bedIsLocator, bool isInterruptable) : base(choreType, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new SleepChore.StatesInstance(this, target.gameObject, bed, bedIsLocator, isInterruptable);
		if (isInterruptable)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		base.AddPrecondition(SleepChore.IsOkayTimeToSleep, null);
		Operational component = bed.GetComponent<Operational>();
		if (component != null)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0006AB5C File Offset: 0x00068D5C
	public static Sleepable GetSafeFloorLocator(GameObject sleeper)
	{
		int num = sleeper.GetComponent<Sensors>().GetSensor<SafeCellSensor>().GetSleepCellQuery();
		if (num == Grid.InvalidCell)
		{
			num = Grid.PosToCell(sleeper.transform.GetPosition());
		}
		return ChoreHelpers.CreateSleepLocator(Grid.CellToPosCBC(num, Grid.SceneLayer.Move)).GetComponent<Sleepable>();
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x0006ABA5 File Offset: 0x00068DA5
	public static bool IsDarkAtCell(int cell)
	{
		return Grid.LightIntensity[cell] <= 0;
	}

	// Token: 0x04000AC8 RID: 2760
	public static readonly Chore.Precondition IsOkayTimeToSleep = new Chore.Precondition
	{
		id = "IsOkayTimeToSleep",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OKAY_TIME_TO_SLEEP,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			bool flag = component != null && component.IsNarcolepsing();
			StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
			bool flag2 = smi != null && smi.NeedsToSleep();
			bool flag3 = ChorePreconditions.instance.IsScheduledTime.fn(ref context, Db.Get().ScheduleBlockTypes.Sleep);
			return flag || flag3 || flag2;
		}
	};

	// Token: 0x02001028 RID: 4136
	public class StatesInstance : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.GameInstance
	{
		// Token: 0x060074DE RID: 29918 RVA: 0x002CA7F8 File Offset: 0x002C89F8
		public StatesInstance(SleepChore master, GameObject sleeper, GameObject bed, bool bedIsLocator, bool isInterruptable) : base(master)
		{
			base.sm.sleeper.Set(sleeper, base.smi, false);
			base.sm.isInterruptable.Set(isInterruptable, base.smi, false);
			Traits component = sleeper.GetComponent<Traits>();
			if (component != null)
			{
				base.sm.needsNightLight.Set(component.HasTrait("NightLight"), base.smi, false);
			}
			if (bedIsLocator)
			{
				this.AddLocator(bed);
				return;
			}
			base.sm.bed.Set(bed, base.smi, false);
		}

		// Token: 0x060074DF RID: 29919 RVA: 0x002CA8AC File Offset: 0x002C8AAC
		public void CheckLightLevel()
		{
			GameObject go = base.sm.sleeper.Get(base.smi);
			int cell = Grid.PosToCell(go);
			if (Grid.IsValidCell(cell))
			{
				bool flag = SleepChore.IsDarkAtCell(cell);
				if (base.sm.needsNightLight.Get(base.smi))
				{
					if (flag)
					{
						go.Trigger(-1307593733, null);
						return;
					}
				}
				else if (!flag && !this.IsLoudSleeper() && !this.IsGlowStick())
				{
					go.Trigger(-1063113160, null);
				}
			}
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x002CA92D File Offset: 0x002C8B2D
		public bool IsLoudSleeper()
		{
			return base.sm.sleeper.Get(base.smi).GetComponent<Snorer>() != null;
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x002CA955 File Offset: 0x002C8B55
		public bool IsGlowStick()
		{
			return base.sm.sleeper.Get(base.smi).GetComponent<GlowStick>() != null;
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x002CA97D File Offset: 0x002C8B7D
		public void EvaluateSleepQuality()
		{
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x002CA980 File Offset: 0x002C8B80
		public void AddLocator(GameObject sleepable)
		{
			this.locator = sleepable;
			int i = Grid.PosToCell(this.locator);
			Grid.Reserved[i] = true;
			base.sm.bed.Set(this.locator, this, false);
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x002CA9C8 File Offset: 0x002C8BC8
		public void DestroyLocator()
		{
			if (this.locator != null)
			{
				Grid.Reserved[Grid.PosToCell(this.locator)] = false;
				ChoreHelpers.DestroyLocator(this.locator);
				base.sm.bed.Set(null, this);
				this.locator = null;
			}
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x002CAA20 File Offset: 0x002C8C20
		public void SetAnim()
		{
			Sleepable sleepable = base.sm.bed.Get<Sleepable>(base.smi);
			if (sleepable.GetComponent<Building>() == null)
			{
				NavType currentNavType = base.sm.sleeper.Get<Navigator>(base.smi).CurrentNavType;
				string s;
				if (currentNavType != NavType.Ladder)
				{
					if (currentNavType != NavType.Pole)
					{
						s = "anim_sleep_floor_kanim";
					}
					else
					{
						s = "anim_sleep_pole_kanim";
					}
				}
				else
				{
					s = "anim_sleep_ladder_kanim";
				}
				sleepable.overrideAnims = new KAnimFile[]
				{
					Assets.GetAnim(s)
				};
			}
		}

		// Token: 0x04005848 RID: 22600
		public bool hadPeacefulSleep;

		// Token: 0x04005849 RID: 22601
		public bool hadNormalSleep;

		// Token: 0x0400584A RID: 22602
		public bool hadBadSleep;

		// Token: 0x0400584B RID: 22603
		public bool hadTerribleSleep;

		// Token: 0x0400584C RID: 22604
		public int lastEvaluatedDay = -1;

		// Token: 0x0400584D RID: 22605
		public float wakeUpBuffer = 2f;

		// Token: 0x0400584E RID: 22606
		public string stateChangeNoiseSource;

		// Token: 0x0400584F RID: 22607
		private GameObject locator;
	}

	// Token: 0x02001029 RID: 4137
	public class States : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore>
	{
		// Token: 0x060074E6 RID: 29926 RVA: 0x002CAAA8 File Offset: 0x002C8CA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.sleeper);
			this.root.Exit("DestroyLocator", delegate(SleepChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.approach.InitializeStates(this.sleeper, this.bed, this.sleep, null, null, null);
			this.sleep.Enter("SetAnims", delegate(SleepChore.StatesInstance smi)
			{
				smi.SetAnim();
			}).DefaultState(this.sleep.normal).ToggleTag(GameTags.Asleep).DoSleep(this.sleeper, this.bed, this.success, null).TriggerOnExit(GameHashes.SleepFinished, null).EventHandler(GameHashes.SleepDisturbedByLight, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByLight.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByNoise, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByNoise.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByFearOfDark, delegate(SleepChore.StatesInstance smi)
			{
				this.isScaredOfDark.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByMovement, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByMovement.Set(true, smi, false);
			});
			this.sleep.uninterruptable.DoNothing();
			this.sleep.normal.ParamTransition<bool>(this.isInterruptable, this.sleep.uninterruptable, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsFalse).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Sleeping, null).QueueAnim("working_loop", true, null).ParamTransition<bool>(this.isDisturbedByNoise, this.sleep.interrupt_noise, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByLight, this.sleep.interrupt_light, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isScaredOfDark, this.sleep.interrupt_scared, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByMovement, this.sleep.interrupt_movement, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).Update(delegate(SleepChore.StatesInstance smi, float dt)
			{
				smi.CheckLightLevel();
			}, UpdateRate.SIM_200ms, false);
			this.sleep.interrupt_scared.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByFearOfDark, null).QueueAnim("interrupt_afraid", false, null).OnAnimQueueComplete(this.sleep.interrupt_scared_transition);
			this.sleep.interrupt_scared_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepAfraidOfDark"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isScaredOfDark.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_movement.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByMovement, null).PlayAnim("interrupt_light").OnAnimQueueComplete(this.sleep.interrupt_movement_transition).Enter(delegate(SleepChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.bed.Get(smi);
				if (gameObject != null)
				{
					gameObject.Trigger(-717201811, null);
				}
			});
			this.sleep.interrupt_movement_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepMovement"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByMovement.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_noise.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByNoise, null).QueueAnim("interrupt_light", false, null).OnAnimQueueComplete(this.sleep.interrupt_noise_transition);
			this.sleep.interrupt_noise_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				Effects component = smi.master.GetComponent<Effects>();
				component.Add(Db.Get().effects.Get("TerribleSleep"), true);
				if (component.HasEffect(Db.Get().effects.Get("BadSleep")))
				{
					component.Remove(Db.Get().effects.Get("BadSleep"));
				}
				this.isDisturbedByNoise.Set(false, smi, false);
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				smi.GoTo(state);
			});
			this.sleep.interrupt_light.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByLight, null).QueueAnim("interrupt", false, null).OnAnimQueueComplete(this.sleep.interrupt_light_transition);
			this.sleep.interrupt_light_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleep"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByLight.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.success.Enter(delegate(SleepChore.StatesInstance smi)
			{
				smi.EvaluateSleepQuality();
			}).ReturnSuccess();
		}

		// Token: 0x04005850 RID: 22608
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter sleeper;

		// Token: 0x04005851 RID: 22609
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter bed;

		// Token: 0x04005852 RID: 22610
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isInterruptable;

		// Token: 0x04005853 RID: 22611
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByNoise;

		// Token: 0x04005854 RID: 22612
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByLight;

		// Token: 0x04005855 RID: 22613
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByMovement;

		// Token: 0x04005856 RID: 22614
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isScaredOfDark;

		// Token: 0x04005857 RID: 22615
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter needsNightLight;

		// Token: 0x04005858 RID: 22616
		public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04005859 RID: 22617
		public SleepChore.States.SleepStates sleep;

		// Token: 0x0400585A RID: 22618
		public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State success;

		// Token: 0x02001FF2 RID: 8178
		public class SleepStates : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State
		{
			// Token: 0x04008FCA RID: 36810
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition;

			// Token: 0x04008FCB RID: 36811
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition_pre;

			// Token: 0x04008FCC RID: 36812
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State uninterruptable;

			// Token: 0x04008FCD RID: 36813
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State normal;

			// Token: 0x04008FCE RID: 36814
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise;

			// Token: 0x04008FCF RID: 36815
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise_transition;

			// Token: 0x04008FD0 RID: 36816
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light;

			// Token: 0x04008FD1 RID: 36817
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light_transition;

			// Token: 0x04008FD2 RID: 36818
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_scared;

			// Token: 0x04008FD3 RID: 36819
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_scared_transition;

			// Token: 0x04008FD4 RID: 36820
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_movement;

			// Token: 0x04008FD5 RID: 36821
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_movement_transition;
		}
	}
}
