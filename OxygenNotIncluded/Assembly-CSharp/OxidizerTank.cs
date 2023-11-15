using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020009AC RID: 2476
[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x060049A9 RID: 18857 RVA: 0x0019EEBE File Offset: 0x0019D0BE
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x060049AA RID: 18858 RVA: 0x0019EEC6 File Offset: 0x0019D0C6
	// (set) Token: 0x060049AB RID: 18859 RVA: 0x0019EED0 File Offset: 0x0019D0D0
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
			base.Trigger(-945020481, this);
			this.OnStorageCapacityChanged(this.targetFillMass);
			if (this.filteredStorage != null)
			{
				this.filteredStorage.FilterChanged();
			}
		}
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x060049AC RID: 18860 RVA: 0x0019EF3C File Offset: 0x0019D13C
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x060049AD RID: 18861 RVA: 0x0019EF43 File Offset: 0x0019D143
	public float MaxCapacity
	{
		get
		{
			return this.maxFillMass;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x060049AE RID: 18862 RVA: 0x0019EF4B File Offset: 0x0019D14B
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x060049AF RID: 18863 RVA: 0x0019EF58 File Offset: 0x0019D158
	public float TotalOxidizerPower
	{
		get
		{
			float num = 0f;
			foreach (GameObject gameObject in this.storage.items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2;
				if (DlcManager.FeatureClusterSpaceEnabled())
				{
					num2 = Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				else
				{
					num2 = RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				num += component.Mass * num2;
			}
			return num;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x060049B0 RID: 18864 RVA: 0x0019EFFC File Offset: 0x0019D1FC
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x060049B1 RID: 18865 RVA: 0x0019EFFF File Offset: 0x0019D1FF
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x0019F008 File Offset: 0x0019D208
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxidizerTank>(-905833192, OxidizerTank.OnCopySettingsDelegate);
		if (this.supportsMultipleOxidizers)
		{
			this.filteredStorage = new FilteredStorage(this, null, this, true, Db.Get().ChoreTypes.Fetch);
			this.filteredStorage.FilterChanged();
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x0019F078 File Offset: 0x0019D278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.discoverResourcesOnSpawn != null)
		{
			foreach (SimHashes hash in this.discoverResourcesOnSpawn)
			{
				Element element = ElementLoader.FindElementByHash(hash);
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			global::Debug.Assert(DlcManager.IsExpansion1Active(), "EXP1 not active but trying to use EXP1 rockety system");
			component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionSufficientOxidizer(this));
		}
		this.UserMaxCapacity = Mathf.Min(this.UserMaxCapacity, this.maxFillMass);
		base.Subscribe<OxidizerTank>(-887025858, OxidizerTank.OnRocketLandedDelegate);
		base.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
	}

	// Token: 0x060049B4 RID: 18868 RVA: 0x0019F174 File Offset: 0x0019D374
	public float GetTotalOxidizerAvailable()
	{
		float num = 0f;
		foreach (Tag tag in this.oxidizerTypes)
		{
			num += this.storage.GetAmountAvailable(tag);
		}
		return num;
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x0019F1B4 File Offset: 0x0019D3B4
	public Dictionary<Tag, float> GetOxidizersAvailable()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (Tag tag in this.oxidizerTypes)
		{
			dictionary[tag] = this.storage.GetAmountAvailable(tag);
		}
		return dictionary;
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x0019F1F8 File Offset: 0x0019D3F8
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x0019F200 File Offset: 0x0019D400
	private void OnStorageCapacityChanged(float newCapacity)
	{
		this.RefreshMeter();
	}

	// Token: 0x060049B8 RID: 18872 RVA: 0x0019F208 File Offset: 0x0019D408
	private void RefreshMeter()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x060049B9 RID: 18873 RVA: 0x0019F21D File Offset: 0x0019D41D
	private void OnRocketLanded(object data)
	{
		if (this.consumeOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x0019F248 File Offset: 0x0019D448
	private void OnCopySettings(object data)
	{
		OxidizerTank component = ((GameObject)data).GetComponent<OxidizerTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x0019F278 File Offset: 0x0019D478
	[ContextMenu("Fill Tank")]
	public void DEBUG_FillTank(SimHashes element)
	{
		base.GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
		if (ElementLoader.FindElementByHash(element).IsLiquid)
		{
			this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, 0, 0, false, true);
			return;
		}
		if (ElementLoader.FindElementByHash(element).IsSolid)
		{
			GameObject go = ElementLoader.FindElementByHash(element).substance.SpawnResource(base.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, false, false, true, false);
		}
	}

	// Token: 0x060049BC RID: 18876 RVA: 0x0019F324 File Offset: 0x0019D524
	public OxidizerTank()
	{
		Tag[] array2;
		if (!DlcManager.IsExpansion1Active())
		{
			Tag[] array = new Tag[2];
			array[0] = SimHashes.OxyRock.CreateTag();
			array2 = array;
			array[1] = SimHashes.LiquidOxygen.CreateTag();
		}
		else
		{
			Tag[] array3 = new Tag[3];
			array3[0] = SimHashes.OxyRock.CreateTag();
			array3[1] = SimHashes.LiquidOxygen.CreateTag();
			array2 = array3;
			array3[2] = SimHashes.Fertilizer.CreateTag();
		}
		this.oxidizerTypes = array2;
		base..ctor();
	}

	// Token: 0x04003067 RID: 12391
	public Storage storage;

	// Token: 0x04003068 RID: 12392
	public bool supportsMultipleOxidizers;

	// Token: 0x04003069 RID: 12393
	private MeterController meter;

	// Token: 0x0400306A RID: 12394
	private bool isSuspended;

	// Token: 0x0400306B RID: 12395
	public bool consumeOnLand = true;

	// Token: 0x0400306C RID: 12396
	[Serialize]
	public float maxFillMass;

	// Token: 0x0400306D RID: 12397
	[Serialize]
	public float targetFillMass;

	// Token: 0x0400306E RID: 12398
	public List<SimHashes> discoverResourcesOnSpawn;

	// Token: 0x0400306F RID: 12399
	[SerializeField]
	private Tag[] oxidizerTypes;

	// Token: 0x04003070 RID: 12400
	private FilteredStorage filteredStorage;

	// Token: 0x04003071 RID: 12401
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003072 RID: 12402
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04003073 RID: 12403
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
