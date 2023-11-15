using System;

// Token: 0x02000402 RID: 1026
public class CellQuery : PathFinderQuery
{
	// Token: 0x060015AD RID: 5549 RVA: 0x00072545 File Offset: 0x00070745
	public CellQuery Reset(int target_cell)
	{
		this.targetCell = target_cell;
		return this;
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0007254F File Offset: 0x0007074F
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return cell == this.targetCell;
	}

	// Token: 0x04000BFC RID: 3068
	private int targetCell;
}
