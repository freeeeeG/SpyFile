using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class CollisionTrigger : MonoBehaviour
{
	// Token: 0x040004AA RID: 1194
	[SerializeField]
	public LayerMask m_collisionFilter = -1;

	// Token: 0x040004AB RID: 1195
	[SerializeField]
	public string m_trigger;
}
