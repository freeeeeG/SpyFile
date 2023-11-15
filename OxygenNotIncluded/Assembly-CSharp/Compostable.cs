using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000491 RID: 1169
[AddComponentMenu("KMonoBehaviour/scripts/Compostable")]
public class Compostable : KMonoBehaviour
{
	// Token: 0x06001A50 RID: 6736 RVA: 0x0008B8AC File Offset: 0x00089AAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.isMarkedForCompost = base.GetComponent<KPrefabID>().HasTag(GameTags.Compostable);
		if (this.isMarkedForCompost)
		{
			this.MarkForCompost(false);
		}
		base.Subscribe<Compostable>(493375141, Compostable.OnRefreshUserMenuDelegate);
		base.Subscribe<Compostable>(856640610, Compostable.OnStoreDelegate);
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x0008B908 File Offset: 0x00089B08
	private void MarkForCompost(bool force = false)
	{
		this.RefreshStatusItem();
		Storage storage = base.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Drop(base.gameObject, true);
		}
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x0008B940 File Offset: 0x00089B40
	private void OnToggleCompost()
	{
		if (!this.isMarkedForCompost)
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component.storage != null)
			{
				component.storage.Drop(base.gameObject, true);
			}
			Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.compostPrefab);
			if (pickupable != null)
			{
				SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
				return;
			}
		}
		else
		{
			Pickupable component2 = base.GetComponent<Pickupable>();
			Pickupable pickupable2 = EntitySplitter.Split(component2, component2.TotalAmount, this.originalPrefab);
			SelectTool.Instance.SelectNextFrame(pickupable2.GetComponent<KSelectable>(), true);
		}
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x0008B9D4 File Offset: 0x00089BD4
	private void RefreshStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, false);
		if (this.isMarkedForCompost)
		{
			if (base.GetComponent<Pickupable>() != null && base.GetComponent<Pickupable>().storage == null)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, null);
				return;
			}
			component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, null);
		}
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x0008BA6E File Offset: 0x00089C6E
	private void OnStore(object data)
	{
		this.RefreshStatusItem();
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x0008BA78 File Offset: 0x00089C78
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.isMarkedForCompost)
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME_OFF, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04000E91 RID: 3729
	[SerializeField]
	public bool isMarkedForCompost;

	// Token: 0x04000E92 RID: 3730
	public GameObject originalPrefab;

	// Token: 0x04000E93 RID: 3731
	public GameObject compostPrefab;

	// Token: 0x04000E94 RID: 3732
	private static readonly EventSystem.IntraObjectHandler<Compostable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04000E95 RID: 3733
	private static readonly EventSystem.IntraObjectHandler<Compostable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnStore(data);
	});
}
