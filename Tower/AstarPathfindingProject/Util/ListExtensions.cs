using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x020000BF RID: 191
	public static class ListExtensions
	{
		// Token: 0x0600083F RID: 2111 RVA: 0x00036E54 File Offset: 0x00035054
		public static T[] ToArrayFromPool<T>(this List<T> list)
		{
			T[] array = ArrayPool<T>.ClaimWithExactLength(list.Count);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i];
			}
			return array;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00036E8A File Offset: 0x0003508A
		public static void ClearFast<T>(this List<T> list)
		{
			if (list.Count * 2 < list.Capacity)
			{
				list.RemoveRange(0, list.Count);
				return;
			}
			list.Clear();
		}
	}
}
