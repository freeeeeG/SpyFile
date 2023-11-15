using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class ServerTriggerCounter : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x0600069C RID: 1692 RVA: 0x0002D482 File Offset: 0x0002B882
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_counter = (TriggerCounter)synchronisedObject;
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002D498 File Offset: 0x0002B898
	public void OnTrigger(string _trigger)
	{
		if (_trigger != this.m_counter.m_inputTrigger)
		{
			return;
		}
		this.m_iCounter++;
		if (this.m_iCounter != this.m_counter.m_count)
		{
			return;
		}
		base.gameObject.SendTrigger(this.m_counter.m_outputTrigger);
		if (!this.m_counter.m_ResetOnCountReached)
		{
			return;
		}
		this.m_iCounter = 0;
	}

	// Token: 0x04000581 RID: 1409
	private TriggerCounter m_counter;

	// Token: 0x04000582 RID: 1410
	private int m_iCounter;
}
