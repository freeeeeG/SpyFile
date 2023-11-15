using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000663 RID: 1635
[SerializationConfig(MemberSerialization.OptIn)]
public class OilRefinery : StateMachineComponent<OilRefinery.StatesInstance>
{
	// Token: 0x06002B33 RID: 11059 RVA: 0x000E64F8 File Offset: 0x000E46F8
	protected override void OnSpawn()
	{
		base.Subscribe<OilRefinery>(-1697596308, OilRefinery.OnStorageChangedDelegate);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		base.smi.StartSM();
		this.maxSrcMass = base.GetComponent<ConduitConsumer>().capacityKG;
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x000E6558 File Offset: 0x000E4758
	private void OnStorageChanged(object data)
	{
		float positionPercent = Mathf.Clamp01(this.storage.GetMassAvailable(SimHashes.CrudeOil) / this.maxSrcMass);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x000E6590 File Offset: 0x000E4790
	private static bool UpdateStateCb(int cell, object data)
	{
		OilRefinery oilRefinery = data as OilRefinery;
		if (Grid.Element[cell].IsGas)
		{
			oilRefinery.cellCount += 1f;
			oilRefinery.envPressure += Grid.Mass[cell];
		}
		return true;
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x000E65E0 File Offset: 0x000E47E0
	private void TestAreaPressure()
	{
		this.envPressure = 0f;
		this.cellCount = 0f;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, new Func<int, object, bool>(OilRefinery.UpdateStateCb));
			this.envPressure /= this.cellCount;
		}
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000E6656 File Offset: 0x000E4856
	private bool IsOverPressure()
	{
		return this.envPressure >= this.overpressureMass;
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000E6669 File Offset: 0x000E4869
	private bool IsOverWarningPressure()
	{
		return this.envPressure >= this.overpressureWarningMass;
	}

	// Token: 0x0400194B RID: 6475
	private bool wasOverPressure;

	// Token: 0x0400194C RID: 6476
	[SerializeField]
	public float overpressureWarningMass = 4.5f;

	// Token: 0x0400194D RID: 6477
	[SerializeField]
	public float overpressureMass = 5f;

	// Token: 0x0400194E RID: 6478
	private float maxSrcMass;

	// Token: 0x0400194F RID: 6479
	private float envPressure;

	// Token: 0x04001950 RID: 6480
	private float cellCount;

	// Token: 0x04001951 RID: 6481
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001952 RID: 6482
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001953 RID: 6483
	[MyCmpAdd]
	private OilRefinery.WorkableTarget workable;

	// Token: 0x04001954 RID: 6484
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04001955 RID: 6485
	private const bool hasMeter = true;

	// Token: 0x04001956 RID: 6486
	private MeterController meter;

	// Token: 0x04001957 RID: 6487
	private static readonly EventSystem.IntraObjectHandler<OilRefinery> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<OilRefinery>(delegate(OilRefinery component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001350 RID: 4944
	public class StatesInstance : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.GameInstance
	{
		// Token: 0x060080A8 RID: 32936 RVA: 0x002F1E5C File Offset: 0x002F005C
		public StatesInstance(OilRefinery smi) : base(smi)
		{
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x002F1E68 File Offset: 0x002F0068
		public void TestAreaPressure()
		{
			base.smi.master.TestAreaPressure();
			bool flag = base.smi.master.IsOverPressure();
			bool flag2 = base.smi.master.IsOverWarningPressure();
			if (flag)
			{
				base.smi.master.wasOverPressure = true;
				base.sm.isOverPressure.Set(true, this, false);
				return;
			}
			if (base.smi.master.wasOverPressure && !flag2)
			{
				base.sm.isOverPressure.Set(false, this, false);
			}
		}
	}

	// Token: 0x02001351 RID: 4945
	public class States : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery>
	{
		// Token: 0x060080AA RID: 32938 RVA: 0x002F1EF8 File Offset: 0x002F00F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OilRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.needResources, (OilRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.needResources.EventTransition(GameHashes.OnStorageChange, this.ready, (OilRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.ready.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.overpressure, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsTrue).Transition(this.needResources, (OilRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false), UpdateRate.SIM_200ms).ToggleChore((OilRefinery.StatesInstance smi) => new WorkChore<OilRefinery.WorkableTarget>(Db.Get().ChoreTypes.Fabricate, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.needResources);
			this.overpressure.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.ready, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null);
		}

		// Token: 0x04006237 RID: 25143
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressure;

		// Token: 0x04006238 RID: 25144
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressureWarning;

		// Token: 0x04006239 RID: 25145
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State disabled;

		// Token: 0x0400623A RID: 25146
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State overpressure;

		// Token: 0x0400623B RID: 25147
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State needResources;

		// Token: 0x0400623C RID: 25148
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State ready;
	}

	// Token: 0x02001352 RID: 4946
	[AddComponentMenu("KMonoBehaviour/Workable/WorkableTarget")]
	public class WorkableTarget : Workable
	{
		// Token: 0x060080AC RID: 32940 RVA: 0x002F20B4 File Offset: 0x002F02B4
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.showProgressBar = false;
			this.workerStatusItem = null;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_oilrefinery_kanim")
			};
		}

		// Token: 0x060080AD RID: 32941 RVA: 0x002F2118 File Offset: 0x002F0318
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x060080AE RID: 32942 RVA: 0x002F212B File Offset: 0x002F032B
		protected override void OnStartWork(Worker worker)
		{
			this.operational.SetActive(true, false);
		}

		// Token: 0x060080AF RID: 32943 RVA: 0x002F213A File Offset: 0x002F033A
		protected override void OnStopWork(Worker worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x060080B0 RID: 32944 RVA: 0x002F2149 File Offset: 0x002F0349
		protected override void OnCompleteWork(Worker worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x060080B1 RID: 32945 RVA: 0x002F2158 File Offset: 0x002F0358
		public override bool InstantlyFinish(Worker worker)
		{
			return false;
		}

		// Token: 0x0400623D RID: 25149
		[MyCmpGet]
		public Operational operational;
	}
}
