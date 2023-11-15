using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
[ExecutionDependency(typeof(TriggerAnimatorSetVariable))]
[RequireComponent(typeof(Animator))]
public class AnimatorCommunications : MonoBehaviour
{
	// Token: 0x06000553 RID: 1363 RVA: 0x00029F9D File Offset: 0x0002839D
	private void Awake()
	{
		this.m_animator = base.gameObject.RequireComponent<Animator>();
		this.m_animatorEnabled = this.m_animator.enabled;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00029FC1 File Offset: 0x000283C1
	public void FireAnimatorTrigger(string _trigger)
	{
		this.m_animator.SetTrigger(_trigger);
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00029FCF File Offset: 0x000283CF
	private void OnEnable()
	{
		this.LateUpdate();
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00029FD8 File Offset: 0x000283D8
	private void LateUpdate()
	{
		if (this.m_animator != null && this.m_animatorEnabled != this.m_animator.enabled)
		{
			this.m_animatorEnabled = this.m_animator.enabled;
			if (this.m_animatorEnabled)
			{
				this.AnimatorEnabledCallback();
			}
			else
			{
				this.AnimatorDisabledCallback();
			}
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0002A043 File Offset: 0x00028443
	private void OnDisable()
	{
		this.LateUpdate();
	}

	// Token: 0x04000496 RID: 1174
	private Animator m_animator;

	// Token: 0x04000497 RID: 1175
	private bool m_animatorEnabled;

	// Token: 0x04000498 RID: 1176
	public CallbackVoid AnimatorEnabledCallback = delegate()
	{
	};

	// Token: 0x04000499 RID: 1177
	public CallbackVoid AnimatorDisabledCallback = delegate()
	{
	};
}
