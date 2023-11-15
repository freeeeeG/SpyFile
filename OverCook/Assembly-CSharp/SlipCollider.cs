using System;
using UnityEngine;

// Token: 0x0200056A RID: 1386
[RequireComponent(typeof(CollisionRecorder))]
public class SlipCollider : MonoBehaviour
{
	// Token: 0x040014BA RID: 5306
	[SerializeField]
	public LayerMask m_slipFilter = -1;

	// Token: 0x040014BB RID: 5307
	[SerializeField]
	public bool m_deleteOnSlip = true;
}
