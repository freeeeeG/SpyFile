using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class ClientTriggerAnimationCoordinator : ClientSynchroniserBase, ITriggerReceiver
{
	// Token: 0x0600064E RID: 1614 RVA: 0x0002C8D7 File Offset: 0x0002ACD7
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_coordinator = (TriggerAnimationCoordinator)synchronisedObject;
		this.Initialise();
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0002C8F4 File Offset: 0x0002ACF4
	private void Initialise()
	{
		this.m_animatorOverride = new AnimatorOverrideController(this.m_coordinator.m_templateAnimator);
		this.m_animatorOverride.name = "Generic Override Animator";
		this.m_animatorOverride.GetOverrides(this.m_overrides);
		this.m_overrideIndex = this.m_overrides.FindIndex((KeyValuePair<AnimationClip, AnimationClip> x) => x.Key == this.m_coordinator.m_templateClip);
		this.m_animator = base.gameObject.AddComponent<Animator>();
		this.m_animator.hideFlags = HideFlags.NotEditable;
		this.m_animator.runtimeAnimatorController = this.m_animatorOverride;
		base.gameObject.AddComponent<AnimatorAudioComponent>();
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002C990 File Offset: 0x0002AD90
	public void TriggerAnimation(ITriggerAnimation _animator, AnimationClip _clip)
	{
		this.m_lastTriggerAnimation = _animator;
		this.m_lastClip = _clip;
		this.m_animator.CrossFade(this.m_coordinator.m_iAnimationReadyStateHash, 0f, 0, 0f);
		this.OverrideTemplateClip(_clip);
		if (this.m_coordinator.m_triggerOnAnimator)
		{
			this.m_animator.SetTrigger(this.m_coordinator.m_iAnimationStartTriggerHash);
		}
		else
		{
			base.gameObject.SendTrigger(this.m_coordinator.m_animationStartTrigger);
		}
		ITriggerAnimation[] array = base.gameObject.RequestInterfaces<ITriggerAnimation>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnNotifyAnimationTriggered(_animator, _clip);
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0002CA40 File Offset: 0x0002AE40
	private void OverrideTemplateClip(AnimationClip _clip)
	{
		KeyValuePair<AnimationClip, AnimationClip> value = new KeyValuePair<AnimationClip, AnimationClip>(this.m_coordinator.m_templateClip, _clip);
		this.m_overrides[this.m_overrideIndex] = value;
		this.m_animatorOverride.ApplyOverrides(this.m_overrides);
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0002CA83 File Offset: 0x0002AE83
	public void OnTrigger(string _trigger)
	{
		if (this.m_coordinator.m_animationFinishedTrigger == _trigger && this.m_lastTriggerAnimation != null)
		{
			this.m_lastTriggerAnimation.OnAnimationFinished(this.m_lastClip);
		}
	}

	// Token: 0x0400053D RID: 1341
	private TriggerAnimationCoordinator m_coordinator;

	// Token: 0x0400053E RID: 1342
	private ITriggerAnimation m_lastTriggerAnimation;

	// Token: 0x0400053F RID: 1343
	private AnimationClip m_lastClip;

	// Token: 0x04000540 RID: 1344
	private Animator m_animator;

	// Token: 0x04000541 RID: 1345
	private AnimatorOverrideController m_animatorOverride;

	// Token: 0x04000542 RID: 1346
	private List<KeyValuePair<AnimationClip, AnimationClip>> m_overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

	// Token: 0x04000543 RID: 1347
	private int m_overrideIndex = -1;
}
