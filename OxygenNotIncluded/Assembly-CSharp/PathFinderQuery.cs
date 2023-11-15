using System;

// Token: 0x020003FA RID: 1018
public class PathFinderQuery
{
	// Token: 0x06001580 RID: 5504 RVA: 0x00071BF6 File Offset: 0x0006FDF6
	public virtual bool IsMatch(int cell, int parent_cell, int cost)
	{
		return true;
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x00071BF9 File Offset: 0x0006FDF9
	public void SetResult(int cell, int cost, NavType nav_type)
	{
		this.resultCell = cell;
		this.resultNavType = nav_type;
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x00071C09 File Offset: 0x0006FE09
	public void ClearResult()
	{
		this.resultCell = -1;
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x00071C12 File Offset: 0x0006FE12
	public virtual int GetResultCell()
	{
		return this.resultCell;
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x00071C1A File Offset: 0x0006FE1A
	public NavType GetResultNavType()
	{
		return this.resultNavType;
	}

	// Token: 0x04000BD8 RID: 3032
	protected int resultCell;

	// Token: 0x04000BD9 RID: 3033
	private NavType resultNavType;
}
