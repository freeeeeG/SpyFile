using System;
using UnityEngine;

// Token: 0x0200081A RID: 2074
[RequireComponent(typeof(PhysicalAttachment))]
[RequireComponent(typeof(Collider))]
public class ThrowableItem : MonoBehaviour
{
	// Token: 0x04001F4C RID: 8012
	[SerializeField]
	public GameObject m_throwParticle;

	// Token: 0x04001F4D RID: 8013
	[SerializeField]
	public float m_throwerTimeout = 2f;
}
