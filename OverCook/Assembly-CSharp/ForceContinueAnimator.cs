using System;
using UnityEngine;

// Token: 0x02000A93 RID: 2707
[RequireComponent(typeof(Animator))]
public class ForceContinueAnimator : MonoBehaviour
{
	// Token: 0x06003590 RID: 13712 RVA: 0x000FA56D File Offset: 0x000F896D
	private void Update()
	{
		this.m_animator.speed = 1f;
	}

	// Token: 0x04002B0E RID: 11022
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Animator m_animator;
}
