using System;
using UnityEngine;

// Token: 0x0200069A RID: 1690
[Serializable]
public class ScoreScreenFlowroutineData : FlowroutineData
{
	// Token: 0x040018B5 RID: 6325
	[SerializeField]
	public GameObject TimesUpUIPrefab;

	// Token: 0x040018B6 RID: 6326
	[SerializeField]
	public float TimesUpUILifetime = 3f;

	// Token: 0x040018B7 RID: 6327
	[SerializeField]
	public StarRatingUIController m_starRatingUIController;

	// Token: 0x040018B8 RID: 6328
	[SerializeField]
	public AwardAvatarUIController m_awardAvatarUIController;

	// Token: 0x040018B9 RID: 6329
	[SerializeField]
	public AwardSceneUIController m_awardSceneUIController;

	// Token: 0x040018BA RID: 6330
	[SerializeField]
	public float m_fTimeout = 20f;

	// Token: 0x040018BB RID: 6331
	[HideInInspector]
	[NonSerialized]
	public object m_scoreData;

	// Token: 0x040018BC RID: 6332
	[HideInInspector]
	[NonSerialized]
	public int m_points;

	// Token: 0x040018BD RID: 6333
	[HideInInspector]
	[NonSerialized]
	public int m_starsAwarded;

	// Token: 0x040018BE RID: 6334
	[HideInInspector]
	[NonSerialized]
	public GameProgress.UnlockData[] m_unlocks;
}
