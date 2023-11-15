using System;
using UnityEngine;

// Token: 0x0200069B RID: 1691
[Serializable]
public class SurvivalModeOutroFlowroutineData : FlowroutineData
{
	// Token: 0x040018BF RID: 6335
	[SerializeField]
	public float m_minRatingDuration = 1f;

	// Token: 0x040018C0 RID: 6336
	[SerializeField]
	public float m_minOutroDuration = 20f;

	// Token: 0x040018C1 RID: 6337
	[SerializeField]
	public GameObject m_timesUpUIPrefab;

	// Token: 0x040018C2 RID: 6338
	[SerializeField]
	public float m_minTimesUpDuration = 3f;

	// Token: 0x040018C3 RID: 6339
	[SerializeField]
	public SurvivalModeRatingUIController m_survivalModeRatingUIController;

	// Token: 0x040018C4 RID: 6340
	[HideInInspector]
	[NonSerialized]
	public object m_scoreData;
}
