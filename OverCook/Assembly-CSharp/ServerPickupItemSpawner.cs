using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200052A RID: 1322
public class ServerPickupItemSpawner : ServerSynchroniserBase, IHandlePickup, IBaseHandlePickup
{
	// Token: 0x060018C7 RID: 6343 RVA: 0x0007DED8 File Offset: 0x0007C2D8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_pickupItemSpawner = (PickupItemSpawner)synchronisedObject;
		this.m_flammable = this.m_pickupItemSpawner.gameObject.RequestComponent<ServerFlammable>();
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_pickupItemSpawner.m_itemPrefab);
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x0007DF24 File Offset: 0x0007C324
	public GameObject GetItemPrefab()
	{
		return this.m_pickupItemSpawner.m_itemPrefab;
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x0007DF31 File Offset: 0x0007C331
	public int GetPickupPriority()
	{
		return this.m_pickupItemSpawner.m_pickupPriority;
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x0007DF3E File Offset: 0x0007C33E
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return (this.m_flammable == null || !this.m_flammable.OnFire()) && !this.m_canHandlePickupCallbacks.CallForResult(false, _carrier);
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x0007DF74 File Offset: 0x0007C374
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		Vector3 position = base.gameObject.transform.position;
		IParentable parentable = _carrier.AccessGameObject().RequestInterface<IParentable>();
		if (parentable as MonoBehaviour != null)
		{
			position = parentable.GetAttachPoint(this.m_pickupItemSpawner.m_itemPrefab).position;
		}
		GameObject @object = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_pickupItemSpawner.m_itemPrefab, position, Quaternion.identity);
		_carrier.CarryItem(@object);
	}

	// Token: 0x040013EA RID: 5098
	private PickupItemSpawner m_pickupItemSpawner;

	// Token: 0x040013EB RID: 5099
	private List<Generic<bool, ICarrier>> m_canHandlePickupCallbacks = new List<Generic<bool, ICarrier>>();

	// Token: 0x040013EC RID: 5100
	private ServerFlammable m_flammable;
}
