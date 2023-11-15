using System;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
[RequireComponent(typeof(Interactable))]
public class SwitchCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000E87 RID: 3719
	[SerializeField]
	public Material m_activeMaterial;

	// Token: 0x04000E88 RID: 3720
	[SerializeField]
	public Material m_inactiveMaterial;

	// Token: 0x04000E89 RID: 3721
	[SerializeField]
	public Renderer m_buttonBit;
}
