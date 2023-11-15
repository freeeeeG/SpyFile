using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x02000698 RID: 1688
	[Serializable]
	public class SurvivalModeConfig : Config
	{
		// Token: 0x040018B0 RID: 6320
		[SerializeField]
		public GameObject m_uiPrefab;

		// Token: 0x040018B1 RID: 6321
		[SerializeField]
		public GameObject m_competitiveUIPrefab;

		// Token: 0x040018B2 RID: 6322
		[SerializeField]
		public float m_timeMultiplier = 1f;

		// Token: 0x040018B3 RID: 6323
		[SerializeField]
		public RecipeIntData m_recipeTimes;

		// Token: 0x040018B4 RID: 6324
		[SerializeField]
		public SurvivalModeOutroFlowroutineData m_outroFlowroutineData;
	}
}
