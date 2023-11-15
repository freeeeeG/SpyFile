using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class ClientTriggerZone : ClientSynchroniserBase
{
	// Token: 0x06000716 RID: 1814 RVA: 0x0002E588 File Offset: 0x0002C988
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerZone;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002E58C File Offset: 0x0002C98C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerZone = (TriggerZone)synchronisedObject;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0002E5A1 File Offset: 0x0002C9A1
	public bool IsOccupied()
	{
		return this.m_occupied;
	}

	// Token: 0x040005E0 RID: 1504
	private TriggerZone m_triggerZone;

	// Token: 0x040005E1 RID: 1505
	private bool m_occupied;
}
