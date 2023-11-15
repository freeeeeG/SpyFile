using System;

namespace flanne
{
	// Token: 0x0200013D RID: 317
	[Serializable]
	public class UnlockData
	{
		// Token: 0x06000852 RID: 2130 RVA: 0x00023035 File Offset: 0x00021235
		public UnlockData(int size)
		{
			this.unlocks = new bool[size];
		}

		// Token: 0x04000622 RID: 1570
		public bool[] unlocks;
	}
}
