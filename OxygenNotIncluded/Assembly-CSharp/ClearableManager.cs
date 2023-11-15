using System;
using System.Collections.Generic;

// Token: 0x020003DB RID: 987
internal class ClearableManager
{
	// Token: 0x060014C5 RID: 5317 RVA: 0x0006DE70 File Offset: 0x0006C070
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.markedClearables.Allocate(new ClearableManager.MarkedClearable
		{
			clearable = clearable,
			pickupable = clearable.GetComponent<Pickupable>(),
			prioritizable = clearable.GetComponent<Prioritizable>()
		});
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0006DEB3 File Offset: 0x0006C0B3
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.markedClearables.Free(handle);
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0006DEC4 File Offset: 0x0006C0C4
	public void CollectAndSortClearables(Navigator navigator)
	{
		this.sortedClearables.Clear();
		foreach (ClearableManager.MarkedClearable markedClearable in this.markedClearables.GetDataList())
		{
			int navigationCost = markedClearable.pickupable.GetNavigationCost(navigator, markedClearable.pickupable.cachedCell);
			if (navigationCost != -1)
			{
				this.sortedClearables.Add(new ClearableManager.SortedClearable
				{
					pickupable = markedClearable.pickupable,
					masterPriority = markedClearable.prioritizable.GetMasterPriority(),
					cost = navigationCost
				});
			}
		}
		this.sortedClearables.Sort(ClearableManager.SortedClearable.comparer);
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0006DF88 File Offset: 0x0006C188
	public void CollectChores(List<GlobalChoreProvider.Fetch> fetches, ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		ChoreType transport = Db.Get().ChoreTypes.Transport;
		int personalPriority = consumer_state.consumer.GetPersonalPriority(transport);
		int priority = Game.Instance.advancedPersonalPriorities ? transport.explicitPriority : transport.priority;
		bool flag = false;
		for (int i = 0; i < this.sortedClearables.Count; i++)
		{
			ClearableManager.SortedClearable sortedClearable = this.sortedClearables[i];
			Pickupable pickupable = sortedClearable.pickupable;
			PrioritySetting masterPriority = sortedClearable.masterPriority;
			Chore.Precondition.Context item = default(Chore.Precondition.Context);
			item.personalPriority = personalPriority;
			KPrefabID kprefabID = pickupable.KPrefabID;
			int num = 0;
			while (fetches != null && num < fetches.Count)
			{
				GlobalChoreProvider.Fetch fetch = fetches[num];
				if ((fetch.chore.criteria == FetchChore.MatchCriteria.MatchID && fetch.chore.tags.Contains(kprefabID.PrefabTag)) || (fetch.chore.criteria == FetchChore.MatchCriteria.MatchTags && kprefabID.HasTag(fetch.chore.tagsFirst)))
				{
					item.Set(fetch.chore, consumer_state, false, pickupable);
					item.choreTypeForPermission = transport;
					item.RunPreconditions();
					if (item.IsSuccess())
					{
						item.masterPriority = masterPriority;
						item.priority = priority;
						item.interruptPriority = transport.interruptPriority;
						succeeded.Add(item);
						flag = true;
						break;
					}
				}
				num++;
			}
			if (flag)
			{
				break;
			}
		}
	}

	// Token: 0x04000B38 RID: 2872
	private KCompactedVector<ClearableManager.MarkedClearable> markedClearables = new KCompactedVector<ClearableManager.MarkedClearable>(0);

	// Token: 0x04000B39 RID: 2873
	private List<ClearableManager.SortedClearable> sortedClearables = new List<ClearableManager.SortedClearable>();

	// Token: 0x0200104E RID: 4174
	private struct MarkedClearable
	{
		// Token: 0x040058C0 RID: 22720
		public Clearable clearable;

		// Token: 0x040058C1 RID: 22721
		public Pickupable pickupable;

		// Token: 0x040058C2 RID: 22722
		public Prioritizable prioritizable;
	}

	// Token: 0x0200104F RID: 4175
	private struct SortedClearable
	{
		// Token: 0x040058C3 RID: 22723
		public Pickupable pickupable;

		// Token: 0x040058C4 RID: 22724
		public PrioritySetting masterPriority;

		// Token: 0x040058C5 RID: 22725
		public int cost;

		// Token: 0x040058C6 RID: 22726
		public static ClearableManager.SortedClearable.Comparer comparer = new ClearableManager.SortedClearable.Comparer();

		// Token: 0x02002000 RID: 8192
		public class Comparer : IComparer<ClearableManager.SortedClearable>
		{
			// Token: 0x0600A45F RID: 42079 RVA: 0x00369800 File Offset: 0x00367A00
			public int Compare(ClearableManager.SortedClearable a, ClearableManager.SortedClearable b)
			{
				int num = b.masterPriority.priority_value - a.masterPriority.priority_value;
				if (num == 0)
				{
					return a.cost - b.cost;
				}
				return num;
			}
		}
	}
}
