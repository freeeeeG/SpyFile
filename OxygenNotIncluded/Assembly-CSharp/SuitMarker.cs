using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200069B RID: 1691
[AddComponentMenu("KMonoBehaviour/scripts/SuitMarker")]
public class SuitMarker : KMonoBehaviour
{
	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06002D85 RID: 11653 RVA: 0x000F193C File Offset: 0x000EFB3C
	// (set) Token: 0x06002D86 RID: 11654 RVA: 0x000F195C File Offset: 0x000EFB5C
	private bool OnlyTraverseIfUnequipAvailable
	{
		get
		{
			DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == (this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) > (Grid.SuitMarker.Flags)0);
			return this.onlyTraverseIfUnequipAvailable;
		}
		set
		{
			this.onlyTraverseIfUnequipAvailable = value;
			this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06002D87 RID: 11655 RVA: 0x000F1972 File Offset: 0x000EFB72
	// (set) Token: 0x06002D88 RID: 11656 RVA: 0x000F197F File Offset: 0x000EFB7F
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06002D89 RID: 11657 RVA: 0x000F1989 File Offset: 0x000EFB89
	// (set) Token: 0x06002D8A RID: 11658 RVA: 0x000F1996 File Offset: 0x000EFB96
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x000F19A0 File Offset: 0x000EFBA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
		global::Debug.Assert(this.interactAnim != null, "interactAnim is null");
		base.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
		this.CreateNewEquipReactable();
		this.CreateNewUnequipReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		base.GetComponent<KAnimControllerBase>().Play("no_suit", KAnim.PlayMode.Once, 1f, 0f);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
		this.RefreshTraverseIfUnequipStatusItem();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x000F1A9C File Offset: 0x000EFC9C
	private void CreateNewEquipReactable()
	{
		this.equipReactable = new SuitMarker.EquipSuitReactable(this);
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x000F1AAA File Offset: 0x000EFCAA
	private void CreateNewUnequipReactable()
	{
		this.unequipReactable = new SuitMarker.UnequipSuitReactable(this);
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x000F1AB8 File Offset: 0x000EFCB8
	public void GetAttachedLockers(List<SuitLocker> suit_lockers)
	{
		int num = this.isRotated ? 1 : -1;
		int num2 = 1;
		for (;;)
		{
			int num3 = Grid.OffsetCell(this.cell, num2 * num, 0);
			GameObject gameObject = Grid.Objects[num3, 1];
			if (gameObject == null)
			{
				break;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!(component == null))
			{
				if (!component.IsAnyPrefabID(this.LockerTags))
				{
					break;
				}
				SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
				if (component2 == null)
				{
					break;
				}
				Operational component3 = gameObject.GetComponent<Operational>();
				if ((!(component3 != null) || component3.GetFlag(BuildingEnabledButton.EnabledFlag)) && !suit_lockers.Contains(component2))
				{
					suit_lockers.Add(component2);
				}
			}
			num2++;
		}
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x000F1B68 File Offset: 0x000EFD68
	public static bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell, Grid.SuitMarker.Flags flags)
	{
		return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags)0);
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x000F1B80 File Offset: 0x000EFD80
	public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell)
	{
		return SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x000F1B90 File Offset: 0x000EFD90
	private void Update()
	{
		ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
		this.GetAttachedLockers(pooledList);
		int num = 0;
		int num2 = 0;
		KPrefabID x = null;
		foreach (SuitLocker suitLocker in pooledList)
		{
			if (suitLocker.CanDropOffSuit())
			{
				num++;
			}
			if (suitLocker.GetPartiallyChargedOutfit() != null)
			{
				num2++;
			}
			if (x == null)
			{
				x = suitLocker.GetStoredOutfit();
			}
		}
		pooledList.Recycle();
		bool flag = x != null;
		if (flag != this.hasAvailableSuit)
		{
			base.GetComponent<KAnimControllerBase>().Play(flag ? "off" : "no_suit", KAnim.PlayMode.Once, 1f, 0f);
			this.hasAvailableSuit = flag;
		}
		Grid.UpdateSuitMarker(this.cell, num2, num, this.gridFlags, this.PathFlag);
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x000F1C84 File Offset: 0x000EFE84
	private void RefreshTraverseIfUnequipStatusItem()
	{
		if (this.OnlyTraverseIfUnequipAvailable)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, false);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, null);
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x000F1D0A File Offset: 0x000EFF0A
	private void OnEnableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = true;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x000F1D19 File Offset: 0x000EFF19
	private void OnDisableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = false;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x000F1D28 File Offset: 0x000EFF28
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x000F1D4C File Offset: 0x000EFF4C
	private void OnOperationalChanged(bool isOperational)
	{
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		this.isOperational = isOperational;
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x000F1D70 File Offset: 0x000EFF70
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = (!this.OnlyTraverseIfUnequipAvailable) ? new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x000F1E0C File Offset: 0x000F000C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.equipReactable != null)
		{
			this.equipReactable.Cleanup();
		}
		if (this.unequipReactable != null)
		{
			this.unequipReactable.Cleanup();
		}
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x04001AE2 RID: 6882
	[MyCmpGet]
	private Building building;

	// Token: 0x04001AE3 RID: 6883
	private SuitMarker.SuitMarkerReactable equipReactable;

	// Token: 0x04001AE4 RID: 6884
	private SuitMarker.SuitMarkerReactable unequipReactable;

	// Token: 0x04001AE5 RID: 6885
	private bool hasAvailableSuit;

	// Token: 0x04001AE6 RID: 6886
	[Serialize]
	private bool onlyTraverseIfUnequipAvailable;

	// Token: 0x04001AE7 RID: 6887
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x04001AE8 RID: 6888
	private int cell;

	// Token: 0x04001AE9 RID: 6889
	public Tag[] LockerTags;

	// Token: 0x04001AEA RID: 6890
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x04001AEB RID: 6891
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x04001AEC RID: 6892
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001AED RID: 6893
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x04001AEE RID: 6894
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x020013C5 RID: 5061
	private class EquipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x06008221 RID: 33313 RVA: 0x002F884B File Offset: 0x002F6A4B
		public EquipSuitReactable(SuitMarker marker) : base("EquipSuitReactable", marker)
		{
		}

		// Token: 0x06008222 RID: 33314 RVA: 0x002F885E File Offset: 0x002F6A5E
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			return !newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x06008223 RID: 33315 RVA: 0x002F888E File Offset: 0x002F6A8E
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewEquipReactable();
		}

		// Token: 0x06008224 RID: 33316 RVA: 0x002F88A4 File Offset: 0x002F6AA4
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return this.IsRocketDoorExitEquip(newReactor, transition) || flag == this.suitMarker.isRotated;
		}

		// Token: 0x06008225 RID: 33317 RVA: 0x002F88DC File Offset: 0x002F6ADC
		private bool IsRocketDoorExitEquip(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.end != NavType.Teleport && transition.start != NavType.Teleport;
			return transition.navGridTransition.x == 0 && new_reactor.GetMyWorld().IsModuleInterior && !flag;
		}

		// Token: 0x06008226 RID: 33318 RVA: 0x002F8924 File Offset: 0x002F6B24
		protected override void Run()
		{
			ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
			this.suitMarker.GetAttachedLockers(pooledList);
			SuitLocker suitLocker = null;
			for (int i = 0; i < pooledList.Count; i++)
			{
				float suitScore = pooledList[i].GetSuitScore();
				if (suitScore >= 1f)
				{
					suitLocker = pooledList[i];
					break;
				}
				if (suitLocker == null || suitScore > suitLocker.GetSuitScore())
				{
					suitLocker = pooledList[i];
				}
			}
			pooledList.Recycle();
			if (suitLocker != null)
			{
				Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
				SuitWearer.Instance smi = this.reactor.GetSMI<SuitWearer.Instance>();
				suitLocker.EquipTo(equipment);
				smi.UnreserveSuits();
				this.suitMarker.Update();
			}
		}
	}

	// Token: 0x020013C6 RID: 5062
	private class UnequipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x06008227 RID: 33319 RVA: 0x002F89D3 File Offset: 0x002F6BD3
		public UnequipSuitReactable(SuitMarker marker) : base("UnequipSuitReactable", marker)
		{
		}

		// Token: 0x06008228 RID: 33320 RVA: 0x002F89E8 File Offset: 0x002F6BE8
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			Navigator component = newReactor.GetComponent<Navigator>();
			return newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x06008229 RID: 33321 RVA: 0x002F8A50 File Offset: 0x002F6C50
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewUnequipReactable();
		}

		// Token: 0x0600822A RID: 33322 RVA: 0x002F8A64 File Offset: 0x002F6C64
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return transition.navGridTransition.x != 0 && flag != this.suitMarker.isRotated;
		}

		// Token: 0x0600822B RID: 33323 RVA: 0x002F8AA0 File Offset: 0x002F6CA0
		protected override void Run()
		{
			Navigator component = this.reactor.GetComponent<Navigator>();
			Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
			if (component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None)
			{
				ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
				this.suitMarker.GetAttachedLockers(pooledList);
				SuitLocker suitLocker = null;
				int num = 0;
				while (suitLocker == null && num < pooledList.Count)
				{
					if (pooledList[num].CanDropOffSuit())
					{
						suitLocker = pooledList[num];
					}
					num++;
				}
				pooledList.Recycle();
				if (suitLocker != null)
				{
					suitLocker.UnequipFrom(equipment);
					component.GetSMI<SuitWearer.Instance>().UnreserveSuits();
					this.suitMarker.Update();
					return;
				}
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable != null)
			{
				assignable.Unassign();
				Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
				assignable.GetComponent<Notifier>().Add(notification, "");
			}
		}
	}

	// Token: 0x020013C7 RID: 5063
	private abstract class SuitMarkerReactable : Reactable
	{
		// Token: 0x0600822C RID: 33324 RVA: 0x002F8BE0 File Offset: 0x002F6DE0
		public SuitMarkerReactable(HashedString id, SuitMarker suit_marker) : base(suit_marker.gameObject, id, Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.suitMarker = suit_marker;
		}

		// Token: 0x0600822D RID: 33325 RVA: 0x002F8C29 File Offset: 0x002F6E29
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.suitMarker == null)
			{
				base.Cleanup();
				return false;
			}
			return this.suitMarker.isOperational && this.MovingTheRightWay(new_reactor, transition);
		}

		// Token: 0x0600822E RID: 33326 RVA: 0x002F8C68 File Offset: 0x002F6E68
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.suitMarker.HasTag(GameTags.JetSuitBlocker))
			{
				KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
				component2.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600822F RID: 33327 RVA: 0x002F8D60 File Offset: 0x002F6F60
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.suitMarker)
			{
				facing.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				if (this.reactor != null && this.suitMarker != null)
				{
					this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
					this.Run();
				}
				base.Cleanup();
			}
		}

		// Token: 0x06008230 RID: 33328 RVA: 0x002F8E0D File Offset: 0x002F700D
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
			}
		}

		// Token: 0x06008231 RID: 33329 RVA: 0x002F8E38 File Offset: 0x002F7038
		protected override void InternalCleanup()
		{
		}

		// Token: 0x06008232 RID: 33330
		protected abstract bool MovingTheRightWay(GameObject reactor, Navigator.ActiveTransition transition);

		// Token: 0x06008233 RID: 33331
		protected abstract void Run();

		// Token: 0x0400635A RID: 25434
		protected SuitMarker suitMarker;

		// Token: 0x0400635B RID: 25435
		protected float startTime;
	}
}
