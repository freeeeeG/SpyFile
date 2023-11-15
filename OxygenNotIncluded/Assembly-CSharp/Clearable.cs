using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200048D RID: 1165
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Clearable")]
public class Clearable : Workable, ISaveLoadable, IRender1000ms
{
	// Token: 0x06001A05 RID: 6661 RVA: 0x00089C74 File Offset: 0x00087E74
	protected override void OnPrefabInit()
	{
		base.Subscribe<Clearable>(2127324410, Clearable.OnCancelDelegate);
		base.Subscribe<Clearable>(856640610, Clearable.OnStoreDelegate);
		base.Subscribe<Clearable>(-2064133523, Clearable.OnAbsorbDelegate);
		base.Subscribe<Clearable>(493375141, Clearable.OnRefreshUserMenuDelegate);
		base.Subscribe<Clearable>(-1617557748, Clearable.OnEquippedDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Clearing;
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x00089CFC File Offset: 0x00087EFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForClear)
		{
			if (this.HasTag(GameTags.Stored))
			{
				if (!base.transform.parent.GetComponent<Storage>().allowClearable)
				{
					this.isMarkedForClear = false;
				}
				else
				{
					this.MarkForClear(true, true);
				}
			}
			else
			{
				this.MarkForClear(true, false);
			}
		}
		this.RefreshClearableStatus(true);
	}

	// Token: 0x06001A07 RID: 6663 RVA: 0x00089D5D File Offset: 0x00087F5D
	private void OnStore(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x06001A08 RID: 6664 RVA: 0x00089D68 File Offset: 0x00087F68
	private void OnCancel(object data)
	{
		for (ObjectLayerListItem objectLayerListItem = this.pickupable.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
		{
			if (objectLayerListItem.gameObject != null)
			{
				objectLayerListItem.gameObject.GetComponent<Clearable>().CancelClearing();
			}
		}
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x00089DAC File Offset: 0x00087FAC
	public void CancelClearing()
	{
		if (this.isMarkedForClear)
		{
			this.isMarkedForClear = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Garbage);
			Prioritizable.RemoveRef(base.gameObject);
			if (this.clearHandle.IsValid())
			{
				GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
				this.clearHandle.Clear();
			}
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Remove(this);
		}
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x00089E20 File Offset: 0x00088020
	public void MarkForClear(bool restoringFromSave = false, bool allowWhenStored = false)
	{
		if (!this.isClearable)
		{
			return;
		}
		if ((!this.isMarkedForClear || restoringFromSave) && !this.pickupable.IsEntombed && !this.clearHandle.IsValid() && (!this.HasTag(GameTags.Stored) || allowWhenStored))
		{
			Prioritizable.AddRef(base.gameObject);
			base.GetComponent<KPrefabID>().AddTag(GameTags.Garbage, false);
			this.isMarkedForClear = true;
			this.clearHandle = GlobalChoreProvider.Instance.RegisterClearable(this);
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
		}
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x00089EBB File Offset: 0x000880BB
	private void OnClickClear()
	{
		this.MarkForClear(false, false);
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x00089EC5 File Offset: 0x000880C5
	private void OnClickCancel()
	{
		this.CancelClearing();
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x00089ECD File Offset: 0x000880CD
	private void OnEquipped(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x00089ED5 File Offset: 0x000880D5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.clearHandle.IsValid())
		{
			GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
			this.clearHandle.Clear();
		}
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x00089F08 File Offset: 0x00088108
	private void OnRefreshUserMenu(object data)
	{
		if (!this.isClearable || base.GetComponent<Health>() != null || this.HasTag(GameTags.Stored) || this.HasTag(GameTags.MarkedForMove))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForClear ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME, new System.Action(this.OnClickClear), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x00089FD4 File Offset: 0x000881D4
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Clearable component = pickupable.GetComponent<Clearable>();
			if (component != null && component.isMarkedForClear)
			{
				this.MarkForClear(false, false);
			}
		}
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x0008A011 File Offset: 0x00088211
	public void Render1000ms(float dt)
	{
		this.RefreshClearableStatus(false);
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x0008A01C File Offset: 0x0008821C
	public void RefreshClearableStatus(bool force_update)
	{
		if (force_update || this.isMarkedForClear)
		{
			bool show = false;
			bool show2 = false;
			if (this.isMarkedForClear)
			{
				show2 = !(show = GlobalChoreProvider.Instance.ClearableHasDestination(this.pickupable));
			}
			this.pendingClearGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, this.pendingClearGuid, show, this);
			this.pendingClearNoStorageGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, this.pendingClearNoStorageGuid, show2, this);
		}
	}

	// Token: 0x04000E6E RID: 3694
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x04000E6F RID: 3695
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04000E70 RID: 3696
	[Serialize]
	private bool isMarkedForClear;

	// Token: 0x04000E71 RID: 3697
	private HandleVector<int>.Handle clearHandle;

	// Token: 0x04000E72 RID: 3698
	public bool isClearable = true;

	// Token: 0x04000E73 RID: 3699
	private Guid pendingClearGuid;

	// Token: 0x04000E74 RID: 3700
	private Guid pendingClearNoStorageGuid;

	// Token: 0x04000E75 RID: 3701
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04000E76 RID: 3702
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04000E77 RID: 3703
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04000E78 RID: 3704
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04000E79 RID: 3705
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnEquipped(data);
	});
}
