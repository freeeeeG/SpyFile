using System;
using UnityEngine;

// Token: 0x020003EA RID: 1002
[RequireComponent(typeof(TriggerZone))]
public class PressureSwitchCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000E6B RID: 3691
	[SerializeField]
	public Material m_occupiedMaterial;

	// Token: 0x04000E6C RID: 3692
	[SerializeField]
	public Material m_unoccuppiedMaterial;

	// Token: 0x04000E6D RID: 3693
	[SerializeField]
	public Renderer m_buttonBit;

	// Token: 0x04000E6E RID: 3694
	[SerializeField]
	public float m_occupiedButtonVerticalOffset;
}
