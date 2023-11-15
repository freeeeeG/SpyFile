using System;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class ClientTriggerAnimationQueue : ClientTimedQueue
{
	// Token: 0x0600065B RID: 1627 RVA: 0x0002CBB2 File Offset: 0x0002AFB2
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerQueue = (TriggerAnimationQueue)synchronisedObject;
		this.m_coordinator = base.gameObject.RequireComponent<ClientTriggerAnimationCoordinator>();
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002CBD8 File Offset: 0x0002AFD8
	protected override void DoEvent(int _index)
	{
		base.DoEvent(_index);
		AnimationClip clip = this.m_triggerQueue.m_queue.m_clips[_index];
		this.m_coordinator.TriggerAnimation(this.m_triggerQueue, clip);
	}

	// Token: 0x0400054E RID: 1358
	private TriggerAnimationQueue m_triggerQueue;

	// Token: 0x0400054F RID: 1359
	private ClientTriggerAnimationCoordinator m_coordinator;
}
