using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200099C RID: 2460
public class FuelTank : KMonoBehaviour, IUserControlledCapacity, IFuelTank
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06004935 RID: 18741 RVA: 0x0019CC41 File Offset: 0x0019AE41
	public IStorage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06004936 RID: 18742 RVA: 0x0019CC49 File Offset: 0x0019AE49
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06004937 RID: 18743 RVA: 0x0019CC51 File Offset: 0x0019AE51
	// (set) Token: 0x06004938 RID: 18744 RVA: 0x0019CC5C File Offset: 0x0019AE5C
	public float UserMaxCapacity
	{
		get
		{
			return this.targetFillMass;
		}
		set
		{
			this.targetFillMass = value;
			this.storage.capacityKg = this.targetFillMass;
			ConduitConsumer component = base.GetComponent<ConduitConsumer>();
			if (component != null)
			{
				component.capacityKG = this.targetFillMass;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.capacity = (component2.refillMass = this.targetFillMass);
			}
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06004939 RID: 18745 RVA: 0x0019CCCE File Offset: 0x0019AECE
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x0600493A RID: 18746 RVA: 0x0019CCD5 File Offset: 0x0019AED5
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x0600493B RID: 18747 RVA: 0x0019CCDD File Offset: 0x0019AEDD
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x0600493C RID: 18748 RVA: 0x0019CCEA File Offset: 0x0019AEEA
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x0600493D RID: 18749 RVA: 0x0019CCED File Offset: 0x0019AEED
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x0600493E RID: 18750 RVA: 0x0019CCF5 File Offset: 0x0019AEF5
	// (set) Token: 0x0600493F RID: 18751 RVA: 0x0019CD00 File Offset: 0x0019AF00
	public Tag FuelType
	{
		get
		{
			return this.fuelType;
		}
		set
		{
			this.fuelType = value;
			if (this.storage.storageFilters == null)
			{
				this.storage.storageFilters = new List<Tag>();
			}
			this.storage.storageFilters.Add(this.fuelType);
			ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
			if (component != null)
			{
				component.RequestedItemTag = this.fuelType;
			}
		}
	}

	// Token: 0x06004940 RID: 18752 RVA: 0x0019CD63 File Offset: 0x0019AF63
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FuelTank>(-905833192, FuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x06004941 RID: 18753 RVA: 0x0019CD7C File Offset: 0x0019AF7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetFillMass == -1f)
		{
			this.targetFillMass = this.physicalFuelCapacity;
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		}
		base.Subscribe<FuelTank>(-887025858, FuelTank.OnRocketLandedDelegate);
		this.UserMaxCapacity = this.UserMaxCapacity;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<FuelTank>(-1697596308, FuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x0019CE71 File Offset: 0x0019B071
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
	}

	// Token: 0x06004943 RID: 18755 RVA: 0x0019CE95 File Offset: 0x0019B095
	private void OnRocketLanded(object data)
	{
		if (this.ConsumeFuelOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
	}

	// Token: 0x06004944 RID: 18756 RVA: 0x0019CEAC File Offset: 0x0019B0AC
	private void OnCopySettings(object data)
	{
		FuelTank component = ((GameObject)data).GetComponent<FuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x06004945 RID: 18757 RVA: 0x0019CEDC File Offset: 0x0019B0DC
	public void DEBUG_FillTank()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			RocketEngine rocketEngine = null;
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				rocketEngine = gameObject.GetComponent<RocketEngine>();
				if (rocketEngine != null && rocketEngine.mainEngine)
				{
					break;
				}
			}
			if (rocketEngine != null)
			{
				Element element = ElementLoader.GetElement(rocketEngine.fuelTag);
				if (element.IsLiquid)
				{
					this.storage.AddLiquid(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsGas)
				{
					this.storage.AddGasChunk(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsSolid)
				{
					this.storage.AddOre(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
			}
			return;
		}
		RocketEngineCluster rocketEngineCluster = null;
		foreach (GameObject gameObject2 in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			rocketEngineCluster = gameObject2.GetComponent<RocketEngineCluster>();
			if (rocketEngineCluster != null && rocketEngineCluster.mainEngine)
			{
				break;
			}
		}
		if (rocketEngineCluster != null)
		{
			Element element2 = ElementLoader.GetElement(rocketEngineCluster.fuelTag);
			if (element2.IsLiquid)
			{
				this.storage.AddLiquid(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsGas)
			{
				this.storage.AddGasChunk(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsSolid)
			{
				this.storage.AddOre(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			rocketEngineCluster.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().UpdateStatusItem();
			return;
		}
		global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
	}

	// Token: 0x0400302B RID: 12331
	public Storage storage;

	// Token: 0x0400302C RID: 12332
	private MeterController meter;

	// Token: 0x0400302D RID: 12333
	[Serialize]
	public float targetFillMass = -1f;

	// Token: 0x0400302E RID: 12334
	[SerializeField]
	public float physicalFuelCapacity;

	// Token: 0x0400302F RID: 12335
	public bool consumeFuelOnLand;

	// Token: 0x04003030 RID: 12336
	[SerializeField]
	private Tag fuelType;

	// Token: 0x04003031 RID: 12337
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003032 RID: 12338
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04003033 RID: 12339
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
