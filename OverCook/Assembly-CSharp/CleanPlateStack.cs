using System;
using UnityEngine;

// Token: 0x02000458 RID: 1112
[RequireComponent(typeof(Stack))]
[RequireComponent(typeof(HandlePickupReferral))]
[RequireComponent(typeof(HandlePlacementReferral))]
public class CleanPlateStack : PlateStackBase
{
	// Token: 0x0600148E RID: 5262 RVA: 0x000704A0 File Offset: 0x0006E8A0
	public override PlatingStepData GetPlatingStep()
	{
		Plate plate = this.m_platePrefab.RequestComponent<Plate>();
		return plate.m_platingStep;
	}
}
