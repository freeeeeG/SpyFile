using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200067E RID: 1662
[AddComponentMenu("KMonoBehaviour/scripts/Refrigerator")]
public class Refrigerator : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x06002C42 RID: 11330 RVA: 0x000EB0D5 File Offset: 0x000E92D5
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, true, Db.Get().ChoreTypes.FoodFetch);
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x000EB108 File Offset: 0x000E9308
	protected override void OnSpawn()
	{
		base.GetComponent<KAnimControllerBase>().Play("off", KAnim.PlayMode.Once, 1f, 0f);
		FoodStorage component = base.GetComponent<FoodStorage>();
		component.FilteredStorage = this.filteredStorage;
		component.SpicedFoodOnly = component.SpicedFoodOnly;
		this.filteredStorage.FilterChanged();
		this.UpdateLogicCircuit();
		base.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
		base.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
		base.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x000EB196 File Offset: 0x000E9396
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x000EB1A3 File Offset: 0x000E93A3
	public bool IsActive()
	{
		return this.operational.IsActive;
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x000EB1B0 File Offset: 0x000E93B0
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		Refrigerator component = gameObject.GetComponent<Refrigerator>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06002C47 RID: 11335 RVA: 0x000EB1EB File Offset: 0x000E93EB
	// (set) Token: 0x06002C48 RID: 11336 RVA: 0x000EB203 File Offset: 0x000E9403
	public float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06002C49 RID: 11337 RVA: 0x000EB21D File Offset: 0x000E941D
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06002C4A RID: 11338 RVA: 0x000EB22A File Offset: 0x000E942A
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000EB231 File Offset: 0x000E9431
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000EB23E File Offset: 0x000E943E
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06002C4D RID: 11341 RVA: 0x000EB241 File Offset: 0x000E9441
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x000EB249 File Offset: 0x000E9449
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x000EB254 File Offset: 0x000E9454
	private void UpdateLogicCircuit()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
	}

	// Token: 0x04001A1F RID: 6687
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001A20 RID: 6688
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001A21 RID: 6689
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001A22 RID: 6690
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001A23 RID: 6691
	private FilteredStorage filteredStorage;

	// Token: 0x04001A24 RID: 6692
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001A25 RID: 6693
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
