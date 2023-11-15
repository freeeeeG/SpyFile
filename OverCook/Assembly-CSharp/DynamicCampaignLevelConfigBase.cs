using System;
using System.Collections.Generic;

// Token: 0x020006CF RID: 1743
public abstract class DynamicCampaignLevelConfigBase : CampaignLevelConfigBase
{
	// Token: 0x06002107 RID: 8455 RVA: 0x0009DCE4 File Offset: 0x0009C0E4
	public override List<OrderDefinitionNode> GetAllRecipes()
	{
		DynamicRoundData dynamicRoundData = (DynamicRoundData)this.GetRoundData();
		List<OrderDefinitionNode> list = new List<OrderDefinitionNode>();
		for (int i = 0; i < dynamicRoundData.Phases.Length; i++)
		{
			for (int j = 0; j < dynamicRoundData.Phases[i].Recipes.m_recipes.Length; j++)
			{
				list.Add(dynamicRoundData.Phases[i].Recipes.m_recipes[j].m_order);
			}
		}
		return list;
	}
}
