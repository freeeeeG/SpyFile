using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x020007BA RID: 1978
	[CreateAssetMenu(fileName = "recipe_money_data", menuName = "Team17/Game Mode/Recipe Money Data", order = 129)]
	public class RecipeMoneyData : KeyValueData<OrderDefinitionNode, int>
	{
		// Token: 0x060025E2 RID: 9698 RVA: 0x000B3368 File Offset: 0x000B1768
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

		// Token: 0x04001DC6 RID: 7622
		[SerializeField]
		private RecipeMoneyData m_baseData;
	}
}
