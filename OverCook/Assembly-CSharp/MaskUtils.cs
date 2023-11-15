using System;
using System.Collections.Generic;

// Token: 0x0200025C RID: 604
public static class MaskUtils
{
	// Token: 0x06000B2A RID: 2858 RVA: 0x0003BF48 File Offset: 0x0003A348
	public static T[] ConvertToArray<T>(int _mask) where T : struct, IConvertible
	{
		int maxFlag = Enum.GetNames(typeof(T)).Length;
		List<int> list = MaskUtils.ConvertToIds(_mask, maxFlag);
		T[] array = new T[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = (T)((object)Enum.ToObject(typeof(T), list[i]));
		}
		return array;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x0003BFB8 File Offset: 0x0003A3B8
	public static int[] ConvertToIdArray(int _mask, int _maxFlag)
	{
		List<int> list = MaskUtils.ConvertToIds(_mask, _maxFlag);
		return list.ToArray();
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x0003BFD4 File Offset: 0x0003A3D4
	private static List<int> ConvertToIds(int _mask, int _maxFlag)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < _maxFlag; i++)
		{
			if ((_mask & 1 << i) != 0)
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0003C010 File Offset: 0x0003A410
	public static bool HasFlag<T>(int _mask, T _flag) where T : struct, IConvertible
	{
		int num = 1 << Convert.ToInt32(_flag);
		return (_mask & num) != 0;
	}
}
