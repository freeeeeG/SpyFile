using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class ContentAttribute : ScriptableObject
{
	// Token: 0x04000059 RID: 89
	public AttType AttType;

	// Token: 0x0400005A RID: 90
	public string Name;

	// Token: 0x0400005B RID: 91
	public Sprite Icon;

	// Token: 0x0400005C RID: 92
	public ReusableObject Prefab;

	// Token: 0x0400005D RID: 93
	public bool isLock;

	// Token: 0x0400005E RID: 94
	public bool initialLock;
}
