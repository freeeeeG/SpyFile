using System;
using System.Collections.Generic;

// Token: 0x020003DA RID: 986
public class GlobalChoreProvider : ChoreProvider, IRender200ms
{
	// Token: 0x060014B5 RID: 5301 RVA: 0x0006D90B File Offset: 0x0006BB0B
	public static void DestroyInstance()
	{
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x0006D913 File Offset: 0x0006BB13
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalChoreProvider.Instance = this;
		this.clearableManager = new ClearableManager();
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x0006D92C File Offset: 0x0006BB2C
	protected override void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<FetchChore> chores;
		if (this.fetchMap.TryGetValue(parentWorldId, out chores))
		{
			base.ClearWorldChores<FetchChore>(chores, num);
		}
		base.OnWorldRemoved(data);
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x0006D970 File Offset: 0x0006BB70
	protected override void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255)
		{
			return;
		}
		base.OnWorldParentChanged(data);
		List<FetchChore> oldChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<FetchChore> newChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.fetchMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<FetchChore>());
		}
		base.TransferChores<FetchChore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x0006D9FC File Offset: 0x0006BBFC
	public override void AddChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list = (this.fetchMap[myParentWorldId] = new List<FetchChore>());
			}
			chore.provider = this;
			list.Add(fetchChore);
			return;
		}
		base.AddChore(chore);
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x0006DA58 File Offset: 0x0006BC58
	public override void RemoveChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list.Remove(fetchChore);
			}
			chore.provider = null;
			return;
		}
		base.RemoveChore(chore);
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x0006DAA4 File Offset: 0x0006BCA4
	public void UpdateFetches(PathProber path_prober)
	{
		List<FetchChore> list = null;
		int myParentWorldId = path_prober.gameObject.GetMyParentWorldId();
		if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		this.fetches.Clear();
		Navigator component = path_prober.GetComponent<Navigator>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			FetchChore fetchChore = list[i];
			if (!(fetchChore.driver != null) && (!(fetchChore.automatable != null) || !fetchChore.automatable.GetAutomationOnly()))
			{
				if (fetchChore.provider == null)
				{
					fetchChore.Cancel("no provider");
					list[i] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					Storage destination = fetchChore.destination;
					if (!(destination == null))
					{
						int navigationCost = component.GetNavigationCost(destination);
						if (navigationCost != -1)
						{
							this.fetches.Add(new GlobalChoreProvider.Fetch
							{
								chore = fetchChore,
								idsHash = fetchChore.tagsHash,
								cost = navigationCost,
								priority = fetchChore.masterPriority,
								category = destination.fetchCategory
							});
						}
					}
				}
			}
		}
		if (this.fetches.Count > 0)
		{
			this.fetches.Sort(GlobalChoreProvider.Comparer);
			int j = 1;
			int num = 0;
			while (j < this.fetches.Count)
			{
				if (!this.fetches[num].IsBetterThan(this.fetches[j]))
				{
					num++;
					this.fetches[num] = this.fetches[j];
				}
				j++;
			}
			this.fetches.RemoveRange(num + 1, this.fetches.Count - num - 1);
		}
		this.clearableManager.CollectAndSortClearables(component);
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x0006DC98 File Offset: 0x0006BE98
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		base.CollectChores(consumer_state, succeeded, failed_contexts);
		this.clearableManager.CollectChores(this.fetches, consumer_state, succeeded, failed_contexts);
		for (int i = 0; i < this.fetches.Count; i++)
		{
			this.fetches[i].chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x0006DCF2 File Offset: 0x0006BEF2
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.clearableManager.RegisterClearable(clearable);
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x0006DD00 File Offset: 0x0006BF00
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.clearableManager.UnregisterClearable(handle);
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x0006DD0E File Offset: 0x0006BF0E
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x0006DD1C File Offset: 0x0006BF1C
	public void Render200ms(float dt)
	{
		this.UpdateStorageFetchableBits();
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x0006DD24 File Offset: 0x0006BF24
	private void UpdateStorageFetchableBits()
	{
		ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
		ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
		this.storageFetchableTags.Clear();
		List<int> worldIDsSorted = ClusterManager.Instance.GetWorldIDsSorted();
		for (int i = 0; i < worldIDsSorted.Count; i++)
		{
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(worldIDsSorted[i], out list))
			{
				for (int j = 0; j < list.Count; j++)
				{
					FetchChore fetchChore = list[j];
					if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && fetchChore.destination)
					{
						int cell = Grid.PosToCell(fetchChore.destination);
						if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
						{
							this.storageFetchableTags.UnionWith(fetchChore.tags);
						}
					}
				}
			}
		}
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x0006DE14 File Offset: 0x0006C014
	public bool ClearableHasDestination(Pickupable pickupable)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		return this.storageFetchableTags.Contains(kprefabID.PrefabTag);
	}

	// Token: 0x04000B32 RID: 2866
	public static GlobalChoreProvider Instance;

	// Token: 0x04000B33 RID: 2867
	public Dictionary<int, List<FetchChore>> fetchMap = new Dictionary<int, List<FetchChore>>();

	// Token: 0x04000B34 RID: 2868
	public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();

	// Token: 0x04000B35 RID: 2869
	private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();

	// Token: 0x04000B36 RID: 2870
	private ClearableManager clearableManager;

	// Token: 0x04000B37 RID: 2871
	private HashSet<Tag> storageFetchableTags = new HashSet<Tag>();

	// Token: 0x0200104B RID: 4171
	public struct Fetch
	{
		// Token: 0x06007547 RID: 30023 RVA: 0x002CCB2C File Offset: 0x002CAD2C
		public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
		{
			if (this.category != fetch.category)
			{
				return false;
			}
			if (this.idsHash != fetch.idsHash)
			{
				return false;
			}
			if (this.chore.choreType != fetch.chore.choreType)
			{
				return false;
			}
			if (this.priority.priority_class > fetch.priority.priority_class)
			{
				return true;
			}
			if (this.priority.priority_class == fetch.priority.priority_class)
			{
				if (this.priority.priority_value > fetch.priority.priority_value)
				{
					return true;
				}
				if (this.priority.priority_value == fetch.priority.priority_value)
				{
					return this.cost <= fetch.cost;
				}
			}
			return false;
		}

		// Token: 0x040058B6 RID: 22710
		public FetchChore chore;

		// Token: 0x040058B7 RID: 22711
		public int idsHash;

		// Token: 0x040058B8 RID: 22712
		public int cost;

		// Token: 0x040058B9 RID: 22713
		public PrioritySetting priority;

		// Token: 0x040058BA RID: 22714
		public Storage.FetchCategory category;
	}

	// Token: 0x0200104C RID: 4172
	private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
	{
		// Token: 0x06007548 RID: 30024 RVA: 0x002CCBEC File Offset: 0x002CADEC
		public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
		{
			int num = b.priority.priority_class - a.priority.priority_class;
			if (num != 0)
			{
				return num;
			}
			int num2 = b.priority.priority_value - a.priority.priority_value;
			if (num2 != 0)
			{
				return num2;
			}
			return a.cost - b.cost;
		}
	}

	// Token: 0x0200104D RID: 4173
	private struct FindTopPriorityTask : IWorkItem<object>
	{
		// Token: 0x0600754A RID: 30026 RVA: 0x002CCC48 File Offset: 0x002CAE48
		public FindTopPriorityTask(int start, int end, List<Prioritizable> worldCollection)
		{
			this.start = start;
			this.end = end;
			this.worldCollection = worldCollection;
			this.found = false;
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x002CCC68 File Offset: 0x002CAE68
		public void Run(object context)
		{
			if (GlobalChoreProvider.FindTopPriorityTask.abort)
			{
				return;
			}
			int num = this.start;
			while (num != this.end && this.worldCollection.Count > num)
			{
				if (!(this.worldCollection[num] == null) && this.worldCollection[num].IsTopPriority())
				{
					this.found = true;
					break;
				}
				num++;
			}
			if (this.found)
			{
				GlobalChoreProvider.FindTopPriorityTask.abort = true;
			}
		}

		// Token: 0x040058BB RID: 22715
		private int start;

		// Token: 0x040058BC RID: 22716
		private int end;

		// Token: 0x040058BD RID: 22717
		private List<Prioritizable> worldCollection;

		// Token: 0x040058BE RID: 22718
		public bool found;

		// Token: 0x040058BF RID: 22719
		public static bool abort;
	}
}
