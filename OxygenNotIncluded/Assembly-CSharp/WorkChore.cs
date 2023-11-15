using System;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class WorkChore<WorkableType> : Chore<WorkChore<WorkableType>.StatesInstance> where WorkableType : Workable
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06001412 RID: 5138 RVA: 0x0006B19B File Offset: 0x0006939B
	// (set) Token: 0x06001413 RID: 5139 RVA: 0x0006B1A3 File Offset: 0x000693A3
	public bool onlyWhenOperational { get; private set; }

	// Token: 0x06001414 RID: 5140 RVA: 0x0006B1AC File Offset: 0x000693AC
	public override string ToString()
	{
		return "WorkChore<" + typeof(WorkableType).ToString() + ">";
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x0006B1CC File Offset: 0x000693CC
	public WorkChore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, bool allow_in_red_alert = true, ScheduleBlockType schedule_block = null, bool ignore_schedule_block = false, bool only_when_operational = true, KAnimFile override_anims = null, bool is_preemptable = false, bool allow_in_context_menu = true, bool allow_prioritization = true, PriorityScreen.PriorityClass priority_class = PriorityScreen.PriorityClass.basic, int priority_class_value = 5, bool ignore_building_assignment = false, bool add_to_daily_report = true) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, priority_class, priority_class_value, is_preemptable, allow_in_context_menu, 0, add_to_daily_report, ReportManager.ReportType.WorkTime)
	{
		base.smi = new WorkChore<WorkableType>.StatesInstance(this, target.gameObject, override_anims);
		this.onlyWhenOperational = only_when_operational;
		if (allow_prioritization)
		{
			base.SetPrioritizable(target.GetComponent<Prioritizable>());
		}
		base.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, null);
		if (!allow_in_red_alert)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		if (schedule_block != null)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, schedule_block);
		}
		else if (!ignore_schedule_block)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		}
		base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, base.smi.sm.workable.Get<WorkableType>(base.smi));
		Operational component = target.GetComponent<Operational>();
		if (only_when_operational && component != null)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
		if (only_when_operational)
		{
			Deconstructable component2 = target.GetComponent<Deconstructable>();
			if (component2 != null)
			{
				base.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component2);
			}
			BuildingEnabledButton component3 = target.GetComponent<BuildingEnabledButton>();
			if (component3 != null)
			{
				base.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component3);
			}
		}
		if (!ignore_building_assignment && base.smi.sm.workable.Get(base.smi).GetComponent<Assignable>() != null)
		{
			base.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, base.smi.sm.workable.Get<Assignable>(base.smi));
		}
		WorkableType workableType = target as WorkableType;
		if (workableType != null)
		{
			if (!string.IsNullOrEmpty(workableType.requiredSkillPerk))
			{
				HashedString hashedString = workableType.requiredSkillPerk;
				base.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, hashedString);
			}
			if (workableType.requireMinionToWork)
			{
				base.AddPrecondition(ChorePreconditions.instance.IsMinion, null);
			}
		}
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0006B3F3 File Offset: 0x000695F3
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.worker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x0006B424 File Offset: 0x00069624
	public bool IsOperationalValid()
	{
		if (this.onlyWhenOperational)
		{
			Operational component = base.smi.master.GetComponent<Operational>();
			if (component != null && !component.IsOperational)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x0006B460 File Offset: 0x00069660
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		if (!base.CanPreempt(context))
		{
			return false;
		}
		if (context.chore.driver == null)
		{
			return false;
		}
		if (context.chore.driver == context.consumerState.choreDriver)
		{
			return false;
		}
		Workable workable = base.smi.sm.workable.Get<WorkableType>(base.smi);
		if (workable == null)
		{
			return false;
		}
		if (workable.worker != null && (workable.worker.state == Worker.State.PendingCompletion || workable.worker.state == Worker.State.Completing))
		{
			return false;
		}
		if (this.preemption_cb != null)
		{
			if (!this.preemption_cb(context))
			{
				return false;
			}
		}
		else
		{
			int num = 4;
			int navigationCost = context.chore.driver.GetComponent<Navigator>().GetNavigationCost(workable);
			if (navigationCost == -1 || navigationCost < num)
			{
				return false;
			}
			if (context.consumerState.navigator.GetNavigationCost(workable) * 2 > navigationCost)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000AD1 RID: 2769
	public Func<Chore.Precondition.Context, bool> preemption_cb;

	// Token: 0x0200103C RID: 4156
	public class StatesInstance : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.GameInstance
	{
		// Token: 0x0600751A RID: 29978 RVA: 0x002CC135 File Offset: 0x002CA335
		public StatesInstance(WorkChore<WorkableType> master, GameObject workable, KAnimFile override_anims) : base(master)
		{
			this.overrideAnims = override_anims;
			base.sm.workable.Set(workable, base.smi, false);
		}

		// Token: 0x0600751B RID: 29979 RVA: 0x002CC15E File Offset: 0x002CA35E
		public void EnableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).AddAnimOverrides(this.overrideAnims, 0f);
			}
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x002CC194 File Offset: 0x002CA394
		public void DisableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).RemoveAnimOverrides(this.overrideAnims);
			}
		}

		// Token: 0x0400588C RID: 22668
		private KAnimFile overrideAnims;
	}

	// Token: 0x0200103D RID: 4157
	public class States : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>>
	{
		// Token: 0x0600751D RID: 29981 RVA: 0x002CC1C8 File Offset: 0x002CA3C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.worker);
			this.approach.InitializeStates(this.worker, this.workable, this.work, null, null, null).Update("CheckOperational", delegate(WorkChore<WorkableType>.StatesInstance smi, float dt)
			{
				if (!smi.master.IsOperationalValid())
				{
					smi.StopSM("Building not operational");
				}
			}, UpdateRate.SIM_200ms, false);
			this.work.Enter(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.EnableAnimOverrides();
			}).ToggleWork<WorkableType>(this.workable, this.success, null, (WorkChore<WorkableType>.StatesInstance smi) => smi.master.IsOperationalValid()).Exit(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.DisableAnimOverrides();
			});
			this.success.ReturnSuccess();
		}

		// Token: 0x0400588D RID: 22669
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.ApproachSubState<WorkableType> approach;

		// Token: 0x0400588E RID: 22670
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State work;

		// Token: 0x0400588F RID: 22671
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State success;

		// Token: 0x04005890 RID: 22672
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter workable;

		// Token: 0x04005891 RID: 22673
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter worker;
	}
}
