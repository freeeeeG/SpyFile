using System;

namespace Pathfinding
{
	// Token: 0x02000022 RID: 34
	public enum GraphUpdateThreading
	{
		// Token: 0x04000117 RID: 279
		UnityThread,
		// Token: 0x04000118 RID: 280
		SeparateThread,
		// Token: 0x04000119 RID: 281
		UnityInit,
		// Token: 0x0400011A RID: 282
		UnityPost = 4,
		// Token: 0x0400011B RID: 283
		SeparateAndUnityInit = 3
	}
}
