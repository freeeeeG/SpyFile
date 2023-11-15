using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class ServerTriggerOnAttach : ServerSynchroniserBase
{
	// Token: 0x060006C8 RID: 1736 RVA: 0x0002D948 File Offset: 0x0002BD48
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerOnAttach = (TriggerOnAttach)synchronisedObject;
		this.m_attachStation = this.m_triggerOnAttach.gameObject.RequireComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0002D9AC File Offset: 0x0002BDAC
	public void OnItemAdded(IAttachment _iHoldable)
	{
		if (this.m_triggerOnAttach.m_triggerTarget != null && !string.IsNullOrEmpty(this.m_triggerOnAttach.m_attachTrigger))
		{
			this.m_triggerOnAttach.m_triggerTarget.SendTrigger(this.m_triggerOnAttach.m_attachTrigger);
		}
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0002DA00 File Offset: 0x0002BE00
	public void OnItemRemoved(IAttachment _iHoldable)
	{
		if (this.m_triggerOnAttach.m_triggerTarget != null && !string.IsNullOrEmpty(this.m_triggerOnAttach.m_detachTrigger))
		{
			this.m_triggerOnAttach.m_triggerTarget.SendTrigger(this.m_triggerOnAttach.m_detachTrigger);
		}
	}

	// Token: 0x0400059F RID: 1439
	private TriggerOnAttach m_triggerOnAttach;

	// Token: 0x040005A0 RID: 1440
	private ServerAttachStation m_attachStation;
}
