using System;

// Token: 0x020003FF RID: 1023
public class CellArrayQuery : PathFinderQuery
{
	// Token: 0x060015A3 RID: 5539 RVA: 0x0007246D File Offset: 0x0007066D
	public CellArrayQuery Reset(int[] target_cells)
	{
		this.targetCells = target_cells;
		return this;
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x00072478 File Offset: 0x00070678
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		for (int i = 0; i < this.targetCells.Length; i++)
		{
			if (this.targetCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000BF8 RID: 3064
	private int[] targetCells;
}
