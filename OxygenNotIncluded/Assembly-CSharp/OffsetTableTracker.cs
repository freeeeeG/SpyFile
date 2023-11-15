using System;

// Token: 0x020008C4 RID: 2244
public class OffsetTableTracker : OffsetTracker
{
	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06004104 RID: 16644 RVA: 0x0016C4FD File Offset: 0x0016A6FD
	private static NavGrid navGrid
	{
		get
		{
			if (OffsetTableTracker.navGridImpl == null)
			{
				OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
			}
			return OffsetTableTracker.navGridImpl;
		}
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x0016C51F File Offset: 0x0016A71F
	public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
	{
		this.table = table;
		this.cmp = cmp;
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x0016C538 File Offset: 0x0016A738
	protected override void UpdateCell(int previous_cell, int current_cell)
	{
		if (previous_cell == current_cell)
		{
			return;
		}
		base.UpdateCell(previous_cell, current_cell);
		Extents extents = new Extents(current_cell, this.table);
		extents.height += 2;
		extents.y--;
		if (!this.solidPartitionerEntry.IsValid())
		{
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnCellChanged));
			this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		}
		else
		{
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, extents);
			GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, extents);
		}
		this.offsets = null;
	}

	// Token: 0x06004107 RID: 16647 RVA: 0x0016C620 File Offset: 0x0016A820
	private static bool IsValidRow(int current_cell, CellOffset[] row, int rowIdx, int[] debugIdxs)
	{
		for (int i = 1; i < row.Length; i++)
		{
			int num = Grid.OffsetCell(current_cell, row[i]);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (Grid.Solid[num])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004108 RID: 16648 RVA: 0x0016C664 File Offset: 0x0016A864
	private void UpdateOffsets(int cell, CellOffset[][] table)
	{
		HashSetPool<CellOffset, OffsetTableTracker>.PooledHashSet pooledHashSet = HashSetPool<CellOffset, OffsetTableTracker>.Allocate();
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < table.Length; i++)
			{
				CellOffset[] array = table[i];
				if (!pooledHashSet.Contains(array[0]))
				{
					int cell2 = Grid.OffsetCell(cell, array[0]);
					for (int j = 0; j < OffsetTableTracker.navGrid.ValidNavTypes.Length; j++)
					{
						NavType navType = OffsetTableTracker.navGrid.ValidNavTypes[j];
						if (navType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell2, navType) && OffsetTableTracker.IsValidRow(cell, array, i, this.DEBUG_rowValidIdx))
						{
							pooledHashSet.Add(array[0]);
							break;
						}
					}
				}
			}
		}
		if (this.offsets == null || this.offsets.Length != pooledHashSet.Count)
		{
			this.offsets = new CellOffset[pooledHashSet.Count];
		}
		pooledHashSet.CopyTo(this.offsets);
		pooledHashSet.Recycle();
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x0016C755 File Offset: 0x0016A955
	protected override void UpdateOffsets(int current_cell)
	{
		base.UpdateOffsets(current_cell);
		this.UpdateOffsets(current_cell, this.table);
	}

	// Token: 0x0600410A RID: 16650 RVA: 0x0016C76B File Offset: 0x0016A96B
	private void OnCellChanged(object data)
	{
		this.offsets = null;
	}

	// Token: 0x0600410B RID: 16651 RVA: 0x0016C774 File Offset: 0x0016A974
	public override void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
	}

	// Token: 0x0600410C RID: 16652 RVA: 0x0016C796 File Offset: 0x0016A996
	public static void OnPathfindingInvalidated()
	{
		OffsetTableTracker.navGridImpl = null;
	}

	// Token: 0x04002A57 RID: 10839
	private readonly CellOffset[][] table;

	// Token: 0x04002A58 RID: 10840
	public HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04002A59 RID: 10841
	public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04002A5A RID: 10842
	private static NavGrid navGridImpl;

	// Token: 0x04002A5B RID: 10843
	private KMonoBehaviour cmp;

	// Token: 0x04002A5C RID: 10844
	private int[] DEBUG_rowValidIdx;
}
