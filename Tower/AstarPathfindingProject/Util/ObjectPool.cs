using System;

namespace Pathfinding.Util
{
	// Token: 0x020000C4 RID: 196
	public static class ObjectPool<T> where T : class, IAstarPooledObject, new()
	{
		// Token: 0x06000856 RID: 2134 RVA: 0x0003797E File Offset: 0x00035B7E
		public static T Claim()
		{
			return ObjectPoolSimple<T>.Claim();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00037985 File Offset: 0x00035B85
		public static void Release(ref T obj)
		{
			obj.OnEnterPool();
			ObjectPoolSimple<T>.Release(ref obj);
		}
	}
}
