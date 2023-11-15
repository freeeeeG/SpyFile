using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200067A RID: 1658
[AddComponentMenu("KMonoBehaviour/scripts/RationBox")]
public class RationBox : KMonoBehaviour, IUserControlledCapacity, IRender1000ms, IRottable
{
	// Token: 0x06002C08 RID: 11272 RVA: 0x000E9F2C File Offset: 0x000E812C
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, false, Db.Get().ChoreTypes.FoodFetch);
		base.Subscribe<RationBox>(-592767678, RationBox.OnOperationalChangedDelegate);
		base.Subscribe<RationBox>(-905833192, RationBox.OnCopySettingsDelegate);
		DiscoveredResources.Instance.Discover("FieldRation".ToTag(), GameTags.Edible);
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x000E9FA3 File Offset: 0x000E81A3
	protected override void OnSpawn()
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
		this.filteredStorage.FilterChanged();
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x000E9FC2 File Offset: 0x000E81C2
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x000E9FCF File Offset: 0x000E81CF
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x000E9FE4 File Offset: 0x000E81E4
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		RationBox component = gameObject.GetComponent<RationBox>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x000EA01F File Offset: 0x000E821F
	public void Render1000ms(float dt)
	{
		Rottable.SetStatusItems(this);
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06002C0E RID: 11278 RVA: 0x000EA027 File Offset: 0x000E8227
	// (set) Token: 0x06002C0F RID: 11279 RVA: 0x000EA03F File Offset: 0x000E823F
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
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06002C10 RID: 11280 RVA: 0x000EA053 File Offset: 0x000E8253
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06002C11 RID: 11281 RVA: 0x000EA060 File Offset: 0x000E8260
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06002C12 RID: 11282 RVA: 0x000EA067 File Offset: 0x000E8267
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000EA074 File Offset: 0x000E8274
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06002C14 RID: 11284 RVA: 0x000EA077 File Offset: 0x000E8277
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06002C15 RID: 11285 RVA: 0x000EA07F File Offset: 0x000E827F
	public float RotTemperature
	{
		get
		{
			return 277.15f;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06002C16 RID: 11286 RVA: 0x000EA086 File Offset: 0x000E8286
	public float PreserveTemperature
	{
		get
		{
			return 255.15f;
		}
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x000EA0D6 File Offset: 0x000E82D6
	GameObject IRottable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001A01 RID: 6657
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001A02 RID: 6658
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001A03 RID: 6659
	private FilteredStorage filteredStorage;

	// Token: 0x04001A04 RID: 6660
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001A05 RID: 6661
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnCopySettings(data);
	});
}
