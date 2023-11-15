using System;
using UnityEngine;

// Token: 0x02000560 RID: 1376
public class RespawnCollider : MonoBehaviour
{
	// Token: 0x04001498 RID: 5272
	[SerializeField]
	public RespawnCollider.RespawnType m_respawnType = RespawnCollider.RespawnType.FallDeath;

	// Token: 0x04001499 RID: 5273
	[SerializeField]
	public LayerMask m_respawnFilter = -1;

	// Token: 0x0400149A RID: 5274
	[SerializeField]
	public bool m_onlyRespawnables;

	// Token: 0x0400149B RID: 5275
	[SerializeField]
	public string m_onRespawnTrigger = string.Empty;

	// Token: 0x0400149C RID: 5276
	[SerializeField]
	public GameObject m_onDeathEffect;

	// Token: 0x02000561 RID: 1377
	public enum RespawnType
	{
		// Token: 0x0400149E RID: 5278
		Hit,
		// Token: 0x0400149F RID: 5279
		Drowning,
		// Token: 0x040014A0 RID: 5280
		FallDeath,
		// Token: 0x040014A1 RID: 5281
		Car
	}
}
