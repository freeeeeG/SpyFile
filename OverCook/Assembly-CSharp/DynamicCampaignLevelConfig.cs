using System;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
[Serializable]
public class DynamicCampaignLevelConfig : DynamicCampaignLevelConfigBase
{
	// Token: 0x06002109 RID: 8457 RVA: 0x0009E295 File Offset: 0x0009C695
	public override RoundData GetRoundData()
	{
		return this.m_data;
	}

	// Token: 0x0400192F RID: 6447
	[Header("Stages")]
	public DynamicRoundData m_data = new DynamicRoundData();
}
