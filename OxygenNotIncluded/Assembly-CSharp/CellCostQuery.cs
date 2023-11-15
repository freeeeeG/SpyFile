using System;

// Token: 0x02000400 RID: 1024
public class CellCostQuery : PathFinderQuery
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060015A6 RID: 5542 RVA: 0x000724AE File Offset: 0x000706AE
	// (set) Token: 0x060015A7 RID: 5543 RVA: 0x000724B6 File Offset: 0x000706B6
	public int resultCost { get; private set; }

	// Token: 0x060015A8 RID: 5544 RVA: 0x000724BF File Offset: 0x000706BF
	public void Reset(int target_cell, int max_cost)
	{
		this.targetCell = target_cell;
		this.maxCost = max_cost;
		this.resultCost = -1;
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x000724D6 File Offset: 0x000706D6
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (cost > this.maxCost)
		{
			return true;
		}
		if (cell == this.targetCell)
		{
			this.resultCost = cost;
			return true;
		}
		return false;
	}

	// Token: 0x04000BF9 RID: 3065
	private int targetCell;

	// Token: 0x04000BFA RID: 3066
	private int maxCost;
}
