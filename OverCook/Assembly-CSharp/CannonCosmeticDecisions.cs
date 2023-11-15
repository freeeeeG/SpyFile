using System;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class CannonCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000D5F RID: 3423
	public Animator m_cannonAnimator;

	// Token: 0x04000D60 RID: 3424
	public GameObject m_targetVisuals;

	// Token: 0x04000D61 RID: 3425
	public Color m_defaultTargetColour;

	// Token: 0x04000D62 RID: 3426
	public float m_disabledTargetAlpha = 0.2f;

	// Token: 0x04000D63 RID: 3427
	public string m_aimStartTrigger;

	// Token: 0x04000D64 RID: 3428
	public string m_aimEndTrigger;

	// Token: 0x04000D65 RID: 3429
	public GameObject m_fireFX;

	// Token: 0x04000D66 RID: 3430
	public GameObject m_fuseFX;
}
