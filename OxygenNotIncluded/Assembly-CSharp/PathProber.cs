using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FC RID: 1020
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/PathProber")]
public class PathProber : KMonoBehaviour
{
	// Token: 0x06001594 RID: 5524 RVA: 0x000720CF File Offset: 0x000702CF
	protected override void OnCleanUp()
	{
		if (this.PathGrid != null)
		{
			this.PathGrid.OnCleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x000720EA File Offset: 0x000702EA
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.PathGrid.SetGroupProber(group_prober);
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x000720F8 File Offset: 0x000702F8
	public void SetValidNavTypes(NavType[] nav_types, int max_probing_radius)
	{
		if (max_probing_radius != 0)
		{
			this.PathGrid = new PathGrid(max_probing_radius * 2, max_probing_radius * 2, true, nav_types);
			return;
		}
		this.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, nav_types);
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x00072128 File Offset: 0x00070328
	public int GetCost(int cell)
	{
		return this.PathGrid.GetCost(cell);
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x00072136 File Offset: 0x00070336
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x00072145 File Offset: 0x00070345
	public PathGrid GetPathGrid()
	{
		return this.PathGrid;
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x00072150 File Offset: 0x00070350
	public void UpdateProbe(NavGrid nav_grid, int cell, NavType nav_type, PathFinderAbilities abilities, PathFinder.PotentialPath.Flags flags)
	{
		if (this.scratchPad == null)
		{
			this.scratchPad = new PathFinder.PotentialScratchPad(nav_grid.maxLinksPerCell);
		}
		bool flag = this.updateCount == -1;
		bool flag2 = this.Potentials.Count == 0 || flag;
		this.PathGrid.BeginUpdate(cell, !flag2);
		if (flag2)
		{
			this.updateCount = 0;
			bool flag3;
			PathFinder.Cell cell2 = this.PathGrid.GetCell(cell, nav_type, out flag3);
			PathFinder.AddPotential(new PathFinder.PotentialPath(cell, nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, 0, this.Potentials, this.PathGrid, ref cell2);
		}
		int num = (this.potentialCellsPerUpdate <= 0 || flag) ? int.MaxValue : this.potentialCellsPerUpdate;
		this.updateCount++;
		while (this.Potentials.Count > 0 && num > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = this.Potentials.Next();
			num--;
			bool flag3;
			PathFinder.Cell cell3 = this.PathGrid.GetCell(keyValuePair.Value, out flag3);
			if (cell3.cost == keyValuePair.Key)
			{
				PathFinder.AddPotentials(this.scratchPad, keyValuePair.Value, cell3.cost, ref abilities, null, nav_grid.maxLinksPerCell, nav_grid.Links, this.Potentials, this.PathGrid, cell3.parent, cell3.parentNavType);
			}
		}
		bool flag4 = this.Potentials.Count == 0;
		this.PathGrid.EndUpdate(flag4);
		if (flag4)
		{
			int num2 = this.updateCount;
		}
	}

	// Token: 0x04000BE9 RID: 3049
	public const int InvalidHandle = -1;

	// Token: 0x04000BEA RID: 3050
	public const int InvalidIdx = -1;

	// Token: 0x04000BEB RID: 3051
	public const int InvalidCell = -1;

	// Token: 0x04000BEC RID: 3052
	public const int InvalidCost = -1;

	// Token: 0x04000BED RID: 3053
	private PathGrid PathGrid;

	// Token: 0x04000BEE RID: 3054
	private PathFinder.PotentialList Potentials = new PathFinder.PotentialList();

	// Token: 0x04000BEF RID: 3055
	public int updateCount = -1;

	// Token: 0x04000BF0 RID: 3056
	private const int updateCountThreshold = 25;

	// Token: 0x04000BF1 RID: 3057
	private PathFinder.PotentialScratchPad scratchPad;

	// Token: 0x04000BF2 RID: 3058
	public int potentialCellsPerUpdate = -1;
}
