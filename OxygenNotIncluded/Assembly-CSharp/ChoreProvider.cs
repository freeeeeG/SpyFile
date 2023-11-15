using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D7 RID: 983
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600149D RID: 5277 RVA: 0x0006D1AF File Offset: 0x0006B3AF
	// (set) Token: 0x0600149E RID: 5278 RVA: 0x0006D1B7 File Offset: 0x0006B3B7
	public string Name { get; private set; }

	// Token: 0x0600149F RID: 5279 RVA: 0x0006D1C0 File Offset: 0x0006B3C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Subscribe(1142724171, new Action<object>(this.OnEntityMigrated));
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x0006D22A File Offset: 0x0006B42A
	protected override void OnSpawn()
	{
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
		base.OnSpawn();
		this.Name = base.name;
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x0006D268 File Offset: 0x0006B468
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Unsubscribe(1142724171, new Action<object>(this.OnEntityMigrated));
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x0006D2F8 File Offset: 0x0006B4F8
	protected virtual void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<Chore> chores;
		if (this.choreWorldMap.TryGetValue(parentWorldId, out chores))
		{
			this.ClearWorldChores<Chore>(chores, num);
		}
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x0006D338 File Offset: 0x0006B538
	protected virtual void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		List<Chore> oldChores;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255 || worldParentChangedEventArgs.lastParentId == worldParentChangedEventArgs.world.ParentWorldId || !this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0006D3E0 File Offset: 0x0006B5E0
	protected virtual void OnEntityMigrated(object data)
	{
		MigrationEventArgs migrationEventArgs = data as MigrationEventArgs;
		List<Chore> oldChores;
		if (migrationEventArgs == null || !(migrationEventArgs.entity == base.gameObject) || migrationEventArgs.prevWorldId == migrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x0006D474 File Offset: 0x0006B674
	protected virtual void OnMinionMigrated(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		List<Chore> oldChores;
		if (minionMigrationEventArgs == null || !(minionMigrationEventArgs.minionId.gameObject == base.gameObject) || minionMigrationEventArgs.prevWorldId == minionMigrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, minionMigrationEventArgs.targetWorldId);
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x0006D510 File Offset: 0x0006B710
	protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
	{
		int num = oldChores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			T t = oldChores[i];
			if (t.isNull)
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"[",
					t.GetType().Name,
					"] ",
					t.GetReportName(null),
					" has no target"
				}));
			}
			else if (t.gameObject.GetMyParentWorldId() == transferId)
			{
				newChores.Add(t);
				oldChores[i] = oldChores[num];
				oldChores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x0006D5CC File Offset: 0x0006B7CC
	protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
	{
		int num = chores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (chores[i].gameObject.GetMyWorldId() == worldId)
			{
				chores[i] = chores[num];
				chores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x0006D620 File Offset: 0x0006B820
	public virtual void AddChore(Chore chore)
	{
		chore.provider = this;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list = (this.choreWorldMap[myParentWorldId] = new List<Chore>());
		}
		list.Add(chore);
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x0006D66C File Offset: 0x0006B86C
	public virtual void RemoveChore(Chore chore)
	{
		if (chore == null)
		{
			return;
		}
		chore.provider = null;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list.Remove(chore);
		}
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0006D6AC File Offset: 0x0006B8AC
	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		List<Chore> list = null;
		int myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			Chore chore = list[i];
			if (chore.provider == null)
			{
				chore.Cancel("no provider");
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
			else
			{
				chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
			}
		}
	}

	// Token: 0x04000B2C RID: 2860
	public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();
}
