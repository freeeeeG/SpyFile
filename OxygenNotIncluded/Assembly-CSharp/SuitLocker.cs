using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200069A RID: 1690
public class SuitLocker : StateMachineComponent<SuitLocker.StatesInstance>
{
	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06002D67 RID: 11623 RVA: 0x000F0DD8 File Offset: 0x000EEFD8
	public float OxygenAvailable
	{
		get
		{
			KPrefabID storedOutfit = this.GetStoredOutfit();
			if (storedOutfit == null)
			{
				return 0f;
			}
			return storedOutfit.GetComponent<SuitTank>().PercentFull();
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06002D68 RID: 11624 RVA: 0x000F0E08 File Offset: 0x000EF008
	public float BatteryAvailable
	{
		get
		{
			KPrefabID storedOutfit = this.GetStoredOutfit();
			if (storedOutfit == null)
			{
				return 0f;
			}
			return storedOutfit.GetComponent<LeadSuitTank>().batteryCharge;
		}
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x000F0E38 File Offset: 0x000EF038
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x000F0EBC File Offset: 0x000EF0BC
	public KPrefabID GetStoredOutfit()
	{
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!(component == null) && component.IsAnyPrefabID(this.OutfitTags))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x000F0F3C File Offset: 0x000EF13C
	public float GetSuitScore()
	{
		float num = -1f;
		KPrefabID partiallyChargedOutfit = this.GetPartiallyChargedOutfit();
		if (partiallyChargedOutfit)
		{
			num = partiallyChargedOutfit.GetComponent<SuitTank>().PercentFull();
			JetSuitTank component = partiallyChargedOutfit.GetComponent<JetSuitTank>();
			if (component && component.PercentFull() < num)
			{
				num = component.PercentFull();
			}
		}
		return num;
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x000F0F8C File Offset: 0x000EF18C
	public KPrefabID GetPartiallyChargedOutfit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!storedOutfit)
		{
			return null;
		}
		if (storedOutfit.GetComponent<SuitTank>().PercentFull() < TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
		{
			return null;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		if (component && component.PercentFull() < TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
		{
			return null;
		}
		return storedOutfit;
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x000F0FE0 File Offset: 0x000EF1E0
	public KPrefabID GetFullyChargedOutfit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!storedOutfit)
		{
			return null;
		}
		if (!storedOutfit.GetComponent<SuitTank>().IsFull())
		{
			return null;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		if (component && !component.IsFull())
		{
			return null;
		}
		return storedOutfit;
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x000F1028 File Offset: 0x000EF228
	private void CreateFetchChore()
	{
		this.fetchChore = new FetchChore(Db.Get().ChoreTypes.EquipmentFetch, base.GetComponent<Storage>(), 1f, new HashSet<Tag>(this.OutfitTags), FetchChore.MatchCriteria.MatchID, Tag.Invalid, new Tag[]
		{
			GameTags.Assigned
		}, null, true, null, null, null, Operational.State.None, 0);
		this.fetchChore.allowMultifetch = false;
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x000F1090 File Offset: 0x000EF290
	private void CancelFetchChore()
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("SuitLocker.CancelFetchChore");
			this.fetchChore = null;
		}
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x000F10B4 File Offset: 0x000EF2B4
	public bool HasOxygen()
	{
		GameObject oxygen = this.GetOxygen();
		return oxygen != null && oxygen.GetComponent<PrimaryElement>().Mass > 0f;
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x000F10E8 File Offset: 0x000EF2E8
	private void RefreshMeter()
	{
		GameObject oxygen = this.GetOxygen();
		float num = 0f;
		if (oxygen != null)
		{
			num = oxygen.GetComponent<PrimaryElement>().Mass / base.GetComponent<ConduitConsumer>().capacityKG;
			num = Math.Min(num, 1f);
		}
		this.meter.SetPositionPercent(num);
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x000F113C File Offset: 0x000EF33C
	public bool IsSuitFullyCharged()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!(storedOutfit != null))
		{
			return false;
		}
		SuitTank component = storedOutfit.GetComponent<SuitTank>();
		if (component != null && component.PercentFull() < 1f)
		{
			return false;
		}
		JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
		if (component2 != null && component2.PercentFull() < 1f)
		{
			return false;
		}
		LeadSuitTank leadSuitTank = (storedOutfit != null) ? storedOutfit.GetComponent<LeadSuitTank>() : null;
		return !(leadSuitTank != null) || leadSuitTank.PercentFull() >= 1f;
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x000F11C8 File Offset: 0x000EF3C8
	public bool IsOxygenTankFull()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= 1f;
		}
		return false;
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x000F1209 File Offset: 0x000EF409
	private void OnRequestOutfit()
	{
		base.smi.sm.isWaitingForSuit.Set(true, base.smi, false);
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x000F1229 File Offset: 0x000EF429
	private void OnCancelRequest()
	{
		base.smi.sm.isWaitingForSuit.Set(false, base.smi, false);
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x000F124C File Offset: 0x000EF44C
	public void DropSuit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		base.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x000F1280 File Offset: 0x000EF480
	public void EquipTo(Equipment equipment)
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		base.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
		Prioritizable component = storedOutfit.GetComponent<Prioritizable>();
		PrioritySetting masterPriority = component.GetMasterPriority();
		PrioritySetting masterPriority2 = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		if (component != null && component.GetMasterPriority().priority_class == PriorityScreen.PriorityClass.topPriority)
		{
			component.SetMasterPriority(masterPriority2);
		}
		storedOutfit.GetComponent<Equippable>().Assign(equipment.GetComponent<IAssignableIdentity>());
		storedOutfit.GetComponent<EquippableWorkable>().CancelChore("Manual equip");
		if (component != null && component.GetMasterPriority() != masterPriority)
		{
			component.SetMasterPriority(masterPriority);
		}
		equipment.Equip(storedOutfit.GetComponent<Equippable>());
		this.returnSuitWorkable.CreateChore();
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x000F133C File Offset: 0x000EF53C
	public void UnequipFrom(Equipment equipment)
	{
		Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
		assignable.Unassign();
		Durability component = assignable.GetComponent<Durability>();
		if (component != null && component.IsWornOut())
		{
			this.ConfigRequestSuit();
			return;
		}
		base.GetComponent<Storage>().Store(assignable.gameObject, false, false, true, false);
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x000F139A File Offset: 0x000EF59A
	public void ConfigRequestSuit()
	{
		base.smi.sm.isConfigured.Set(true, base.smi, false);
		base.smi.sm.isWaitingForSuit.Set(true, base.smi, false);
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x000F13D8 File Offset: 0x000EF5D8
	public void ConfigNoSuit()
	{
		base.smi.sm.isConfigured.Set(true, base.smi, false);
		base.smi.sm.isWaitingForSuit.Set(false, base.smi, false);
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x000F1418 File Offset: 0x000EF618
	public bool CanDropOffSuit()
	{
		return base.smi.sm.isConfigured.Get(base.smi) && !base.smi.sm.isWaitingForSuit.Get(base.smi) && this.GetStoredOutfit() == null;
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x000F146D File Offset: 0x000EF66D
	private GameObject GetOxygen()
	{
		return base.GetComponent<Storage>().FindFirst(GameTags.Oxygen);
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x000F1480 File Offset: 0x000EF680
	private void ChargeSuit(float dt)
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		GameObject oxygen = this.GetOxygen();
		if (oxygen == null)
		{
			return;
		}
		SuitTank component = storedOutfit.GetComponent<SuitTank>();
		float num = component.capacity * 15f * dt / 600f;
		num = Mathf.Min(num, component.capacity - component.GetTankAmount());
		num = Mathf.Min(oxygen.GetComponent<PrimaryElement>().Mass, num);
		if (num > 0f)
		{
			base.GetComponent<Storage>().Transfer(component.storage, component.elementTag, num, false, true);
		}
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x000F1514 File Offset: 0x000EF714
	public void SetSuitMarker(SuitMarker suit_marker)
	{
		SuitLocker.SuitMarkerState suitMarkerState = SuitLocker.SuitMarkerState.HasMarker;
		if (suit_marker == null)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.NoMarker;
		}
		else if (suit_marker.transform.GetPosition().x > base.transform.GetPosition().x && suit_marker.GetComponent<Rotatable>().IsRotated)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
		}
		else if (suit_marker.transform.GetPosition().x < base.transform.GetPosition().x && !suit_marker.GetComponent<Rotatable>().IsRotated)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
		}
		else if (!suit_marker.GetComponent<Operational>().IsOperational)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.NotOperational;
		}
		if (suitMarkerState != this.suitMarkerState)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, false);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, false);
			switch (suitMarkerState)
			{
			case SuitLocker.SuitMarkerState.NoMarker:
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, null);
				break;
			case SuitLocker.SuitMarkerState.WrongSide:
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, null);
				break;
			}
			this.suitMarkerState = suitMarkerState;
		}
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x000F163E File Offset: 0x000EF83E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x000F165C File Offset: 0x000EF85C
	private static void GatherSuitBuildings(int cell, int dir, List<SuitLocker.SuitLockerEntry> suit_lockers, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		int num = dir;
		for (;;)
		{
			int cell2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(cell2) && !SuitLocker.GatherSuitBuildingsOnCell(cell2, suit_lockers, suit_markers))
			{
				break;
			}
			num += dir;
		}
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x000F168C File Offset: 0x000EF88C
	private static bool GatherSuitBuildingsOnCell(int cell, List<SuitLocker.SuitLockerEntry> suit_lockers, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return false;
		}
		SuitMarker component = gameObject.GetComponent<SuitMarker>();
		if (component != null)
		{
			suit_markers.Add(new SuitLocker.SuitMarkerEntry
			{
				suitMarker = component,
				cell = cell
			});
			return true;
		}
		SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
		if (component2 != null)
		{
			suit_lockers.Add(new SuitLocker.SuitLockerEntry
			{
				suitLocker = component2,
				cell = cell
			});
			return true;
		}
		return false;
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x000F1718 File Offset: 0x000EF918
	private static SuitMarker FindSuitMarker(int cell, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		foreach (SuitLocker.SuitMarkerEntry suitMarkerEntry in suit_markers)
		{
			if (suitMarkerEntry.cell == cell)
			{
				return suitMarkerEntry.suitMarker;
			}
		}
		return null;
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x000F1780 File Offset: 0x000EF980
	public static void UpdateSuitMarkerStates(int cell, GameObject self)
	{
		ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
		ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList pooledList2 = ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.Allocate();
		if (self != null)
		{
			SuitLocker component = self.GetComponent<SuitLocker>();
			if (component != null)
			{
				pooledList.Add(new SuitLocker.SuitLockerEntry
				{
					suitLocker = component,
					cell = cell
				});
			}
			SuitMarker component2 = self.GetComponent<SuitMarker>();
			if (component2 != null)
			{
				pooledList2.Add(new SuitLocker.SuitMarkerEntry
				{
					suitMarker = component2,
					cell = cell
				});
			}
		}
		SuitLocker.GatherSuitBuildings(cell, 1, pooledList, pooledList2);
		SuitLocker.GatherSuitBuildings(cell, -1, pooledList, pooledList2);
		pooledList.Sort(SuitLocker.SuitLockerEntry.comparer);
		for (int i = 0; i < pooledList.Count; i++)
		{
			SuitLocker.SuitLockerEntry suitLockerEntry = pooledList[i];
			SuitLocker.SuitLockerEntry suitLockerEntry2 = suitLockerEntry;
			ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList3 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
			pooledList3.Add(suitLockerEntry);
			for (int j = i + 1; j < pooledList.Count; j++)
			{
				SuitLocker.SuitLockerEntry suitLockerEntry3 = pooledList[j];
				if (Grid.CellRight(suitLockerEntry2.cell) != suitLockerEntry3.cell)
				{
					break;
				}
				i++;
				suitLockerEntry2 = suitLockerEntry3;
				pooledList3.Add(suitLockerEntry3);
			}
			int cell2 = Grid.CellLeft(suitLockerEntry.cell);
			int cell3 = Grid.CellRight(suitLockerEntry2.cell);
			SuitMarker suitMarker = SuitLocker.FindSuitMarker(cell2, pooledList2);
			if (suitMarker == null)
			{
				suitMarker = SuitLocker.FindSuitMarker(cell3, pooledList2);
			}
			foreach (SuitLocker.SuitLockerEntry suitLockerEntry4 in pooledList3)
			{
				suitLockerEntry4.suitLocker.SetSuitMarker(suitMarker);
			}
			pooledList3.Recycle();
		}
		pooledList.Recycle();
		pooledList2.Recycle();
	}

	// Token: 0x04001ADC RID: 6876
	[MyCmpGet]
	private Building building;

	// Token: 0x04001ADD RID: 6877
	public Tag[] OutfitTags;

	// Token: 0x04001ADE RID: 6878
	private FetchChore fetchChore;

	// Token: 0x04001ADF RID: 6879
	[MyCmpAdd]
	public SuitLocker.ReturnSuitWorkable returnSuitWorkable;

	// Token: 0x04001AE0 RID: 6880
	private MeterController meter;

	// Token: 0x04001AE1 RID: 6881
	private SuitLocker.SuitMarkerState suitMarkerState;

	// Token: 0x020013BF RID: 5055
	[AddComponentMenu("KMonoBehaviour/Workable/ReturnSuitWorkable")]
	public class ReturnSuitWorkable : Workable
	{
		// Token: 0x06008212 RID: 33298 RVA: 0x002F7D4B File Offset: 0x002F5F4B
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.workTime = 0.25f;
			this.synchronizeAnims = false;
		}

		// Token: 0x06008213 RID: 33299 RVA: 0x002F7D6C File Offset: 0x002F5F6C
		public void CreateChore()
		{
			if (this.urgentChore == null)
			{
				SuitLocker component = base.GetComponent<SuitLocker>();
				this.urgentChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitUrgent, this, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
				this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingUrgent, null);
				this.urgentChore.AddPrecondition(this.HasSuitMarker, component);
				this.urgentChore.AddPrecondition(this.SuitTypeMatchesLocker, component);
				this.idleChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitIdle, this, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.idle, 5, false, false);
				this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingIdle, null);
				this.idleChore.AddPrecondition(this.HasSuitMarker, component);
				this.idleChore.AddPrecondition(this.SuitTypeMatchesLocker, component);
			}
		}

		// Token: 0x06008214 RID: 33300 RVA: 0x002F7E4D File Offset: 0x002F604D
		public void CancelChore()
		{
			if (this.urgentChore != null)
			{
				this.urgentChore.Cancel("ReturnSuitWorkable.CancelChore");
				this.urgentChore = null;
			}
			if (this.idleChore != null)
			{
				this.idleChore.Cancel("ReturnSuitWorkable.CancelChore");
				this.idleChore = null;
			}
		}

		// Token: 0x06008215 RID: 33301 RVA: 0x002F7E8D File Offset: 0x002F608D
		protected override void OnStartWork(Worker worker)
		{
			base.ShowProgressBar(false);
		}

		// Token: 0x06008216 RID: 33302 RVA: 0x002F7E96 File Offset: 0x002F6096
		protected override bool OnWorkTick(Worker worker, float dt)
		{
			return true;
		}

		// Token: 0x06008217 RID: 33303 RVA: 0x002F7E9C File Offset: 0x002F609C
		protected override void OnCompleteWork(Worker worker)
		{
			Equipment equipment = worker.GetComponent<MinionIdentity>().GetEquipment();
			if (equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit))
			{
				if (base.GetComponent<SuitLocker>().CanDropOffSuit())
				{
					base.GetComponent<SuitLocker>().UnequipFrom(equipment);
				}
				else
				{
					equipment.GetAssignable(Db.Get().AssignableSlots.Suit).Unassign();
				}
			}
			if (this.urgentChore != null)
			{
				this.CancelChore();
				this.CreateChore();
			}
		}

		// Token: 0x06008218 RID: 33304 RVA: 0x002F7F15 File Offset: 0x002F6115
		public override HashedString[] GetWorkAnims(Worker worker)
		{
			return new HashedString[]
			{
				new HashedString("none")
			};
		}

		// Token: 0x06008219 RID: 33305 RVA: 0x002F7F30 File Offset: 0x002F6130
		public ReturnSuitWorkable()
		{
			Chore.Precondition precondition = default(Chore.Precondition);
			precondition.id = "IsValid";
			precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER;
			precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return ((SuitLocker)data).suitMarkerState == SuitLocker.SuitMarkerState.HasMarker;
			};
			this.HasSuitMarker = precondition;
			precondition = default(Chore.Precondition);
			precondition.id = "IsValid";
			precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER;
			precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				SuitLocker suitLocker = (SuitLocker)data;
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				return !(slot.assignable == null) && slot.assignable.GetComponent<KPrefabID>().IsAnyPrefabID(suitLocker.OutfitTags);
			};
			this.SuitTypeMatchesLocker = precondition;
			base..ctor();
		}

		// Token: 0x04006343 RID: 25411
		public static readonly Chore.Precondition DoesSuitNeedRechargingUrgent = new Chore.Precondition
		{
			id = "DoesSuitNeedRechargingUrgent",
			description = DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_URGENT,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				if (slot.assignable == null)
				{
					return false;
				}
				Equippable component = slot.assignable.GetComponent<Equippable>();
				if (component == null || !component.isEquipped)
				{
					return false;
				}
				SuitTank component2 = slot.assignable.GetComponent<SuitTank>();
				if (component2 != null && component2.NeedsRecharging())
				{
					return true;
				}
				JetSuitTank component3 = slot.assignable.GetComponent<JetSuitTank>();
				if (component3 != null && component3.NeedsRecharging())
				{
					return true;
				}
				LeadSuitTank component4 = slot.assignable.GetComponent<LeadSuitTank>();
				return component4 != null && component4.NeedsRecharging();
			}
		};

		// Token: 0x04006344 RID: 25412
		public static readonly Chore.Precondition DoesSuitNeedRechargingIdle = new Chore.Precondition
		{
			id = "DoesSuitNeedRechargingIdle",
			description = DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_IDLE,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				if (slot.assignable == null)
				{
					return false;
				}
				Equippable component = slot.assignable.GetComponent<Equippable>();
				return !(component == null) && component.isEquipped && (slot.assignable.GetComponent<SuitTank>() != null || slot.assignable.GetComponent<JetSuitTank>() != null || slot.assignable.GetComponent<LeadSuitTank>() != null);
			}
		};

		// Token: 0x04006345 RID: 25413
		public Chore.Precondition HasSuitMarker;

		// Token: 0x04006346 RID: 25414
		public Chore.Precondition SuitTypeMatchesLocker;

		// Token: 0x04006347 RID: 25415
		private WorkChore<SuitLocker.ReturnSuitWorkable> urgentChore;

		// Token: 0x04006348 RID: 25416
		private WorkChore<SuitLocker.ReturnSuitWorkable> idleChore;
	}

	// Token: 0x020013C0 RID: 5056
	public class StatesInstance : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.GameInstance
	{
		// Token: 0x0600821B RID: 33307 RVA: 0x002F8079 File Offset: 0x002F6279
		public StatesInstance(SuitLocker suit_locker) : base(suit_locker)
		{
		}
	}

	// Token: 0x020013C1 RID: 5057
	public class States : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker>
	{
		// Token: 0x0600821C RID: 33308 RVA: 0x002F8084 File Offset: 0x002F6284
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(SuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.DefaultState(this.empty.notconfigured).EventTransition(GameHashes.OnStorageChange, this.charging, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null).ParamTransition<bool>(this.isWaitingForSuit, this.waitingforsuit, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).Enter("CreateReturnSuitChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.returnSuitWorkable.CreateChore();
			}).RefreshUserMenuOnEnter().Exit("CancelReturnSuitChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.returnSuitWorkable.CancelChore();
			}).PlayAnim("no_suit_pre").QueueAnim("no_suit", false, null);
			this.empty.notconfigured.ParamTransition<bool>(this.isConfigured, this.empty.configured, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.TOOLTIP, "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
			this.empty.configured.RefreshUserMenuOnEnter().ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.READY.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
			this.waitingforsuit.EventTransition(GameHashes.OnStorageChange, this.charging, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null).Enter("CreateFetchChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.CreateFetchChore();
			}).ParamTransition<bool>(this.isWaitingForSuit, this.empty, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsFalse).RefreshUserMenuOnEnter().PlayAnim("no_suit_pst").QueueAnim("awaiting_suit", false, null).Exit("ClearIsWaitingForSuit", delegate(SuitLocker.StatesInstance smi)
			{
				this.isWaitingForSuit.Set(false, smi, false);
			}).Exit("CancelFetchChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.CancelFetchChore();
			}).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
			this.charging.DefaultState(this.charging.pre).RefreshUserMenuOnEnter().EventTransition(GameHashes.OnStorageChange, this.empty, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit().gameObject).Enter(delegate(SuitLocker.StatesInstance smi)
			{
				KAnim.Build.Symbol symbol = smi.master.GetStoredOutfit().GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("suit");
				SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
				component.TryRemoveSymbolOverride("suit_swap", 0);
				if (symbol != null)
				{
					component.AddSymbolOverride("suit_swap", symbol, 0);
				}
			});
			this.charging.pre.Enter(delegate(SuitLocker.StatesInstance smi)
			{
				if (smi.master.IsSuitFullyCharged())
				{
					smi.GoTo(this.suitfullycharged);
					return;
				}
				smi.GetComponent<KBatchedAnimController>().Play("no_suit_pst", KAnim.PlayMode.Once, 1f, 0f);
				smi.GetComponent<KBatchedAnimController>().Queue("charging_pre", KAnim.PlayMode.Once, 1f, 0f);
			}).OnAnimQueueComplete(this.charging.operational);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nooxygen, (SuitLocker.StatesInstance smi) => !smi.master.HasOxygen(), UpdateRate.SIM_200ms).PlayAnim("charging_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).Update("ChargeSuit", delegate(SuitLocker.StatesInstance smi, float dt)
			{
				smi.master.ChargeSuit(dt);
			}, UpdateRate.SIM_200ms, false).Exit("ClearActive", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			}).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
			this.charging.nooxygen.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (SuitLocker.StatesInstance smi) => smi.master.HasOxygen(), UpdateRate.SIM_200ms).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).PlayAnim("no_o2_loop", KAnim.PlayMode.Loop).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.TOOLTIP, "status_item_suit_locker_no_oxygen", StatusItem.IconType.Custom, NotificationType.BadMinor, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false).PlayAnim("not_charging_loop", KAnim.PlayMode.Loop).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
			this.charging.pst.PlayAnim("charging_pst").OnAnimQueueComplete(this.suitfullycharged);
			this.suitfullycharged.EventTransition(GameHashes.OnStorageChange, this.empty, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).PlayAnim("has_suit").RefreshUserMenuOnEnter().ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit().gameObject).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		}

		// Token: 0x04006349 RID: 25417
		public SuitLocker.States.EmptyStates empty;

		// Token: 0x0400634A RID: 25418
		public SuitLocker.States.ChargingStates charging;

		// Token: 0x0400634B RID: 25419
		public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State waitingforsuit;

		// Token: 0x0400634C RID: 25420
		public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State suitfullycharged;

		// Token: 0x0400634D RID: 25421
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isWaitingForSuit;

		// Token: 0x0400634E RID: 25422
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isConfigured;

		// Token: 0x0400634F RID: 25423
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter hasSuitMarker;

		// Token: 0x02002130 RID: 8496
		public class ChargingStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
		{
			// Token: 0x040094C1 RID: 38081
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pre;

			// Token: 0x040094C2 RID: 38082
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pst;

			// Token: 0x040094C3 RID: 38083
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State operational;

			// Token: 0x040094C4 RID: 38084
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State nooxygen;

			// Token: 0x040094C5 RID: 38085
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notoperational;
		}

		// Token: 0x02002131 RID: 8497
		public class EmptyStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
		{
			// Token: 0x040094C6 RID: 38086
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State configured;

			// Token: 0x040094C7 RID: 38087
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notconfigured;
		}
	}

	// Token: 0x020013C2 RID: 5058
	private enum SuitMarkerState
	{
		// Token: 0x04006351 RID: 25425
		HasMarker,
		// Token: 0x04006352 RID: 25426
		NoMarker,
		// Token: 0x04006353 RID: 25427
		WrongSide,
		// Token: 0x04006354 RID: 25428
		NotOperational
	}

	// Token: 0x020013C3 RID: 5059
	private struct SuitLockerEntry
	{
		// Token: 0x04006355 RID: 25429
		public SuitLocker suitLocker;

		// Token: 0x04006356 RID: 25430
		public int cell;

		// Token: 0x04006357 RID: 25431
		public static SuitLocker.SuitLockerEntry.Comparer comparer = new SuitLocker.SuitLockerEntry.Comparer();

		// Token: 0x02002133 RID: 8499
		public class Comparer : IComparer<SuitLocker.SuitLockerEntry>
		{
			// Token: 0x0600A94D RID: 43341 RVA: 0x00371E14 File Offset: 0x00370014
			public int Compare(SuitLocker.SuitLockerEntry a, SuitLocker.SuitLockerEntry b)
			{
				return a.cell - b.cell;
			}
		}
	}

	// Token: 0x020013C4 RID: 5060
	private struct SuitMarkerEntry
	{
		// Token: 0x04006358 RID: 25432
		public SuitMarker suitMarker;

		// Token: 0x04006359 RID: 25433
		public int cell;
	}
}
