using System;

namespace LitJson
{
	// Token: 0x02000251 RID: 593
	internal enum ParserToken
	{
		// Token: 0x04000894 RID: 2196
		None = 65536,
		// Token: 0x04000895 RID: 2197
		Number,
		// Token: 0x04000896 RID: 2198
		True,
		// Token: 0x04000897 RID: 2199
		False,
		// Token: 0x04000898 RID: 2200
		Null,
		// Token: 0x04000899 RID: 2201
		CharSeq,
		// Token: 0x0400089A RID: 2202
		Char,
		// Token: 0x0400089B RID: 2203
		Text,
		// Token: 0x0400089C RID: 2204
		Object,
		// Token: 0x0400089D RID: 2205
		ObjectPrime,
		// Token: 0x0400089E RID: 2206
		Pair,
		// Token: 0x0400089F RID: 2207
		PairRest,
		// Token: 0x040008A0 RID: 2208
		Array,
		// Token: 0x040008A1 RID: 2209
		ArrayPrime,
		// Token: 0x040008A2 RID: 2210
		Value,
		// Token: 0x040008A3 RID: 2211
		ValueRest,
		// Token: 0x040008A4 RID: 2212
		String,
		// Token: 0x040008A5 RID: 2213
		End,
		// Token: 0x040008A6 RID: 2214
		Epsilon
	}
}
