using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000742 RID: 1858
	public static class WitchMasteryFormerPrice
	{
		// Token: 0x060025D4 RID: 9684 RVA: 0x000721E8 File Offset: 0x000703E8
		public static int GeRefundAmount(int stage, int level)
		{
			int num = 0;
			try
			{
				int[] array = WitchMasteryFormerPrice.prices[stage];
				for (int i = 0; i < level; i++)
				{
					num += array[i];
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
			return num;
		}

		// Token: 0x04001FFD RID: 8189
		private static readonly int[][] prices = new int[][]
		{
			new int[]
			{
				10,
				20,
				30,
				40,
				50,
				60,
				70,
				80,
				90,
				100
			},
			new int[]
			{
				10,
				30,
				60,
				90,
				120,
				150,
				180,
				210,
				240,
				270,
				300
			},
			new int[]
			{
				500,
				1000
			},
			new int[]
			{
				2000,
				4000
			}
		};
	}
}
