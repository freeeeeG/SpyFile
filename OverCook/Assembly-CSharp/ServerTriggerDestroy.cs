using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class ServerTriggerDestroy : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060006A4 RID: 1700 RVA: 0x0002D5F5 File Offset: 0x0002B9F5
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_trigger = (TriggerDestroy)synchronisedObject;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0002D60A File Offset: 0x0002BA0A
	public void OnTrigger(string _name)
	{
		if (_name == this.m_trigger.m_trigger)
		{
			NetworkUtils.DestroyObject(base.gameObject);
		}
	}

	// Token: 0x0400058C RID: 1420
	private TriggerDestroy m_trigger;
}
