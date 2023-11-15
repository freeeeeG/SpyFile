using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class CancellableMove : Cancellable
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0008982B File Offset: 0x00087A2B
	public List<Ref<Movable>> movingObjects
	{
		get
		{
			return this.movables;
		}
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x00089834 File Offset: 0x00087A34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!component.IsPrioritizable())
		{
			component.AddRef();
		}
		if (this.fetchChore == null)
		{
			GameObject nextTarget = this.GetNextTarget();
			if (!(nextTarget != null) || nextTarget.IsNullOrDestroyed())
			{
				global::Debug.LogWarning("MovePickupable spawned with no objects to move. Destroying placer.");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
		}
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = base.gameObject;
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x0008990C File Offset: 0x00087B0C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = null;
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x0008993F File Offset: 0x00087B3F
	public void CancelAll()
	{
		this.OnCancel(null);
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x00089948 File Offset: 0x00087B48
	public void OnCancel(Movable cancel_movable = null)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			Ref<Movable> @ref = this.movables[i];
			if (@ref != null)
			{
				Movable movable = @ref.Get();
				if (cancel_movable == null || movable == cancel_movable)
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("CancelMove");
			if (this.fetchChore.driver == null && this.movables.Count <= 0)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x000899EC File Offset: 0x00087BEC
	protected override void OnCancel(object data)
	{
		this.OnCancel(null);
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x000899F8 File Offset: 0x00087BF8
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.CancelAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true), 1f);
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x00089A54 File Offset: 0x00087C54
	public void SetMovable(Movable movable)
	{
		if (this.fetchChore == null)
		{
			this.fetchChore = new MovePickupableChore(this, movable.gameObject, new Action<Chore>(this.OnChoreEnd));
		}
		if (this.movables.Find((Ref<Movable> move) => move.Get() == movable) == null)
		{
			this.movables.Add(new Ref<Movable>(movable));
		}
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x00089AC8 File Offset: 0x00087CC8
	public void OnChoreEnd(Chore chore)
	{
		GameObject nextTarget = this.GetNextTarget();
		if (nextTarget == null)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x00089B0A File Offset: 0x00087D0A
	public bool IsDeliveryComplete()
	{
		this.ValidateMovables();
		return this.movables.Count <= 0;
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x00089B24 File Offset: 0x00087D24
	public void RemoveMovable(Movable moved)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i].Get() == null || this.movables[i].Get() == moved)
			{
				this.movables.RemoveAt(i);
			}
		}
		if (this.movables.Count <= 0)
		{
			this.OnCancel(null);
		}
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x00089B9C File Offset: 0x00087D9C
	public GameObject GetNextTarget()
	{
		this.ValidateMovables();
		if (this.movables.Count > 0)
		{
			return this.movables[0].Get().gameObject;
		}
		return null;
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x00089BCC File Offset: 0x00087DCC
	private void ValidateMovables()
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i] == null)
			{
				this.movables.RemoveAt(i);
			}
			else
			{
				Movable movable = this.movables[i].Get();
				if (movable != null && Grid.PosToCell(movable) == Grid.PosToCell(this))
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x04000E6C RID: 3692
	[Serialize]
	private List<Ref<Movable>> movables = new List<Ref<Movable>>();

	// Token: 0x04000E6D RID: 3693
	private MovePickupableChore fetchChore;
}
