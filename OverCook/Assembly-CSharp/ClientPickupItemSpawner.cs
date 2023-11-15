using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200052B RID: 1323
public class ClientPickupItemSpawner : ClientSynchroniserBase, IClientHandlePickup, IBaseHandlePickup
{
	// Token: 0x060018CD RID: 6349 RVA: 0x0007E000 File Offset: 0x0007C400
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_pickupItemSpawner = (PickupItemSpawner)synchronisedObject;
		this.m_flammable = this.m_pickupItemSpawner.gameObject.RequestComponent<ClientFlammable>();
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_pickupItemSpawner.m_itemPrefab, new VoidGeneric<GameObject>(this.OnItemSpawned));
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x0007E058 File Offset: 0x0007C458
	public GameObject GetItemPrefab()
	{
		return this.m_pickupItemSpawner.m_itemPrefab;
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x0007E065 File Offset: 0x0007C465
	private void OnItemSpawned(GameObject _spawned)
	{
		base.gameObject.SendMessage("OnPickupItem", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x0007E078 File Offset: 0x0007C478
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return (this.m_flammable == null || !this.m_flammable.OnFire()) && !this.m_canHandlePickupCallbacks.CallForResult(false, _carrier);
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x0007E0AE File Offset: 0x0007C4AE
	public int GetPickupPriority()
	{
		return this.m_pickupItemSpawner.m_pickupPriority;
	}

	// Token: 0x040013ED RID: 5101
	private PickupItemSpawner m_pickupItemSpawner;

	// Token: 0x040013EE RID: 5102
	private List<Generic<bool, ICarrier>> m_canHandlePickupCallbacks = new List<Generic<bool, ICarrier>>();

	// Token: 0x040013EF RID: 5103
	private ClientFlammable m_flammable;
}
