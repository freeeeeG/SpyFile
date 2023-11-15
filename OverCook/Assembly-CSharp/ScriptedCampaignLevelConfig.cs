using System;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
[Serializable]
public class ScriptedCampaignLevelConfig : CampaignLevelConfigBase
{
	// Token: 0x06002121 RID: 8481 RVA: 0x0009E300 File Offset: 0x0009C700
	public override RoundData GetRoundData()
	{
		return this.m_rounds[0];
	}

	// Token: 0x0400193A RID: 6458
	[Header("Rounds")]
	public ScriptedRoundData[] m_rounds;
}
