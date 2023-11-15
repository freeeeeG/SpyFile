using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class ServerTriggerColourCycle : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06000692 RID: 1682 RVA: 0x0002D31A File Offset: 0x0002B71A
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerColourCycle = (TriggerColourCycle)synchronisedObject;
		this.m_currentColourIndex = 0;
		this.m_message.m_colourIndex = 0;
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0002D342 File Offset: 0x0002B742
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerColourCycle;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002D348 File Offset: 0x0002B748
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerColourCycle.m_trigger == _trigger)
		{
			this.m_currentColourIndex = (this.m_currentColourIndex + 1) % this.m_triggerColourCycle.m_materials.Length;
			this.m_message.m_colourIndex = this.m_currentColourIndex;
			this.SendServerEvent(this.m_message);
		}
	}

	// Token: 0x0400057D RID: 1405
	private TriggerColourCycle m_triggerColourCycle;

	// Token: 0x0400057E RID: 1406
	private int m_currentColourIndex;

	// Token: 0x0400057F RID: 1407
	private TriggerColourCycleMessage m_message = new TriggerColourCycleMessage();
}
