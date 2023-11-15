using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200072F RID: 1839
[SkipSaveFileSerialization]
public class PressureVulnerable : StateMachineComponent<PressureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06003289 RID: 12937 RVA: 0x0010C670 File Offset: 0x0010A870
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

	// Token: 0x0600328A RID: 12938 RVA: 0x0010C692 File Offset: 0x0010A892
	public bool IsSafeElement(Element element)
	{
		return this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_atmospheres.Contains(element);
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x0600328B RID: 12939 RVA: 0x0010C6BA File Offset: 0x0010A8BA
	public PressureVulnerable.PressureState ExternalPressureState
	{
		get
		{
			return this.pressureState;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x0600328C RID: 12940 RVA: 0x0010C6C2 File Offset: 0x0010A8C2
	public bool IsLethal
	{
		get
		{
			return this.pressureState == PressureVulnerable.PressureState.LethalHigh || this.pressureState == PressureVulnerable.PressureState.LethalLow || !this.testAreaElementSafe;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x0600328D RID: 12941 RVA: 0x0010C6E0 File Offset: 0x0010A8E0
	public bool IsNormal
	{
		get
		{
			return this.testAreaElementSafe && this.pressureState == PressureVulnerable.PressureState.Normal;
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x0010C6F8 File Offset: 0x0010A8F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayPressureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.AirPressure, base.gameObject));
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x0010C740 File Offset: 0x0010A940
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.RegisterUpdate1000ms(this);
		this.cell = Grid.PosToCell(this);
		base.smi.sm.pressure.Set(1f, base.smi, false);
		base.smi.sm.safe_element.Set(this.testAreaElementSafe, base.smi, false);
		base.smi.master.pressureAccumulator = Game.Instance.accumulators.Add("pressureAccumulator", this);
		base.smi.master.elementAccumulator = Game.Instance.accumulators.Add("elementAccumulator", this);
		base.smi.StartSM();
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x0010C804 File Offset: 0x0010AA04
	protected override void OnCleanUp()
	{
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x0010C818 File Offset: 0x0010AA18
	public void Configure(SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = false;
		this.pressureWarning_Low = float.MinValue;
		this.pressureLethal_Low = float.MinValue;
		this.pressureLethal_High = float.MaxValue;
		this.pressureWarning_High = float.MaxValue;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x0010C88C File Offset: 0x0010AA8C
	public void Configure(float pressureWarningLow = 0.25f, float pressureLethalLow = 0.01f, float pressureWarningHigh = 10f, float pressureLethalHigh = 30f, SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = true;
		this.pressureWarning_Low = pressureWarningLow;
		this.pressureLethal_Low = pressureLethalLow;
		this.pressureLethal_High = pressureLethalHigh;
		this.pressureWarning_High = pressureWarningHigh;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06003293 RID: 12947 RVA: 0x0010C8F3 File Offset: 0x0010AAF3
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Pressure,
				WiltCondition.Condition.AtmosphereElement
			};
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06003294 RID: 12948 RVA: 0x0010C904 File Offset: 0x0010AB04
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.warningLow) || base.smi.IsInsideState(base.smi.sm.lethalLow))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooLow.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOLOW.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.warningHigh) || base.smi.IsInsideState(base.smi.sm.lethalHigh))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooHigh.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOHIGH.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.unsafeElement))
			{
				text += Db.Get().CreatureStatusItems.WrongAtmosphere.resolveStringCallback(CREATURES.STATUSITEMS.WRONGATMOSPHERE.NAME, this);
			}
			return text;
		}
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x0010CA31 File Offset: 0x0010AC31
	public bool IsSafePressure(float pressure)
	{
		return !this.pressure_sensitive || (pressure > this.pressureLethal_Low && pressure < this.pressureLethal_High);
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x0010CA54 File Offset: 0x0010AC54
	public void SlicedSim1000ms(float dt)
	{
		float pressureOverArea = this.GetPressureOverArea(this.cell);
		Game.Instance.accumulators.Accumulate(base.smi.master.pressureAccumulator, pressureOverArea);
		float averageRate = Game.Instance.accumulators.GetAverageRate(base.smi.master.pressureAccumulator);
		this.displayPressureAmount.value = averageRate;
		Game.Instance.accumulators.Accumulate(base.smi.master.elementAccumulator, this.testAreaElementSafe ? 1f : 0f);
		float averageRate2 = Game.Instance.accumulators.GetAverageRate(base.smi.master.elementAccumulator);
		bool value = this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || averageRate2 > 0f;
		base.smi.sm.safe_element.Set(value, base.smi, false);
		base.smi.sm.pressure.Set(averageRate, base.smi, false);
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x0010CB6A File Offset: 0x0010AD6A
	public float GetExternalPressure()
	{
		return this.GetPressureOverArea(this.cell);
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x0010CB78 File Offset: 0x0010AD78
	private float GetPressureOverArea(int cell)
	{
		bool flag = this.testAreaElementSafe;
		PressureVulnerable.testAreaPressure = 0f;
		PressureVulnerable.testAreaCount = 0;
		this.testAreaElementSafe = false;
		this.currentAtmoElement = null;
		this.occupyArea.TestArea(cell, this, PressureVulnerable.testAreaCB);
		this.occupyArea.TestAreaAbove(cell, this, PressureVulnerable.testAreaCB);
		PressureVulnerable.testAreaPressure = ((PressureVulnerable.testAreaCount > 0) ? (PressureVulnerable.testAreaPressure / (float)PressureVulnerable.testAreaCount) : 0f);
		if (this.testAreaElementSafe != flag)
		{
			base.Trigger(-2023773544, null);
		}
		return PressureVulnerable.testAreaPressure;
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x0010CC0C File Offset: 0x0010AE0C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.pressure_sensitive)
		{
			list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		}
		if (this.safe_atmospheres != null && this.safe_atmospheres.Count > 0)
		{
			string text = "";
			bool flag = false;
			bool flag2 = false;
			foreach (Element element in this.safe_atmospheres)
			{
				flag |= element.IsGas;
				flag2 |= element.IsLiquid;
				text = text + "\n        • " + element.name;
			}
			if (flag && flag2)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_MIXED, text), Descriptor.DescriptorType.Requirement, false));
			}
			if (flag)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE, text), Descriptor.DescriptorType.Requirement, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_LIQUID, text), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x04001E56 RID: 7766
	private HandleVector<int>.Handle pressureAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001E57 RID: 7767
	private HandleVector<int>.Handle elementAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001E58 RID: 7768
	private OccupyArea _occupyArea;

	// Token: 0x04001E59 RID: 7769
	public float pressureLethal_Low;

	// Token: 0x04001E5A RID: 7770
	public float pressureWarning_Low;

	// Token: 0x04001E5B RID: 7771
	public float pressureWarning_High;

	// Token: 0x04001E5C RID: 7772
	public float pressureLethal_High;

	// Token: 0x04001E5D RID: 7773
	private static float testAreaPressure;

	// Token: 0x04001E5E RID: 7774
	private static int testAreaCount;

	// Token: 0x04001E5F RID: 7775
	public bool testAreaElementSafe = true;

	// Token: 0x04001E60 RID: 7776
	public Element currentAtmoElement;

	// Token: 0x04001E61 RID: 7777
	private static Func<int, object, bool> testAreaCB = delegate(int test_cell, object data)
	{
		PressureVulnerable pressureVulnerable = (PressureVulnerable)data;
		if (!Grid.IsSolidCell(test_cell))
		{
			Element element = Grid.Element[test_cell];
			if (pressureVulnerable.IsSafeElement(element))
			{
				PressureVulnerable.testAreaPressure += Grid.Mass[test_cell];
				PressureVulnerable.testAreaCount++;
				pressureVulnerable.testAreaElementSafe = true;
				pressureVulnerable.currentAtmoElement = element;
			}
			if (pressureVulnerable.currentAtmoElement == null)
			{
				pressureVulnerable.currentAtmoElement = element;
			}
		}
		return true;
	};

	// Token: 0x04001E62 RID: 7778
	private AmountInstance displayPressureAmount;

	// Token: 0x04001E63 RID: 7779
	public bool pressure_sensitive = true;

	// Token: 0x04001E64 RID: 7780
	public HashSet<Element> safe_atmospheres = new HashSet<Element>();

	// Token: 0x04001E65 RID: 7781
	private int cell;

	// Token: 0x04001E66 RID: 7782
	private PressureVulnerable.PressureState pressureState = PressureVulnerable.PressureState.Normal;

	// Token: 0x020014B9 RID: 5305
	public class StatesInstance : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.GameInstance
	{
		// Token: 0x060085B9 RID: 34233 RVA: 0x00306E47 File Offset: 0x00305047
		public StatesInstance(PressureVulnerable master) : base(master)
		{
			if (Db.Get().Amounts.Maturity.Lookup(base.gameObject) != null)
			{
				this.hasMaturity = true;
			}
		}

		// Token: 0x04006641 RID: 26177
		public bool hasMaturity;
	}

	// Token: 0x020014BA RID: 5306
	public class States : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable>
	{
		// Token: 0x060085BA RID: 34234 RVA: 0x00306E74 File Offset: 0x00305074
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalLow.ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalLow;
			}).TriggerOnEnter(GameHashes.LowPressureFatal, null);
			this.lethalHigh.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalHigh;
			}).TriggerOnEnter(GameHashes.HighPressureFatal, null);
			this.warningLow.ParamTransition<float>(this.pressure, this.lethalLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_Low).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningLow;
			}).TriggerOnEnter(GameHashes.LowPressureWarning, null);
			this.unsafeElement.ParamTransition<bool>(this.safe_element, this.normal, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsTrue).TriggerOnExit(GameHashes.CorrectAtmosphere, null).TriggerOnEnter(GameHashes.WrongAtmosphere, null);
			this.warningHigh.ParamTransition<float>(this.pressure, this.lethalHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_High).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningHigh;
			}).TriggerOnEnter(GameHashes.HighPressureWarning, null);
			this.normal.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_High).ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalPressureAchieved, null);
		}

		// Token: 0x04006642 RID: 26178
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.FloatParameter pressure;

		// Token: 0x04006643 RID: 26179
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.BoolParameter safe_element;

		// Token: 0x04006644 RID: 26180
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State unsafeElement;

		// Token: 0x04006645 RID: 26181
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalLow;

		// Token: 0x04006646 RID: 26182
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalHigh;

		// Token: 0x04006647 RID: 26183
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningLow;

		// Token: 0x04006648 RID: 26184
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningHigh;

		// Token: 0x04006649 RID: 26185
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State normal;
	}

	// Token: 0x020014BB RID: 5307
	public enum PressureState
	{
		// Token: 0x0400664B RID: 26187
		LethalLow,
		// Token: 0x0400664C RID: 26188
		WarningLow,
		// Token: 0x0400664D RID: 26189
		Normal,
		// Token: 0x0400664E RID: 26190
		WarningHigh,
		// Token: 0x0400664F RID: 26191
		LethalHigh
	}
}
