using System;

namespace Team17.Online.Shared
{
	// Token: 0x02000974 RID: 2420
	public static class BitHelper
	{
		// Token: 0x06002F3E RID: 12094 RVA: 0x000DCACC File Offset: 0x000DAECC
		public static int CalculateRequirement(uint maxVal)
		{
			int num = ((maxVal & 4294901760U) <= 0U) ? 0 : 16;
			if (((maxVal >>= num) & 65280U) > 0U)
			{
				num |= 8;
				maxVal >>= 8;
			}
			if ((maxVal & 240U) > 0U)
			{
				num |= 4;
				maxVal >>= 4;
			}
			if ((maxVal & 12U) > 0U)
			{
				num |= 2;
				maxVal >>= 2;
			}
			return (num | (int)(maxVal >> 1)) + 1;
		}
	}
}
