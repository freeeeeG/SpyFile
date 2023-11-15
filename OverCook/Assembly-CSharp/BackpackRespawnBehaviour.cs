using System;
using UnityEngine;

// Token: 0x020005F2 RID: 1522
public class BackpackRespawnBehaviour : MonoBehaviour
{
	// Token: 0x0400168B RID: 5771
	[SerializeField]
	public float m_respawnTime = 5f;

	// Token: 0x0400168C RID: 5772
	[SerializeField]
	public float m_particleTime = 1f;

	// Token: 0x0400168D RID: 5773
	[SerializeField]
	public GameObject m_spawnEffect;
}
