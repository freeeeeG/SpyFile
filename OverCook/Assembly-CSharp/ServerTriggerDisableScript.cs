using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class ServerTriggerDisableScript : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060006AE RID: 1710 RVA: 0x0002D6BC File Offset: 0x0002BABC
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerDisable;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0002D6C0 File Offset: 0x0002BAC0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerDisable = (TriggerDisableScript)synchronisedObject;
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0002D6D5 File Offset: 0x0002BAD5
	private void SetEnabled(bool _enabled)
	{
		this.m_data.Initialise(_enabled);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002D6EF File Offset: 0x0002BAEF
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerDisable.m_enableTrigger == _trigger)
		{
			this.SetEnabled(true);
		}
		if (this.m_triggerDisable.m_disableTrigger == _trigger)
		{
			this.SetEnabled(false);
		}
	}

	// Token: 0x0400058F RID: 1423
	private TriggerDisableScript m_triggerDisable;

	// Token: 0x04000590 RID: 1424
	private TriggerDisableMessage m_data = new TriggerDisableMessage();
}
