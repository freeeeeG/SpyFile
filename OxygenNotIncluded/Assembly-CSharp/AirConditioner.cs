using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AirConditioner")]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x170001AB RID: 427
	// (get) Token: 0x060023B5 RID: 9141 RVA: 0x000C35CC File Offset: 0x000C17CC
	// (set) Token: 0x060023B6 RID: 9142 RVA: 0x000C35D4 File Offset: 0x000C17D4
	public float lastEnvTemp { get; private set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x060023B7 RID: 9143 RVA: 0x000C35DD File Offset: 0x000C17DD
	// (set) Token: 0x060023B8 RID: 9144 RVA: 0x000C35E5 File Offset: 0x000C17E5
	public float lastGasTemp { get; private set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x060023B9 RID: 9145 RVA: 0x000C35EE File Offset: 0x000C17EE
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000C35F6 File Offset: 0x000C17F6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AirConditioner>(-592767678, AirConditioner.OnOperationalChangedDelegate);
		base.Subscribe<AirConditioner>(824508782, AirConditioner.OnActiveChangedDelegate);
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x000C3620 File Offset: 0x000C1820
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		this.cooledAirOutputCell = this.building.GetUtilityOutputCell();
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000C3690 File Offset: 0x000C1890
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.UpdateState(dt);
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000C36C2 File Offset: 0x000C18C2
	private static bool UpdateStateCb(int cell, object data)
	{
		AirConditioner airConditioner = data as AirConditioner;
		airConditioner.cellCount++;
		airConditioner.envTemp += Grid.Temperature[cell];
		return true;
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000C36F0 File Offset: 0x000C18F0
	private void UpdateState(float dt)
	{
		bool value = this.consumer.IsSatisfied;
		this.envTemp = 0f;
		this.cellCount = 0;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, AirConditioner.UpdateStateCbDelegate);
			this.envTemp /= (float)this.cellCount;
		}
		this.lastEnvTemp = this.envTemp;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < items.Count; i++)
		{
			PrimaryElement component = items[i].GetComponent<PrimaryElement>();
			if (component.Mass > 0f && (!this.isLiquidConditioner || !component.Element.IsGas) && (this.isLiquidConditioner || !component.Element.IsLiquid))
			{
				value = true;
				this.lastGasTemp = component.Temperature;
				float num = component.Temperature + this.temperatureDelta;
				if (num < 1f)
				{
					num = 1f;
					this.lowTempLag = Mathf.Min(this.lowTempLag + dt / 5f, 1f);
				}
				else
				{
					this.lowTempLag = Mathf.Min(this.lowTempLag - dt / 5f, 0f);
				}
				float num2 = (this.isLiquidConditioner ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.cooledAirOutputCell, component.ElementID, component.Mass, num, component.DiseaseIdx, component.DiseaseCount);
				component.KeepZeroMassObject = true;
				float num3 = num2 / component.Mass;
				int num4 = (int)((float)component.DiseaseCount * num3);
				component.Mass -= num2;
				component.ModifyDiseaseCount(-num4, "AirConditioner.UpdateState");
				float num5 = (num - component.Temperature) * component.Element.specificHeatCapacity * num2;
				float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
				this.lastSampleTime = Time.time;
				GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, -num5, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, display_dt);
				break;
			}
		}
		if (Time.time - this.lastSampleTime > 2f)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 0f, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time - this.lastSampleTime);
			this.lastSampleTime = Time.time;
		}
		this.operational.SetActive(value, false);
		this.UpdateStatus();
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000C3994 File Offset: 0x000C1B94
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateState(0f);
		}
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000C39AE File Offset: 0x000C1BAE
	private void OnActiveChanged(object data)
	{
		this.UpdateStatus();
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000C39B8 File Offset: 0x000C1BB8
	private void UpdateStatus()
	{
		if (this.operational.IsActive)
		{
			if (this.lowTempLag >= 1f && !this.showingLowTemp)
			{
				this.statusHandle = (this.isLiquidConditioner ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdLiquid, this) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdGas, this));
				this.showingLowTemp = true;
				this.showingHotEnv = false;
				return;
			}
			if (this.lowTempLag <= 0f && (this.showingHotEnv || this.showingLowTemp))
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
			if (this.statusHandle == Guid.Empty)
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
		}
		else
		{
			this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000C3B2C File Offset: 0x000C1D2C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(this.temperatureDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
		Element element = ElementLoader.FindElementByName(this.isLiquidConditioner ? "Water" : "Oxygen");
		float num;
		if (this.isLiquidConditioner)
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 10000f);
		}
		else
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 1000f);
		}
		float dtu = num * 1f;
		Descriptor item = default(Descriptor);
		string txt = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		string tooltip = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Descriptor item2 = default(Descriptor);
		item2.SetupDescriptor(string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.GASCOOLING, formattedTemperature), string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING, formattedTemperature), Descriptor.DescriptorType.Effect);
		list.Add(item2);
		return list;
	}

	// Token: 0x0400145E RID: 5214
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400145F RID: 5215
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04001460 RID: 5216
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04001461 RID: 5217
	[MyCmpReq]
	private ConduitConsumer consumer;

	// Token: 0x04001462 RID: 5218
	[MyCmpReq]
	private BuildingComplete building;

	// Token: 0x04001463 RID: 5219
	[MyCmpGet]
	private OccupyArea occupyArea;

	// Token: 0x04001464 RID: 5220
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001465 RID: 5221
	public float temperatureDelta = -14f;

	// Token: 0x04001466 RID: 5222
	public float maxEnvironmentDelta = -50f;

	// Token: 0x04001467 RID: 5223
	private float lowTempLag;

	// Token: 0x04001468 RID: 5224
	private bool showingLowTemp;

	// Token: 0x04001469 RID: 5225
	public bool isLiquidConditioner;

	// Token: 0x0400146A RID: 5226
	private bool showingHotEnv;

	// Token: 0x0400146D RID: 5229
	private Guid statusHandle;

	// Token: 0x0400146E RID: 5230
	[Serialize]
	private float targetTemperature;

	// Token: 0x0400146F RID: 5231
	private int cooledAirOutputCell = -1;

	// Token: 0x04001470 RID: 5232
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001471 RID: 5233
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04001472 RID: 5234
	private float lastSampleTime = -1f;

	// Token: 0x04001473 RID: 5235
	private float envTemp;

	// Token: 0x04001474 RID: 5236
	private int cellCount;

	// Token: 0x04001475 RID: 5237
	private static readonly Func<int, object, bool> UpdateStateCbDelegate = (int cell, object data) => AirConditioner.UpdateStateCb(cell, data);
}
