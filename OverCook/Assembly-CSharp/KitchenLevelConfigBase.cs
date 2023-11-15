using System;
using GameModes;
using UnityEngine;

// Token: 0x020006CC RID: 1740
public abstract class KitchenLevelConfigBase : LevelConfigBase
{
	// Token: 0x060020FF RID: 8447
	public abstract RoundData GetRoundData();

	// Token: 0x06002100 RID: 8448
	public abstract float GetTimeLimit();

	// Token: 0x04001924 RID: 6436
	[Header("Order Parameters")]
	public float m_orderLifetime = 100f;

	// Token: 0x04001925 RID: 6437
	public float m_timeBetweenOrders = 15f;

	// Token: 0x04001926 RID: 6438
	public float m_plateReturnTime = 10f;

	// Token: 0x04001927 RID: 6439
	[Range(0f, 20f)]
	[SerializeField]
	public int m_recipesBeforeTimerStarts;

	// Token: 0x04001928 RID: 6440
	[Header("Freestyle Recipes")]
	public int[] m_freestyleTimes = new int[0];

	// Token: 0x04001929 RID: 6441
	public int[] m_freestyleScoreThresholds = new int[0];

	// Token: 0x0400192A RID: 6442
	public int m_freestyleComboInterval = 4;

	// Token: 0x0400192B RID: 6443
	[Header("Game Mode Configs")]
	[SerializeField]
	public CampaignModeConfig m_campaignConfig = new CampaignModeConfig();

	// Token: 0x0400192C RID: 6444
	[SerializeField]
	public PracticeModeConfig m_practiceConfig = new PracticeModeConfig();

	// Token: 0x0400192D RID: 6445
	[SerializeField]
	public SurvivalModeConfig m_survivalConfig = new SurvivalModeConfig();
}
