using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class ClientTriggerDisableScript : ClientSynchroniserBase
{
	// Token: 0x060006B3 RID: 1715 RVA: 0x0002D733 File Offset: 0x0002BB33
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerDisable;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0002D738 File Offset: 0x0002BB38
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerDisable = (TriggerDisableScript)synchronisedObject;
		if (this.m_triggerDisable.m_script != null)
		{
			this.m_triggerDisable.m_script.enabled = this.m_triggerDisable.m_startEnabled;
		}
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002D78C File Offset: 0x0002BB8C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TriggerDisableMessage triggerDisableMessage = (TriggerDisableMessage)serialisable;
		if (this.m_triggerDisable.m_script != null)
		{
			this.m_triggerDisable.m_script.enabled = triggerDisableMessage.m_enabled;
		}
	}

	// Token: 0x04000591 RID: 1425
	private TriggerDisableScript m_triggerDisable;
}
