using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200078F RID: 1935
[SerializationConfig(MemberSerialization.OptIn)]
public class EnergyGenerator : Generator, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060035CD RID: 13773 RVA: 0x00122E63 File Offset: 0x00121063
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060035CE RID: 13774 RVA: 0x00122E6A File Offset: 0x0012106A
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x00122E76 File Offset: 0x00121076
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x00122E79 File Offset: 0x00121079
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x00122E80 File Offset: 0x00121080
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x00122E87 File Offset: 0x00121087
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x00122E95 File Offset: 0x00121095
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x00122EA4 File Offset: 0x001210A4
	string ISliderControl.GetSliderTooltip(int index)
	{
		ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP"), component.RequestedItemTag.ProperName(), this.batteryRefillPercent * 100f);
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x00122EE8 File Offset: 0x001210E8
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x00122EF0 File Offset: 0x001210F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EnergyGenerator.EnsureStatusItemAvailable();
		base.Subscribe<EnergyGenerator>(824508782, EnergyGenerator.OnActiveChangedDelegate);
		if (!this.ignoreBatteryRefillPercent)
		{
			base.gameObject.AddOrGet<CopyBuildingSettings>();
			base.Subscribe<EnergyGenerator>(-905833192, EnergyGenerator.OnCopySettingsDelegate);
		}
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x00122F40 File Offset: 0x00121140
	private void OnCopySettings(object data)
	{
		EnergyGenerator component = ((GameObject)data).GetComponent<EnergyGenerator>();
		if (component != null)
		{
			this.batteryRefillPercent = component.batteryRefillPercent;
		}
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00122F70 File Offset: 0x00121170
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x00122FC8 File Offset: 0x001211C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasMeter)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_fill",
				"meter_frame",
				"meter_OL"
			});
		}
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x0012302C File Offset: 0x0012122C
	private bool IsConvertible(float dt)
	{
		bool flag = true;
		foreach (EnergyGenerator.InputItem inputItem in this.formula.inputs)
		{
			float massAvailable = this.storage.GetMassAvailable(inputItem.tag);
			float num = inputItem.consumptionRate * dt;
			flag = (flag && massAvailable >= num);
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x00123090 File Offset: 0x00121290
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.hasMeter)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[0];
			float positionPercent = this.storage.GetMassAvailable(inputItem.tag) / inputItem.maxStoredMass;
			this.meter.SetPositionPercent(positionPercent);
		}
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		bool value = false;
		if (this.operational.IsOperational)
		{
			bool flag = false;
			List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID);
			if (!this.ignoreBatteryRefillPercent && batteriesOnCircuit.Count > 0)
			{
				using (List<Battery>.Enumerator enumerator = batteriesOnCircuit.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Battery battery = enumerator.Current;
						if (this.batteryRefillPercent <= 0f && battery.PercentFull <= 0f)
						{
							flag = true;
							break;
						}
						if (battery.PercentFull < this.batteryRefillPercent)
						{
							flag = true;
							break;
						}
					}
					goto IL_105;
				}
			}
			flag = true;
			IL_105:
			if (!this.ignoreBatteryRefillPercent)
			{
				this.selectable.ToggleStatusItem(EnergyGenerator.batteriesSufficientlyFull, !flag, null);
			}
			if (this.delivery != null)
			{
				this.delivery.Pause(!flag, "Circuit has sufficient energy");
			}
			if (this.formula.inputs != null)
			{
				bool flag2 = this.IsConvertible(dt);
				this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag2, this.formula);
				if (flag2)
				{
					foreach (EnergyGenerator.InputItem inputItem2 in this.formula.inputs)
					{
						float amount = inputItem2.consumptionRate * dt;
						this.storage.ConsumeIgnoringDisease(inputItem2.tag, amount);
					}
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					foreach (EnergyGenerator.OutputItem output in this.formula.outputs)
					{
						this.Emit(output, dt, component);
					}
					base.GenerateJoules(base.WattageRating * dt, false);
					this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
					value = true;
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x00123310 File Offset: 0x00121510
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.inputs == null || this.formula.inputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.inputs.Length; i++)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[i];
			string arg = inputItem.tag.ProperName();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x001233DC File Offset: 0x001215DC
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.outputs == null || this.formula.outputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.outputs.Length; i++)
		{
			EnergyGenerator.OutputItem outputItem = this.formula.outputs[i];
			string arg = ElementLoader.FindElementByHash(outputItem.element).tag.ProperName();
			Descriptor item = default(Descriptor);
			if (outputItem.minTemperature > 0f)
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
			}
			else
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect);
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x0012352C File Offset: 0x0012172C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x060035DF RID: 13791 RVA: 0x001235C8 File Offset: 0x001217C8
	public static StatusItem BatteriesSufficientlyFull
	{
		get
		{
			return EnergyGenerator.batteriesSufficientlyFull;
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x001235D0 File Offset: 0x001217D0
	public static void EnsureStatusItemAvailable()
	{
		if (EnergyGenerator.batteriesSufficientlyFull == null)
		{
			EnergyGenerator.batteriesSufficientlyFull = new StatusItem("BatteriesSufficientlyFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x0012360C File Offset: 0x0012180C
	public static EnergyGenerator.Formula CreateSimpleFormula(Tag input_element, float input_mass_rate, float max_stored_input_mass, SimHashes output_element = SimHashes.Void, float output_mass_rate = 0f, bool store_output_mass = true, CellOffset output_offset = default(CellOffset), float min_output_temperature = 0f)
	{
		EnergyGenerator.Formula result = default(EnergyGenerator.Formula);
		result.inputs = new EnergyGenerator.InputItem[]
		{
			new EnergyGenerator.InputItem(input_element, input_mass_rate, max_stored_input_mass)
		};
		if (output_element != SimHashes.Void)
		{
			result.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(output_element, output_mass_rate, store_output_mass, output_offset, min_output_temperature)
			};
		}
		else
		{
			result.outputs = null;
		}
		return result;
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x00123674 File Offset: 0x00121874
	private void Emit(EnergyGenerator.OutputItem output, float dt, PrimaryElement root_pe)
	{
		Element element = ElementLoader.FindElementByHash(output.element);
		float num = output.creationRate * dt;
		if (output.store)
		{
			if (element.IsGas)
			{
				this.storage.AddGasChunk(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			if (element.IsLiquid)
			{
				this.storage.AddLiquid(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num, root_pe.Temperature, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, true, false, true, false);
			return;
		}
		else
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), output.emitOffset);
			float temperature = Mathf.Max(root_pe.Temperature, output.minTemperature);
			if (element.IsGas)
			{
				SimMessages.ModifyMass(num2, num, byte.MaxValue, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temperature, output.element);
				return;
			}
			if (element.IsLiquid)
			{
				ushort elementIndex = ElementLoader.GetElementIndex(output.element);
				FallingWater.instance.AddParticle(num2, elementIndex, num, temperature, byte.MaxValue, 0, true, false, false, false);
				return;
			}
			element.substance.SpawnResource(Grid.CellToPosCCC(num2, Grid.SceneLayer.Front), num, temperature, byte.MaxValue, 0, true, false, false);
			return;
		}
	}

	// Token: 0x040020DF RID: 8415
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040020E0 RID: 8416
	[MyCmpGet]
	private ManualDeliveryKG delivery;

	// Token: 0x040020E1 RID: 8417
	[SerializeField]
	[Serialize]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x040020E2 RID: 8418
	public bool ignoreBatteryRefillPercent;

	// Token: 0x040020E3 RID: 8419
	public bool hasMeter = true;

	// Token: 0x040020E4 RID: 8420
	private static StatusItem batteriesSufficientlyFull;

	// Token: 0x040020E5 RID: 8421
	public Meter.Offset meterOffset;

	// Token: 0x040020E6 RID: 8422
	[SerializeField]
	public EnergyGenerator.Formula formula;

	// Token: 0x040020E7 RID: 8423
	private MeterController meter;

	// Token: 0x040020E8 RID: 8424
	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040020E9 RID: 8425
	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001512 RID: 5394
	[DebuggerDisplay("{tag} -{consumptionRate} kg/s")]
	[Serializable]
	public struct InputItem
	{
		// Token: 0x060086D2 RID: 34514 RVA: 0x0030973C File Offset: 0x0030793C
		public InputItem(Tag tag, float consumption_rate, float max_stored_mass)
		{
			this.tag = tag;
			this.consumptionRate = consumption_rate;
			this.maxStoredMass = max_stored_mass;
		}

		// Token: 0x0400673A RID: 26426
		public Tag tag;

		// Token: 0x0400673B RID: 26427
		public float consumptionRate;

		// Token: 0x0400673C RID: 26428
		public float maxStoredMass;
	}

	// Token: 0x02001513 RID: 5395
	[DebuggerDisplay("{element} {creationRate} kg/s")]
	[Serializable]
	public struct OutputItem
	{
		// Token: 0x060086D3 RID: 34515 RVA: 0x00309753 File Offset: 0x00307953
		public OutputItem(SimHashes element, float creation_rate, bool store, float min_temperature = 0f)
		{
			this = new EnergyGenerator.OutputItem(element, creation_rate, store, CellOffset.none, min_temperature);
		}

		// Token: 0x060086D4 RID: 34516 RVA: 0x00309765 File Offset: 0x00307965
		public OutputItem(SimHashes element, float creation_rate, bool store, CellOffset emit_offset, float min_temperature = 0f)
		{
			this.element = element;
			this.creationRate = creation_rate;
			this.store = store;
			this.emitOffset = emit_offset;
			this.minTemperature = min_temperature;
		}

		// Token: 0x0400673D RID: 26429
		public SimHashes element;

		// Token: 0x0400673E RID: 26430
		public float creationRate;

		// Token: 0x0400673F RID: 26431
		public bool store;

		// Token: 0x04006740 RID: 26432
		public CellOffset emitOffset;

		// Token: 0x04006741 RID: 26433
		public float minTemperature;
	}

	// Token: 0x02001514 RID: 5396
	[Serializable]
	public struct Formula
	{
		// Token: 0x04006742 RID: 26434
		public EnergyGenerator.InputItem[] inputs;

		// Token: 0x04006743 RID: 26435
		public EnergyGenerator.OutputItem[] outputs;

		// Token: 0x04006744 RID: 26436
		public Tag meterTag;
	}
}
