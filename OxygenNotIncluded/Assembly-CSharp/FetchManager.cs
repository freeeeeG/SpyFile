using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x0600370D RID: 14093 RVA: 0x0012A30B File Offset: 0x0012850B
	private static int QuantizeRotValue(float rot_value)
	{
		return (int)(4f * rot_value);
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x0012A315 File Offset: 0x00128515
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x0012A317 File Offset: 0x00128517
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x0012A319 File Offset: 0x00128519
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x0012A31B File Offset: 0x0012851B
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x0012A320 File Offset: 0x00128520
	public HandleVector<int>.Handle Add(Pickupable pickupable)
	{
		Tag tag = pickupable.KPrefabID.PrefabID();
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId = null;
		if (!this.prefabIdToFetchables.TryGetValue(tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(tag);
			this.prefabIdToFetchables[tag] = fetchablesByPrefabId;
		}
		return fetchablesByPrefabId.AddPickupable(pickupable);
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x0012A368 File Offset: 0x00128568
	public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.RemovePickupable(fetchable_handle);
		}
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x0012A38C File Offset: 0x0012858C
	public void UpdateStorage(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle, Storage storage)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.UpdateStorage(fetchable_handle, storage);
		}
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x0012A3B1 File Offset: 0x001285B1
	public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x0012A3C8 File Offset: 0x001285C8
	public void Sim1000ms(float dt)
	{
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			keyValuePair.Value.Sim1000ms(dt);
		}
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x0012A424 File Offset: 0x00128624
	public void UpdatePickups(PathProber path_prober, Worker worker)
	{
		Navigator component = worker.GetComponent<Navigator>();
		this.updatePickupsWorkItems.Reset(null);
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			FetchManager.FetchablesByPrefabId value = keyValuePair.Value;
			value.UpdateOffsetTables();
			this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem
			{
				fetchablesByPrefabId = value,
				pathProber = path_prober,
				navigator = component,
				worker = worker.gameObject
			});
		}
		OffsetTracker.isExecutingWithinJob = true;
		GlobalJobManager.Run(this.updatePickupsWorkItems);
		OffsetTracker.isExecutingWithinJob = false;
		this.pickups.Clear();
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair2 in this.prefabIdToFetchables)
		{
			this.pickups.AddRange(keyValuePair2.Value.finalPickups);
		}
		this.pickups.Sort(FetchManager.ComparerNoPriority);
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x0012A550 File Offset: 0x00128750
	public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
	{
		KPrefabID kprefabID = pickup.KPrefabID;
		Storage storage = pickup.storage;
		if (pickup.UnreservedAmount <= 0f)
		{
			return false;
		}
		if (kprefabID == null)
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst))
		{
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.MarkedForMove))
		{
			return false;
		}
		if (storage != null)
		{
			if (!storage.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= storage.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == storage.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0012A640 File Offset: 0x00128840
	public static Pickupable FindFetchTarget(List<Pickupable> pickupables, Storage destination, FetchChore chore)
	{
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup(pickupable, chore, destination))
			{
				return pickupable;
			}
		}
		return null;
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x0012A698 File Offset: 0x00128898
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		foreach (FetchManager.Pickup pickup in this.pickups)
		{
			if (FetchManager.IsFetchablePickup(pickup.pickupable, chore, destination))
			{
				return pickup.pickupable;
			}
		}
		return null;
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x0012A700 File Offset: 0x00128900
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag required_tag, Storage destination)
	{
		if (pickup_unreserved_amount <= 0f)
		{
			return false;
		}
		if (pickup_id == null)
		{
			return false;
		}
		if (exclude_tags.Contains(pickup_id.PrefabTag))
		{
			return false;
		}
		if (!pickup_id.HasTag(required_tag))
		{
			return false;
		}
		if (source != null)
		{
			if (!source.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= source.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == source.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x0012A78C File Offset: 0x0012898C
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag required_tag)
	{
		FetchManager.Pickup pickup = new FetchManager.Pickup
		{
			PathCost = ushort.MaxValue,
			foodQuality = int.MinValue
		};
		int num = int.MaxValue;
		foreach (FetchManager.Pickup pickup2 in this.pickups)
		{
			Pickupable pickupable = pickup2.pickupable;
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, required_tag, destination))
			{
				int num2 = (int)pickup2.PathCost + (5 - pickup2.foodQuality) * 50;
				if (num2 < num)
				{
					pickup = pickup2;
					num = num2;
				}
			}
		}
		return pickup.pickupable;
	}

	// Token: 0x040021EC RID: 8684
	private static readonly FetchManager.PickupComparerIncludingPriority ComparerIncludingPriority = new FetchManager.PickupComparerIncludingPriority();

	// Token: 0x040021ED RID: 8685
	private static readonly FetchManager.PickupComparerNoPriority ComparerNoPriority = new FetchManager.PickupComparerNoPriority();

	// Token: 0x040021EE RID: 8686
	private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();

	// Token: 0x040021EF RID: 8687
	public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();

	// Token: 0x040021F0 RID: 8688
	private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

	// Token: 0x02001531 RID: 5425
	public struct Fetchable
	{
		// Token: 0x0400677E RID: 26494
		public Pickupable pickupable;

		// Token: 0x0400677F RID: 26495
		public int tagBitsHash;

		// Token: 0x04006780 RID: 26496
		public int masterPriority;

		// Token: 0x04006781 RID: 26497
		public int freshness;

		// Token: 0x04006782 RID: 26498
		public int foodQuality;
	}

	// Token: 0x02001532 RID: 5426
	[DebuggerDisplay("{pickupable.name}")]
	public struct Pickup
	{
		// Token: 0x04006783 RID: 26499
		public Pickupable pickupable;

		// Token: 0x04006784 RID: 26500
		public int tagBitsHash;

		// Token: 0x04006785 RID: 26501
		public ushort PathCost;

		// Token: 0x04006786 RID: 26502
		public int masterPriority;

		// Token: 0x04006787 RID: 26503
		public int freshness;

		// Token: 0x04006788 RID: 26504
		public int foodQuality;
	}

	// Token: 0x02001533 RID: 5427
	private class PickupComparerIncludingPriority : IComparer<FetchManager.Pickup>
	{
		// Token: 0x0600870D RID: 34573 RVA: 0x00309DC4 File Offset: 0x00307FC4
		public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.tagBitsHash.CompareTo(b.tagBitsHash);
			if (num != 0)
			{
				return num;
			}
			num = b.masterPriority.CompareTo(a.masterPriority);
			if (num != 0)
			{
				return num;
			}
			num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}
	}

	// Token: 0x02001534 RID: 5428
	private class PickupComparerNoPriority : IComparer<FetchManager.Pickup>
	{
		// Token: 0x0600870F RID: 34575 RVA: 0x00309E50 File Offset: 0x00308050
		public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}
	}

	// Token: 0x02001535 RID: 5429
	public class FetchablesByPrefabId
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06008711 RID: 34577 RVA: 0x00309EA9 File Offset: 0x003080A9
		// (set) Token: 0x06008712 RID: 34578 RVA: 0x00309EB1 File Offset: 0x003080B1
		public Tag prefabId { get; private set; }

		// Token: 0x06008713 RID: 34579 RVA: 0x00309EBC File Offset: 0x003080BC
		public FetchablesByPrefabId(Tag prefab_id)
		{
			this.prefabId = prefab_id;
			this.fetchables = new KCompactedVector<FetchManager.Fetchable>(0);
			this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
			this.finalPickups = new List<FetchManager.Pickup>();
		}

		// Token: 0x06008714 RID: 34580 RVA: 0x00309F1C File Offset: 0x0030811C
		public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
		{
			int foodQuality = 5;
			Edible component = pickupable.GetComponent<Edible>();
			if (component != null)
			{
				foodQuality = component.GetQuality();
			}
			int masterPriority = 0;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
			int freshness = 0;
			if (!smi.IsNullOrStopped())
			{
				freshness = FetchManager.QuantizeRotValue(smi.RotValue);
			}
			KPrefabID kprefabID = pickupable.KPrefabID;
			HandleVector<int>.Handle handle = this.fetchables.Allocate(new FetchManager.Fetchable
			{
				pickupable = pickupable,
				foodQuality = foodQuality,
				freshness = freshness,
				masterPriority = masterPriority,
				tagBitsHash = kprefabID.GetTagsHash()
			});
			if (!smi.IsNullOrStopped())
			{
				this.rotUpdaters[handle] = smi;
			}
			return handle;
		}

		// Token: 0x06008715 RID: 34581 RVA: 0x00309FFF File Offset: 0x003081FF
		public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
		{
			this.fetchables.Free(fetchable_handle);
			this.rotUpdaters.Remove(fetchable_handle);
		}

		// Token: 0x06008716 RID: 34582 RVA: 0x0030A01C File Offset: 0x0030821C
		public void UpdatePickups(PathProber path_prober, Navigator worker_navigator, GameObject worker_go)
		{
			this.GatherPickupablesWhichCanBePickedUp(worker_go);
			this.GatherReachablePickups(worker_navigator);
			this.finalPickups.Sort(FetchManager.ComparerIncludingPriority);
			if (this.finalPickups.Count > 0)
			{
				FetchManager.Pickup pickup = this.finalPickups[0];
				int num = pickup.tagBitsHash;
				int num2 = this.finalPickups.Count;
				int num3 = 0;
				for (int i = 1; i < this.finalPickups.Count; i++)
				{
					bool flag = false;
					FetchManager.Pickup pickup2 = this.finalPickups[i];
					int tagBitsHash = pickup2.tagBitsHash;
					if (pickup.masterPriority == pickup2.masterPriority && tagBitsHash == num)
					{
						flag = true;
					}
					if (flag)
					{
						num2--;
					}
					else
					{
						num3++;
						pickup = pickup2;
						num = tagBitsHash;
						if (i > num3)
						{
							this.finalPickups[num3] = pickup2;
						}
					}
				}
				this.finalPickups.RemoveRange(num2, this.finalPickups.Count - num2);
			}
		}

		// Token: 0x06008717 RID: 34583 RVA: 0x0030A108 File Offset: 0x00308308
		private void GatherPickupablesWhichCanBePickedUp(GameObject worker_go)
		{
			this.pickupsWhichCanBePickedUp.Clear();
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				Pickupable pickupable = fetchable.pickupable;
				if (pickupable.CouldBePickedUpByMinion(worker_go))
				{
					this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = fetchable.tagBitsHash,
						PathCost = ushort.MaxValue,
						masterPriority = fetchable.masterPriority,
						freshness = fetchable.freshness,
						foodQuality = fetchable.foodQuality
					});
				}
			}
		}

		// Token: 0x06008718 RID: 34584 RVA: 0x0030A1D0 File Offset: 0x003083D0
		public void UpdateOffsetTables()
		{
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				fetchable.pickupable.GetOffsets(fetchable.pickupable.cachedCell);
			}
		}

		// Token: 0x06008719 RID: 34585 RVA: 0x0030A238 File Offset: 0x00308438
		private void GatherReachablePickups(Navigator navigator)
		{
			this.cellCosts.Clear();
			this.finalPickups.Clear();
			foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
			{
				Pickupable pickupable = pickup.pickupable;
				int num = -1;
				if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
				{
					num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
					this.cellCosts[pickupable.cachedCell] = num;
				}
				if (num != -1)
				{
					this.finalPickups.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = pickup.tagBitsHash,
						PathCost = (ushort)num,
						masterPriority = pickup.masterPriority,
						freshness = pickup.freshness,
						foodQuality = pickup.foodQuality
					});
				}
			}
		}

		// Token: 0x0600871A RID: 34586 RVA: 0x0030A33C File Offset: 0x0030853C
		public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			int masterPriority = 0;
			Pickupable pickupable = data.pickupable;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			data.masterPriority = masterPriority;
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x0600871B RID: 34587 RVA: 0x0030A3A8 File Offset: 0x003085A8
		public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x0600871C RID: 34588 RVA: 0x0030A3E8 File Offset: 0x003085E8
		public void Sim1000ms(float dt)
		{
			foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> keyValuePair in this.rotUpdaters)
			{
				HandleVector<int>.Handle key = keyValuePair.Key;
				Rottable.Instance value = keyValuePair.Value;
				FetchManager.Fetchable data = this.fetchables.GetData(key);
				data.freshness = FetchManager.QuantizeRotValue(value.RotValue);
				this.fetchables.SetData(key, data);
			}
		}

		// Token: 0x04006789 RID: 26505
		public KCompactedVector<FetchManager.Fetchable> fetchables;

		// Token: 0x0400678A RID: 26506
		public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();

		// Token: 0x0400678B RID: 26507
		private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

		// Token: 0x0400678C RID: 26508
		private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();

		// Token: 0x0400678D RID: 26509
		private Dictionary<int, int> cellCosts = new Dictionary<int, int>();
	}

	// Token: 0x02001536 RID: 5430
	private struct UpdatePickupWorkItem : IWorkItem<object>
	{
		// Token: 0x0600871D RID: 34589 RVA: 0x0030A474 File Offset: 0x00308674
		public void Run(object shared_data)
		{
			this.fetchablesByPrefabId.UpdatePickups(this.pathProber, this.navigator, this.worker);
		}

		// Token: 0x0400678F RID: 26511
		public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;

		// Token: 0x04006790 RID: 26512
		public PathProber pathProber;

		// Token: 0x04006791 RID: 26513
		public Navigator navigator;

		// Token: 0x04006792 RID: 26514
		public GameObject worker;
	}
}
