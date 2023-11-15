using System;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class FireHazardSpawner : MonoBehaviour
{
	// Token: 0x0400109F RID: 4255
	[SerializeField]
	public GameObject m_hazardPrefab;

	// Token: 0x040010A0 RID: 4256
	[SerializeField]
	public string m_spawnTrigger;

	// Token: 0x040010A1 RID: 4257
	[SerializeField]
	public bool m_alignToGrid = true;
}
