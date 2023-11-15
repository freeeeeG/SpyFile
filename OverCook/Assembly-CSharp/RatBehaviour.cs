using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
[RequireComponent(typeof(GridNavigator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerAttachmentCarrier))]
[RequireComponent(typeof(Interactable))]
public class RatBehaviour : MonoBehaviour
{
	// Token: 0x04000401 RID: 1025
	[SerializeField]
	private int m_lives = 1;

	// Token: 0x04000402 RID: 1026
	[SerializeField]
	private float m_knockbackDistance = 3f;
}
