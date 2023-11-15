using System;
using System.Collections.Generic;

// Token: 0x0200013F RID: 319
public static class ShufflingExtention
{
	// Token: 0x0600083D RID: 2109 RVA: 0x00015810 File Offset: 0x00013A10
	public static void Shuffle<T>(this IList<T> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = ShufflingExtention.rng.Next(i + 1);
			T value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
	}

	// Token: 0x04000411 RID: 1041
	private static Random rng = new Random();
}
