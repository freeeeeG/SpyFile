using System;
using UnityEngine;

// Token: 0x020003FC RID: 1020
[AddComponentMenu("Scripts/CosmeticDecisions/TeleportalCosmeticDecisions")]
public class TeleportalCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000E8C RID: 3724
	[SerializeField]
	public Animator m_portalAnimator;

	// Token: 0x04000E8D RID: 3725
	[SerializeField]
	public string m_portalTeleportTrigger = string.Empty;
}
