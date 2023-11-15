using System;
using UnityEngine;

// Token: 0x020003BD RID: 957
[RequireComponent(typeof(HeatedStation))]
public class FurnaceCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000DDE RID: 3550
	[SerializeField]
	[Range(0f, 2f)]
	public float m_heatedAnimatorMinSpeed;

	// Token: 0x04000DDF RID: 3551
	[SerializeField]
	[Range(0f, 2f)]
	public float m_heatedAnimatorMaxSpeed = 1f;

	// Token: 0x04000DE0 RID: 3552
	[SerializeField]
	public Animator[] m_heatedAnimators = new Animator[0];

	// Token: 0x04000DE1 RID: 3553
	[Space]
	[SerializeField]
	public GameObject m_highEffect;

	// Token: 0x04000DE2 RID: 3554
	[SerializeField]
	public GameObject m_mediumEffect;

	// Token: 0x04000DE3 RID: 3555
	[SerializeField]
	public GameObject m_lowEffect;

	// Token: 0x04000DE4 RID: 3556
	public static readonly int s_heatedAnimatorParameterHash = Animator.StringToHash("FurnaceHeat");
}
