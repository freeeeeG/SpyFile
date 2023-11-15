using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x020000C5 RID: 197
	public static class ObjectPoolSimple<T> where T : class, new()
	{
		// Token: 0x06000858 RID: 2136 RVA: 0x0003799C File Offset: 0x00035B9C
		public static T Claim()
		{
			List<T> obj = ObjectPoolSimple<T>.pool;
			T result;
			lock (obj)
			{
				if (ObjectPoolSimple<T>.pool.Count > 0)
				{
					T t = ObjectPoolSimple<T>.pool[ObjectPoolSimple<T>.pool.Count - 1];
					ObjectPoolSimple<T>.pool.RemoveAt(ObjectPoolSimple<T>.pool.Count - 1);
					ObjectPoolSimple<T>.inPool.Remove(t);
					result = t;
				}
				else
				{
					result = Activator.CreateInstance<T>();
				}
			}
			return result;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00037A28 File Offset: 0x00035C28
		public static void Release(ref T obj)
		{
			List<T> obj2 = ObjectPoolSimple<T>.pool;
			lock (obj2)
			{
				ObjectPoolSimple<T>.pool.Add(obj);
			}
			obj = default(T);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00037A78 File Offset: 0x00035C78
		public static void Clear()
		{
			List<T> obj = ObjectPoolSimple<T>.pool;
			lock (obj)
			{
				ObjectPoolSimple<T>.pool.Clear();
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00037ABC File Offset: 0x00035CBC
		public static int GetSize()
		{
			return ObjectPoolSimple<T>.pool.Count;
		}

		// Token: 0x040004DF RID: 1247
		private static List<T> pool = new List<T>();

		// Token: 0x040004E0 RID: 1248
		private static readonly HashSet<T> inPool = new HashSet<T>();
	}
}
