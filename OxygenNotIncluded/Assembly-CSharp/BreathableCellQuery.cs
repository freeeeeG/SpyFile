using System;

// Token: 0x020003FD RID: 1021
public class BreathableCellQuery : PathFinderQuery
{
	// Token: 0x0600159C RID: 5532 RVA: 0x000722E9 File Offset: 0x000704E9
	public BreathableCellQuery Reset(Brain brain)
	{
		this.breather = brain.GetComponent<OxygenBreather>();
		return this;
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000722F8 File Offset: 0x000704F8
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return this.breather.IsBreathableElementAtCell(cell, null);
	}

	// Token: 0x04000BF3 RID: 3059
	private OxygenBreather breather;
}
