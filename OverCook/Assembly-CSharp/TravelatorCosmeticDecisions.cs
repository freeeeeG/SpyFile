using System;
using UnityEngine;

// Token: 0x02000409 RID: 1033
[AddComponentMenu("Scripts/CosmeticDecisions/TravelatorCosmeticDecisions")]
[RequireComponent(typeof(Travelator))]
public class TravelatorCosmeticDecisions : MonoBehaviour
{
	// Token: 0x060012BB RID: 4795 RVA: 0x0006915C File Offset: 0x0006755C
	private void Start()
	{
		this.m_travelator = base.gameObject.RequireComponent<Travelator>();
	}

	// Token: 0x04000EB8 RID: 3768
	private Travelator m_travelator;
}
