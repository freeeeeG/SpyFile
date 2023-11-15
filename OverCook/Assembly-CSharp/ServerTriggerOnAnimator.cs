using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class ServerTriggerOnAnimator : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060006BD RID: 1725 RVA: 0x0002D838 File Offset: 0x0002BC38
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerOnAnimator;
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0002D83C File Offset: 0x0002BC3C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerOnAnimator = (TriggerOnAnimator)synchronisedObject;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0002D854 File Offset: 0x0002BC54
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerOnAnimator.enabled && this.m_triggerOnAnimator.m_targetAnimator != null && this.m_triggerOnAnimator.m_triggerToReceive == _trigger)
		{
			this.SendServerEvent(this.m_data);
		}
	}

	// Token: 0x04000598 RID: 1432
	private TriggerOnAnimator m_triggerOnAnimator;

	// Token: 0x04000599 RID: 1433
	private TriggerOnAnimatorMessage m_data = new TriggerOnAnimatorMessage();
}
