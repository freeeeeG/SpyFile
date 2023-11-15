using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class ServerTriggerAdapter : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06000648 RID: 1608 RVA: 0x0002C862 File Offset: 0x0002AC62
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_adapter = (TriggerAdapter)synchronisedObject;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0002C877 File Offset: 0x0002AC77
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_adapter.m_inputTrigger)
		{
			base.gameObject.SendTrigger(this.m_adapter.m_outputTrigger);
		}
	}

	// Token: 0x0400053A RID: 1338
	private TriggerAdapter m_adapter;
}
