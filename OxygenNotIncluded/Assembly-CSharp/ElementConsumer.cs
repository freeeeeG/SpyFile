using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000786 RID: 1926
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConsumer : SimComponent, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x14000019 RID: 25
	// (add) Token: 0x0600353D RID: 13629 RVA: 0x00120460 File Offset: 0x0011E660
	// (remove) Token: 0x0600353E RID: 13630 RVA: 0x00120498 File Offset: 0x0011E698
	public event Action<Sim.ConsumedMassInfo> OnElementConsumed;

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x0600353F RID: 13631 RVA: 0x001204CD File Offset: 0x0011E6CD
	public float AverageConsumeRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x001204E4 File Offset: 0x0011E6E4
	public static void ClearInstanceMap()
	{
		ElementConsumer.handleInstanceMap.Clear();
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x001204F0 File Offset: 0x0011E6F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		if (this.elementToConsume == SimHashes.Void)
		{
			throw new ArgumentException("No consumable elements specified");
		}
		if (!this.ignoreActiveChanged)
		{
			base.Subscribe<ElementConsumer>(824508782, ElementConsumer.OnActiveChangedDelegate);
		}
		if (this.capacityKG != float.PositiveInfinity)
		{
			this.hasAvailableCapacity = !this.IsStorageFull();
			base.Subscribe<ElementConsumer>(-1697596308, ElementConsumer.OnStorageChangeDelegate);
		}
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x0012057C File Offset: 0x0011E77C
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x0012059A File Offset: 0x0011E79A
	protected virtual bool IsActive()
	{
		return this.operational == null || this.operational.IsActive;
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x001205B8 File Offset: 0x0011E7B8
	public void EnableConsumption(bool enabled)
	{
		bool flag = this.consumptionEnabled;
		this.consumptionEnabled = enabled;
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		if (enabled != flag)
		{
			this.UpdateSimData();
		}
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x001205EC File Offset: 0x0011E7EC
	private bool IsStorageFull()
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.elementToConsume);
		return primaryElement != null && primaryElement.Mass >= this.capacityKG;
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x00120627 File Offset: 0x0011E827
	public void RefreshConsumptionRate()
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimData();
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x00120640 File Offset: 0x0011E840
	private void UpdateSimData()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		int sampleCell = this.GetSampleCell();
		float num = (this.consumptionEnabled && this.hasAvailableCapacity) ? this.consumptionRate : 0f;
		SimMessages.SetElementConsumerData(this.simHandle, sampleCell, num);
		this.UpdateStatusItem();
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x00120698 File Offset: 0x0011E898
	public static void AddMass(Sim.ConsumedMassInfo consumed_info)
	{
		if (!Sim.IsValidHandle(consumed_info.simHandle))
		{
			return;
		}
		ElementConsumer elementConsumer;
		if (ElementConsumer.handleInstanceMap.TryGetValue(consumed_info.simHandle, out elementConsumer))
		{
			elementConsumer.AddMassInternal(consumed_info);
		}
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x001206CE File Offset: 0x0011E8CE
	private int GetSampleCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.sampleCellOffset);
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x001206EC File Offset: 0x0011E8EC
	private void AddMassInternal(Sim.ConsumedMassInfo consumed_info)
	{
		if (consumed_info.mass > 0f)
		{
			if (this.storeOnConsume)
			{
				Element element = ElementLoader.elements[(int)consumed_info.removedElemIdx];
				if (this.elementToConsume == SimHashes.Vacuum || this.elementToConsume == element.id)
				{
					if (element.IsLiquid)
					{
						this.storage.AddLiquid(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
					else if (element.IsGas)
					{
						this.storage.AddGasChunk(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
				}
			}
			else
			{
				this.consumedTemperature = GameUtil.GetFinalTemperature(consumed_info.temperature, consumed_info.mass, this.consumedTemperature, this.consumedMass);
				this.consumedMass += consumed_info.mass;
				if (this.OnElementConsumed != null)
				{
					this.OnElementConsumed(consumed_info);
				}
			}
		}
		Game.Instance.accumulators.Accumulate(this.accumulator, consumed_info.mass);
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x0600354B RID: 13643 RVA: 0x00120818 File Offset: 0x0011EA18
	public bool IsElementAvailable
	{
		get
		{
			int sampleCell = this.GetSampleCell();
			SimHashes id = Grid.Element[sampleCell].id;
			return this.elementToConsume == id && Grid.Mass[sampleCell] >= this.minimumMass;
		}
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x0012085C File Offset: 0x0011EA5C
	private void UpdateStatusItem()
	{
		if (this.showInStatusPanel)
		{
			if (this.statusHandle == Guid.Empty && this.IsActive() && this.consumptionEnabled)
			{
				this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ElementConsumer, this);
				return;
			}
			if (this.statusHandle != Guid.Empty)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
			}
		}
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x001208DC File Offset: 0x0011EADC
	private void OnStorageChange(object data)
	{
		bool flag = !this.IsStorageFull();
		if (flag != this.hasAvailableCapacity)
		{
			this.hasAvailableCapacity = flag;
			this.RefreshConsumptionRate();
		}
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x00120909 File Offset: 0x0011EB09
	protected override void OnCmpEnable()
	{
		if (!base.isSpawned)
		{
			return;
		}
		if (!this.IsActive())
		{
			return;
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x00120923 File Offset: 0x0011EB23
	protected override void OnCmpDisable()
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x0012092C File Offset: 0x0011EB2C
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.isRequired && this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESELEMENT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESELEMENT, arg), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x001209E4 File Offset: 0x0011EBE4
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x00120AD0 File Offset: 0x0011ECD0
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

	// Token: 0x06003553 RID: 13651 RVA: 0x00120B6C File Offset: 0x0011ED6C
	private void OnActiveChanged(object data)
	{
		bool isActive = this.operational.IsActive;
		this.EnableConsumption(isActive);
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x00120B8C File Offset: 0x0011ED8C
	protected override void OnSimUnregister()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		ElementConsumer.handleInstanceMap.Remove(this.simHandle);
		ElementConsumer.StaticUnregister(this.simHandle);
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x00120BBA File Offset: 0x0011EDBA
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		SimMessages.AddElementConsumer(this.GetSampleCell(), this.configuration, this.elementToConsume, this.consumptionRadius, cb_handle.index);
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x00120BE0 File Offset: 0x0011EDE0
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementConsumer.StaticUnregister);
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x00120BEE File Offset: 0x0011EDEE
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementConsumer(-1, sim_handle);
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x00120C02 File Offset: 0x0011EE02
	protected override void OnSimRegistered()
	{
		if (this.consumptionEnabled)
		{
			this.UpdateSimData();
		}
		ElementConsumer.handleInstanceMap[this.simHandle] = this;
	}

	// Token: 0x04002089 RID: 8329
	[HashedEnum]
	[SerializeField]
	public SimHashes elementToConsume = SimHashes.Vacuum;

	// Token: 0x0400208A RID: 8330
	[SerializeField]
	public float consumptionRate;

	// Token: 0x0400208B RID: 8331
	[SerializeField]
	public byte consumptionRadius = 1;

	// Token: 0x0400208C RID: 8332
	[SerializeField]
	public float minimumMass;

	// Token: 0x0400208D RID: 8333
	[SerializeField]
	public bool showInStatusPanel = true;

	// Token: 0x0400208E RID: 8334
	[SerializeField]
	public Vector3 sampleCellOffset;

	// Token: 0x0400208F RID: 8335
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04002090 RID: 8336
	[SerializeField]
	public ElementConsumer.Configuration configuration;

	// Token: 0x04002091 RID: 8337
	[Serialize]
	[NonSerialized]
	public float consumedMass;

	// Token: 0x04002092 RID: 8338
	[Serialize]
	[NonSerialized]
	public float consumedTemperature;

	// Token: 0x04002093 RID: 8339
	[SerializeField]
	public bool storeOnConsume;

	// Token: 0x04002094 RID: 8340
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04002095 RID: 8341
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002096 RID: 8342
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002098 RID: 8344
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002099 RID: 8345
	public bool ignoreActiveChanged;

	// Token: 0x0400209A RID: 8346
	private Guid statusHandle;

	// Token: 0x0400209B RID: 8347
	public bool showDescriptor = true;

	// Token: 0x0400209C RID: 8348
	public bool isRequired = true;

	// Token: 0x0400209D RID: 8349
	private bool consumptionEnabled;

	// Token: 0x0400209E RID: 8350
	private bool hasAvailableCapacity = true;

	// Token: 0x0400209F RID: 8351
	private static Dictionary<int, ElementConsumer> handleInstanceMap = new Dictionary<int, ElementConsumer>();

	// Token: 0x040020A0 RID: 8352
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040020A1 RID: 8353
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001509 RID: 5385
	public enum Configuration
	{
		// Token: 0x0400671E RID: 26398
		Element,
		// Token: 0x0400671F RID: 26399
		AllLiquid,
		// Token: 0x04006720 RID: 26400
		AllGas
	}
}
