using System;
using UnityEngine;

// Token: 0x02000467 RID: 1127
[RequireComponent(typeof(Stack))]
[RequireComponent(typeof(HandlePlacementReferral))]
public class DirtyPlateStack : PlateStackBase
{
	// Token: 0x060014FB RID: 5371 RVA: 0x000727DE File Offset: 0x00070BDE
	public override PlatingStepData GetPlatingStep()
	{
		return this.m_plateType;
	}

	// Token: 0x04001025 RID: 4133
	[SerializeField]
	public PlatingStepData m_plateType;

	// Token: 0x04001026 RID: 4134
	[SerializeField]
	public GameObject m_washedPrefab;

	// Token: 0x04001027 RID: 4135
	[SerializeField]
	public GameObject m_cleanPlatePrefab;
}
