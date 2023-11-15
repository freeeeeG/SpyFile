using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x020006B8 RID: 1720
	internal static class SurvivalModeUtil
	{
		// Token: 0x0600208E RID: 8334 RVA: 0x0009D575 File Offset: 0x0009B975
		public static int CalculateDeliveryBonus(SurvivalModeConfig config, float percentage)
		{
			return config.m_recipeTimes.RecipeDeliveryBonuses[Mathf.Clamp(Mathf.FloorToInt(percentage * (float)config.m_recipeTimes.RecipeDeliveryBonuses.Length), 0, config.m_recipeTimes.RecipeDeliveryBonuses.Length - 1)];
		}
	}
}
