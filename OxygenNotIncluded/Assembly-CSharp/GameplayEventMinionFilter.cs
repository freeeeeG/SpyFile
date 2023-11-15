using System;

// Token: 0x020003E3 RID: 995
public class GameplayEventMinionFilter
{
	// Token: 0x04000B5F RID: 2911
	public string id;

	// Token: 0x04000B60 RID: 2912
	public GameplayEventMinionFilter.FilterFn filter;

	// Token: 0x02001054 RID: 4180
	// (Invoke) Token: 0x0600755A RID: 30042
	public delegate bool FilterFn(MinionIdentity minion);
}
