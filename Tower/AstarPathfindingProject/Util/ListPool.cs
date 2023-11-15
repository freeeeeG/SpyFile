using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x020000C1 RID: 193
	public static class ListPool<T>
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x000372B0 File Offset: 0x000354B0
		public static List<T> Claim()
		{
			List<List<T>> obj = ListPool<T>.pool;
			List<T> result;
			lock (obj)
			{
				if (ListPool<T>.pool.Count > 0)
				{
					List<T> list = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
					ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
					ListPool<T>.inPool.Remove(list);
					result = list;
				}
				else
				{
					result = new List<T>();
				}
			}
			return result;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0003733C File Offset: 0x0003553C
		private static int FindCandidate(List<List<T>> pool, int capacity)
		{
			List<T> list = null;
			int result = -1;
			int num = 0;
			while (num < pool.Count && num < 8)
			{
				List<T> list2 = pool[pool.Count - 1 - num];
				if ((list == null || list2.Capacity > list.Capacity) && list2.Capacity < capacity * 16)
				{
					list = list2;
					result = pool.Count - 1 - num;
					if (list.Capacity >= capacity)
					{
						return result;
					}
				}
				num++;
			}
			return result;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000373AC File Offset: 0x000355AC
		public static List<T> Claim(int capacity)
		{
			List<List<T>> obj = ListPool<T>.pool;
			List<T> result;
			lock (obj)
			{
				List<List<T>> list = ListPool<T>.pool;
				int num = ListPool<T>.FindCandidate(ListPool<T>.pool, capacity);
				if (capacity > 5000)
				{
					int num2 = ListPool<T>.FindCandidate(ListPool<T>.largePool, capacity);
					if (num2 != -1)
					{
						list = ListPool<T>.largePool;
						num = num2;
					}
				}
				if (num == -1)
				{
					result = new List<T>(capacity);
				}
				else
				{
					List<T> list2 = list[num];
					ListPool<T>.inPool.Remove(list2);
					list[num] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
					result = list2;
				}
			}
			return result;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00037468 File Offset: 0x00035668
		public static void Warmup(int count, int size)
		{
			List<List<T>> obj = ListPool<T>.pool;
			lock (obj)
			{
				List<T>[] array = new List<T>[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = ListPool<T>.Claim(size);
				}
				for (int j = 0; j < count; j++)
				{
					ListPool<T>.Release(array[j]);
				}
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000374D8 File Offset: 0x000356D8
		public static void Release(ref List<T> list)
		{
			ListPool<T>.Release(list);
			list = null;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000374E4 File Offset: 0x000356E4
		public static void Release(List<T> list)
		{
			list.ClearFast<T>();
			List<List<T>> obj = ListPool<T>.pool;
			lock (obj)
			{
				if (list.Capacity > 5000)
				{
					ListPool<T>.largePool.Add(list);
					if (ListPool<T>.largePool.Count > 8)
					{
						ListPool<T>.largePool.RemoveAt(0);
					}
				}
				else
				{
					ListPool<T>.pool.Add(list);
				}
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00037560 File Offset: 0x00035760
		public static void Clear()
		{
			List<List<T>> obj = ListPool<T>.pool;
			lock (obj)
			{
				ListPool<T>.pool.Clear();
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000375A4 File Offset: 0x000357A4
		public static int GetSize()
		{
			return ListPool<T>.pool.Count;
		}

		// Token: 0x040004D9 RID: 1241
		private static readonly List<List<T>> pool = new List<List<T>>();

		// Token: 0x040004DA RID: 1242
		private static readonly List<List<T>> largePool = new List<List<T>>();

		// Token: 0x040004DB RID: 1243
		private static readonly HashSet<List<T>> inPool = new HashSet<List<T>>();

		// Token: 0x040004DC RID: 1244
		private const int MaxCapacitySearchLength = 8;

		// Token: 0x040004DD RID: 1245
		private const int LargeThreshold = 5000;

		// Token: 0x040004DE RID: 1246
		private const int MaxLargePoolSize = 8;
	}
}
