using System;

namespace Pathfinding.Util
{
	// Token: 0x020000CE RID: 206
	public static class Memory
	{
		// Token: 0x060008B1 RID: 2225 RVA: 0x0003AC00 File Offset: 0x00038E00
		public static void MemSet<T>(T[] array, T value, int byteSize) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = 32;
			int i = 0;
			int num2 = Math.Min(num, array.Length);
			while (i < num2)
			{
				array[i] = value;
				i++;
			}
			num2 = array.Length;
			while (i < num2)
			{
				Buffer.BlockCopy(array, 0, array, i * byteSize, Math.Min(num, num2 - i) * byteSize);
				i += num;
				num *= 2;
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0003AC64 File Offset: 0x00038E64
		public static void MemSet<T>(T[] array, T value, int totalSize, int byteSize) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = 32;
			int i = 0;
			int num2 = Math.Min(num, totalSize);
			while (i < num2)
			{
				array[i] = value;
				i++;
			}
			while (i < totalSize)
			{
				Buffer.BlockCopy(array, 0, array, i * byteSize, Math.Min(num, totalSize - i) * byteSize);
				i += num;
				num *= 2;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0003ACC4 File Offset: 0x00038EC4
		public static T[] ShrinkArray<T>(T[] arr, int newLength)
		{
			newLength = Math.Min(newLength, arr.Length);
			T[] array = new T[newLength];
			Array.Copy(arr, array, newLength);
			return array;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0003ACEC File Offset: 0x00038EEC
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}
	}
}
