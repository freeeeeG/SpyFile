using System;

namespace flanne
{
	// Token: 0x02000133 RID: 307
	[Serializable]
	public class TieredUnlockData
	{
		// Token: 0x06000835 RID: 2101 RVA: 0x00022C00 File Offset: 0x00020E00
		public TieredUnlockData(int size)
		{
			this.unlocks = new int[size];
		}

		// Token: 0x0400060E RID: 1550
		public int[] unlocks;
	}
}
