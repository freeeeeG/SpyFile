using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class ServerTriggerAnimationQueue : ServerTimedQueue
{
	// Token: 0x06000657 RID: 1623 RVA: 0x0002CB40 File Offset: 0x0002AF40
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerQueue = (TriggerAnimationQueue)synchronisedObject;
		this.m_triggerQueue.RegisterAnimationFinishedCallback(new GenericVoid<AnimationClip>(this.OnAnimationFinished));
		this.m_triggerQueue.RegisterAnimationTriggeredCallback(new GenericVoid<ITriggerAnimation, AnimationClip>(this.OnNotifyAnimationTriggered));
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002CB8E File Offset: 0x0002AF8E
	public void OnAnimationFinished(AnimationClip _clip)
	{
		base.AdvanceQueue();
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0002CB96 File Offset: 0x0002AF96
	public void OnNotifyAnimationTriggered(ITriggerAnimation _animator, AnimationClip _clip)
	{
		if (_animator != this.m_triggerQueue)
		{
			base.ResetQueue();
		}
	}

	// Token: 0x0400054D RID: 1357
	private TriggerAnimationQueue m_triggerQueue;
}
