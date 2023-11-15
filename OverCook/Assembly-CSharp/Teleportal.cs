using System;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
public class Teleportal : MonoBehaviour
{
	// Token: 0x04001611 RID: 5649
	[SerializeField]
	[AssignChild("TeleportPoint", Editorbility.NonEditable)]
	public Transform m_teleportPoint;

	// Token: 0x04001612 RID: 5650
	[SerializeField]
	[Range(0f, 180f)]
	public float m_teleportArc = 90f;

	// Token: 0x04001613 RID: 5651
	[SerializeField]
	public GameObject m_exitPortal;

	// Token: 0x04001614 RID: 5652
	[SerializeField]
	public float m_receiveDelay;

	// Token: 0x04001615 RID: 5653
	[SerializeField]
	public float m_cooldownTime;

	// Token: 0x04001616 RID: 5654
	[SerializeField]
	public bool m_allowImmediateReteleport;
}
