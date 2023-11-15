using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class ServerMultiTriggerAdapter : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060005EC RID: 1516 RVA: 0x0002B8B4 File Offset: 0x00029CB4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_adapter = (MultiTriggerAdapter)synchronisedObject;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002B8CC File Offset: 0x00029CCC
	public void OnTrigger(string _trigger)
	{
		for (int i = 0; i < this.m_adapter.m_adapters.Count; i++)
		{
			if (_trigger == this.m_adapter.m_adapters[i].m_inputTrigger)
			{
				base.gameObject.SendTrigger(this.m_adapter.m_adapters[i].m_outputTrigger);
			}
		}
	}

	// Token: 0x040004F8 RID: 1272
	private MultiTriggerAdapter m_adapter;
}
