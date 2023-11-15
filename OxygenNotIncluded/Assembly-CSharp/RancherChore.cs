using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020003C1 RID: 961
public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
	// Token: 0x060013EE RID: 5102 RVA: 0x0006A344 File Offset: 0x00068544
	public RancherChore(KPrefabID rancher_station)
	{
		Chore.Precondition isOpenForRanching = default(Chore.Precondition);
		isOpenForRanching.id = "IsCreatureAvailableForRanching";
		isOpenForRanching.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING;
		isOpenForRanching.sortOrder = -3;
		isOpenForRanching.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RanchStation.Instance instance = data as RanchStation.Instance;
			return !instance.HasRancher && instance.IsCritterAvailableForRanching;
		};
		this.IsOpenForRanching = isOpenForRanching;
		base..ctor(Db.Get().ChoreTypes.Ranch, rancher_station, null, false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		base.AddPrecondition(this.IsOpenForRanching, rancher_station.GetSMI<RanchStation.Instance>());
		SkillPerkMissingComplainer component = base.GetComponent<SkillPerkMissingComplainer>();
		base.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, component.requiredSkillPerk);
		base.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, rancher_station.GetComponent<Building>());
		Operational component2 = rancher_station.GetComponent<Operational>();
		base.AddPrecondition(ChorePreconditions.instance.IsOperational, component2);
		Deconstructable component3 = rancher_station.GetComponent<Deconstructable>();
		base.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component3);
		BuildingEnabledButton component4 = rancher_station.GetComponent<BuildingEnabledButton>();
		base.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component4);
		base.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
		base.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x0006A495 File Offset: 0x00068695
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0006A4C6 File Offset: 0x000686C6
	protected override void End(string reason)
	{
		base.End(reason);
		base.smi.sm.rancher.Set(null, base.smi);
	}

	// Token: 0x04000AC3 RID: 2755
	public Chore.Precondition IsOpenForRanching;

	// Token: 0x02001019 RID: 4121
	public class RancherChoreStates : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
	{
		// Token: 0x060074A8 RID: 29864 RVA: 0x002C9510 File Offset: 0x002C7710
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveToRanch;
			base.Target(this.rancher);
			this.root.Exit("TriggerRanchStationNoLongerAvailable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.TriggerRanchStationNoLongerAvailable();
			});
			this.moveToRanch.MoveTo((RancherChore.RancherChoreStates.Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), this.waitForAvailableRanchable, null, false);
			this.waitForAvailableRanchable.Enter("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.WaitForAvailableRanchable(0f);
			}).Update("FindRanchable", delegate(RancherChore.RancherChoreStates.Instance smi, float dt)
			{
				smi.WaitForAvailableRanchable(dt);
			}, UpdateRate.SIM_200ms, false);
			this.ranchCritter.ScheduleGoTo(0.5f, this.ranchCritter.callForCritter).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.waitForAvailableRanchable, null);
			this.ranchCritter.callForCritter.ToggleAnims("anim_interacts_rancherstation_kanim", 0f, "").PlayAnim("calling_loop", KAnim.PlayMode.Loop).ScheduleActionNextFrame("TellCreatureRancherIsReady", delegate(RancherChore.RancherChoreStates.Instance smi)
			{
				smi.ranchStation.MessageRancherReady();
			}).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranchCritter.working, null);
			this.ranchCritter.working.ToggleWork<RancherChore.RancherWorkable>(this.masterTarget, this.ranchCritter.pst, this.waitForAvailableRanchable, null);
			this.ranchCritter.pst.ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherInteractAnim)).QueueAnim("wipe_brow", false, null).OnAnimQueueComplete(this.waitForAvailableRanchable);
		}

		// Token: 0x060074A9 RID: 29865 RVA: 0x002C96EE File Offset: 0x002C78EE
		private static HashedString GetRancherInteractAnim(RancherChore.RancherChoreStates.Instance smi)
		{
			return smi.ranchStation.def.RancherInteractAnim;
		}

		// Token: 0x060074AA RID: 29866 RVA: 0x002C9700 File Offset: 0x002C7900
		public static bool TryRanchCreature(RancherChore.RancherChoreStates.Instance smi)
		{
			Debug.Assert(smi.ranchStation != null, "smi.ranchStation was null");
			RanchedStates.Instance activeRanchable = smi.ranchStation.ActiveRanchable;
			if (activeRanchable.IsNullOrStopped())
			{
				return false;
			}
			KPrefabID component = activeRanchable.GetComponent<KPrefabID>();
			smi.sm.rancher.Get(smi).Trigger(937885943, component.PrefabTag.Name);
			smi.ranchStation.RanchCreature();
			return true;
		}

		// Token: 0x04005825 RID: 22565
		public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;

		// Token: 0x04005826 RID: 22566
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State moveToRanch;

		// Token: 0x04005827 RID: 22567
		private RancherChore.RancherChoreStates.RanchState ranchCritter;

		// Token: 0x04005828 RID: 22568
		private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitForAvailableRanchable;

		// Token: 0x02001FE8 RID: 8168
		private class RanchState : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x04008FA2 RID: 36770
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State callForCritter;

			// Token: 0x04008FA3 RID: 36771
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State working;

			// Token: 0x04008FA4 RID: 36772
			public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x02001FE9 RID: 8169
		public new class Instance : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600A401 RID: 41985 RVA: 0x00368D7E File Offset: 0x00366F7E
			public Instance(KPrefabID rancher_station) : base(rancher_station)
			{
				this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
			}

			// Token: 0x0600A402 RID: 41986 RVA: 0x00368D94 File Offset: 0x00366F94
			public void WaitForAvailableRanchable(float dt)
			{
				this.waitTime += dt;
				GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state = this.ranchStation.IsCritterAvailableForRanching ? base.sm.ranchCritter : null;
				if (state != null || this.waitTime >= 2f)
				{
					this.waitTime = 0f;
					this.GoTo(state);
				}
			}

			// Token: 0x04008FA5 RID: 36773
			private const float WAIT_FOR_RANCHABLE_TIMEOUT = 2f;

			// Token: 0x04008FA6 RID: 36774
			public RanchStation.Instance ranchStation;

			// Token: 0x04008FA7 RID: 36775
			private float waitTime;
		}
	}

	// Token: 0x0200101A RID: 4122
	public class RancherWorkable : Workable
	{
		// Token: 0x060074AC RID: 29868 RVA: 0x002C9778 File Offset: 0x002C7978
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.ranch = base.gameObject.GetSMI<RanchStation.Instance>();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.ranch.def.RancherInteractAnim)
			};
			base.SetWorkTime(this.ranch.def.WorkTime);
			base.SetWorkerStatusItem(this.ranch.def.RanchingStatusItem);
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.lightEfficiencyBonus = false;
		}

		// Token: 0x060074AD RID: 29869 RVA: 0x002C9823 File Offset: 0x002C7A23
		public override Klei.AI.Attribute GetWorkAttribute()
		{
			return Db.Get().Attributes.Ranching;
		}

		// Token: 0x060074AE RID: 29870 RVA: 0x002C9834 File Offset: 0x002C7A34
		protected override void OnStartWork(Worker worker)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.critterAnimController = this.ranch.ActiveRanchable.AnimController;
			this.critterAnimController.Play(this.ranch.def.RanchedPreAnim, KAnim.PlayMode.Once, 1f, 0f);
			this.critterAnimController.Queue(this.ranch.def.RanchedLoopAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x060074AF RID: 29871 RVA: 0x002C98AC File Offset: 0x002C7AAC
		protected override bool OnWorkTick(Worker worker, float dt)
		{
			if (this.ranch.def.OnRanchWorkTick != null)
			{
				this.ranch.def.OnRanchWorkTick(this.ranch.ActiveRanchable.gameObject, dt, this);
			}
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x002C98FC File Offset: 0x002C7AFC
		public override void OnPendingCompleteWork(Worker work)
		{
			RancherChore.RancherChoreStates.Instance smi = base.gameObject.GetSMI<RancherChore.RancherChoreStates.Instance>();
			if (this.ranch == null || smi == null)
			{
				return;
			}
			if (RancherChore.RancherChoreStates.TryRanchCreature(smi))
			{
				this.critterAnimController.Play(this.ranch.def.RanchedPstAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x002C994F File Offset: 0x002C7B4F
		protected override void OnAbortWork(Worker worker)
		{
			if (this.ranch == null || this.critterAnimController == null)
			{
				return;
			}
			this.critterAnimController.Play(this.ranch.def.RanchedAbortAnim, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x04005829 RID: 22569
		private RanchStation.Instance ranch;

		// Token: 0x0400582A RID: 22570
		private KBatchedAnimController critterAnimController;
	}
}
