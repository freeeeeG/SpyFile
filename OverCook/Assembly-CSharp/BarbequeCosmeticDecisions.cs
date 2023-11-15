using System;
using UnityEngine;

// Token: 0x0200037E RID: 894
[RequireComponent(typeof(HeatedCookingStation), typeof(AttachStation))]
public class BarbequeCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000D1A RID: 3354
	[SerializeField]
	public GameObject m_highEffect;

	// Token: 0x04000D1B RID: 3355
	[SerializeField]
	public GameObject m_mediumEffect;

	// Token: 0x04000D1C RID: 3356
	[SerializeField]
	public GameObject m_lowEffect;
}
