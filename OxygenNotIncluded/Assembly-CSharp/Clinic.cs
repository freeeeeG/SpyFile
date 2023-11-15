using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
[AddComponentMenu("KMonoBehaviour/Workable/Clinic")]
public class Clinic : Workable, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x06002F83 RID: 12163 RVA: 0x000FB150 File Offset: 0x000F9350
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.assignable.subSlots = new AssignableSlot[]
		{
			Db.Get().AssignableSlots.MedicalBed
		};
		this.assignable.AddAutoassignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAutoAssignTo));
		this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanManuallyAssignTo));
	}

	// Token: 0x06002F84 RID: 12164 RVA: 0x000FB1BB File Offset: 0x000F93BB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		Components.Clinics.Add(this);
		base.SetWorkTime(float.PositiveInfinity);
		this.clinicSMI = new Clinic.ClinicSM.Instance(this);
		this.clinicSMI.StartSM();
	}

	// Token: 0x06002F85 RID: 12165 RVA: 0x000FB1FB File Offset: 0x000F93FB
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		Components.Clinics.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002F86 RID: 12166 RVA: 0x000FB21C File Offset: 0x000F941C
	private KAnimFile[] GetAppropriateOverrideAnims(Worker worker)
	{
		KAnimFile[] result = null;
		if (!worker.GetSMI<WoundMonitor.Instance>().ShouldExitInfirmary())
		{
			result = this.workerInjuredAnims;
		}
		else if (this.workerDiseasedAnims != null && this.IsValidEffect(this.diseaseEffect) && worker.GetSMI<SicknessMonitor.Instance>().IsSick())
		{
			result = this.workerDiseasedAnims;
		}
		return result;
	}

	// Token: 0x06002F87 RID: 12167 RVA: 0x000FB26C File Offset: 0x000F946C
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		this.overrideAnims = this.GetAppropriateOverrideAnims(worker);
		return base.GetAnim(worker);
	}

	// Token: 0x06002F88 RID: 12168 RVA: 0x000FB282 File Offset: 0x000F9482
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("Sleep", false);
	}

	// Token: 0x06002F89 RID: 12169 RVA: 0x000FB2A0 File Offset: 0x000F94A0
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		KAnimFile[] appropriateOverrideAnims = this.GetAppropriateOverrideAnims(worker);
		if (appropriateOverrideAnims == null || appropriateOverrideAnims != this.overrideAnims)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06002F8A RID: 12170 RVA: 0x000FB2CD File Offset: 0x000F94CD
	protected override void OnStopWork(Worker worker)
	{
		worker.GetComponent<Effects>().Remove("Sleep");
		base.OnStopWork(worker);
	}

	// Token: 0x06002F8B RID: 12171 RVA: 0x000FB2E8 File Offset: 0x000F94E8
	protected override void OnCompleteWork(Worker worker)
	{
		this.assignable.Unassign();
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Clinic.EffectsRemoved.Length; i++)
		{
			string effect_id = Clinic.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x06002F8C RID: 12172 RVA: 0x000FB32F File Offset: 0x000F952F
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x000FB334 File Offset: 0x000F9534
	private Chore CreateWorkChore(ChoreType chore_type, bool allow_prioritization, bool allow_in_red_alert, PriorityScreen.PriorityClass priority_class, bool ignore_schedule_block = false)
	{
		return new WorkChore<Clinic>(chore_type, this, null, true, null, null, null, allow_in_red_alert, null, ignore_schedule_block, true, null, false, true, allow_prioritization, priority_class, 5, false, false);
	}

	// Token: 0x06002F8E RID: 12174 RVA: 0x000FB360 File Offset: 0x000F9560
	private bool CanAutoAssignTo(MinionAssignablesProxy worker)
	{
		bool flag = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			if (this.IsValidEffect(this.healthEffect))
			{
				Health component = minionIdentity.GetComponent<Health>();
				if (component != null && component.hitPoints < component.maxHitPoints)
				{
					flag = true;
				}
			}
			if (!flag && this.IsValidEffect(this.diseaseEffect))
			{
				flag = (minionIdentity.GetComponent<MinionModifiers>().sicknesses.Count > 0);
			}
		}
		return flag;
	}

	// Token: 0x06002F8F RID: 12175 RVA: 0x000FB3D8 File Offset: 0x000F95D8
	private bool CanManuallyAssignTo(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = this.IsHealthBelowThreshold(minionIdentity.gameObject);
		}
		return result;
	}

	// Token: 0x06002F90 RID: 12176 RVA: 0x000FB40C File Offset: 0x000F960C
	private bool IsHealthBelowThreshold(GameObject minion)
	{
		Health health = (minion != null) ? minion.GetComponent<Health>() : null;
		if (health != null)
		{
			float num = health.hitPoints / health.maxHitPoints;
			if (health != null)
			{
				return num < this.MedicalAttentionMinimum;
			}
		}
		return false;
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x000FB457 File Offset: 0x000F9657
	private bool IsValidEffect(string effect)
	{
		return effect != null && effect != "";
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x000FB469 File Offset: 0x000F9669
	private bool AllowDoctoring()
	{
		return this.IsValidEffect(this.doctoredDiseaseEffect) || this.IsValidEffect(this.doctoredHealthEffect);
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x000FB488 File Offset: 0x000F9688
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.IsValidEffect(this.healthEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.healthEffect, false);
		}
		if (this.diseaseEffect != this.healthEffect && this.IsValidEffect(this.diseaseEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.diseaseEffect, false);
		}
		if (this.AllowDoctoring())
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.DOCTORING, UI.BUILDINGEFFECTS.TOOLTIPS.DOCTORING, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			if (this.IsValidEffect(this.doctoredHealthEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredHealthEffect, true);
			}
			if (this.doctoredDiseaseEffect != this.doctoredHealthEffect && this.IsValidEffect(this.doctoredDiseaseEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredDiseaseEffect, true);
			}
		}
		return descriptors;
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06002F94 RID: 12180 RVA: 0x000FB57E File Offset: 0x000F977E
	public float MedicalAttentionMinimum
	{
		get
		{
			return this.sicknessSliderValue / 100f;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06002F95 RID: 12181 RVA: 0x000FB58C File Offset: 0x000F978C
	string ISliderControl.SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06002F96 RID: 12182 RVA: 0x000FB593 File Offset: 0x000F9793
	string ISliderControl.SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06002F97 RID: 12183 RVA: 0x000FB59F File Offset: 0x000F979F
	int ISliderControl.SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002F98 RID: 12184 RVA: 0x000FB5A2 File Offset: 0x000F97A2
	float ISliderControl.GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002F99 RID: 12185 RVA: 0x000FB5A9 File Offset: 0x000F97A9
	float ISliderControl.GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06002F9A RID: 12186 RVA: 0x000FB5B0 File Offset: 0x000F97B0
	float ISliderControl.GetSliderValue(int index)
	{
		return this.sicknessSliderValue;
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x000FB5B8 File Offset: 0x000F97B8
	void ISliderControl.SetSliderValue(float percent, int index)
	{
		if (percent != this.sicknessSliderValue)
		{
			this.sicknessSliderValue = (float)Mathf.RoundToInt(percent);
			Game.Instance.Trigger(875045922, null);
		}
	}

	// Token: 0x06002F9C RID: 12188 RVA: 0x000FB5E0 File Offset: 0x000F97E0
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP"), this.sicknessSliderValue);
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x000FB601 File Offset: 0x000F9801
	string ISliderControl.GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP";
	}

	// Token: 0x04001C32 RID: 7218
	[MyCmpReq]
	private Assignable assignable;

	// Token: 0x04001C33 RID: 7219
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x04001C34 RID: 7220
	private const int MAX_RANGE = 10;

	// Token: 0x04001C35 RID: 7221
	private const float CHECK_RANGE_INTERVAL = 10f;

	// Token: 0x04001C36 RID: 7222
	public float doctorVisitInterval = 300f;

	// Token: 0x04001C37 RID: 7223
	public KAnimFile[] workerInjuredAnims;

	// Token: 0x04001C38 RID: 7224
	public KAnimFile[] workerDiseasedAnims;

	// Token: 0x04001C39 RID: 7225
	public string diseaseEffect;

	// Token: 0x04001C3A RID: 7226
	public string healthEffect;

	// Token: 0x04001C3B RID: 7227
	public string doctoredDiseaseEffect;

	// Token: 0x04001C3C RID: 7228
	public string doctoredHealthEffect;

	// Token: 0x04001C3D RID: 7229
	public string doctoredPlaceholderEffect;

	// Token: 0x04001C3E RID: 7230
	private Clinic.ClinicSM.Instance clinicSMI;

	// Token: 0x04001C3F RID: 7231
	public static readonly Chore.Precondition IsOverSicknessThreshold = new Chore.Precondition
	{
		id = "IsOverSicknessThreshold",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((Clinic)data).IsHealthBelowThreshold(context.consumerState.gameObject);
		}
	};

	// Token: 0x04001C40 RID: 7232
	[Serialize]
	private float sicknessSliderValue = 70f;

	// Token: 0x0200140E RID: 5134
	public class ClinicSM : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic>
	{
		// Token: 0x06008340 RID: 33600 RVA: 0x002FCEDC File Offset: 0x002FB0DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Clinic.ClinicSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Assignable>().Unassign();
			});
			this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Clinic.ClinicSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.AssigneeChanged, this.unoperational, null).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.Heal, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.HealCritical, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.RestDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.SleepDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect));
			this.operational.idle.WorkableStartTransition((Clinic.ClinicSM.Instance smi) => smi.master, this.operational.healing);
			this.operational.healing.DefaultState(this.operational.healing.undoctored).WorkableStopTransition((Clinic.ClinicSM.Instance smi) => smi.GetComponent<Clinic>(), this.operational.idle).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.healing.undoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.healthEffect, false);
				smi.StartEffect(smi.master.diseaseEffect, false);
				bool flag = false;
				if (smi.master.worker != null)
				{
					flag = (smi.HasEffect(smi.master.doctoredHealthEffect) || smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredPlaceholderEffect));
				}
				if (smi.master.AllowDoctoring())
				{
					if (flag)
					{
						smi.GoTo(this.operational.healing.doctored);
						return;
					}
					smi.StartDoctorChore();
				}
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StopEffect(smi.master.healthEffect);
				smi.StopEffect(smi.master.diseaseEffect);
				smi.StopDoctorChore();
			});
			this.operational.healing.newlyDoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
				smi.StartEffect(smi.master.doctoredHealthEffect, true);
				smi.GoTo(this.operational.healing.doctored);
			});
			this.operational.healing.doctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredPlaceholderEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredPlaceholderEffect);
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
					if (effectInstance2 != null)
					{
						float num = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance2.timeRemaining = effectInstance2.effect.duration - num;
					}
					EffectInstance effectInstance3 = smi.StartEffect(smi.master.doctoredHealthEffect, true);
					if (effectInstance3 != null)
					{
						float num2 = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance3.timeRemaining = effectInstance3.effect.duration - num2;
					}
					component.Remove(smi.master.doctoredPlaceholderEffect);
				}
			}).ScheduleGoTo(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				float num = smi.master.doctorVisitInterval;
				if (smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredHealthEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				return num;
			}, this.operational.healing.undoctored).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					if (effectInstance == null)
					{
						effectInstance = component.Get(smi.master.doctoredHealthEffect);
					}
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredPlaceholderEffect, true);
					effectInstance2.timeRemaining = effectInstance2.effect.duration - (effectInstance.effect.duration - effectInstance.timeRemaining);
					component.Remove(smi.master.doctoredDiseaseEffect);
					component.Remove(smi.master.doctoredHealthEffect);
				}
			});
		}

		// Token: 0x04006437 RID: 25655
		public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State unoperational;

		// Token: 0x04006438 RID: 25656
		public Clinic.ClinicSM.OperationalStates operational;

		// Token: 0x0200214D RID: 8525
		public class OperationalStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x04009553 RID: 38227
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State idle;

			// Token: 0x04009554 RID: 38228
			public Clinic.ClinicSM.HealingStates healing;
		}

		// Token: 0x0200214E RID: 8526
		public class HealingStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x04009555 RID: 38229
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State undoctored;

			// Token: 0x04009556 RID: 38230
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State doctored;

			// Token: 0x04009557 RID: 38231
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State newlyDoctored;
		}

		// Token: 0x0200214F RID: 8527
		public new class Instance : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.GameInstance
		{
			// Token: 0x0600A9C9 RID: 43465 RVA: 0x003725AE File Offset: 0x003707AE
			public Instance(Clinic master) : base(master)
			{
			}

			// Token: 0x0600A9CA RID: 43466 RVA: 0x003725B8 File Offset: 0x003707B8
			public void StartDoctorChore()
			{
				if (base.master.IsValidEffect(base.master.doctoredHealthEffect) || base.master.IsValidEffect(base.master.doctoredDiseaseEffect))
				{
					this.doctorChore = new WorkChore<DoctorChoreWorkable>(Db.Get().ChoreTypes.Doctor, base.smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
					WorkChore<DoctorChoreWorkable> workChore = this.doctorChore;
					workChore.onComplete = (Action<Chore>)Delegate.Combine(workChore.onComplete, new Action<Chore>(delegate(Chore chore)
					{
						base.smi.GoTo(base.smi.sm.operational.healing.newlyDoctored);
					}));
				}
			}

			// Token: 0x0600A9CB RID: 43467 RVA: 0x00372652 File Offset: 0x00370852
			public void StopDoctorChore()
			{
				if (this.doctorChore != null)
				{
					this.doctorChore.Cancel("StopDoctorChore");
					this.doctorChore = null;
				}
			}

			// Token: 0x0600A9CC RID: 43468 RVA: 0x00372674 File Offset: 0x00370874
			public bool HasEffect(string effect)
			{
				bool result = false;
				if (base.master.IsValidEffect(effect))
				{
					result = base.smi.master.worker.GetComponent<Effects>().HasEffect(effect);
				}
				return result;
			}

			// Token: 0x0600A9CD RID: 43469 RVA: 0x003726B0 File Offset: 0x003708B0
			public EffectInstance StartEffect(string effect, bool should_save)
			{
				if (base.master.IsValidEffect(effect))
				{
					Worker worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (!component.HasEffect(effect))
						{
							return component.Add(effect, should_save);
						}
					}
				}
				return null;
			}

			// Token: 0x0600A9CE RID: 43470 RVA: 0x00372700 File Offset: 0x00370900
			public void StopEffect(string effect)
			{
				if (base.master.IsValidEffect(effect))
				{
					Worker worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (component.HasEffect(effect))
						{
							component.Remove(effect);
						}
					}
				}
			}

			// Token: 0x04009558 RID: 38232
			private WorkChore<DoctorChoreWorkable> doctorChore;
		}
	}
}
