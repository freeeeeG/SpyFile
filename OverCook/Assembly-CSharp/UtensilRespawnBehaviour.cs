using System;
using UnityEngine;

// Token: 0x0200061F RID: 1567
public class UtensilRespawnBehaviour : MonoBehaviour
{
	// Token: 0x040016EA RID: 5866
	[SerializeField]
	public float m_respawnTime = 5f;

	// Token: 0x040016EB RID: 5867
	[SerializeField]
	public float m_particleTime = 1f;

	// Token: 0x040016EC RID: 5868
	[SerializeField]
	public GameObject m_spawnEffect;
}
