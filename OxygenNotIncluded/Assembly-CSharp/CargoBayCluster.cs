using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200098E RID: 2446
public class CargoBayCluster : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06004832 RID: 18482 RVA: 0x0019763D File Offset: 0x0019583D
	// (set) Token: 0x06004833 RID: 18483 RVA: 0x00197645 File Offset: 0x00195845
	public float UserMaxCapacity
	{
		get
		{
			return this.userMaxCapacity;
		}
		set
		{
			this.userMaxCapacity = value;
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06004834 RID: 18484 RVA: 0x0019765A File Offset: 0x0019585A
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06004835 RID: 18485 RVA: 0x00197661 File Offset: 0x00195861
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06004836 RID: 18486 RVA: 0x0019766E File Offset: 0x0019586E
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06004837 RID: 18487 RVA: 0x0019767B File Offset: 0x0019587B
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06004838 RID: 18488 RVA: 0x0019767E File Offset: 0x0019587E
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06004839 RID: 18489 RVA: 0x00197686 File Offset: 0x00195886
	public float RemainingCapacity
	{
		get
		{
			return this.userMaxCapacity - this.storage.MassStored();
		}
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x0019769A File Offset: 0x0019589A
	protected override void OnPrefabInit()
	{
		this.userMaxCapacity = this.storage.capacityKg;
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x001976B0 File Offset: 0x001958B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBayCluster>(493375141, CargoBayCluster.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
		component.matchParentOffset = true;
		component.forceAlwaysAlive = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBayCluster>(-1697596308, CargoBayCluster.OnStorageChangeDelegate);
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x00197770 File Offset: 0x00195970
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x001977CC File Offset: 0x001959CC
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		this.UpdateCargoStatusItem();
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x001977F8 File Offset: 0x001959F8
	private void UpdateCargoStatusItem()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		CraftModuleInterface craftInterface = component.CraftInterface;
		if (craftInterface == null)
		{
			return;
		}
		Clustercraft component2 = craftInterface.GetComponent<Clustercraft>();
		if (component2 == null)
		{
			return;
		}
		component2.UpdateStatusItem();
	}

	// Token: 0x04002FD2 RID: 12242
	private MeterController meter;

	// Token: 0x04002FD3 RID: 12243
	[SerializeField]
	public Storage storage;

	// Token: 0x04002FD4 RID: 12244
	[SerializeField]
	public CargoBay.CargoType storageType;

	// Token: 0x04002FD5 RID: 12245
	[Serialize]
	private float userMaxCapacity;

	// Token: 0x04002FD6 RID: 12246
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002FD7 RID: 12247
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnStorageChange(data);
	});
}
