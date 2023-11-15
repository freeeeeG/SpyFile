using System;
using UnityEngine;

// Token: 0x020009D0 RID: 2512
public class AttachmentThrower : MonoBehaviour
{
	// Token: 0x0400276F RID: 10095
	[Range(0f, 90f)]
	[SerializeField]
	public float m_throwInclination;

	// Token: 0x04002770 RID: 10096
	[SerializeField]
	public float m_throwForce;
}
