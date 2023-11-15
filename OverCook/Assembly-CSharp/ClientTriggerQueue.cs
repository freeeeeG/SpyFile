using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class ClientTriggerQueue : ClientTimedQueue
{
	// Token: 0x060006D7 RID: 1751 RVA: 0x0002DBD6 File Offset: 0x0002BFD6
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerQueue = (TriggerQueue)synchronisedObject;
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0002DBEC File Offset: 0x0002BFEC
	protected override void DoEvent(int _index)
	{
		base.DoEvent(_index);
		TriggerQueue.TriggerType targetType = this.m_triggerQueue.m_targetType;
		if (targetType != TriggerQueue.TriggerType.Animator)
		{
			if (targetType == TriggerQueue.TriggerType.Object)
			{
				this.m_triggerQueue.m_targetObject.SendTrigger(this.m_triggerQueue.m_queue.m_triggers[_index]);
			}
		}
		else
		{
			this.m_triggerQueue.m_animator.SetTrigger(this.m_triggerQueue.m_queue.m_triggerHashs[_index]);
		}
	}

	// Token: 0x040005AA RID: 1450
	private TriggerQueue m_triggerQueue;
}
