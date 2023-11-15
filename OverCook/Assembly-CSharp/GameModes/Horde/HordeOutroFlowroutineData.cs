using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007D4 RID: 2004
	[Serializable]
	public class HordeOutroFlowroutineData : FlowroutineData
	{
		// Token: 0x04001E8E RID: 7822
		[SerializeField]
		public GameObject m_failureOutroPrefab;

		// Token: 0x04001E8F RID: 7823
		[SerializeField]
		public float m_failureOutroDelaySeconds = 3f;

		// Token: 0x04001E90 RID: 7824
		[SerializeField]
		public GameObject m_successUIPrefab;

		// Token: 0x04001E91 RID: 7825
		[SerializeField]
		public GameObject m_failedUIPrefab;

		// Token: 0x04001E92 RID: 7826
		[SerializeField]
		public float m_successFailPrefabDelaySeconds = 3f;

		// Token: 0x04001E93 RID: 7827
		[SerializeField]
		public float m_minOutroDelaySeconds = 20f;

		// Token: 0x04001E94 RID: 7828
		[SerializeField]
		public HordeRatingUIController m_hordeRatingUIController;

		// Token: 0x04001E95 RID: 7829
		[HideInInspector]
		[NonSerialized]
		public bool m_success;

		// Token: 0x04001E96 RID: 7830
		[HideInInspector]
		[NonSerialized]
		public float m_health;

		// Token: 0x04001E97 RID: 7831
		[HideInInspector]
		[NonSerialized]
		public GameProgress.UnlockData[] m_unlocks;

		// Token: 0x04001E98 RID: 7832
		[HideInInspector]
		[NonSerialized]
		public object m_scoreData;
	}
}
