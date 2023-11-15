using System;

namespace KMod
{
	// Token: 0x02000D7D RID: 3453
	[Flags]
	public enum Content : byte
	{
		// Token: 0x04004EBA RID: 20154
		LayerableFiles = 1,
		// Token: 0x04004EBB RID: 20155
		Strings = 2,
		// Token: 0x04004EBC RID: 20156
		DLL = 4,
		// Token: 0x04004EBD RID: 20157
		Translation = 8,
		// Token: 0x04004EBE RID: 20158
		Animation = 16
	}
}
