using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public static class ShuffleListExtensions
{
	// Token: 0x06000627 RID: 1575 RVA: 0x000179B0 File Offset: 0x00015BB0
	public static void Shuffle<T>(this IList<T> list)
	{
		Random random = new Random();
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = random.Next(i + 1);
			T value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x000179FC File Offset: 0x00015BFC
	public static T RandomItem<T>(this IList<T> list)
	{
		if (list.Count == 0)
		{
			throw new IndexOutOfRangeException("Cannot select a random item from an empty list");
		}
		return list[Random.Range(0, list.Count)];
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00017A24 File Offset: 0x00015C24
	public static T RemoveRandom<T>(this IList<T> list)
	{
		if (list.Count == 0)
		{
			throw new IndexOutOfRangeException("Cannot remove a random item from an empty list");
		}
		int index = Random.Range(0, list.Count);
		T result = list[index];
		list.RemoveAt(index);
		return result;
	}
}
