using System;

namespace LitJson
{
	// Token: 0x02000249 RID: 585
	public enum JsonToken
	{
		// Token: 0x04000834 RID: 2100
		None,
		// Token: 0x04000835 RID: 2101
		ObjectStart,
		// Token: 0x04000836 RID: 2102
		PropertyName,
		// Token: 0x04000837 RID: 2103
		ObjectEnd,
		// Token: 0x04000838 RID: 2104
		ArrayStart,
		// Token: 0x04000839 RID: 2105
		ArrayEnd,
		// Token: 0x0400083A RID: 2106
		Int,
		// Token: 0x0400083B RID: 2107
		Long,
		// Token: 0x0400083C RID: 2108
		Double,
		// Token: 0x0400083D RID: 2109
		String,
		// Token: 0x0400083E RID: 2110
		Boolean,
		// Token: 0x0400083F RID: 2111
		Null
	}
}
