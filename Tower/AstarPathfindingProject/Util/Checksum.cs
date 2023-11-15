using System;

namespace Pathfinding.Util
{
	// Token: 0x020000CD RID: 205
	public class Checksum
	{
		// Token: 0x060008AF RID: 2223 RVA: 0x0003ABC4 File Offset: 0x00038DC4
		public static uint GetChecksum(byte[] arr, uint hash)
		{
			hash ^= 2166136261U;
			for (int i = 0; i < arr.Length; i++)
			{
				hash = (hash ^ (uint)arr[i]) * 16777619U;
			}
			return hash;
		}
	}
}
