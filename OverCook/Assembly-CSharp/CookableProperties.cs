using System;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public class CookableProperties : MonoBehaviour
{
	// Token: 0x0600153B RID: 5435 RVA: 0x00073CFC File Offset: 0x000720FC
	public bool AllowsCookingStep(CookingStepData _stepData)
	{
		return Array.FindIndex<CookingStepData>(this.AllowedCookingSteps, (CookingStepData x) => x != null && x.m_uID == _stepData.m_uID) != -1;
	}

	// Token: 0x0400104D RID: 4173
	[SerializeField]
	private CookingStepData[] AllowedCookingSteps = new CookingStepData[0];
}
