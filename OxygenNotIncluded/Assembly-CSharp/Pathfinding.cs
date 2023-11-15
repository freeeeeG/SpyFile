using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004EA RID: 1258
[AddComponentMenu("KMonoBehaviour/scripts/Pathfinding")]
public class Pathfinding : KMonoBehaviour
{
	// Token: 0x06001D34 RID: 7476 RVA: 0x0009B1A9 File Offset: 0x000993A9
	public static void DestroyInstance()
	{
		Pathfinding.Instance = null;
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x0009B1B6 File Offset: 0x000993B6
	protected override void OnPrefabInit()
	{
		Pathfinding.Instance = this;
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x0009B1BE File Offset: 0x000993BE
	public void AddNavGrid(NavGrid nav_grid)
	{
		this.NavGrids.Add(nav_grid);
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x0009B1CC File Offset: 0x000993CC
	public NavGrid GetNavGrid(string id)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			if (navGrid.id == id)
			{
				return navGrid;
			}
		}
		global::Debug.LogError("Could not find nav grid: " + id);
		return null;
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x0009B240 File Offset: 0x00099440
	public List<NavGrid> GetNavGrids()
	{
		return this.NavGrids;
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x0009B248 File Offset: 0x00099448
	public void ResetNavGrids()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.InitializeGraph();
		}
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x0009B298 File Offset: 0x00099498
	public void FlushNavGridsOnLoad()
	{
		if (this.navGridsHaveBeenFlushedOnLoad)
		{
			return;
		}
		this.navGridsHaveBeenFlushedOnLoad = true;
		this.UpdateNavGrids(true);
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x0009B2B4 File Offset: 0x000994B4
	public void UpdateNavGrids(bool update_all = false)
	{
		update_all = true;
		if (update_all)
		{
			using (List<NavGrid>.Enumerator enumerator = this.NavGrids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NavGrid navGrid = enumerator.Current;
					navGrid.UpdateGraph();
				}
				return;
			}
		}
		foreach (NavGrid navGrid2 in this.NavGrids)
		{
			if (navGrid2.updateEveryFrame)
			{
				navGrid2.UpdateGraph();
			}
		}
		this.NavGrids[this.UpdateIdx].UpdateGraph();
		this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x0009B384 File Offset: 0x00099584
	public void RenderEveryTick()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.DebugUpdate();
		}
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x0009B3D4 File Offset: 0x000995D4
	public void AddDirtyNavGridCell(int cell)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.AddDirtyCell(cell);
		}
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x0009B428 File Offset: 0x00099628
	public void RefreshNavCell(int cell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		hashSet.Add(cell);
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.UpdateGraph(hashSet);
		}
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0009B488 File Offset: 0x00099688
	protected override void OnCleanUp()
	{
		this.NavGrids.Clear();
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x04001030 RID: 4144
	private List<NavGrid> NavGrids = new List<NavGrid>();

	// Token: 0x04001031 RID: 4145
	private int UpdateIdx;

	// Token: 0x04001032 RID: 4146
	private bool navGridsHaveBeenFlushedOnLoad;

	// Token: 0x04001033 RID: 4147
	public static Pathfinding Instance;
}
