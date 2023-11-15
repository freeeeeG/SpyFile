using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class ServerMultiTriggerDisableScript : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060005F2 RID: 1522 RVA: 0x0002B972 File Offset: 0x00029D72
	public override EntityType GetEntityType()
	{
		return EntityType.MultiTriggerDisable;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0002B978 File Offset: 0x00029D78
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerDisable = (MultiTriggerDisableScript)synchronisedObject;
		this.m_triggers = new bool[this.m_triggerDisable.m_triggers.Length];
		if (this.m_triggerDisable.m_startEnabled)
		{
			for (int i = 0; i < this.m_triggers.Length; i++)
			{
				this.m_triggers[i] = true;
			}
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0002B9E2 File Offset: 0x00029DE2
	private void SetEnabled(bool _enabled)
	{
		this.m_data.Initialise(_enabled);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0002B9FC File Offset: 0x00029DFC
	public void OnTrigger(string _trigger)
	{
		bool flag = true;
		for (int i = 0; i < this.m_triggers.Length; i++)
		{
			TriggerPair triggerPair = this.m_triggerDisable.m_triggers[i];
			if (triggerPair.m_enableTrigger == _trigger)
			{
				this.m_triggers[i] = true;
			}
			else if (triggerPair.m_disableTrigger == _trigger)
			{
				this.m_triggers[i] = false;
			}
			flag &= this.m_triggers[i];
		}
		this.SetEnabled(flag);
	}

	// Token: 0x040004FC RID: 1276
	private MultiTriggerDisableScript m_triggerDisable;

	// Token: 0x040004FD RID: 1277
	private bool[] m_triggers;

	// Token: 0x040004FE RID: 1278
	private TriggerDisableMessage m_data = new TriggerDisableMessage();
}
