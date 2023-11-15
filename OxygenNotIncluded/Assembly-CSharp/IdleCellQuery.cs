using System;

// Token: 0x02000405 RID: 1029
public class IdleCellQuery : PathFinderQuery
{
	// Token: 0x060015B7 RID: 5559 RVA: 0x000726EB File Offset: 0x000708EB
	public IdleCellQuery Reset(MinionBrain brain, int max_cost)
	{
		this.brain = brain;
		this.maxCost = max_cost;
		this.targetCell = Grid.InvalidCell;
		return this;
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x00072708 File Offset: 0x00070908
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, false);
		if ((flags & SafeCellQuery.SafeFlags.IsClear) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLadder) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotTube) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0)
		{
			this.targetCell = cell;
		}
		return cost > this.maxCost;
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x00072751 File Offset: 0x00070951
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000C00 RID: 3072
	private MinionBrain brain;

	// Token: 0x04000C01 RID: 3073
	private int targetCell;

	// Token: 0x04000C02 RID: 3074
	private int maxCost;
}
