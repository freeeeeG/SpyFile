using System;

// Token: 0x020009FE RID: 2558
public interface IReadonlyTags
{
	// Token: 0x06004C85 RID: 19589
	bool HasTag(string tag);

	// Token: 0x06004C86 RID: 19590
	bool HasTag(int hashtag);

	// Token: 0x06004C87 RID: 19591
	bool HasTags(int[] tags);
}
