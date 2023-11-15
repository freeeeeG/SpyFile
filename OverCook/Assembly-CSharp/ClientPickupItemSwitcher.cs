using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class ClientPickupItemSwitcher : ClientSynchroniserBase
{
	// Token: 0x060018DB RID: 6363 RVA: 0x0007E1E0 File Offset: 0x0007C5E0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_pickupItemSwitcher = (PickupItemSwitcher)synchronisedObject;
		for (int i = 0; i < this.m_pickupItemSwitcher.m_itemPrefabs.Length; i++)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_pickupItemSwitcher.m_itemPrefabs[i], new VoidGeneric<GameObject>(this.OnItemSpawned));
		}
		this.m_pickupItemSpawner = this.m_pickupItemSwitcher.GetComponent<PickupItemSpawner>();
		if (this.m_pickupItemSwitcher.m_itemPrefabs.Length > 0)
		{
			this.m_pickupItemSpawner.m_itemPrefab = this.m_pickupItemSwitcher.m_itemPrefabs[0];
		}
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x0007E280 File Offset: 0x0007C680
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		PickupItemSwitcherMessage pickupItemSwitcherMessage = (PickupItemSwitcherMessage)serialisable;
		this.m_pickupItemSpawner.m_itemPrefab = this.m_pickupItemSwitcher.m_itemPrefabs[pickupItemSwitcherMessage.m_itemIndex];
		base.gameObject.SendMessage("OnItemSwitched", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x0007E2C2 File Offset: 0x0007C6C2
	public override EntityType GetEntityType()
	{
		return EntityType.PickupItemSwitcher;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x0007E2C6 File Offset: 0x0007C6C6
	public void OnItemSpawned(GameObject _spawned)
	{
		base.gameObject.SendMessage("OnPickupItem", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x040013F8 RID: 5112
	private PickupItemSwitcher m_pickupItemSwitcher;

	// Token: 0x040013F9 RID: 5113
	private PickupItemSpawner m_pickupItemSpawner;
}
