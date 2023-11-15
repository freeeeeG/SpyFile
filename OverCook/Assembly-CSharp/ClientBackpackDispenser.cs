using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class ClientBackpackDispenser : ClientSynchroniserBase, IClientHandlePickup, IBaseHandlePickup
{
	// Token: 0x06001CF7 RID: 7415 RVA: 0x0008E2F1 File Offset: 0x0008C6F1
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpack = (synchronisedObject as Backpack);
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x0008E306 File Offset: 0x0008C706
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return this.m_backpack.CanHandleDispenserPickup(_carrier);
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x0008E314 File Offset: 0x0008C714
	public int GetPickupPriority()
	{
		return 0;
	}

	// Token: 0x04001689 RID: 5769
	private Backpack m_backpack;

	// Token: 0x0400168A RID: 5770
	private ClientPickupItemSpawner m_itemSpawner;
}
