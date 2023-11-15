using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
public class CreatureDeliveryPoint : StateMachineComponent<CreatureDeliveryPoint.SMInstance>, IUserControlledCapacity
{
	// Token: 0x0600258C RID: 9612 RVA: 0x000CC0F8 File Offset: 0x000CA2F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.fetches = new List<FetchOrder2>();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.GetComponent<Storage>().SetOffsets(this.deliveryOffsets);
		Prioritizable.AddRef(base.gameObject);
		if (CreatureDeliveryPoint.capacityStatusItem == null)
		{
			CreatureDeliveryPoint.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			CreatureDeliveryPoint.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				IUserControlledCapacity userControlledCapacity = (IUserControlledCapacity)data;
				string newValue = Util.FormatWholeNumber(Mathf.Floor(userControlledCapacity.AmountStored));
				string newValue2 = Util.FormatWholeNumber(userControlledCapacity.UserMaxCapacity);
				str = str.Replace("{Stored}", newValue).Replace("{Capacity}", newValue2).Replace("{Units}", userControlledCapacity.CapacityUnits);
				return str;
			};
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, CreatureDeliveryPoint.capacityStatusItem, this);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000CC1D2 File Offset: 0x000CA3D2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.Subscribe<CreatureDeliveryPoint>(-905833192, CreatureDeliveryPoint.OnCopySettingsDelegate);
		base.Subscribe<CreatureDeliveryPoint>(643180843, CreatureDeliveryPoint.RefreshCreatureCountDelegate);
		this.RefreshCreatureCount(null);
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x000CC210 File Offset: 0x000CA410
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		CreatureDeliveryPoint component = gameObject.GetComponent<CreatureDeliveryPoint>();
		if (component == null)
		{
			return;
		}
		this.creatureLimit = component.creatureLimit;
		this.RebalanceFetches();
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x000CC251 File Offset: 0x000CA451
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		this.ClearFetches();
		this.RebalanceFetches();
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x000CC260 File Offset: 0x000CA460
	private void RefreshCreatureCount(object data = null)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this), this.spawnOffset);
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
		int num = this.storedCreatureCount;
		this.storedCreatureCount = 0;
		if (cavityForCell != null)
		{
			foreach (KPrefabID kprefabID in cavityForCell.creatures)
			{
				if (!kprefabID.HasTag(GameTags.Creatures.Bagged) && !kprefabID.HasTag(GameTags.Trapped))
				{
					this.storedCreatureCount++;
				}
			}
		}
		if (this.storedCreatureCount != num)
		{
			this.RebalanceFetches();
		}
	}

	// Token: 0x06002591 RID: 9617 RVA: 0x000CC31C File Offset: 0x000CA51C
	private void ClearFetches()
	{
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			this.fetches[i].Cancel("clearing all fetches");
		}
		this.fetches.Clear();
	}

	// Token: 0x06002592 RID: 9618 RVA: 0x000CC364 File Offset: 0x000CA564
	private void RebalanceFetches()
	{
		HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
		ChoreType creatureFetch = Db.Get().ChoreTypes.CreatureFetch;
		Storage component = base.GetComponent<Storage>();
		int num = this.creatureLimit - this.storedCreatureCount;
		int count = this.fetches.Count;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			if (this.fetches[i].IsComplete())
			{
				this.fetches.RemoveAt(i);
				num2++;
			}
		}
		int num6 = 0;
		for (int j = 0; j < this.fetches.Count; j++)
		{
			if (!this.fetches[j].InProgress)
			{
				num6++;
			}
		}
		if (num6 == 0 && this.fetches.Count < num)
		{
			FetchOrder2 fetchOrder = new FetchOrder2(creatureFetch, tags, FetchChore.MatchCriteria.MatchID, GameTags.Creatures.Deliverable, null, component, 1f, Operational.State.Operational, 0);
			fetchOrder.validateRequiredTagOnTagChange = true;
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchComplete), false, new Action<FetchOrder2, Pickupable>(this.OnFetchBegun));
			this.fetches.Add(fetchOrder);
			num3++;
		}
		int num7 = this.fetches.Count - num;
		for (int k = this.fetches.Count - 1; k >= 0; k--)
		{
			if (num7 <= 0)
			{
				break;
			}
			if (!this.fetches[k].InProgress)
			{
				this.fetches[k].Cancel("fewer creatures in room");
				this.fetches.RemoveAt(k);
				num7--;
				num4++;
			}
		}
		while (num7 > 0 && this.fetches.Count > 0)
		{
			this.fetches[this.fetches.Count - 1].Cancel("fewer creatures in room");
			this.fetches.RemoveAt(this.fetches.Count - 1);
			num7--;
			num5++;
		}
	}

	// Token: 0x06002593 RID: 9619 RVA: 0x000CC568 File Offset: 0x000CA768
	private void OnFetchComplete(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x06002594 RID: 9620 RVA: 0x000CC570 File Offset: 0x000CA770
	private void OnFetchBegun(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x000CC578 File Offset: 0x000CA778
	protected override void OnCleanUp()
	{
		base.smi.StopSM("OnCleanUp");
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.OnCleanUp();
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06002596 RID: 9622 RVA: 0x000CC5B7 File Offset: 0x000CA7B7
	// (set) Token: 0x06002597 RID: 9623 RVA: 0x000CC5C0 File Offset: 0x000CA7C0
	float IUserControlledCapacity.UserMaxCapacity
	{
		get
		{
			return (float)this.creatureLimit;
		}
		set
		{
			this.creatureLimit = Mathf.RoundToInt(value);
			this.RebalanceFetches();
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06002598 RID: 9624 RVA: 0x000CC5D4 File Offset: 0x000CA7D4
	float IUserControlledCapacity.AmountStored
	{
		get
		{
			return (float)this.storedCreatureCount;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06002599 RID: 9625 RVA: 0x000CC5DD File Offset: 0x000CA7DD
	float IUserControlledCapacity.MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x0600259A RID: 9626 RVA: 0x000CC5E4 File Offset: 0x000CA7E4
	float IUserControlledCapacity.MaxCapacity
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x0600259B RID: 9627 RVA: 0x000CC5EB File Offset: 0x000CA7EB
	bool IUserControlledCapacity.WholeValues
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x0600259C RID: 9628 RVA: 0x000CC5EE File Offset: 0x000CA7EE
	LocString IUserControlledCapacity.CapacityUnits
	{
		get
		{
			return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;
		}
	}

	// Token: 0x04001572 RID: 5490
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04001573 RID: 5491
	[Serialize]
	private int creatureLimit = 20;

	// Token: 0x04001574 RID: 5492
	private int storedCreatureCount;

	// Token: 0x04001575 RID: 5493
	public CellOffset[] deliveryOffsets = new CellOffset[1];

	// Token: 0x04001576 RID: 5494
	public CellOffset spawnOffset = new CellOffset(0, 0);

	// Token: 0x04001577 RID: 5495
	private List<FetchOrder2> fetches;

	// Token: 0x04001578 RID: 5496
	private static StatusItem capacityStatusItem;

	// Token: 0x04001579 RID: 5497
	public bool playAnimsOnFetch;

	// Token: 0x0400157A RID: 5498
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400157B RID: 5499
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> RefreshCreatureCountDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.RefreshCreatureCount(data);
	});

	// Token: 0x0200127B RID: 4731
	public class SMInstance : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.GameInstance
	{
		// Token: 0x06007D90 RID: 32144 RVA: 0x002E4B30 File Offset: 0x002E2D30
		public SMInstance(CreatureDeliveryPoint master) : base(master)
		{
		}
	}

	// Token: 0x0200127C RID: 4732
	public class States : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint>
	{
		// Token: 0x06007D91 RID: 32145 RVA: 0x002E4B3C File Offset: 0x002E2D3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waiting;
			this.root.Update("RefreshCreatureCount", delegate(CreatureDeliveryPoint.SMInstance smi, float dt)
			{
				smi.master.RefreshCreatureCount(null);
			}, UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new StateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State.Callback(CreatureDeliveryPoint.States.DropAllCreatures));
			this.waiting.EnterTransition(this.interact_waiting, (CreatureDeliveryPoint.SMInstance smi) => smi.master.playAnimsOnFetch);
			this.interact_waiting.WorkableStartTransition((CreatureDeliveryPoint.SMInstance smi) => smi.master.GetComponent<Storage>(), this.interact_delivery);
			this.interact_delivery.PlayAnim("working_pre").QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.interact_waiting);
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x002E4C24 File Offset: 0x002E2E24
		public static void DropAllCreatures(CreatureDeliveryPoint.SMInstance smi)
		{
			Storage component = smi.master.GetComponent<Storage>();
			if (component.IsEmpty())
			{
				return;
			}
			List<GameObject> items = component.items;
			int count = items.Count;
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), smi.master.spawnOffset), Grid.SceneLayer.Creatures);
			for (int i = count - 1; i >= 0; i--)
			{
				GameObject gameObject = items[i];
				component.Drop(gameObject, true);
				gameObject.transform.SetPosition(position);
				gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
			}
			smi.master.RefreshCreatureCount(null);
		}

		// Token: 0x04005FCE RID: 24526
		public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State waiting;

		// Token: 0x04005FCF RID: 24527
		public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_waiting;

		// Token: 0x04005FD0 RID: 24528
		public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_delivery;
	}
}
