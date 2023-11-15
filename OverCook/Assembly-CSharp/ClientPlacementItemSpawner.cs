using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class ClientPlacementItemSpawner : ClientSynchroniserBase, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x0600191B RID: 6427 RVA: 0x0007F5E7 File Offset: 0x0007D9E7
	public override EntityType GetEntityType()
	{
		return EntityType.PlacementItemSpawner;
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x0007F5EC File Offset: 0x0007D9EC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_placementItemSpawner = (PlacementItemSpawner)synchronisedObject;
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
		this.m_attachStation = base.gameObject.RequestComponent<ClientAttachStation>();
		this.m_holderAdapter = new ClientPlacementItemSpawner.HolderAdapter(this);
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x0007F63A File Offset: 0x0007DA3A
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.OnItemSpawned();
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x0007F642 File Offset: 0x0007DA42
	private void OnItemSpawned()
	{
		base.gameObject.SendMessage("OnPickupItem", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x0007F658 File Offset: 0x0007DA58
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		ClientPlacementContainer clientPlacementContainer = _carrier.InspectCarriedItem().RequestComponent<ClientPlacementContainer>();
		return clientPlacementContainer != null && clientPlacementContainer.CanHandlePlacement(this.m_holderAdapter, -_directionXZ, _context);
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x0007F692 File Offset: 0x0007DA92
	public int GetPlacementPriority()
	{
		return this.m_placementItemSpawner.m_pickupPriority;
	}

	// Token: 0x0400141A RID: 5146
	private PlacementItemSpawner m_placementItemSpawner;

	// Token: 0x0400141B RID: 5147
	private IOrderDefinition m_orderDefinition;

	// Token: 0x0400141C RID: 5148
	private ClientAttachStation m_attachStation;

	// Token: 0x0400141D RID: 5149
	private ClientPlacementItemSpawner.HolderAdapter m_holderAdapter;

	// Token: 0x0200053D RID: 1341
	private class HolderAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x06001921 RID: 6433 RVA: 0x0007F69F File Offset: 0x0007DA9F
		public HolderAdapter(ClientPlacementItemSpawner _spawner)
		{
			this.m_itemSpawner = _spawner;
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0007F6AE File Offset: 0x0007DAAE
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0007F6B0 File Offset: 0x0007DAB0
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0007F6B2 File Offset: 0x0007DAB2
		public void CarryItem(GameObject _object)
		{
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0007F6B4 File Offset: 0x0007DAB4
		public GameObject TakeItem()
		{
			return null;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0007F6B7 File Offset: 0x0007DAB7
		public void DestroyCarriedItem()
		{
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0007F6B9 File Offset: 0x0007DAB9
		public GameObject InspectCarriedItem()
		{
			return this.m_itemSpawner.gameObject;
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0007F6C6 File Offset: 0x0007DAC6
		public GameObject AccessGameObject()
		{
			return this.m_itemSpawner.gameObject;
		}

		// Token: 0x0400141E RID: 5150
		private ClientPlacementItemSpawner m_itemSpawner;
	}
}
