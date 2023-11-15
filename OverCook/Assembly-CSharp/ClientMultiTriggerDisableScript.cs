using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class ClientMultiTriggerDisableScript : ClientSynchroniserBase
{
	// Token: 0x060005F7 RID: 1527 RVA: 0x0002BA85 File Offset: 0x00029E85
	public override EntityType GetEntityType()
	{
		return EntityType.MultiTriggerDisable;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002BA8C File Offset: 0x00029E8C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerDisable = (MultiTriggerDisableScript)synchronisedObject;
		if (this.m_triggerDisable.m_script != null)
		{
			this.m_triggerDisable.m_script.enabled = this.m_triggerDisable.m_startEnabled;
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0002BAE0 File Offset: 0x00029EE0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TriggerDisableMessage triggerDisableMessage = (TriggerDisableMessage)serialisable;
		if (this.m_triggerDisable.m_script != null)
		{
			this.m_triggerDisable.m_script.enabled = triggerDisableMessage.m_enabled;
		}
	}

	// Token: 0x040004FF RID: 1279
	private MultiTriggerDisableScript m_triggerDisable;
}
