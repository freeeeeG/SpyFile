using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Movable")]
public class Movable : Workable
{
	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06001C7E RID: 7294 RVA: 0x000987F0 File Offset: 0x000969F0
	public bool IsMarkedForMove
	{
		get
		{
			return this.isMarkedForMove;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06001C7F RID: 7295 RVA: 0x000987F8 File Offset: 0x000969F8
	public Storage StorageProxy
	{
		get
		{
			if (this.storageProxy == null)
			{
				return null;
			}
			return this.storageProxy.Get();
		}
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x0009880F File Offset: 0x00096A0F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00098848 File Offset: 0x00096A48
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				if (this.reachableChangedHandle < 0)
				{
					this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.storageReachableChangedHandle < 0)
				{
					this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.cancelHandle < 0)
				{
					this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
				}
				base.gameObject.AddTag(GameTags.MarkedForMove);
				return;
			}
			this.isMarkedForMove = false;
		}
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00098908 File Offset: 0x00096B08
	private void OnReachableChanged(object data)
	{
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				int num = Grid.PosToCell(this.pickupable);
				int num2 = Grid.PosToCell(this.StorageProxy);
				if (num != num2)
				{
					bool flag = MinionGroupProber.Get().IsReachable(num, OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(num2, OffsetGroups.Standard);
					if (this.pickupable.HasTag(GameTags.Creatures.Confined))
					{
						flag = false;
					}
					KSelectable component = base.GetComponent<KSelectable>();
					this.pendingMoveGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MarkedForMove, this.pendingMoveGuid, flag, this);
					this.storageUnreachableGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MoveStorageUnreachable, this.storageUnreachableGuid, !flag, this);
					return;
				}
			}
			else
			{
				this.ClearMove();
			}
		}
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000989E4 File Offset: 0x00096BE4
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable != null)
		{
			Movable component = pickupable.GetComponent<Movable>();
			if (component.isMarkedForMove)
			{
				this.storageProxy = new Ref<Storage>(component.StorageProxy);
				this.MarkForMove();
			}
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00098A27 File Offset: 0x00096C27
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isMarkedForMove && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().RemoveMovable(this);
			this.ClearStorageProxy();
		}
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00098A5C File Offset: 0x00096C5C
	private void CleanupMove(object data)
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00098A80 File Offset: 0x00096C80
	public void ClearMove()
	{
		if (this.isMarkedForMove)
		{
			this.isMarkedForMove = false;
			KSelectable component = base.GetComponent<KSelectable>();
			this.pendingMoveGuid = component.RemoveStatusItem(this.pendingMoveGuid, false);
			this.storageUnreachableGuid = component.RemoveStatusItem(this.storageUnreachableGuid, false);
			this.ClearStorageProxy();
			base.gameObject.RemoveTag(GameTags.MarkedForMove);
			if (this.reachableChangedHandle != -1)
			{
				base.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				this.reachableChangedHandle = -1;
			}
			if (this.cancelHandle != -1)
			{
				base.Unsubscribe(2127324410, new Action<object>(this.CleanupMove));
				this.cancelHandle = -1;
			}
		}
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x00098B30 File Offset: 0x00096D30
	private void ClearStorageProxy()
	{
		if (this.storageReachableChangedHandle != -1)
		{
			this.StorageProxy.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
			this.storageReachableChangedHandle = -1;
		}
		this.storageProxy = null;
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00098B65 File Offset: 0x00096D65
	private void OnClickMove()
	{
		MoveToLocationTool.Instance.Activate(this);
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x00098B72 File Offset: 0x00096D72
	private void OnClickCancel()
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x00098B94 File Offset: 0x00096D94
	private void OnRefreshUserMenu(object data)
	{
		if (this.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForMove ? new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME, new System.Action(this.OnClickMove), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x00098C3B File Offset: 0x00096E3B
	public void MoveToLocation(int cell)
	{
		this.CreateStorageProxy(cell);
		this.MarkForMove();
		base.gameObject.Trigger(1122777325, base.gameObject);
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x00098C60 File Offset: 0x00096E60
	private void MarkForMove()
	{
		base.Trigger(2127324410, null);
		this.isMarkedForMove = true;
		this.OnReachableChanged(null);
		this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.StorageProxy.GetComponent<CancellableMove>().SetMovable(this);
		base.gameObject.AddTag(GameTags.MarkedForMove);
		this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x00098D04 File Offset: 0x00096F04
	public bool CanMoveTo(int cell)
	{
		return !Grid.IsSolidCell(cell) && Grid.IsWorldValidCell(cell) && base.gameObject.IsMyParentWorld(cell);
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x00098D24 File Offset: 0x00096F24
	private void CreateStorageProxy(int cell)
	{
		if (this.storageProxy == null || this.storageProxy.Get() == null)
		{
			if (Grid.Objects[cell, 44] != null)
			{
				Storage component = Grid.Objects[cell, 44].GetComponent<Storage>();
				this.storageProxy = new Ref<Storage>(component);
				return;
			}
			Vector3 position = Grid.CellToPosCBC(cell, MoveToLocationTool.Instance.visualizerLayer);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MovePickupablePlacerConfig.ID), position);
			Storage component2 = gameObject.GetComponent<Storage>();
			gameObject.SetActive(true);
			this.storageProxy = new Ref<Storage>(component2);
		}
	}

	// Token: 0x04000FBE RID: 4030
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x04000FBF RID: 4031
	[Serialize]
	private bool isMarkedForMove;

	// Token: 0x04000FC0 RID: 4032
	[Serialize]
	private Ref<Storage> storageProxy;

	// Token: 0x04000FC1 RID: 4033
	private int storageReachableChangedHandle = -1;

	// Token: 0x04000FC2 RID: 4034
	private int reachableChangedHandle = -1;

	// Token: 0x04000FC3 RID: 4035
	private int cancelHandle = -1;

	// Token: 0x04000FC4 RID: 4036
	private Guid pendingMoveGuid;

	// Token: 0x04000FC5 RID: 4037
	private Guid storageUnreachableGuid;
}
