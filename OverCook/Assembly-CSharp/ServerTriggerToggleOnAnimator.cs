using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class ServerTriggerToggleOnAnimator : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060006F8 RID: 1784 RVA: 0x0002E127 File Offset: 0x0002C527
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerToggleOnAnimator;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002E12B File Offset: 0x0002C52B
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerOnAnimator = (TriggerToggleOnAnimator)synchronisedObject;
		this.m_data.m_value = this.m_triggerOnAnimator.m_initialValue;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002E158 File Offset: 0x0002C558
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerOnAnimator.enabled && this.m_triggerOnAnimator.m_targetAnimator != null && this.m_triggerOnAnimator.m_triggerToReceive == _trigger)
		{
			this.m_data.m_value = !this.m_triggerOnAnimator.m_targetAnimator.GetBool(this.m_triggerOnAnimator.m_targetParameterHash);
			this.SendServerEvent(this.m_data);
		}
	}

	// Token: 0x040005CE RID: 1486
	private TriggerToggleOnAnimator m_triggerOnAnimator;

	// Token: 0x040005CF RID: 1487
	private TriggerToggleOnAnimatorMessage m_data = new TriggerToggleOnAnimatorMessage();
}
