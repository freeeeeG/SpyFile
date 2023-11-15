using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200064A RID: 1610
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/ManualGenerator")]
public class ManualGenerator : Workable, ISingleSliderControl, ISliderControl
{
	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06002A47 RID: 10823 RVA: 0x000E1E7F File Offset: 0x000E007F
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000E1E86 File Offset: 0x000E0086
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x000E1E92 File Offset: 0x000E0092
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000E1E95 File Offset: 0x000E0095
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000E1E9C File Offset: 0x000E009C
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x000E1EA3 File Offset: 0x000E00A3
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000E1EB1 File Offset: 0x000E00B1
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000E1EC0 File Offset: 0x000E00C0
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000E1EC7 File Offset: 0x000E00C7
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP"), this.batteryRefillPercent * 100f);
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000E1EEE File Offset: 0x000E00EE
	public bool IsPowered
	{
		get
		{
			return this.operational.IsActive;
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000E1EFB File Offset: 0x000E00FB
	private ManualGenerator()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x000E1F18 File Offset: 0x000E0118
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualGenerator>(-592767678, ManualGenerator.OnOperationalChangedDelegate);
		base.Subscribe<ManualGenerator>(824508782, ManualGenerator.OnActiveChangedDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.GeneratingPower;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		EnergyGenerator.EnsureStatusItemAvailable();
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000E1FAC File Offset: 0x000E01AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		foreach (KAnimHashedString symbol in ManualGenerator.symbol_names)
		{
			component.SetSymbolVisiblity(symbol, false);
		}
		Building component2 = base.GetComponent<Building>();
		this.powerCell = component2.GetPowerOutputCell();
		this.OnActiveChanged(null);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_generatormanual_kanim")
		};
		this.smi = new ManualGenerator.GeneratePowerSM.Instance(this);
		this.smi.StartSM();
		Game.Instance.energySim.AddManualGenerator(this);
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x000E2056 File Offset: 0x000E0256
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveManualGenerator(this);
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000E207E File Offset: 0x000E027E
	protected void OnActiveChanged(object is_active)
	{
		if (this.operational.IsActive)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ManualGeneratorChargingUp, null);
		}
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000E20B8 File Offset: 0x000E02B8
	public void EnergySim200ms(float dt)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.operational.IsActive)
		{
			this.generator.GenerateJoules(this.generator.WattageRating * dt, false);
			component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this.generator);
			return;
		}
		this.generator.ResetJoules();
		component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.GeneratorOffline, null);
		if (this.operational.IsOperational)
		{
			CircuitManager circuitManager = Game.Instance.circuitManager;
			if (circuitManager == null)
			{
				return;
			}
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag = circuitManager.HasBatteries(circuitID);
			bool flag2 = false;
			if (!flag && circuitManager.HasConsumers(circuitID))
			{
				flag2 = true;
			}
			else if (flag)
			{
				if (this.batteryRefillPercent <= 0f && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) <= 0f)
				{
					flag2 = true;
				}
				else if (circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < this.batteryRefillPercent)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (this.chore == null && this.smi.GetCurrentState() == this.smi.sm.on)
				{
					this.chore = new WorkChore<ManualGenerator>(Db.Get().ChoreTypes.GeneratePower, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				}
			}
			else if (this.chore != null)
			{
				this.chore.Cancel("No refill needed");
				this.chore = null;
			}
			component.ToggleStatusItem(EnergyGenerator.BatteriesSufficientlyFull, !flag2, null);
		}
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x000E2254 File Offset: 0x000E0454
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x000E226C File Offset: 0x000E046C
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		CircuitManager circuitManager = Game.Instance.circuitManager;
		bool flag = false;
		if (circuitManager != null)
		{
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag2 = circuitManager.HasBatteries(circuitID);
			flag = ((flag2 && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < 1f) || (!flag2 && circuitManager.HasConsumers(circuitID)));
		}
		AttributeLevels component = worker.GetComponent<AttributeLevels>();
		if (component != null)
		{
			component.AddExperience(Db.Get().Attributes.Athletics.Id, dt, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
		return !flag;
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000E22F8 File Offset: 0x000E04F8
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000E230E File Offset: 0x000E050E
	protected override void OnCompleteWork(Worker worker)
	{
		this.operational.SetActive(false, false);
		if (this.chore != null)
		{
			this.chore.Cancel("complete");
			this.chore = null;
		}
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000E233C File Offset: 0x000E053C
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000E233F File Offset: 0x000E053F
	private void OnOperationalChanged(object data)
	{
		if (!this.buildingEnabledButton.IsEnabled)
		{
			this.generator.ResetJoules();
		}
	}

	// Token: 0x040018B2 RID: 6322
	[Serialize]
	[SerializeField]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x040018B3 RID: 6323
	private const float batteryStopRunningPercent = 1f;

	// Token: 0x040018B4 RID: 6324
	[MyCmpReq]
	private Generator generator;

	// Token: 0x040018B5 RID: 6325
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040018B6 RID: 6326
	[MyCmpGet]
	private BuildingEnabledButton buildingEnabledButton;

	// Token: 0x040018B7 RID: 6327
	private Chore chore;

	// Token: 0x040018B8 RID: 6328
	private int powerCell;

	// Token: 0x040018B9 RID: 6329
	private ManualGenerator.GeneratePowerSM.Instance smi;

	// Token: 0x040018BA RID: 6330
	private static readonly KAnimHashedString[] symbol_names = new KAnimHashedString[]
	{
		"meter",
		"meter_target",
		"meter_fill",
		"meter_frame",
		"meter_light",
		"meter_tubing"
	};

	// Token: 0x040018BB RID: 6331
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040018BC RID: 6332
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x0200131A RID: 4890
	public class GeneratePowerSM : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance>
	{
		// Token: 0x06007FA9 RID: 32681 RVA: 0x002ED78C File Offset: 0x002EB98C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.on.EventTransition(GameHashes.OperationalChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsActive).PlayAnim("on");
			this.working.DefaultState(this.working.pre);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => this.masterTarget.Get(smi) != null && !smi.master.GetComponent<Operational>().IsActive);
		}

		// Token: 0x04006178 RID: 24952
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State off;

		// Token: 0x04006179 RID: 24953
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State on;

		// Token: 0x0400617A RID: 24954
		public ManualGenerator.GeneratePowerSM.WorkingStates working;

		// Token: 0x020020F0 RID: 8432
		public class WorkingStates : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400937B RID: 37755
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pre;

			// Token: 0x0400937C RID: 37756
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State loop;

			// Token: 0x0400937D RID: 37757
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x020020F1 RID: 8433
		public new class Instance : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600A805 RID: 43013 RVA: 0x00370086 File Offset: 0x0036E286
			public Instance(IStateMachineTarget master) : base(master)
			{
			}
		}
	}
}
