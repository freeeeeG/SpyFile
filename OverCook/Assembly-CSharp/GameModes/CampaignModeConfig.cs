using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x02000696 RID: 1686
	[Serializable]
	public class CampaignModeConfig : Config
	{
		// Token: 0x040018AD RID: 6317
		[SerializeField]
		public GameObject m_uiPrefab;

		// Token: 0x040018AE RID: 6318
		[SerializeField]
		public ScoreScreenFlowroutineData m_scoreScreenData;
	}
}
