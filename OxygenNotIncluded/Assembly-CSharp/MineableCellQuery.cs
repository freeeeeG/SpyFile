using System;
using System.Collections.Generic;

// Token: 0x02000407 RID: 1031
public class MineableCellQuery : PathFinderQuery
{
	// Token: 0x060015BE RID: 5566 RVA: 0x000727D8 File Offset: 0x000709D8
	public MineableCellQuery Reset(Tag element, int max_results)
	{
		this.element = element;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x060015BF RID: 5567 RVA: 0x000727F4 File Offset: 0x000709F4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidMineCell(this.element, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x00072840 File Offset: 0x00070A40
	private bool CheckValidMineCell(Tag element, int testCell)
	{
		if (!Grid.IsValidCell(testCell))
		{
			return false;
		}
		foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
		{
			int cellInDirection = Grid.GetCellInDirection(testCell, d);
			if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && !Grid.Foundation[cellInDirection] && Grid.Element[cellInDirection].tag == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000C06 RID: 3078
	public List<int> result_cells = new List<int>();

	// Token: 0x04000C07 RID: 3079
	private Tag element;

	// Token: 0x04000C08 RID: 3080
	private int max_results;

	// Token: 0x04000C09 RID: 3081
	public static List<Direction> DIRECTION_CHECKS = new List<Direction>
	{
		Direction.Down,
		Direction.Right,
		Direction.Left,
		Direction.Up
	};
}
