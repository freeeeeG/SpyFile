using System;

// Token: 0x0200040B RID: 1035
public class SafetyQuery : PathFinderQuery
{
	// Token: 0x060015CD RID: 5581 RVA: 0x00072EE3 File Offset: 0x000710E3
	public SafetyQuery(SafetyChecker checker, KMonoBehaviour cmp, int max_cost)
	{
		this.checker = checker;
		this.cmp = cmp;
		this.maxCost = max_cost;
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x00072F00 File Offset: 0x00071100
	public void Reset()
	{
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetConditions = 0;
		this.context = new SafetyChecker.Context(this.cmp);
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x00072F30 File Offset: 0x00071130
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		bool flag = false;
		int safetyConditions = this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (safetyConditions != 0 && (safetyConditions > this.targetConditions || (safetyConditions == this.targetConditions && cost < this.targetCost)))
		{
			this.targetCell = cell;
			this.targetConditions = safetyConditions;
			this.targetCost = cost;
			if (flag)
			{
				return true;
			}
		}
		return cost >= this.maxCost;
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x00072F99 File Offset: 0x00071199
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000C21 RID: 3105
	private int targetCell;

	// Token: 0x04000C22 RID: 3106
	private int targetCost;

	// Token: 0x04000C23 RID: 3107
	private int targetConditions;

	// Token: 0x04000C24 RID: 3108
	private int maxCost;

	// Token: 0x04000C25 RID: 3109
	private SafetyChecker checker;

	// Token: 0x04000C26 RID: 3110
	private KMonoBehaviour cmp;

	// Token: 0x04000C27 RID: 3111
	private SafetyChecker.Context context;
}
