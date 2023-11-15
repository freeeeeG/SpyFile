using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x020000BE RID: 190
	public static class ArrayPool<T>
	{
		// Token: 0x0600083B RID: 2107 RVA: 0x00036BBC File Offset: 0x00034DBC
		public static T[] Claim(int minimumLength)
		{
			if (minimumLength <= 0)
			{
				return ArrayPool<T>.ClaimWithExactLength(0);
			}
			int num = 0;
			while (1 << num < minimumLength && num < 30)
			{
				num++;
			}
			if (num == 30)
			{
				throw new ArgumentException("Too high minimum length");
			}
			Stack<T[]>[] obj = ArrayPool<T>.pool;
			lock (obj)
			{
				if (ArrayPool<T>.pool[num] == null)
				{
					ArrayPool<T>.pool[num] = new Stack<T[]>();
				}
				if (ArrayPool<T>.pool[num].Count > 0)
				{
					return ArrayPool<T>.pool[num].Pop();
				}
			}
			return new T[1 << num];
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00036C68 File Offset: 0x00034E68
		public static T[] ClaimWithExactLength(int length)
		{
			if (length != 0 && (length & length - 1) == 0)
			{
				return ArrayPool<T>.Claim(length);
			}
			if (length <= 256)
			{
				Stack<T[]>[] obj = ArrayPool<T>.pool;
				lock (obj)
				{
					Stack<T[]> stack = ArrayPool<T>.exactPool[length];
					if (stack != null && stack.Count > 0)
					{
						return stack.Pop();
					}
				}
			}
			return new T[length];
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00036CE8 File Offset: 0x00034EE8
		public static void Release(ref T[] array, bool allowNonPowerOfTwo = false)
		{
			if (array == null)
			{
				return;
			}
			if (array.GetType() != typeof(T[]))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Expected array type ",
					typeof(T[]).Name,
					" but found ",
					array.GetType().Name,
					"\nAre you using the correct generic class?\n"
				}));
			}
			bool flag = array.Length != 0 && (array.Length & array.Length - 1) == 0;
			if (!flag && !allowNonPowerOfTwo && array.Length != 0)
			{
				throw new ArgumentException("Length is not a power of 2");
			}
			Stack<T[]>[] obj = ArrayPool<T>.pool;
			lock (obj)
			{
				if (flag)
				{
					int num = 0;
					while (1 << num < array.Length && num < 30)
					{
						num++;
					}
					if (ArrayPool<T>.pool[num] == null)
					{
						ArrayPool<T>.pool[num] = new Stack<T[]>();
					}
					ArrayPool<T>.pool[num].Push(array);
				}
				else if (array.Length <= 256)
				{
					Stack<T[]> stack = ArrayPool<T>.exactPool[array.Length];
					if (stack == null)
					{
						stack = (ArrayPool<T>.exactPool[array.Length] = new Stack<T[]>());
					}
					stack.Push(array);
				}
			}
			array = null;
		}

		// Token: 0x040004D2 RID: 1234
		private const int MaximumExactArrayLength = 256;

		// Token: 0x040004D3 RID: 1235
		private static readonly Stack<T[]>[] pool = new Stack<T[]>[31];

		// Token: 0x040004D4 RID: 1236
		private static readonly Stack<T[]>[] exactPool = new Stack<T[]>[257];
	}
}
