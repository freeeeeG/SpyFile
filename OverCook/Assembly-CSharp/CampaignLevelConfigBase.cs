using System;
using System.Collections.Generic;

// Token: 0x020006CD RID: 1741
public abstract class CampaignLevelConfigBase : KitchenLevelConfigBase
{
	// Token: 0x06002102 RID: 8450 RVA: 0x0009DC6C File Offset: 0x0009C06C
	public override float GetTimeLimit()
	{
		RoundData roundData = this.GetRoundData();
		return roundData.m_roundTimer;
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x0009DC88 File Offset: 0x0009C088
	public override List<OrderDefinitionNode> GetAllRecipes()
	{
		RoundData roundData = this.GetRoundData();
		List<OrderDefinitionNode> list = new List<OrderDefinitionNode>();
		for (int i = 0; i < roundData.m_recipes.m_recipes.Length; i++)
		{
			list.Add(roundData.m_recipes.m_recipes[i].m_order);
		}
		return list;
	}
}
