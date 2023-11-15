using System;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[AddComponentMenu("Scripts/Game/Environment/IngredientContainer")]
public class IngredientContainer : MonoBehaviour
{
	// Token: 0x060016B5 RID: 5813 RVA: 0x00076DEA File Offset: 0x000751EA
	public int GetCapacity()
	{
		return this.m_capacity;
	}

	// Token: 0x040010F9 RID: 4345
	[SerializeField]
	public int m_capacity = 3;

	// Token: 0x040010FA RID: 4346
	[SerializeField]
	public Collider m_onSurfaceTriggerZone;
}
