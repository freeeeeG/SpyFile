using System;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public class FurnaceOvenCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000DF0 RID: 3568
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	public Animator m_animator;

	// Token: 0x04000DF1 RID: 3569
	[SerializeField]
	public GameObject m_highEffect;

	// Token: 0x04000DF2 RID: 3570
	[SerializeField]
	public GameObject m_mediumEffect;

	// Token: 0x04000DF3 RID: 3571
	[SerializeField]
	public GameObject m_lowEffect;
}
