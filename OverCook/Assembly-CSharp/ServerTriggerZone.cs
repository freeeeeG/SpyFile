using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class ServerTriggerZone : ServerSynchroniserBase
{
	// Token: 0x0600070E RID: 1806 RVA: 0x0002E436 File Offset: 0x0002C836
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerZone;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002E43A File Offset: 0x0002C83A
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerZone = (TriggerZone)synchronisedObject;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0002E450 File Offset: 0x0002C850
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_triggerZone.m_fallPad && this.m_collidersOccupying.Count > 0)
		{
			for (int i = 0; i < this.m_collidersOccupying.Count; i++)
			{
				if (!this.m_collidersOccupying[i].enabled)
				{
					this.m_collidersOccupying.Remove(this.m_collidersOccupying[i]);
				}
			}
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002E4CE File Offset: 0x0002C8CE
	private void SyncOccupied()
	{
		this.m_data.Initialise(this.IsOccupied());
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0002E4ED File Offset: 0x0002C8ED
	public bool IsOccupied()
	{
		return this.m_collidersOccupying.Count != 0;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002E500 File Offset: 0x0002C900
	private void OnTriggerEnter(Collider other)
	{
		if (this.m_collidersOccupying.Count == 0)
		{
			base.gameObject.SendTrigger(this.m_triggerZone.m_onOccupationTrigger);
		}
		this.m_collidersOccupying.Add(other);
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0002E534 File Offset: 0x0002C934
	private void OnTriggerExit(Collider other)
	{
		this.m_collidersOccupying.RemoveAll(new Predicate<Collider>(other.Equals));
		if (this.m_collidersOccupying.Count == 0)
		{
			base.gameObject.SendTrigger(this.m_triggerZone.m_onDeoccupationTrigger);
		}
	}

	// Token: 0x040005DD RID: 1501
	private TriggerZone m_triggerZone;

	// Token: 0x040005DE RID: 1502
	private TriggerZoneMessage m_data = new TriggerZoneMessage();

	// Token: 0x040005DF RID: 1503
	private List<Collider> m_collidersOccupying = new List<Collider>();
}
