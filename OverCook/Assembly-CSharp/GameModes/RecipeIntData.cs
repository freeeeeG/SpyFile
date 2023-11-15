using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x020006B7 RID: 1719
	[CreateAssetMenu(fileName = "RecipeIntData", menuName = "Team17/Game Mode/Recipe Int Data", order = 129)]
	public class RecipeIntData : KeyValueData<OrderDefinitionNode, int>
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600208B RID: 8331 RVA: 0x0009D518 File Offset: 0x0009B918
		public int RecipeFailedPenalty
		{
			get
			{
				return this.m_recipeFailedPenalty;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x0009D520 File Offset: 0x0009B920
		public int[] RecipeDeliveryBonuses
		{
			get
			{
				return this.m_recipeDeliveryBonuses;
			}
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0009D528 File Offset: 0x0009B928
		public override int Get(OrderDefinitionNode key)
		{
			int num = this.IndexOf(key);
			if (num != -1)
			{
				return this.m_values[num];
			}
			if (this.m_baseData != null)
			{
				return this.m_baseData.Get(key);
			}
			return this.m_defaultValue;
		}

		// Token: 0x040018F9 RID: 6393
		[SerializeField]
		private RecipeIntData m_baseData;

		// Token: 0x040018FA RID: 6394
		[SerializeField]
		private int m_recipeFailedPenalty = 1;

		// Token: 0x040018FB RID: 6395
		[SerializeField]
		private int[] m_recipeDeliveryBonuses = new int[]
		{
			1,
			2,
			3
		};
	}
}
