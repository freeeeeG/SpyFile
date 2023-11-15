using System;
using UnityEngine;

// Token: 0x0200054F RID: 1359
[RequireComponent(typeof(Stack))]
public class PlateStackBase : MonoBehaviour
{
	// Token: 0x060019A4 RID: 6564 RVA: 0x00070495 File Offset: 0x0006E895
	public virtual PlatingStepData GetPlatingStep()
	{
		return null;
	}

	// Token: 0x04001454 RID: 5204
	[SerializeField]
	public GameObject m_platePrefab;
}
