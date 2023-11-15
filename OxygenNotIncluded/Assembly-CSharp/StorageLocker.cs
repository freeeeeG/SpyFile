using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000698 RID: 1688
[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x06002D50 RID: 11600 RVA: 0x000F0B1B File Offset: 0x000EED1B
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x000F0B24 File Offset: 0x000EED24
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("StorageLocker", 35);
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, this, use_logic_meter, fetch_chore_type);
		base.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x000F0B80 File Offset: 0x000EED80
	protected override void OnSpawn()
	{
		this.filteredStorage.FilterChanged();
		if (this.nameable != null && !this.lockerName.IsNullOrWhiteSpace())
		{
			this.nameable.SetName(this.lockerName);
		}
		base.Trigger(-1683615038, null);
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x000F0BD0 File Offset: 0x000EEDD0
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x000F0BE0 File Offset: 0x000EEDE0
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		StorageLocker component = gameObject.GetComponent<StorageLocker>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x000F0C1B File Offset: 0x000EEE1B
	public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
	{
		if (forbidden)
		{
			this.filteredStorage.RemoveForbiddenTag(game_tag);
			return;
		}
		this.filteredStorage.AddForbiddenTag(game_tag);
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06002D56 RID: 11606 RVA: 0x000F0C39 File Offset: 0x000EEE39
	// (set) Token: 0x06002D57 RID: 11607 RVA: 0x000F0C51 File Offset: 0x000EEE51
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06002D58 RID: 11608 RVA: 0x000F0C65 File Offset: 0x000EEE65
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06002D59 RID: 11609 RVA: 0x000F0C72 File Offset: 0x000EEE72
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06002D5A RID: 11610 RVA: 0x000F0C79 File Offset: 0x000EEE79
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06002D5B RID: 11611 RVA: 0x000F0C86 File Offset: 0x000EEE86
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06002D5C RID: 11612 RVA: 0x000F0C89 File Offset: 0x000EEE89
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x04001AD2 RID: 6866
	private LoggerFS log;

	// Token: 0x04001AD3 RID: 6867
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04001AD4 RID: 6868
	[Serialize]
	public string lockerName = "";

	// Token: 0x04001AD5 RID: 6869
	protected FilteredStorage filteredStorage;

	// Token: 0x04001AD6 RID: 6870
	[MyCmpGet]
	private UserNameable nameable;

	// Token: 0x04001AD7 RID: 6871
	public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

	// Token: 0x04001AD8 RID: 6872
	private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>(delegate(StorageLocker component, object data)
	{
		component.OnCopySettings(data);
	});
}
