using System;
using UnityEngine;

// Token: 0x020009CD RID: 2509
[RequireComponent(typeof(PlayerAttachmentCarrier))]
public class AttachmentCatcher : MonoBehaviour
{
	// Token: 0x04002765 RID: 10085
	[Range(0f, 360f)]
	[SerializeField]
	public float m_catchAngleMax;

	// Token: 0x04002766 RID: 10086
	[SerializeField]
	public float m_catchDistance;
}
