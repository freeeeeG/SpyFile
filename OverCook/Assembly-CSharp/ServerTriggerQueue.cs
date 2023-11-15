using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class ServerTriggerQueue : ServerTimedQueue
{
	// Token: 0x060006D3 RID: 1747 RVA: 0x0002DB3F File Offset: 0x0002BF3F
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerQueue = (TriggerQueue)synchronisedObject;
		this.m_triggerQueue.RegisterFinishedCallback(new CallbackVoid(this.OnTriggerFinished));
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0002DB6B File Offset: 0x0002BF6B
	protected override void DoEvent(int _index)
	{
		base.DoEvent(_index);
		if (this.m_triggerQueue.m_targetType == TriggerQueue.TriggerType.Object || !this.m_triggerQueue.m_waitForFinished)
		{
			base.AdvanceQueue();
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0002DB9A File Offset: 0x0002BF9A
	public void OnTriggerFinished()
	{
		if (this.m_triggerQueue.m_targetType == TriggerQueue.TriggerType.Animator && this.m_triggerQueue.m_waitForFinished && base.IsActive())
		{
			base.AdvanceQueue();
		}
	}

	// Token: 0x040005A9 RID: 1449
	private TriggerQueue m_triggerQueue;
}
