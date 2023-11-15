using System;
using System.Collections.Generic;

// Token: 0x02000404 RID: 1028
public class FloorCellQuery : PathFinderQuery
{
	// Token: 0x060015B3 RID: 5555 RVA: 0x000725D2 File Offset: 0x000707D2
	public FloorCellQuery Reset(int max_results, int adjacent_cells_buffer = 0)
	{
		this.max_results = max_results;
		this.adjacent_cells_buffer = adjacent_cells_buffer;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x000725EE File Offset: 0x000707EE
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidFloorCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x0007262C File Offset: 0x0007082C
	private bool CheckValidFloorCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Up);
		int cellInDirection2 = Grid.GetCellInDirection(testCell, Direction.Down);
		if (!Grid.ObjectLayers[1].ContainsKey(testCell) && Grid.IsValidCell(cellInDirection2) && Grid.IsSolidCell(cellInDirection2) && Grid.IsValidCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection))
		{
			int cell = testCell;
			int cell2 = testCell;
			for (int i = 0; i < this.adjacent_cells_buffer; i++)
			{
				cell = Grid.CellLeft(cell);
				cell2 = Grid.CellRight(cell2);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell))
				{
					return false;
				}
				if (!Grid.IsValidCell(cell2) || Grid.IsSolidCell(cell2))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000BFD RID: 3069
	public List<int> result_cells = new List<int>();

	// Token: 0x04000BFE RID: 3070
	private int max_results;

	// Token: 0x04000BFF RID: 3071
	private int adjacent_cells_buffer;
}
