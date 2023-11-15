using System;
using Klei;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000664 RID: 1636
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/OilWellCap")]
public class OilWellCap : Workable, ISingleSliderControl, ISliderControl, IElementEmitter
{
	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000E66B6 File Offset: 0x000E48B6
	public SimHashes Element
	{
		get
		{
			return this.gasElement;
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06002B3C RID: 11068 RVA: 0x000E66BE File Offset: 0x000E48BE
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06002B3D RID: 11069 RVA: 0x000E66D5 File Offset: 0x000E48D5
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06002B3E RID: 11070 RVA: 0x000E66DC File Offset: 0x000E48DC
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x000E66E8 File Offset: 0x000E48E8
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x000E66EB File Offset: 0x000E48EB
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000E66F2 File Offset: 0x000E48F2
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x000E66F9 File Offset: 0x000E48F9
	public float GetSliderValue(int index)
	{
		return this.depressurizePercent * 100f;
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x000E6707 File Offset: 0x000E4907
	public void SetSliderValue(float value, int index)
	{
		this.depressurizePercent = value / 100f;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000E6716 File Offset: 0x000E4916
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000E671D File Offset: 0x000E491D
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP"), this.depressurizePercent * 100f);
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000E6744 File Offset: 0x000E4944
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OilWellCap>(-905833192, OilWellCap.OnCopySettingsDelegate);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000E6760 File Offset: 0x000E4960
	private void OnCopySettings(object data)
	{
		OilWellCap component = ((GameObject)data).GetComponent<OilWellCap>();
		if (component != null)
		{
			this.depressurizePercent = component.depressurizePercent;
		}
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000E6790 File Offset: 0x000E4990
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.accumulator = Game.Instance.accumulators.Add("pressuregas", this);
		this.showProgressBar = false;
		base.SetWorkTime(float.PositiveInfinity);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oil_cap_kanim")
		};
		this.workingStatusItem = Db.Get().BuildingStatusItems.ReleasingPressure;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.pressureMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.smi = new OilWellCap.StatesInstance(this);
		this.smi.StartSM();
		this.UpdatePressurePercent();
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000E68A5 File Offset: 0x000E4AA5
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000E68CE File Offset: 0x000E4ACE
	public void AddGasPressure(float dt)
	{
		this.storage.AddGasChunk(this.gasElement, this.addGasRate * dt, this.gasTemperature, 0, 0, true, true);
		this.UpdatePressurePercent();
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x000E68FC File Offset: 0x000E4AFC
	public void ReleaseGasPressure(float dt)
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.gasElement);
		if (primaryElement != null && primaryElement.Mass > 0f)
		{
			float num = this.releaseGasRate * dt;
			if (base.worker != null)
			{
				num *= this.GetEfficiencyMultiplier(base.worker);
			}
			num = Mathf.Min(num, primaryElement.Mass);
			SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(primaryElement, num / primaryElement.Mass);
			primaryElement.Mass -= num;
			Game.Instance.accumulators.Accumulate(this.accumulator, num);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(this), ElementLoader.GetElementIndex(this.gasElement), null, num, primaryElement.Temperature, percentOfDisease.idx, percentOfDisease.count, true, -1);
		}
		this.UpdatePressurePercent();
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x000E69D0 File Offset: 0x000E4BD0
	private void UpdatePressurePercent()
	{
		float num = this.storage.GetMassAvailable(this.gasElement) / this.maxGasPressure;
		num = Mathf.Clamp01(num);
		this.smi.sm.pressurePercent.Set(num, this.smi, false);
		this.pressureMeter.SetPositionPercent(num);
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x000E6A27 File Offset: 0x000E4C27
	public bool NeedsDepressurizing()
	{
		return this.smi.GetPressurePercent() >= this.depressurizePercent;
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x000E6A40 File Offset: 0x000E4C40
	private WorkChore<OilWellCap> CreateWorkChore()
	{
		this.DepressurizeChore = new WorkChore<OilWellCap>(Db.Get().ChoreTypes.Depressurize, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.DepressurizeChore.AddPrecondition(OilWellCap.AllowedToDepressurize, this);
		return this.DepressurizeChore;
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000E6A90 File Offset: 0x000E4C90
	private void CancelChore(string reason)
	{
		if (this.DepressurizeChore != null)
		{
			this.DepressurizeChore.Cancel(reason);
		}
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x000E6AA6 File Offset: 0x000E4CA6
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.working.Set(true, this.smi, false);
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x000E6ACD File Offset: 0x000E4CCD
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.working.Set(false, this.smi, false);
		this.DepressurizeChore = null;
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x000E6AFB File Offset: 0x000E4CFB
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		return this.smi.GetPressurePercent() <= 0f;
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x000E6B12 File Offset: 0x000E4D12
	public override bool InstantlyFinish(Worker worker)
	{
		this.ReleaseGasPressure(60f);
		return true;
	}

	// Token: 0x04001958 RID: 6488
	private OilWellCap.StatesInstance smi;

	// Token: 0x04001959 RID: 6489
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400195A RID: 6490
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400195B RID: 6491
	public SimHashes gasElement;

	// Token: 0x0400195C RID: 6492
	public float gasTemperature;

	// Token: 0x0400195D RID: 6493
	public float addGasRate = 1f;

	// Token: 0x0400195E RID: 6494
	public float maxGasPressure = 10f;

	// Token: 0x0400195F RID: 6495
	public float releaseGasRate = 10f;

	// Token: 0x04001960 RID: 6496
	[Serialize]
	private float depressurizePercent = 0.75f;

	// Token: 0x04001961 RID: 6497
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001962 RID: 6498
	private MeterController pressureMeter;

	// Token: 0x04001963 RID: 6499
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001964 RID: 6500
	private static readonly EventSystem.IntraObjectHandler<OilWellCap> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OilWellCap>(delegate(OilWellCap component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001965 RID: 6501
	private WorkChore<OilWellCap> DepressurizeChore;

	// Token: 0x04001966 RID: 6502
	private static readonly Chore.Precondition AllowedToDepressurize = new Chore.Precondition
	{
		id = "AllowedToDepressurize",
		description = DUPLICANTS.CHORES.PRECONDITIONS.ALLOWED_TO_DEPRESSURIZE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((OilWellCap)data).NeedsDepressurizing();
		}
	};

	// Token: 0x02001354 RID: 4948
	public class StatesInstance : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.GameInstance
	{
		// Token: 0x060080B6 RID: 32950 RVA: 0x002F2180 File Offset: 0x002F0380
		public StatesInstance(OilWellCap master) : base(master)
		{
		}

		// Token: 0x060080B7 RID: 32951 RVA: 0x002F2189 File Offset: 0x002F0389
		public float GetPressurePercent()
		{
			return base.sm.pressurePercent.Get(base.smi);
		}
	}

	// Token: 0x02001355 RID: 4949
	public class States : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap>
	{
		// Token: 0x060080B8 RID: 32952 RVA: 0x002F21A4 File Offset: 0x002F03A4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational));
			this.operational.DefaultState(this.operational.idle).ToggleRecurringChore((OilWellCap.StatesInstance smi) => smi.master.CreateWorkChore(), null).EventHandler(GameHashes.WorkChoreDisabled, delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.CancelChore("WorkChoreDisabled");
			});
			this.operational.idle.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational.active, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsAbleToPump));
			this.operational.active.DefaultState(this.operational.active.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).Enter(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.AddGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.active.pre.PlayAnim("working_pre").ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).OnAnimQueueComplete(this.operational.active.loop);
			this.operational.active.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.pressurePercent, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping)).EventTransition(GameHashes.OnStorageChange, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping));
			this.operational.active.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.idle);
			this.operational.overpressure.PlayAnim("over_pressured_pre", KAnim.PlayMode.Once).QueueAnim("over_pressured_loop", true, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WellOverpressure, null).ParamTransition<float>(this.pressurePercent, this.operational.idle, (OilWellCap.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue);
			this.operational.releasing_pressure.DefaultState(this.operational.releasing_pressure.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (OilWellCap.StatesInstance smi) => smi.master);
			this.operational.releasing_pressure.pre.PlayAnim("steam_out_pre").OnAnimQueueComplete(this.operational.releasing_pressure.loop);
			this.operational.releasing_pressure.loop.PlayAnim("steam_out_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).ParamTransition<bool>(this.working, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsFalse).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.ReleaseGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.releasing_pressure.pst.PlayAnim("steam_out_pst").OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x060080B9 RID: 32953 RVA: 0x002F268F File Offset: 0x002F088F
		private bool IsOperational(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x060080BA RID: 32954 RVA: 0x002F26A1 File Offset: 0x002F08A1
		private bool IsAbleToPump(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x060080BB RID: 32955 RVA: 0x002F26C3 File Offset: 0x002F08C3
		private bool MustStopPumping(OilWellCap.StatesInstance smi)
		{
			return !smi.master.operational.IsOperational || !smi.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x0400623F RID: 25151
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.FloatParameter pressurePercent;

		// Token: 0x04006240 RID: 25152
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.BoolParameter working;

		// Token: 0x04006241 RID: 25153
		public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State inoperational;

		// Token: 0x04006242 RID: 25154
		public OilWellCap.States.OperationalStates operational;

		// Token: 0x02002103 RID: 8451
		public class OperationalStates : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State
		{
			// Token: 0x040093D8 RID: 37848
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State idle;

			// Token: 0x040093D9 RID: 37849
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState active;

			// Token: 0x040093DA RID: 37850
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State overpressure;

			// Token: 0x040093DB RID: 37851
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState releasing_pressure;
		}
	}
}
