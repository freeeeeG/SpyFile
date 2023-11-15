using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
[RequireComponent(typeof(Animator))]
public class AnimatorOverrides : MonoBehaviour
{
	// Token: 0x0600055B RID: 1371 RVA: 0x0002A058 File Offset: 0x00028458
	private void Awake()
	{
		Animator animator = base.gameObject.RequireComponent<Animator>();
		this.overrideController = new AnimatorOverrideController();
		this.overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
		this.overrideController.clips = this.m_overrides;
		animator.runtimeAnimatorController = this.overrideController;
	}

	// Token: 0x0400049C RID: 1180
	[SerializeField]
	private AnimationClipPair[] m_overrides;

	// Token: 0x0400049D RID: 1181
	private AnimatorOverrideController overrideController;
}
