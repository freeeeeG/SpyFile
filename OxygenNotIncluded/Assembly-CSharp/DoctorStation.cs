using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000770 RID: 1904
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStation")]
public class DoctorStation : Workable
{
	// Token: 0x0600349D RID: 13469 RVA: 0x0011AAFC File Offset: 0x00118CFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x0011AB04 File Offset: 0x00118D04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.doctor_workable = base.GetComponent<DoctorStationDoctorWorkable>();
		base.SetWorkTime(float.PositiveInfinity);
		this.smi = new DoctorStation.StatesInstance(this);
		this.smi.StartSM();
		this.OnStorageChange(null);
		base.Subscribe<DoctorStation>(-1697596308, DoctorStation.OnStorageChangeDelegate);
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x0011AB68 File Offset: 0x00118D68
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
			this.smi = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x0011AB9C File Offset: 0x00118D9C
	private void OnStorageChange(object data = null)
	{
		this.treatments_available.Clear();
		foreach (GameObject gameObject in this.storage.items)
		{
			MedicinalPill component = gameObject.GetComponent<MedicinalPill>();
			if (component != null)
			{
				Tag tag = gameObject.PrefabID();
				foreach (string id in component.info.curedSicknesses)
				{
					this.AddTreatment(id, tag);
				}
			}
		}
		bool value = this.treatments_available.Count > 0;
		this.smi.sm.hasSupplies.Set(value, this.smi, false);
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0011AC8C File Offset: 0x00118E8C
	private void AddTreatment(string id, Tag tag)
	{
		if (!this.treatments_available.ContainsKey(id))
		{
			this.treatments_available.Add(id, tag);
		}
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0011ACB3 File Offset: 0x00118EB3
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.hasPatient.Set(true, this.smi, false);
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0011ACDA File Offset: 0x00118EDA
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.hasPatient.Set(false, this.smi, false);
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0011AD01 File Offset: 0x00118F01
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x0011AD04 File Offset: 0x00118F04
	public void SetHasDoctor(bool has)
	{
		this.smi.sm.hasDoctor.Set(has, this.smi, false);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x0011AD24 File Offset: 0x00118F24
	public void CompleteDoctoring()
	{
		if (!base.worker)
		{
			return;
		}
		this.CompleteDoctoring(base.worker.gameObject);
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x0011AD48 File Offset: 0x00118F48
	private void CompleteDoctoring(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			bool flag = false;
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					Game.Instance.savedInfo.curedDisease = true;
					sicknessInstance.Cure();
					this.storage.ConsumeIgnoringDisease(tag, 1f);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				global::Debug.LogWarningFormat(base.gameObject, "Failed to treat any disease for {0}", new object[]
				{
					target
				});
			}
		}
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x0011ADFC File Offset: 0x00118FFC
	public bool IsDoctorAvailable(GameObject target)
	{
		if (!string.IsNullOrEmpty(this.doctor_workable.requiredSkillPerk))
		{
			MinionResume component = target.GetComponent<MinionResume>();
			if (!MinionResume.AnyOtherMinionHasPerk(this.doctor_workable.requiredSkillPerk, component))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x0011AE38 File Offset: 0x00119038
	public bool IsTreatmentAvailable(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04001FC4 RID: 8132
	private static readonly EventSystem.IntraObjectHandler<DoctorStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DoctorStation>(delegate(DoctorStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001FC5 RID: 8133
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001FC6 RID: 8134
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04001FC7 RID: 8135
	private DoctorStationDoctorWorkable doctor_workable;

	// Token: 0x04001FC8 RID: 8136
	private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();

	// Token: 0x04001FC9 RID: 8137
	private DoctorStation.StatesInstance smi;

	// Token: 0x04001FCA RID: 8138
	public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition
	{
		id = "TreatmentAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsTreatmentAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x04001FCB RID: 8139
	public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition
	{
		id = "DoctorAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsDoctorAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x020014ED RID: 5357
	public class States : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation>
	{
		// Token: 0x06008644 RID: 34372 RVA: 0x00308474 File Offset: 0x00306674
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DoctorStation.StatesInstance smi) => smi.master.operational.IsOperational);
			this.operational.EventTransition(GameHashes.OperationalChanged, this.unoperational, (DoctorStation.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.operational.not_ready);
			this.operational.not_ready.ParamTransition<bool>(this.hasSupplies, this.operational.ready, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.DefaultState(this.operational.ready.idle).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreatePatientChore), null).ParamTransition<bool>(this.hasSupplies, this.operational.not_ready, (DoctorStation.StatesInstance smi, bool p) => !p);
			this.operational.ready.idle.ParamTransition<bool>(this.hasPatient, this.operational.ready.has_patient, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.ParamTransition<bool>(this.hasPatient, this.operational.ready.idle, (DoctorStation.StatesInstance smi, bool p) => !p).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreateDoctorChore), null);
			this.operational.ready.has_patient.waiting.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.being_treated, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.being_treated.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.waiting, (DoctorStation.StatesInstance smi, bool p) => !p).Enter(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x06008645 RID: 34373 RVA: 0x0030876C File Offset: 0x0030696C
		private Chore CreatePatientChore(DoctorStation.StatesInstance smi)
		{
			WorkChore<DoctorStation> workChore = new WorkChore<DoctorStation>(Db.Get().ChoreTypes.GetDoctored, smi.master, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
			workChore.AddPrecondition(DoctorStation.TreatmentAvailable, smi.master);
			workChore.AddPrecondition(DoctorStation.DoctorAvailable, smi.master);
			return workChore;
		}

		// Token: 0x06008646 RID: 34374 RVA: 0x003087C8 File Offset: 0x003069C8
		private Chore CreateDoctorChore(DoctorStation.StatesInstance smi)
		{
			DoctorStationDoctorWorkable component = smi.master.GetComponent<DoctorStationDoctorWorkable>();
			return new WorkChore<DoctorStationDoctorWorkable>(Db.Get().ChoreTypes.Doctor, component, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x040066D5 RID: 26325
		public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State unoperational;

		// Token: 0x040066D6 RID: 26326
		public DoctorStation.States.OperationalStates operational;

		// Token: 0x040066D7 RID: 26327
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasSupplies;

		// Token: 0x040066D8 RID: 26328
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasPatient;

		// Token: 0x040066D9 RID: 26329
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasDoctor;

		// Token: 0x0200216F RID: 8559
		public class OperationalStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x040095D0 RID: 38352
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State not_ready;

			// Token: 0x040095D1 RID: 38353
			public DoctorStation.States.ReadyStates ready;
		}

		// Token: 0x02002170 RID: 8560
		public class ReadyStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x040095D2 RID: 38354
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State idle;

			// Token: 0x040095D3 RID: 38355
			public DoctorStation.States.PatientStates has_patient;
		}

		// Token: 0x02002171 RID: 8561
		public class PatientStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x040095D4 RID: 38356
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State waiting;

			// Token: 0x040095D5 RID: 38357
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State being_treated;
		}
	}

	// Token: 0x020014EE RID: 5358
	public class StatesInstance : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.GameInstance
	{
		// Token: 0x06008648 RID: 34376 RVA: 0x0030880F File Offset: 0x00306A0F
		public StatesInstance(DoctorStation master) : base(master)
		{
		}
	}
}
