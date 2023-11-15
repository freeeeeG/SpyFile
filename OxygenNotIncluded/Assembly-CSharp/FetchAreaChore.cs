using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x000689D6 File Offset: 0x00066BD6
	public bool IsFetching
	{
		get
		{
			return base.smi.pickingup;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x000689E3 File Offset: 0x00066BE3
	public bool IsDelivering
	{
		get
		{
			return base.smi.delivering;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060013AD RID: 5037 RVA: 0x000689F0 File Offset: 0x00066BF0
	public GameObject GetFetchTarget
	{
		get
		{
			return base.smi.sm.fetchTarget.Get(base.smi);
		}
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x00068A10 File Offset: 0x00066C10
	public FetchAreaChore(Chore.Precondition.Context context) : base(context.chore.choreType, context.consumerState.consumer, context.consumerState.choreProvider, false, null, null, null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new FetchAreaChore.StatesInstance(this, context);
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x00068A78 File Offset: 0x00066C78
	public override void Cleanup()
	{
		base.Cleanup();
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x00068A80 File Offset: 0x00066C80
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.Begin(context);
		base.Begin(context);
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x00068A95 File Offset: 0x00066C95
	protected override void End(string reason)
	{
		base.smi.End();
		base.End(reason);
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x00068AA9 File Offset: 0x00066CA9
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.fetchTarget.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x00068ADC File Offset: 0x00066CDC
	private static bool IsPickupableStillValidForChore(Pickupable pickupable, FetchChore chore)
	{
		KPrefabID component = pickupable.GetComponent<KPrefabID>();
		if ((chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(component.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && !component.HasTag(chore.tagsFirst)))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it is not or does not contain one of these tags: {1}", pickupable, string.Join<Tag>(",", chore.tags)));
			return false;
		}
		if (chore.requiredTag.IsValid && !component.HasTag(chore.requiredTag))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it does not have the required tag: {1}", pickupable, chore.requiredTag));
			return false;
		}
		if (component.HasAnyTags(chore.forbiddenTags))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it has the forbidden tags: {1}", pickupable, string.Join<Tag>(",", chore.forbiddenTags)));
			return false;
		}
		if (component.HasTag(GameTags.MarkedForMove))
		{
			global::Debug.Log(string.Format("Pickupable {0} is marked for move.", pickupable));
			return false;
		}
		return true;
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x00068BCC File Offset: 0x00066DCC
	public static void GatherNearbyFetchChores(FetchChore root_chore, Chore.Precondition.Context context, int x, int y, int radius, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts)
	{
		ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x - radius, y - radius, radius * 2 + 1, radius * 2 + 1, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			(pooledList[i].obj as FetchChore).CollectChoresFromGlobalChoreProvider(context.consumerState, succeeded_contexts, failed_contexts, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x02000FF6 RID: 4086
	public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06007436 RID: 29750 RVA: 0x002C5EDB File Offset: 0x002C40DB
		public Tag RootChore_RequiredTag
		{
			get
			{
				return this.rootChore.requiredTag;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06007437 RID: 29751 RVA: 0x002C5EE8 File Offset: 0x002C40E8
		public bool RootChore_ValidateRequiredTagOnTagChange
		{
			get
			{
				return this.rootChore.validateRequiredTagOnTagChange;
			}
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x002C5EF8 File Offset: 0x002C40F8
		public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context) : base(master)
		{
			this.rootContext = context;
			this.rootChore = (context.chore as FetchChore);
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x002C5F5C File Offset: 0x002C415C
		public void Begin(Chore.Precondition.Context context)
		{
			base.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
			this.chores.Clear();
			this.chores.Add(this.rootChore);
			int x;
			int y;
			Grid.CellToXY(Grid.PosToCell(this.rootChore.destination.transform.GetPosition()), out x, out y);
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList2 = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			if (this.rootChore.allowMultifetch)
			{
				FetchAreaChore.GatherNearbyFetchChores(this.rootChore, context, x, y, 3, pooledList, pooledList2);
			}
			float num = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(context.consumerState.consumer).GetTotalValue());
			Pickupable pickupable = context.data as Pickupable;
			if (pickupable == null)
			{
				global::Debug.Assert(pooledList.Count > 0, "succeeded_contexts was empty");
				FetchChore fetchChore = (FetchChore)pooledList[0].chore;
				global::Debug.Assert(fetchChore != null, "fetch_chore was null");
				DebugUtil.LogWarningArgs(new object[]
				{
					"Missing root_fetchable for FetchAreaChore",
					fetchChore.destination,
					fetchChore.tagsFirst
				});
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
			}
			global::Debug.Assert(pickupable != null, "root_fetchable was null");
			List<Pickupable> list = new List<Pickupable>();
			list.Add(pickupable);
			float num2 = pickupable.UnreservedAmount;
			float minTakeAmount = pickupable.MinTakeAmount;
			int num3 = 0;
			int num4 = 0;
			Grid.CellToXY(Grid.PosToCell(pickupable.transform.GetPosition()), out num3, out num4);
			int num5 = 9;
			num3 -= 3;
			num4 -= 3;
			ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(num3, num4, num5, num5, GameScenePartitioner.Instance.pickupablesLayer, pooledList3);
			Tag prefabTag = pickupable.GetComponent<KPrefabID>().PrefabTag;
			for (int i = 0; i < pooledList3.Count; i++)
			{
				ScenePartitionerEntry scenePartitionerEntry = pooledList3[i];
				if (num2 > num)
				{
					break;
				}
				Pickupable pickupable2 = scenePartitionerEntry.obj as Pickupable;
				KPrefabID component = pickupable2.GetComponent<KPrefabID>();
				if (!(component.PrefabTag != prefabTag) && pickupable2.UnreservedAmount > 0f && (this.rootChore.criteria != FetchChore.MatchCriteria.MatchID || this.rootChore.tags.Contains(component.PrefabTag)) && (this.rootChore.criteria != FetchChore.MatchCriteria.MatchTags || component.HasTag(this.rootChore.tagsFirst)) && (!this.rootChore.requiredTag.IsValid || component.HasTag(this.rootChore.requiredTag)) && !component.HasAnyTags(this.rootChore.forbiddenTags) && !list.Contains(pickupable2) && this.rootContext.consumerState.consumer.CanReach(pickupable2) && !pickupable2.HasTag(GameTags.MarkedForMove))
				{
					float unreservedAmount = pickupable2.UnreservedAmount;
					list.Add(pickupable2);
					num2 += unreservedAmount;
					if (list.Count >= 10)
					{
						break;
					}
				}
			}
			pooledList3.Recycle();
			num2 = Mathf.Min(num, num2);
			if (minTakeAmount > 0f)
			{
				num2 -= num2 % minTakeAmount;
			}
			this.deliveries.Clear();
			float num6 = Mathf.Min(this.rootChore.originalAmount, num2);
			if (minTakeAmount > 0f)
			{
				num6 -= num6 % minTakeAmount;
			}
			this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(this.rootContext, num6, new Action<FetchChore>(this.OnFetchChoreCancelled)));
			float num7 = num6;
			int num8 = 0;
			while (num8 < pooledList.Count && num7 < num2)
			{
				Chore.Precondition.Context context2 = pooledList[num8];
				FetchChore fetchChore2 = context2.chore as FetchChore;
				if (fetchChore2 != this.rootChore && fetchChore2.overrideTarget == null && fetchChore2.driver == null && fetchChore2.tagsHash == this.rootChore.tagsHash && fetchChore2.requiredTag == this.rootChore.requiredTag && fetchChore2.forbidHash == this.rootChore.forbidHash)
				{
					num6 = Mathf.Min(fetchChore2.originalAmount, num2 - num7);
					if (minTakeAmount > 0f)
					{
						num6 -= num6 % minTakeAmount;
					}
					this.chores.Add(fetchChore2);
					this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(context2, num6, new Action<FetchChore>(this.OnFetchChoreCancelled)));
					num7 += num6;
					if (this.deliveries.Count >= 10)
					{
						break;
					}
				}
				num8++;
			}
			num7 = Mathf.Min(num7, num2);
			float num9 = num7;
			this.fetchables.Clear();
			int num10 = 0;
			while (num10 < list.Count && num9 > 0f)
			{
				Pickupable pickupable3 = list[num10];
				num9 -= pickupable3.UnreservedAmount;
				this.fetchables.Add(pickupable3);
				num10++;
			}
			this.fetchAmountRequested = num7;
			this.reservations.Clear();
			pooledList.Recycle();
			pooledList2.Recycle();
		}

		// Token: 0x0600743A RID: 29754 RVA: 0x002C64A4 File Offset: 0x002C46A4
		public void End()
		{
			foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
			{
				delivery.Cleanup();
			}
			this.deliveries.Clear();
		}

		// Token: 0x0600743B RID: 29755 RVA: 0x002C6504 File Offset: 0x002C4704
		public void SetupDelivery()
		{
			if (this.deliveries.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			FetchAreaChore.StatesInstance.Delivery nextDelivery = this.deliveries[0];
			if (FetchAreaChore.StatesInstance.s_transientDeliveryTags.Contains(nextDelivery.chore.requiredTag))
			{
				nextDelivery.chore.requiredTag = Tag.Invalid;
			}
			this.deliverables.RemoveAll(delegate(Pickupable x)
			{
				if (x == null || x.TotalAmount <= 0f)
				{
					return true;
				}
				if (!FetchAreaChore.IsPickupableStillValidForChore(x, nextDelivery.chore))
				{
					global::Debug.LogWarning(string.Format("Removing deliverable {0} for a delivery to {1} which did not request it", x, nextDelivery.chore.destination));
					return true;
				}
				return false;
			});
			if (this.deliverables.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			base.sm.deliveryDestination.Set(nextDelivery.destination, base.smi);
			base.sm.deliveryObject.Set(this.deliverables[0], base.smi);
			if (!(nextDelivery.destination != null))
			{
				base.smi.GoTo(base.sm.delivering.deliverfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.delivering.movetostorage);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.deliveries[0].destination))
			{
				this.GoTo(base.sm.delivering.storing);
				return;
			}
			this.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x0600743C RID: 29756 RVA: 0x002C669C File Offset: 0x002C489C
		public void SetupFetch()
		{
			if (this.reservations.Count <= 0)
			{
				this.GoTo(base.sm.delivering.next);
				return;
			}
			this.SetFetchTarget(this.reservations[0].pickupable);
			base.sm.fetchResultTarget.Set(null, base.smi);
			base.sm.fetchAmount.Set(this.reservations[0].amount, base.smi, false);
			if (!(this.reservations[0].pickupable != null))
			{
				this.GoTo(base.sm.fetching.fetchfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.fetching.movetopickupable);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.reservations[0].pickupable))
			{
				this.GoTo(base.sm.fetching.pickup);
				return;
			}
			this.GoTo(base.sm.fetching.fetchfail);
		}

		// Token: 0x0600743D RID: 29757 RVA: 0x002C67E5 File Offset: 0x002C49E5
		public void SetFetchTarget(Pickupable fetching)
		{
			base.sm.fetchTarget.Set(fetching, base.smi);
			if (fetching != null)
			{
				fetching.Subscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
		}

		// Token: 0x0600743E RID: 29758 RVA: 0x002C6820 File Offset: 0x002C4A20
		public void DeliverFail()
		{
			if (this.deliveries.Count > 0)
			{
				this.deliveries[0].Cleanup();
				this.deliveries.RemoveAt(0);
			}
			this.GoTo(base.sm.delivering.next);
		}

		// Token: 0x0600743F RID: 29759 RVA: 0x002C6874 File Offset: 0x002C4A74
		public void DeliverComplete()
		{
			Pickupable pickupable = base.sm.deliveryObject.Get<Pickupable>(base.smi);
			if (!(pickupable == null) && pickupable.TotalAmount > 0f)
			{
				if (this.deliveries.Count > 0)
				{
					FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
					Chore chore = delivery.chore;
					delivery.Complete(this.deliverables);
					delivery.Cleanup();
					if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
					{
						this.deliveries.RemoveAt(0);
					}
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			if (this.deliveries.Count > 0 && this.deliveries[0].chore.amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				FetchAreaChore.StatesInstance.Delivery delivery2 = this.deliveries[0];
				Chore chore2 = delivery2.chore;
				delivery2.Complete(this.deliverables);
				delivery2.Cleanup();
				if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore2)
				{
					this.deliveries.RemoveAt(0);
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			base.smi.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x06007440 RID: 29760 RVA: 0x002C69F0 File Offset: 0x002C4BF0
		public void FetchFail()
		{
			if (base.smi.sm.fetchTarget.Get(base.smi) != null)
			{
				base.smi.sm.fetchTarget.Get(base.smi).Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x06007441 RID: 29761 RVA: 0x002C6A88 File Offset: 0x002C4C88
		public void FetchComplete()
		{
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x06007442 RID: 29762 RVA: 0x002C6ACC File Offset: 0x002C4CCC
		public void SetupDeliverables()
		{
			foreach (GameObject gameObject in base.sm.fetcher.Get<Storage>(base.smi).items)
			{
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!(component == null))
					{
						Pickupable component2 = component.GetComponent<Pickupable>();
						if (component2 != null)
						{
							this.deliverables.Add(component2);
						}
					}
				}
			}
		}

		// Token: 0x06007443 RID: 29763 RVA: 0x002C6B64 File Offset: 0x002C4D64
		public void ReservePickupables()
		{
			ChoreConsumer consumer = base.sm.fetcher.Get<ChoreConsumer>(base.smi);
			float num = this.fetchAmountRequested;
			foreach (Pickupable pickupable in this.fetchables)
			{
				if (num <= 0f)
				{
					break;
				}
				if (!pickupable.HasTag(GameTags.MarkedForMove))
				{
					float num2 = Math.Min(num, pickupable.UnreservedAmount);
					num -= num2;
					FetchAreaChore.StatesInstance.Reservation item = new FetchAreaChore.StatesInstance.Reservation(consumer, pickupable, num2);
					this.reservations.Add(item);
				}
			}
		}

		// Token: 0x06007444 RID: 29764 RVA: 0x002C6C10 File Offset: 0x002C4E10
		private void OnFetchChoreCancelled(FetchChore chore)
		{
			int i = 0;
			while (i < this.deliveries.Count)
			{
				if (this.deliveries[i].chore == chore)
				{
					if (this.deliveries.Count == 1)
					{
						this.StopSM("AllDelivericesCancelled");
						return;
					}
					if (i == 0)
					{
						base.sm.currentdeliverycancelled.Trigger(this);
						return;
					}
					this.deliveries[i].Cleanup();
					this.deliveries.RemoveAt(i);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06007445 RID: 29765 RVA: 0x002C6C9C File Offset: 0x002C4E9C
		public void UnreservePickupables()
		{
			foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
			{
				reservation.Cleanup();
			}
			this.reservations.Clear();
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x002C6CFC File Offset: 0x002C4EFC
		public bool SameDestination(FetchChore fetch)
		{
			using (List<FetchChore>.Enumerator enumerator = this.chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.destination == fetch.destination)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x002C6D60 File Offset: 0x002C4F60
		public void OnMarkForMove(object data)
		{
			GameObject x = base.smi.sm.fetchTarget.Get(base.smi);
			GameObject gameObject = data as GameObject;
			if (x != null)
			{
				if (x == gameObject)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
					base.smi.sm.fetchTarget.Set(null, base.smi);
					return;
				}
				global::Debug.LogError("Listening for MarkForMove on the incorrect fetch target. Subscriptions did not update correctly.");
			}
		}

		// Token: 0x040057A6 RID: 22438
		private List<FetchChore> chores = new List<FetchChore>();

		// Token: 0x040057A7 RID: 22439
		private List<Pickupable> fetchables = new List<Pickupable>();

		// Token: 0x040057A8 RID: 22440
		private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();

		// Token: 0x040057A9 RID: 22441
		private List<Pickupable> deliverables = new List<Pickupable>();

		// Token: 0x040057AA RID: 22442
		public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();

		// Token: 0x040057AB RID: 22443
		private FetchChore rootChore;

		// Token: 0x040057AC RID: 22444
		private Chore.Precondition.Context rootContext;

		// Token: 0x040057AD RID: 22445
		private float fetchAmountRequested;

		// Token: 0x040057AE RID: 22446
		public bool delivering;

		// Token: 0x040057AF RID: 22447
		public bool pickingup;

		// Token: 0x040057B0 RID: 22448
		private static Tag[] s_transientDeliveryTags = new Tag[]
		{
			GameTags.Garbage,
			GameTags.Creatures.Deliverable
		};

		// Token: 0x02001FD1 RID: 8145
		public struct Delivery
		{
			// Token: 0x17000A69 RID: 2665
			// (get) Token: 0x0600A395 RID: 41877 RVA: 0x00368357 File Offset: 0x00366557
			// (set) Token: 0x0600A396 RID: 41878 RVA: 0x0036835F File Offset: 0x0036655F
			public Storage destination { readonly get; private set; }

			// Token: 0x17000A6A RID: 2666
			// (get) Token: 0x0600A397 RID: 41879 RVA: 0x00368368 File Offset: 0x00366568
			// (set) Token: 0x0600A398 RID: 41880 RVA: 0x00368370 File Offset: 0x00366570
			public float amount { readonly get; private set; }

			// Token: 0x17000A6B RID: 2667
			// (get) Token: 0x0600A399 RID: 41881 RVA: 0x00368379 File Offset: 0x00366579
			// (set) Token: 0x0600A39A RID: 41882 RVA: 0x00368381 File Offset: 0x00366581
			public FetchChore chore { readonly get; private set; }

			// Token: 0x0600A39B RID: 41883 RVA: 0x0036838C File Offset: 0x0036658C
			public Delivery(Chore.Precondition.Context context, float amount_to_be_fetched, Action<FetchChore> on_cancelled)
			{
				this = default(FetchAreaChore.StatesInstance.Delivery);
				this.chore = (context.chore as FetchChore);
				this.amount = this.chore.originalAmount;
				this.destination = this.chore.destination;
				this.chore.SetOverrideTarget(context.consumerState.consumer);
				this.onCancelled = on_cancelled;
				this.onFetchChoreCleanup = new Action<Chore>(this.OnFetchChoreCleanup);
				this.chore.FetchAreaBegin(context, amount_to_be_fetched);
				FetchChore chore = this.chore;
				chore.onCleanup = (Action<Chore>)Delegate.Combine(chore.onCleanup, this.onFetchChoreCleanup);
			}

			// Token: 0x0600A39C RID: 41884 RVA: 0x0036843C File Offset: 0x0036663C
			public void Complete(List<Pickupable> deliverables)
			{
				using (new KProfiler.Region("FAC.Delivery.Complete", null))
				{
					if (!(this.destination == null) && !this.destination.IsEndOfLife())
					{
						FetchChore chore = this.chore;
						chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
						float num = this.amount;
						Pickupable pickupable = null;
						int num2 = 0;
						while (num2 < deliverables.Count && num > 0f)
						{
							if (deliverables[num2] == null)
							{
								if (num < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
								{
									this.destination.ForceStore(this.chore.tagsFirst, num);
								}
							}
							else if (!FetchAreaChore.IsPickupableStillValidForChore(deliverables[num2], this.chore))
							{
								global::Debug.LogError(string.Format("Attempting to store {0} in a {1} which did not request it", deliverables[num2], this.destination));
							}
							else
							{
								Pickupable pickupable2 = deliverables[num2].Take(num);
								if (pickupable2 != null && pickupable2.TotalAmount > 0f)
								{
									num -= pickupable2.TotalAmount;
									this.destination.Store(pickupable2.gameObject, false, false, true, false);
									pickupable = pickupable2;
									if (pickupable2 == deliverables[num2])
									{
										deliverables[num2] = null;
									}
								}
							}
							num2++;
						}
						if (this.chore.overrideTarget != null)
						{
							this.chore.FetchAreaEnd(this.chore.overrideTarget.GetComponent<ChoreDriver>(), pickupable, true);
						}
						this.chore = null;
					}
				}
			}

			// Token: 0x0600A39D RID: 41885 RVA: 0x003685F4 File Offset: 0x003667F4
			private void OnFetchChoreCleanup(Chore chore)
			{
				if (this.onCancelled != null)
				{
					this.onCancelled(chore as FetchChore);
				}
			}

			// Token: 0x0600A39E RID: 41886 RVA: 0x0036860F File Offset: 0x0036680F
			public void Cleanup()
			{
				if (this.chore != null)
				{
					FetchChore chore = this.chore;
					chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
					this.chore.FetchAreaEnd(null, null, false);
				}
			}

			// Token: 0x04008F3C RID: 36668
			private Action<FetchChore> onCancelled;

			// Token: 0x04008F3D RID: 36669
			private Action<Chore> onFetchChoreCleanup;
		}

		// Token: 0x02001FD2 RID: 8146
		public struct Reservation
		{
			// Token: 0x17000A6C RID: 2668
			// (get) Token: 0x0600A39F RID: 41887 RVA: 0x00368648 File Offset: 0x00366848
			// (set) Token: 0x0600A3A0 RID: 41888 RVA: 0x00368650 File Offset: 0x00366850
			public float amount { readonly get; private set; }

			// Token: 0x17000A6D RID: 2669
			// (get) Token: 0x0600A3A1 RID: 41889 RVA: 0x00368659 File Offset: 0x00366859
			// (set) Token: 0x0600A3A2 RID: 41890 RVA: 0x00368661 File Offset: 0x00366861
			public Pickupable pickupable { readonly get; private set; }

			// Token: 0x0600A3A3 RID: 41891 RVA: 0x0036866C File Offset: 0x0036686C
			public Reservation(ChoreConsumer consumer, Pickupable pickupable, float reservation_amount)
			{
				this = default(FetchAreaChore.StatesInstance.Reservation);
				if (reservation_amount <= 0f)
				{
					global::Debug.LogError("Invalid amount: " + reservation_amount.ToString());
				}
				this.amount = reservation_amount;
				this.pickupable = pickupable;
				this.handle = pickupable.Reserve("FetchAreaChore", consumer.gameObject, reservation_amount);
			}

			// Token: 0x0600A3A4 RID: 41892 RVA: 0x003686C4 File Offset: 0x003668C4
			public void Cleanup()
			{
				if (this.pickupable != null)
				{
					this.pickupable.Unreserve("FetchAreaChore", this.handle);
				}
			}

			// Token: 0x04008F40 RID: 36672
			private int handle;
		}
	}

	// Token: 0x02000FF7 RID: 4087
	public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
	{
		// Token: 0x06007449 RID: 29769 RVA: 0x002C6E08 File Offset: 0x002C5008
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetching;
			base.Target(this.fetcher);
			this.fetching.DefaultState(this.fetching.next).Enter("ReservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.ReservePickupables();
			}).Exit("UnreservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.UnreservePickupables();
			}).Enter("pickingup-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = true;
			}).Exit("pickingup-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = false;
			});
			this.fetching.next.Enter("SetupFetch", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupFetch();
			});
			this.fetching.movetopickupable.InitializeStates(this.fetcher, this.fetchTarget, this.fetching.pickup, this.fetching.fetchfail, null, NavigationTactics.ReduceTravelDistance).Target(this.fetchTarget).EventHandlerTransition(GameHashes.TagsChanged, this.fetching.fetchfail, (FetchAreaChore.StatesInstance smi, object obj) => smi.RootChore_ValidateRequiredTagOnTagChange && smi.RootChore_RequiredTag.IsValid && !this.fetchTarget.Get(smi).HasTag(smi.RootChore_RequiredTag)).Target(this.fetcher);
			this.fetching.pickup.DoPickup(this.fetchTarget, this.fetchResultTarget, this.fetchAmount, this.fetching.fetchcomplete, this.fetching.fetchfail).Exit(delegate(FetchAreaChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.fetchTarget.Get(smi);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(smi.OnMarkForMove));
				}
			});
			this.fetching.fetchcomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchComplete();
			});
			this.fetching.fetchfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchFail();
			});
			this.delivering.DefaultState(this.delivering.next).OnSignal(this.currentdeliverycancelled, this.delivering.deliverfail).Enter("SetupDeliverables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDeliverables();
			}).Enter("delivering-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = true;
			}).Exit("delivering-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = false;
			});
			this.delivering.next.Enter("SetupDelivery", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDelivery();
			});
			this.delivering.movetostorage.InitializeStates(this.fetcher, this.deliveryDestination, this.delivering.storing, this.delivering.deliverfail, null, NavigationTactics.ReduceTravelDistance).Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				if (this.deliveryObject.Get(smi) != null && this.deliveryObject.Get(smi).GetComponent<MinionIdentity>() != null)
				{
					this.deliveryObject.Get(smi).transform.SetLocalPosition(Vector3.zero);
					KBatchedAnimTracker component = this.deliveryObject.Get(smi).GetComponent<KBatchedAnimTracker>();
					component.symbol = new HashedString("snapTo_chest");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			});
			this.delivering.storing.DoDelivery(this.fetcher, this.deliveryDestination, this.delivering.delivercomplete, this.delivering.deliverfail);
			this.delivering.deliverfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverFail();
			});
			this.delivering.delivercomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverComplete();
			});
		}

		// Token: 0x040057B1 RID: 22449
		public FetchAreaChore.States.FetchStates fetching;

		// Token: 0x040057B2 RID: 22450
		public FetchAreaChore.States.DeliverStates delivering;

		// Token: 0x040057B3 RID: 22451
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;

		// Token: 0x040057B4 RID: 22452
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;

		// Token: 0x040057B5 RID: 22453
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;

		// Token: 0x040057B6 RID: 22454
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;

		// Token: 0x040057B7 RID: 22455
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;

		// Token: 0x040057B8 RID: 22456
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;

		// Token: 0x040057B9 RID: 22457
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;

		// Token: 0x040057BA RID: 22458
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

		// Token: 0x02001FD4 RID: 8148
		public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x04008F42 RID: 36674
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x04008F43 RID: 36675
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;

			// Token: 0x04008F44 RID: 36676
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;

			// Token: 0x04008F45 RID: 36677
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;

			// Token: 0x04008F46 RID: 36678
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
		}

		// Token: 0x02001FD5 RID: 8149
		public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x04008F47 RID: 36679
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x04008F48 RID: 36680
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;

			// Token: 0x04008F49 RID: 36681
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;

			// Token: 0x04008F4A RID: 36682
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;

			// Token: 0x04008F4B RID: 36683
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
		}
	}
}
