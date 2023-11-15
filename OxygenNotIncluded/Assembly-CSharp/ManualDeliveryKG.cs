using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000857 RID: 2135
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0015A8E8 File Offset: 0x00158AE8
	public bool IsPaused
	{
		get
		{
			return this.paused;
		}
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06003E68 RID: 15976 RVA: 0x0015A8F0 File Offset: 0x00158AF0
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0015A8F8 File Offset: 0x00158AF8
	// (set) Token: 0x06003E6A RID: 15978 RVA: 0x0015A900 File Offset: 0x00158B00
	public Tag RequestedItemTag
	{
		get
		{
			return this.requestedItemTag;
		}
		set
		{
			this.requestedItemTag = value;
			this.AbortDelivery("Requested Item Tag Changed");
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06003E6B RID: 15979 RVA: 0x0015A914 File Offset: 0x00158B14
	// (set) Token: 0x06003E6C RID: 15980 RVA: 0x0015A91C File Offset: 0x00158B1C
	public Tag[] ForbiddenTags
	{
		get
		{
			return this.forbiddenTags;
		}
		set
		{
			this.forbiddenTags = value;
			this.AbortDelivery("Forbidden Tags Changed");
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0015A930 File Offset: 0x00158B30
	public Storage DebugStorage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06003E6E RID: 15982 RVA: 0x0015A938 File Offset: 0x00158B38
	public FetchList2 DebugFetchList
	{
		get
		{
			return this.fetchList;
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06003E6F RID: 15983 RVA: 0x0015A940 File Offset: 0x00158B40
	private float MassStoredPerUnit
	{
		get
		{
			return this.storage.GetMassAvailable(this.requestedItemTag) / this.MassPerUnit;
		}
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0015A95C File Offset: 0x00158B5C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", base.name);
		if (this.allowPause)
		{
			base.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
			base.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
		}
		base.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
		if (this.storage != null)
		{
			this.SetStorage(this.storage);
		}
		Prioritizable.AddRef(base.gameObject);
		if (this.userPaused && this.allowPause)
		{
			this.OnPause();
		}
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x0015AA00 File Offset: 0x00158C00
	protected override void OnCleanUp()
	{
		this.AbortDelivery("ManualDeliverKG destroyed");
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x0015AA20 File Offset: 0x00158C20
	public void SetStorage(Storage storage)
	{
		if (this.storage != null)
		{
			this.storage.Unsubscribe(this.onStorageChangeSubscription);
			this.onStorageChangeSubscription = -1;
		}
		this.AbortDelivery("storage pointer changed");
		this.storage = storage;
		if (this.storage != null && base.isSpawned)
		{
			global::Debug.Assert(this.onStorageChangeSubscription == -1);
			this.onStorageChangeSubscription = this.storage.Subscribe<ManualDeliveryKG>(-1697596308, ManualDeliveryKG.OnStorageChangedDelegate);
		}
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x0015AAA4 File Offset: 0x00158CA4
	public void Pause(bool pause, string reason)
	{
		if (this.paused != pause)
		{
			this.paused = pause;
			if (pause)
			{
				this.AbortDelivery(reason);
			}
		}
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0015AAC0 File Offset: 0x00158CC0
	public void Sim1000ms(float dt)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0015AAC8 File Offset: 0x00158CC8
	[ContextMenu("UpdateDeliveryState")]
	public void UpdateDeliveryState()
	{
		if (!this.requestedItemTag.IsValid)
		{
			return;
		}
		if (this.storage == null)
		{
			return;
		}
		this.UpdateFetchList();
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0015AAF0 File Offset: 0x00158CF0
	public void RequestDelivery()
	{
		if (this.fetchList != null)
		{
			return;
		}
		float massStoredPerUnit = this.MassStoredPerUnit;
		if (massStoredPerUnit < this.capacity)
		{
			this.CreateFetchChore(massStoredPerUnit);
		}
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x0015AB20 File Offset: 0x00158D20
	private void CreateFetchChore(float stored_mass)
	{
		float num = this.capacity - stored_mass;
		num = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, num);
		if (this.RoundFetchAmountToInt)
		{
			num = (float)((int)num);
			if (num < 0.1f)
			{
				return;
			}
		}
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash);
		this.fetchList = new FetchList2(this.storage, byHash);
		this.fetchList.ShowStatusItem = this.ShowStatusItem;
		this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.MinimumMass);
		FetchList2 fetchList = this.fetchList;
		Tag tag = this.requestedItemTag;
		float amount = num;
		fetchList.Add(tag, this.forbiddenTags, amount, Operational.State.None);
		this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x0015ABEC File Offset: 0x00158DEC
	private void OnFetchComplete()
	{
		if (this.FillToCapacity && this.storage != null)
		{
			float amountAvailable = this.storage.GetAmountAvailable(this.requestedItemTag);
			if (amountAvailable < this.capacity)
			{
				this.CreateFetchChore(amountAvailable);
			}
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0015AC34 File Offset: 0x00158E34
	private void UpdateFetchList()
	{
		if (this.paused)
		{
			return;
		}
		if (this.fetchList != null && this.fetchList.IsComplete)
		{
			this.fetchList = null;
		}
		if (!(this.operational == null) && !this.operational.MeetsRequirements(this.operationalRequirement))
		{
			if (this.fetchList != null)
			{
				this.fetchList.Cancel("Operational requirements");
				this.fetchList = null;
				return;
			}
		}
		else if (this.fetchList == null && this.MassStoredPerUnit < this.refillMass)
		{
			this.RequestDelivery();
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0015ACC3 File Offset: 0x00158EC3
	public void AbortDelivery(string reason)
	{
		if (this.fetchList != null)
		{
			FetchList2 fetchList = this.fetchList;
			this.fetchList = null;
			fetchList.Cancel(reason);
		}
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0015ACE0 File Offset: 0x00158EE0
	protected void OnStorageChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0015ACE8 File Offset: 0x00158EE8
	private void OnPause()
	{
		this.userPaused = true;
		this.Pause(true, "Forbid manual delivery");
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0015ACFD File Offset: 0x00158EFD
	private void OnResume()
	{
		this.userPaused = false;
		this.Pause(false, "Allow manual delivery");
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0015AD14 File Offset: 0x00158F14
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowPause)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.paused) ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0015ADB6 File Offset: 0x00158FB6
	private void OnOperationalChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x04002870 RID: 10352
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002871 RID: 10353
	[SerializeField]
	private Storage storage;

	// Token: 0x04002872 RID: 10354
	[SerializeField]
	public Tag requestedItemTag;

	// Token: 0x04002873 RID: 10355
	private Tag[] forbiddenTags;

	// Token: 0x04002874 RID: 10356
	[SerializeField]
	public float capacity = 100f;

	// Token: 0x04002875 RID: 10357
	[SerializeField]
	public float refillMass = 10f;

	// Token: 0x04002876 RID: 10358
	[SerializeField]
	public float MinimumMass = 10f;

	// Token: 0x04002877 RID: 10359
	[SerializeField]
	public bool RoundFetchAmountToInt;

	// Token: 0x04002878 RID: 10360
	[SerializeField]
	public float MassPerUnit = 1f;

	// Token: 0x04002879 RID: 10361
	[SerializeField]
	public bool FillToCapacity;

	// Token: 0x0400287A RID: 10362
	[SerializeField]
	public Operational.State operationalRequirement;

	// Token: 0x0400287B RID: 10363
	[SerializeField]
	public bool allowPause;

	// Token: 0x0400287C RID: 10364
	[SerializeField]
	private bool paused;

	// Token: 0x0400287D RID: 10365
	[SerializeField]
	public HashedString choreTypeIDHash;

	// Token: 0x0400287E RID: 10366
	[Serialize]
	private bool userPaused;

	// Token: 0x0400287F RID: 10367
	public bool ShowStatusItem = true;

	// Token: 0x04002880 RID: 10368
	private FetchList2 fetchList;

	// Token: 0x04002881 RID: 10369
	private int onStorageChangeSubscription = -1;

	// Token: 0x04002882 RID: 10370
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002883 RID: 10371
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002884 RID: 10372
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnStorageChanged(data);
	});
}
