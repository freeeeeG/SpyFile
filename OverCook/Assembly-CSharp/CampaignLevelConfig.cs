using System;
using UnityEngine;

// Token: 0x020006CE RID: 1742
[Serializable]
public class CampaignLevelConfig : CampaignLevelConfigBase
{
	// Token: 0x06002105 RID: 8453 RVA: 0x0009E278 File Offset: 0x0009C678
	public override RoundData GetRoundData()
	{
		return this.m_rounds[0];
	}

	// Token: 0x0400192E RID: 6446
	[Header("Rounds")]
	public RoundData[] m_rounds;
}
