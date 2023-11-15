using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200066F RID: 1647
public class AnimatedDynamicTransition : DynamicTransitionBase
{
	// Token: 0x06001F68 RID: 8040 RVA: 0x00098E2D File Offset: 0x0009722D
	protected virtual void Awake()
	{
		this.m_worldAnimatorTriggerHash = Animator.StringToHash(this.m_worldAnimatorTrigger);
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x00098E40 File Offset: 0x00097240
	public override void Setup(CallbackVoid _endTransitionCallback)
	{
		this.m_endTransitionCallback = _endTransitionCallback;
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x00098E4C File Offset: 0x0009724C
	public override IEnumerator Run()
	{
		this.m_worldAnimator.SetTrigger(this.m_worldAnimatorTriggerHash);
		yield return null;
		while (this.m_worldAnimator.GetBool(AnimatedDynamicTransition.m_iIsTransitioning))
		{
			yield return null;
		}
		this.m_worldAnimator.ResetTrigger(this.m_worldAnimatorTriggerHash);
		this.Shutdown();
		yield break;
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x00098E67 File Offset: 0x00097267
	private void Shutdown()
	{
		this.m_endTransitionCallback();
	}

	// Token: 0x040017EE RID: 6126
	[SerializeField]
	private Animator m_worldAnimator;

	// Token: 0x040017EF RID: 6127
	[SerializeField]
	private string m_worldAnimatorTrigger = string.Empty;

	// Token: 0x040017F0 RID: 6128
	private int m_worldAnimatorTriggerHash;

	// Token: 0x040017F1 RID: 6129
	private static readonly int m_iIsTransitioning = Animator.StringToHash("IsTransitioning");

	// Token: 0x040017F2 RID: 6130
	private CallbackVoid m_endTransitionCallback = delegate()
	{
	};
}
