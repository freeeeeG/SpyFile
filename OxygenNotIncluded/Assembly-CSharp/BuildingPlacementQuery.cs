using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class BuildingPlacementQuery : PathFinderQuery
{
	// Token: 0x0600159F RID: 5535 RVA: 0x00072314 File Offset: 0x00070514
	public BuildingPlacementQuery Reset(int max_results, GameObject toPlace)
	{
		this.max_results = max_results;
		this.toPlace = toPlace;
		this.cellOffsets = toPlace.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x00072341 File Offset: 0x00070541
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlaceCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x0007237C File Offset: 0x0007057C
	private bool CheckValidPlaceCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell) || Grid.ObjectLayers[1].ContainsKey(testCell))
		{
			return false;
		}
		bool flag = true;
		int widthInCells = this.toPlace.GetComponent<OccupyArea>().GetWidthInCells();
		int cell = testCell;
		for (int i = 0; i < widthInCells; i++)
		{
			int cellInDirection = Grid.GetCellInDirection(cell, Direction.Down);
			if (!Grid.IsValidCell(cellInDirection) || !Grid.IsSolidCell(cellInDirection))
			{
				flag = false;
				break;
			}
			cell = Grid.GetCellInDirection(cell, Direction.Right);
		}
		if (flag)
		{
			for (int j = 0; j < this.cellOffsets.Length; j++)
			{
				CellOffset offset = this.cellOffsets[j];
				int num = Grid.OffsetCell(testCell, offset);
				if (!Grid.IsValidCell(num) || Grid.IsSolidCell(num) || !Grid.IsValidBuildingCell(num) || Grid.ObjectLayers[1].ContainsKey(num))
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04000BF4 RID: 3060
	public List<int> result_cells = new List<int>();

	// Token: 0x04000BF5 RID: 3061
	private int max_results;

	// Token: 0x04000BF6 RID: 3062
	private GameObject toPlace;

	// Token: 0x04000BF7 RID: 3063
	private CellOffset[] cellOffsets;
}
