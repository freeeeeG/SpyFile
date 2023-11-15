using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200052E RID: 1326
public class ServerPickupItemSwitcher : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060018D7 RID: 6359 RVA: 0x0007E104 File Offset: 0x0007C504
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_pickupItemSwitcher = (PickupItemSwitcher)synchronisedObject;
		for (int i = 0; i < this.m_pickupItemSwitcher.m_itemPrefabs.Length; i++)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_pickupItemSwitcher.m_itemPrefabs[i]);
		}
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x0007E15A File Offset: 0x0007C55A
	public override EntityType GetEntityType()
	{
		return EntityType.PickupItemSwitcher;
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x0007E160 File Offset: 0x0007C560
	public void OnTrigger(string _trigger)
	{
		if (this.m_pickupItemSwitcher.enabled && _trigger == this.m_pickupItemSwitcher.m_switchTrigger)
		{
			this.m_currentItemPrefabIndex++;
			this.m_currentItemPrefabIndex %= this.m_pickupItemSwitcher.m_itemPrefabs.Length;
			this.m_message.m_itemIndex = this.m_currentItemPrefabIndex;
			this.SendServerEvent(this.m_message);
		}
	}

	// Token: 0x040013F5 RID: 5109
	private PickupItemSwitcher m_pickupItemSwitcher;

	// Token: 0x040013F6 RID: 5110
	private int m_currentItemPrefabIndex;

	// Token: 0x040013F7 RID: 5111
	private PickupItemSwitcherMessage m_message = new PickupItemSwitcherMessage();
}
