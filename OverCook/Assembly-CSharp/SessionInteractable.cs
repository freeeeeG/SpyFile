using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
public abstract class SessionInteractable : MonoBehaviour
{
	// Token: 0x0400159B RID: 5531
	[SerializeField]
	public string m_onSessionBegun = string.Empty;

	// Token: 0x0400159C RID: 5532
	[SerializeField]
	public string m_onSesionEnded = string.Empty;
}
