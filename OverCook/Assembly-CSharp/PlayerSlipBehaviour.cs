using System;
using UnityEngine;

// Token: 0x02000A22 RID: 2594
[RequireComponent(typeof(PlayerControls))]
public class PlayerSlipBehaviour : MonoBehaviour
{
	// Token: 0x04002966 RID: 10598
	private const float c_lerpEpsilon = 0.01f;

	// Token: 0x04002967 RID: 10599
	[SerializeField]
	public float m_downTime;

	// Token: 0x04002968 RID: 10600
	[SerializeField]
	[ReadOnly]
	public float m_fallTime = 0.38f;

	// Token: 0x04002969 RID: 10601
	[SerializeField]
	[ReadOnly]
	public float m_standTime = 0.42f;

	// Token: 0x0400296A RID: 10602
	[SerializeField]
	[ReadOnly]
	public string m_impactTrigger = "FallImpact";

	// Token: 0x0400296B RID: 10603
	[SerializeField]
	public PlayerSlipBehaviour.PFXReferences m_pfxReferences = new PlayerSlipBehaviour.PFXReferences();

	// Token: 0x02000A23 RID: 2595
	[Serializable]
	public class PFXReferences
	{
		// Token: 0x0400296C RID: 10604
		public GameObject m_slipEffect;

		// Token: 0x0400296D RID: 10605
		public GameObject m_streakEffect;

		// Token: 0x0400296E RID: 10606
		public GameObject m_impactEffect;
	}
}
