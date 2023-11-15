using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class TriggerAnimationCoordinator : MonoBehaviour
{
	// Token: 0x06000655 RID: 1621 RVA: 0x0002CB00 File Offset: 0x0002AF00
	protected virtual void Awake()
	{
		this.m_iAnimationStartTriggerHash = Animator.StringToHash(this.m_animationStartTrigger);
		this.m_iAnimationFinishedTriggerHash = Animator.StringToHash(this.m_animationFinishedTrigger);
		this.m_iAnimationReadyStateHash = Animator.StringToHash(this.m_animationReadyState);
	}

	// Token: 0x04000544 RID: 1348
	[SerializeField]
	[AssignResource("BaseGenericAnimator", Editorbility.NonEditable)]
	public RuntimeAnimatorController m_templateAnimator;

	// Token: 0x04000545 RID: 1349
	[SerializeField]
	[AssignResource("Blank", Editorbility.NonEditable)]
	public AnimationClip m_templateClip;

	// Token: 0x04000546 RID: 1350
	[SerializeField]
	[ReadOnly]
	public string m_animationStartTrigger = "Animate";

	// Token: 0x04000547 RID: 1351
	[SerializeField]
	[ReadOnly]
	public string m_animationFinishedTrigger = "AnimationFinished";

	// Token: 0x04000548 RID: 1352
	[SerializeField]
	[ReadOnly]
	public string m_animationReadyState = "Idle";

	// Token: 0x04000549 RID: 1353
	[SerializeField]
	public bool m_triggerOnAnimator = true;

	// Token: 0x0400054A RID: 1354
	public int m_iAnimationStartTriggerHash;

	// Token: 0x0400054B RID: 1355
	public int m_iAnimationFinishedTriggerHash;

	// Token: 0x0400054C RID: 1356
	public int m_iAnimationReadyStateHash;
}
