using System;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public class LimitedQuantityItemManager : Manager
{
	// Token: 0x0400136E RID: 4974
	[SerializeField]
	public ParticleSystem m_DestroyPFXPrefab;

	// Token: 0x0400136F RID: 4975
	[SerializeField]
	public int m_MaxObjects = 30;

	// Token: 0x04001370 RID: 4976
	[Header("Limited Object Score Adjustments (highest score -> first to cull)")]
	[SerializeField]
	public float m_AttachedDeletionScoreModifier = -10000f;

	// Token: 0x04001371 RID: 4977
	[SerializeField]
	public float m_OrderComplexityMultiplier = -50f;
}
