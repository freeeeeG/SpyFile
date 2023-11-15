using System;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
[Serializable]
public class BossCampaignLevelConfig : DynamicCampaignLevelConfigBase
{
	// Token: 0x060020F8 RID: 8440 RVA: 0x0009DD74 File Offset: 0x0009C174
	public override RoundData GetRoundData()
	{
		return this.m_data;
	}

	// Token: 0x04001922 RID: 6434
	[Header("Stages")]
	public BossRoundData m_data = new BossRoundData();
}
