using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
[ExecutionDependency(typeof(BootstrapManager))]
[ExecutionDependency(typeof(CompetitiveKitchenLoaderManager))]
[ExecutionDependency(typeof(CampaignKitchenLoaderManager))]
public abstract class KitchenFlowControllerBase : FlowControllerBase
{
	// Token: 0x060020F4 RID: 8436 RVA: 0x00095B16 File Offset: 0x00093F16
	public int CalculateBaseScore(RecipeList.Entry _entry)
	{
		return _entry.m_scoreForMeal;
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x00095B20 File Offset: 0x00093F20
	public int CalculateTip(float _remainingTimePercent)
	{
		RecipeTipBoundary[] array = this.m_gameConfig.TipBoundaries.FindAll((RecipeTipBoundary x) => x.PercentageTimeRemaining < _remainingTimePercent);
		KeyValuePair<int, RecipeTipBoundary> keyValuePair = array.FindHighestScoring((RecipeTipBoundary x) => x.PercentageTimeRemaining);
		if (keyValuePair.Value != null)
		{
			return keyValuePair.Value.ScoreValue;
		}
		return 0;
	}

	// Token: 0x04001920 RID: 6432
	[SerializeField]
	public int m_maxOrdersAllowed = 5;
}
