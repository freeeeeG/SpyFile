using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
public class ServerBackpackDispenser : ServerSynchroniserBase, IHandlePickup, IBaseHandlePickup
{
	// Token: 0x06001CF2 RID: 7410 RVA: 0x0008E27D File Offset: 0x0008C67D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpack = (synchronisedObject as Backpack);
		this.m_itemSpawner = base.gameObject.RequireComponent<ServerPickupItemSpawner>();
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x0008E2A3 File Offset: 0x0008C6A3
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return this.m_backpack.CanHandleDispenserPickup(_carrier) && this.m_itemSpawner.CanHandlePickup(_carrier);
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0008E2C5 File Offset: 0x0008C6C5
	public int GetPickupPriority()
	{
		return 0;
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0008E2C8 File Offset: 0x0008C6C8
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		ServerMessenger.Achievement(_carrier.AccessGameObject(), 502, 1);
		this.m_itemSpawner.HandlePickup(_carrier, _directionXZ);
	}

	// Token: 0x04001687 RID: 5767
	private Backpack m_backpack;

	// Token: 0x04001688 RID: 5768
	private ServerPickupItemSpawner m_itemSpawner;
}
