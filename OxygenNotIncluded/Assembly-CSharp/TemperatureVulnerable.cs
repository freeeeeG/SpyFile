using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200073B RID: 1851
[SkipSaveFileSerialization]
public class TemperatureVulnerable : StateMachineComponent<TemperatureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000393 RID: 915
	// (get) Token: 0x060032D8 RID: 13016 RVA: 0x0010DE94 File Offset: 0x0010C094
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x060032D9 RID: 13017 RVA: 0x0010DEB6 File Offset: 0x0010C0B6
	public float TemperatureLethalLow
	{
		get
		{
			return this.internalTemperatureLethal_Low;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x060032DA RID: 13018 RVA: 0x0010DEBE File Offset: 0x0010C0BE
	public float TemperatureLethalHigh
	{
		get
		{
			return this.internalTemperatureLethal_High;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x060032DB RID: 13019 RVA: 0x0010DEC6 File Offset: 0x0010C0C6
	public float TemperatureWarningLow
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_Low + (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_Low;
		}
	}

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x060032DC RID: 13020 RVA: 0x0010DEF6 File Offset: 0x0010C0F6
	public float TemperatureWarningHigh
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_High - (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_High;
		}
	}

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x060032DD RID: 13021 RVA: 0x0010DF28 File Offset: 0x0010C128
	// (remove) Token: 0x060032DE RID: 13022 RVA: 0x0010DF60 File Offset: 0x0010C160
	public event Action<float, float> OnTemperature;

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x060032DF RID: 13023 RVA: 0x0010DF95 File Offset: 0x0010C195
	public float InternalTemperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x060032E0 RID: 13024 RVA: 0x0010DFA2 File Offset: 0x0010C1A2
	public TemperatureVulnerable.TemperatureState GetInternalTemperatureState
	{
		get
		{
			return this.internalTemperatureState;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x060032E1 RID: 13025 RVA: 0x0010DFAA File Offset: 0x0010C1AA
	public bool IsLethal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot || this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold;
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x060032E2 RID: 13026 RVA: 0x0010DFC0 File Offset: 0x0010C1C0
	public bool IsNormal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x060032E3 RID: 13027 RVA: 0x0010DFCB File Offset: 0x0010C1CB
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[1];
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060032E4 RID: 13028 RVA: 0x0010DFD4 File Offset: 0x0010C1D4
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.warningCold))
			{
				return Db.Get().CreatureStatusItems.Cold_Crop.resolveStringCallback(CREATURES.STATUSITEMS.COLD_CROP.NAME, this);
			}
			if (base.smi.IsInsideState(base.smi.sm.warningHot))
			{
				return Db.Get().CreatureStatusItems.Hot_Crop.resolveStringCallback(CREATURES.STATUSITEMS.HOT_CROP.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x0010E06C File Offset: 0x0010C26C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayTemperatureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.Temperature, base.gameObject));
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x0010E0B4 File Offset: 0x0010C2B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wiltTempRangeModAttribute = this.GetAttributes().Get(Db.Get().PlantAttributes.WiltTempRangeMod);
		this.temperatureRangeModScalar = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.RegisterUpdate1000ms(this);
		base.smi.sm.internalTemp.Set(this.primaryElement.Temperature, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x0010E13E File Offset: 0x0010C33E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.UnregisterUpdate1000ms(this);
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x0010E151 File Offset: 0x0010C351
	public void Configure(float tempWarningLow, float tempLethalLow, float tempWarningHigh, float tempLethalHigh)
	{
		this.internalTemperatureWarning_Low = tempWarningLow;
		this.internalTemperatureLethal_Low = tempLethalLow;
		this.internalTemperatureLethal_High = tempLethalHigh;
		this.internalTemperatureWarning_High = tempWarningHigh;
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x0010E170 File Offset: 0x0010C370
	public bool IsCellSafe(int cell)
	{
		float averageTemperature = this.GetAverageTemperature(cell);
		return averageTemperature > -1f && averageTemperature > this.TemperatureLethalLow && averageTemperature < this.internalTemperatureLethal_High;
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x0010E1A4 File Offset: 0x0010C3A4
	public void SlicedSim1000ms(float dt)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)))
		{
			return;
		}
		base.smi.sm.internalTemp.Set(this.InternalTemperature, base.smi, false);
		this.displayTemperatureAmount.value = this.InternalTemperature;
		if (this.OnTemperature != null)
		{
			this.OnTemperature(dt, this.InternalTemperature);
		}
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x0010E214 File Offset: 0x0010C414
	private static bool GetAverageTemperatureCb(int cell, object data)
	{
		TemperatureVulnerable temperatureVulnerable = data as TemperatureVulnerable;
		if (Grid.Mass[cell] > 0.1f)
		{
			temperatureVulnerable.averageTemp += Grid.Temperature[cell];
			temperatureVulnerable.cellCount++;
		}
		return true;
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x0010E264 File Offset: 0x0010C464
	private float GetAverageTemperature(int cell)
	{
		this.averageTemp = 0f;
		this.cellCount = 0;
		this.occupyArea.TestArea(cell, this, TemperatureVulnerable.GetAverageTemperatureCbDelegate);
		if (this.cellCount > 0)
		{
			return this.averageTemp / (float)this.cellCount;
		}
		return -1f;
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x0010E2B4 File Offset: 0x0010C4B4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		float num = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		float temp = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningLow : (this.internalTemperatureWarning_Low + (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		float temp2 = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningHigh : (this.internalTemperatureWarning_High - (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04001E82 RID: 7810
	private OccupyArea _occupyArea;

	// Token: 0x04001E83 RID: 7811
	[SerializeField]
	private float internalTemperatureLethal_Low;

	// Token: 0x04001E84 RID: 7812
	[SerializeField]
	private float internalTemperatureWarning_Low;

	// Token: 0x04001E85 RID: 7813
	[SerializeField]
	private float internalTemperatureWarning_High;

	// Token: 0x04001E86 RID: 7814
	[SerializeField]
	private float internalTemperatureLethal_High;

	// Token: 0x04001E87 RID: 7815
	private AttributeInstance wiltTempRangeModAttribute;

	// Token: 0x04001E88 RID: 7816
	private float temperatureRangeModScalar;

	// Token: 0x04001E8A RID: 7818
	private const float minimumMassForReading = 0.1f;

	// Token: 0x04001E8B RID: 7819
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001E8C RID: 7820
	[MyCmpReq]
	private SimTemperatureTransfer temperatureTransfer;

	// Token: 0x04001E8D RID: 7821
	private AmountInstance displayTemperatureAmount;

	// Token: 0x04001E8E RID: 7822
	private TemperatureVulnerable.TemperatureState internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;

	// Token: 0x04001E8F RID: 7823
	private float averageTemp;

	// Token: 0x04001E90 RID: 7824
	private int cellCount;

	// Token: 0x04001E91 RID: 7825
	private static readonly Func<int, object, bool> GetAverageTemperatureCbDelegate = (int cell, object data) => TemperatureVulnerable.GetAverageTemperatureCb(cell, data);

	// Token: 0x020014D4 RID: 5332
	public class StatesInstance : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.GameInstance
	{
		// Token: 0x060085F7 RID: 34295 RVA: 0x00307A65 File Offset: 0x00305C65
		public StatesInstance(TemperatureVulnerable master) : base(master)
		{
			if (Db.Get().Amounts.Maturity.Lookup(base.gameObject) != null)
			{
				this.hasMaturity = true;
			}
		}

		// Token: 0x04006684 RID: 26244
		public bool hasMaturity;
	}

	// Token: 0x020014D5 RID: 5333
	public class States : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable>
	{
		// Token: 0x060085F8 RID: 34296 RVA: 0x00307A94 File Offset: 0x00305C94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalCold;
			}).TriggerOnEnter(GameHashes.TooColdFatal, null).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalLow).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.lethalHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalHot;
			}).TriggerOnEnter(GameHashes.TooHotFatal, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalHigh).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.warningCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningCold;
			}).TriggerOnEnter(GameHashes.TooColdWarning, null).ParamTransition<float>(this.internalTemp, this.lethalCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalLow).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningLow);
			this.warningHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningHot;
			}).TriggerOnEnter(GameHashes.TooHotWarning, null).ParamTransition<float>(this.internalTemp, this.lethalHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalHigh).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningHigh);
			this.normal.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningHigh).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningLow);
		}

		// Token: 0x060085F9 RID: 34297 RVA: 0x00307D5C File Offset: 0x00305F5C
		private static void Kill(StateMachine.Instance smi)
		{
			DeathMonitor.Instance smi2 = smi.GetSMI<DeathMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.Kill(Db.Get().Deaths.Generic);
			}
		}

		// Token: 0x04006685 RID: 26245
		public StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.FloatParameter internalTemp;

		// Token: 0x04006686 RID: 26246
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalCold;

		// Token: 0x04006687 RID: 26247
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalHot;

		// Token: 0x04006688 RID: 26248
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningCold;

		// Token: 0x04006689 RID: 26249
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningHot;

		// Token: 0x0400668A RID: 26250
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State normal;
	}

	// Token: 0x020014D6 RID: 5334
	public enum TemperatureState
	{
		// Token: 0x0400668C RID: 26252
		LethalCold,
		// Token: 0x0400668D RID: 26253
		WarningCold,
		// Token: 0x0400668E RID: 26254
		Normal,
		// Token: 0x0400668F RID: 26255
		WarningHot,
		// Token: 0x04006690 RID: 26256
		LethalHot
	}
}
