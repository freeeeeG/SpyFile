using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005BB RID: 1467
public class ServerTriggerConveyorAdjacentUpdate : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06001BDA RID: 7130 RVA: 0x0008858D File Offset: 0x0008698D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_adjacentUpdate = (TriggerConveyorAdjacentUpdate)synchronisedObject;
		this.m_station = base.gameObject.RequireComponent<ServerConveyorStation>();
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x000885B3 File Offset: 0x000869B3
	public void OnTrigger(string _trigger)
	{
		if (this.m_adjacentUpdate.m_updateTrigger == _trigger)
		{
			this.m_station.UpdateAdjacentReceiver();
		}
	}

	// Token: 0x040015DE RID: 5598
	private TriggerConveyorAdjacentUpdate m_adjacentUpdate;

	// Token: 0x040015DF RID: 5599
	private ServerConveyorStation m_station;
}
