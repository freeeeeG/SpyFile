using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
[RequireComponent(typeof(TriggerAnimationCoordinator))]
public class TriggerAnimationQueue : TimedQueue, ITriggerAnimation
{
	// Token: 0x0600065E RID: 1630 RVA: 0x0002CC78 File Offset: 0x0002B078
	public void RegisterAnimationFinishedCallback(GenericVoid<AnimationClip> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<AnimationClip>)Delegate.Combine(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0002CC91 File Offset: 0x0002B091
	public void DeregisterAnimationFinishedCallback(GenericVoid<AnimationClip> _callback)
	{
		this.m_animFinishedCallback = (GenericVoid<AnimationClip>)Delegate.Remove(this.m_animFinishedCallback, _callback);
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0002CCAA File Offset: 0x0002B0AA
	public void RegisterAnimationTriggeredCallback(GenericVoid<ITriggerAnimation, AnimationClip> _callback)
	{
		this.m_animTriggeredCallback = (GenericVoid<ITriggerAnimation, AnimationClip>)Delegate.Combine(this.m_animTriggeredCallback, _callback);
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002CCC3 File Offset: 0x0002B0C3
	public void DeregisterAnimationTriggeredCallback(GenericVoid<ITriggerAnimation, AnimationClip> _callback)
	{
		this.m_animTriggeredCallback = (GenericVoid<ITriggerAnimation, AnimationClip>)Delegate.Remove(this.m_animTriggeredCallback, _callback);
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0002CCDC File Offset: 0x0002B0DC
	public override float GetQueueLength()
	{
		return (float)this.m_queue.m_clips.Length;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0002CCEC File Offset: 0x0002B0EC
	public override float GetDelay(int _index)
	{
		return this.m_queue.m_delays[_index];
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0002CCFB File Offset: 0x0002B0FB
	public void OnAnimationFinished(AnimationClip _clip)
	{
		this.m_animFinishedCallback(_clip);
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0002CD09 File Offset: 0x0002B109
	public void OnNotifyAnimationTriggered(ITriggerAnimation _animator, AnimationClip _clip)
	{
		this.m_animTriggeredCallback(_animator, _clip);
	}

	// Token: 0x04000550 RID: 1360
	[SerializeField]
	public TriggerAnimationQueue.AnimationQueue m_queue = new TriggerAnimationQueue.AnimationQueue();

	// Token: 0x04000551 RID: 1361
	private GenericVoid<AnimationClip> m_animFinishedCallback = delegate(AnimationClip _clip)
	{
	};

	// Token: 0x04000552 RID: 1362
	private GenericVoid<ITriggerAnimation, AnimationClip> m_animTriggeredCallback = delegate(ITriggerAnimation _animator, AnimationClip _clip)
	{
	};

	// Token: 0x0200016C RID: 364
	[Serializable]
	public class AnimationQueue
	{
		// Token: 0x04000555 RID: 1365
		[SerializeField]
		public AnimationClip[] m_clips = new AnimationClip[0];

		// Token: 0x04000556 RID: 1366
		[SerializeField]
		public float[] m_delays = new float[0];
	}
}
