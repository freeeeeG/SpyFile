using System;
using UnityEngine;

// Token: 0x0200052C RID: 1324
[RequireComponent(typeof(HandlePickupReferral))]
public class PickupItemSpawner : MonoBehaviour
{
	// Token: 0x040013F0 RID: 5104
	[SerializeField]
	public GameObject m_itemPrefab;

	// Token: 0x040013F1 RID: 5105
	[SerializeField]
	public int m_pickupPriority;

	// Token: 0x040013F2 RID: 5106
	[SerializeField]
	public int m_spawnCost = 1;
}
