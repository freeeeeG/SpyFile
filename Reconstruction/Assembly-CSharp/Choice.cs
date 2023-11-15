using System;
using System.Collections.Generic;

// Token: 0x02000013 RID: 19
[Serializable]
public struct Choice
{
	// Token: 0x0400004D RID: 77
	public ChallengeChoiceType ChoiceType;

	// Token: 0x0400004E RID: 78
	public string Value1;

	// Token: 0x0400004F RID: 79
	public List<int> Elements;
}
