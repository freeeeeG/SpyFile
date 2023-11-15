using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000774 RID: 1908
[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060034BC RID: 13500 RVA: 0x0011B341 File Offset: 0x00119541
	// (set) Token: 0x060034BD RID: 13501 RVA: 0x0011B349 File Offset: 0x00119549
	private Chore Chore
	{
		get
		{
			return this._chore;
		}
		set
		{
			this._chore = value;
			this.markedForDrop = (this._chore != null);
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0011B361 File Offset: 0x00119561
	protected DropAllWorkable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x0011B380 File Offset: 0x00119580
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DropAllWorkable>(493375141, DropAllWorkable.OnRefreshUserMenuDelegate);
		base.Subscribe<DropAllWorkable>(-1697596308, DropAllWorkable.OnStorageChangeDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.synchronizeAnims = false;
		base.SetWorkTime(this.dropWorkTime);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x0011B3E8 File Offset: 0x001195E8
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x0011B404 File Offset: 0x00119604
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showCmd = this.GetNewShowCmd();
		if (this.markedForDrop)
		{
			this.DropAll();
		}
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x0011B428 File Offset: 0x00119628
	public void DropAll()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
		}
		else if (this.Chore == null)
		{
			ChoreType chore_type = (!string.IsNullOrEmpty(this.choreTypeID)) ? Db.Get().ChoreTypes.Get(this.choreTypeID) : Db.Get().ChoreTypes.EmptyStorage;
			this.Chore = new WorkChore<DropAllWorkable>(chore_type, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
		else
		{
			this.Chore.Cancel("Cancelled emptying");
			this.Chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
		this.RefreshStatusItem();
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0011B4DC File Offset: 0x001196DC
	protected override void OnCompleteWork(Worker worker)
	{
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			List<GameObject> list = new List<GameObject>(array[i].items);
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject = array[i].Drop(list[j], true);
				if (gameObject != null)
				{
					foreach (Tag tag in this.removeTags)
					{
						gameObject.RemoveTag(tag);
					}
					gameObject.Trigger(580035959, worker);
				}
			}
		}
		this.Chore = null;
		this.RefreshStatusItem();
		base.Trigger(-1957399615, null);
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x0011B5B0 File Offset: 0x001197B0
	private void OnRefreshUserMenu(object data)
	{
		if (this.showCmd)
		{
			KIconButtonMenu.ButtonInfo button = (this.Chore == null) ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x0011B654 File Offset: 0x00119854
	private bool GetNewShowCmd()
	{
		bool flag = false;
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			flag = (flag || !array[i].IsEmpty());
		}
		return flag;
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0011B68C File Offset: 0x0011988C
	private void OnStorageChange(object data)
	{
		bool newShowCmd = this.GetNewShowCmd();
		if (newShowCmd != this.showCmd)
		{
			this.showCmd = newShowCmd;
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x0011B6C8 File Offset: 0x001198C8
	private void RefreshStatusItem()
	{
		if (this.Chore != null && this.statusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.statusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
			return;
		}
		if (this.Chore == null && this.statusItem != Guid.Empty)
		{
			KSelectable component2 = base.GetComponent<KSelectable>();
			this.statusItem = component2.RemoveStatusItem(this.statusItem, false);
		}
	}

	// Token: 0x04001FDE RID: 8158
	[Serialize]
	private bool markedForDrop;

	// Token: 0x04001FDF RID: 8159
	private Chore _chore;

	// Token: 0x04001FE0 RID: 8160
	private bool showCmd;

	// Token: 0x04001FE1 RID: 8161
	private Storage[] storages;

	// Token: 0x04001FE2 RID: 8162
	public float dropWorkTime = 0.1f;

	// Token: 0x04001FE3 RID: 8163
	public string choreTypeID;

	// Token: 0x04001FE4 RID: 8164
	[MyCmpAdd]
	private Prioritizable _prioritizable;

	// Token: 0x04001FE5 RID: 8165
	public List<Tag> removeTags;

	// Token: 0x04001FE6 RID: 8166
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001FE7 RID: 8167
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001FE8 RID: 8168
	private Guid statusItem;
}
