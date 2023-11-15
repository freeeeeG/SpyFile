using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000C7 RID: 199
	public static class StackPool<T>
	{
		// Token: 0x06000871 RID: 2161 RVA: 0x0003826C File Offset: 0x0003646C
		public static Stack<T> Claim()
		{
			List<Stack<T>> obj = StackPool<T>.pool;
			lock (obj)
			{
				if (StackPool<T>.pool.Count > 0)
				{
					Stack<T> result = StackPool<T>.pool[StackPool<T>.pool.Count - 1];
					StackPool<T>.pool.RemoveAt(StackPool<T>.pool.Count - 1);
					return result;
				}
			}
			return new Stack<T>();
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000382E8 File Offset: 0x000364E8
		public static void Warmup(int count)
		{
			Stack<T>[] array = new Stack<T>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = StackPool<T>.Claim();
			}
			for (int j = 0; j < count; j++)
			{
				StackPool<T>.Release(array[j]);
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00038324 File Offset: 0x00036524
		public static void Release(Stack<T> stack)
		{
			stack.Clear();
			List<Stack<T>> obj = StackPool<T>.pool;
			lock (obj)
			{
				for (int i = 0; i < StackPool<T>.pool.Count; i++)
				{
					if (StackPool<T>.pool[i] == stack)
					{
						Debug.LogError("The Stack is released even though it is inside the pool");
					}
				}
				StackPool<T>.pool.Add(stack);
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0003839C File Offset: 0x0003659C
		public static void Clear()
		{
			List<Stack<T>> obj = StackPool<T>.pool;
			lock (obj)
			{
				StackPool<T>.pool.Clear();
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000383E0 File Offset: 0x000365E0
		public static int GetSize()
		{
			return StackPool<T>.pool.Count;
		}

		// Token: 0x040004E7 RID: 1255
		private static readonly List<Stack<T>> pool = new List<Stack<T>>();
	}
}
