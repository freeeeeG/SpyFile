using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class ClientAttachStation : ClientSynchroniserBase, IClientHandlePickup, IClientHandlePlacement, IClientHandleCatch, IBaseHandlePickup, IBaseHandlePlacement
{
	// Token: 0x060013AE RID: 5038 RVA: 0x0006D177 File Offset: 0x0006B577
	public override EntityType GetEntityType()
	{
		return EntityType.AttachStation;
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x0006D17C File Offset: 0x0006B57C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		AttachStationMessage attachStationMessage = (AttachStationMessage)serialisable;
		MonoBehaviour x = this.m_item as MonoBehaviour;
		GameObject x2 = (!(x != null)) ? null : this.m_item.AccessGameObject();
		if ((x == null && attachStationMessage.m_item != null) || (x != null && x2 != attachStationMessage.m_item))
		{
			if (attachStationMessage.m_item != null)
			{
				this.OnItemPlaced(attachStationMessage.m_item.RequireInterface<IClientAttachment>());
			}
			else
			{
				this.OnItemTaken();
			}
		}
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x0006D221 File Offset: 0x0006B621
	public void RegisterOnItemAdded(ClientAttachStation.OnItemAdded _added)
	{
		this.m_itemAdded = (ClientAttachStation.OnItemAdded)Delegate.Combine(this.m_itemAdded, _added);
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x0006D23A File Offset: 0x0006B63A
	public void UnregisterOnItemAdded(ClientAttachStation.OnItemAdded _added)
	{
		this.m_itemAdded = (ClientAttachStation.OnItemAdded)Delegate.Remove(this.m_itemAdded, _added);
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x0006D253 File Offset: 0x0006B653
	public void RegisterOnItemRemoved(ClientAttachStation.OnItemRemoved _removed)
	{
		this.m_itemRemoved = (ClientAttachStation.OnItemRemoved)Delegate.Combine(this.m_itemRemoved, _removed);
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x0006D26C File Offset: 0x0006B66C
	public void UnregisterOnItemRemoved(ClientAttachStation.OnItemRemoved _removed)
	{
		this.m_itemRemoved = (ClientAttachStation.OnItemRemoved)Delegate.Remove(this.m_itemRemoved, _removed);
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x0006D285 File Offset: 0x0006B685
	public void RegisterAllowItemPickup(Generic<bool> _allowPickupCallback)
	{
		this.m_allowPickupCallbacks.Add(_allowPickupCallback);
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x0006D293 File Offset: 0x0006B693
	public void UnregisterAllowItemPickup(Generic<bool> _allowPickupCallback)
	{
		this.m_allowPickupCallbacks.Remove(_allowPickupCallback);
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x0006D2A2 File Offset: 0x0006B6A2
	public void RegisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallbacks.Add(_allowPlacementCallback);
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x0006D2B0 File Offset: 0x0006B6B0
	public void UnregisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallbacks.Remove(_allowPlacementCallback);
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x0006D2BF File Offset: 0x0006B6BF
	private void Awake()
	{
		this.m_station = base.gameObject.RequireComponent<AttachStation>();
		this.m_holderAdapter = new ClientAttachStation.HolderAdapter(this);
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x0006D2DE File Offset: 0x0006B6DE
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x0006D2E7 File Offset: 0x0006B6E7
	public bool HasItem()
	{
		return this.m_item != null;
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x0006D2F5 File Offset: 0x0006B6F5
	public GameObject InspectItem()
	{
		return (this.m_item == null) ? null : this.m_item.AccessGameObject();
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x0006D314 File Offset: 0x0006B714
	private void OnItemPlaced(IClientAttachment _item)
	{
		this.m_item = _item;
		ClientLimitedQuantityItem component = this.m_item.AccessGameObject().GetComponent<ClientLimitedQuantityItem>();
		if (null != component)
		{
			component.RegisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		ClientHandlePickupReferral component2 = this.m_item.AccessGameObject().GetComponent<ClientHandlePickupReferral>();
		if (component2)
		{
			component2.SetHandlePickupReferree(this);
		}
		ClientHandlePlacementReferral clientHandlePlacementReferral = this.m_item.AccessGameObject().RequestComponent<ClientHandlePlacementReferral>();
		if (clientHandlePlacementReferral != null)
		{
			clientHandlePlacementReferral.SetHandlePlacementReferree(this);
		}
		foreach (IClientSurfacePlacementNotified clientSurfacePlacementNotified in this.m_item.AccessGameObject().RequestInterfaces<IClientSurfacePlacementNotified>())
		{
			clientSurfacePlacementNotified.OnSurfacePlacement(this);
		}
		this.m_itemAdded(this.m_item);
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x0006D3E8 File Offset: 0x0006B7E8
	private void OnItemTaken()
	{
		foreach (IClientSurfacePlacementNotified clientSurfacePlacementNotified in this.m_item.AccessGameObject().RequestInterfaces<IClientSurfacePlacementNotified>())
		{
			clientSurfacePlacementNotified.OnSurfaceDeplacement(this);
		}
		IClientAttachment item = this.m_item;
		this.m_item = null;
		ClientLimitedQuantityItem component = item.AccessGameObject().GetComponent<ClientLimitedQuantityItem>();
		if (null != component)
		{
			component.UnregisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		this.m_itemRemoved(item);
		ClientHandlePickupReferral component2 = item.AccessGameObject().GetComponent<ClientHandlePickupReferral>();
		if (component2 && component2.GetHandlePickupReferree() == this)
		{
			component2.SetHandlePickupReferree(null);
		}
		ClientHandlePlacementReferral clientHandlePlacementReferral = item.AccessGameObject().RequestComponent<ClientHandlePlacementReferral>();
		if (clientHandlePlacementReferral != null && clientHandlePlacementReferral.GetHandlePlacementReferree() == this)
		{
			clientHandlePlacementReferral.SetHandlePlacementReferree(null);
		}
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x0006D4CC File Offset: 0x0006B8CC
	private void OnAttachmentDestroyed(GameObject toDeDestroyed)
	{
		IClientAttachment clientAttachment = toDeDestroyed.RequestInterface<IClientAttachment>();
		if (this.m_item != null && clientAttachment == this.m_item)
		{
			this.OnItemTaken();
		}
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x0006D500 File Offset: 0x0006B900
	public bool CanHandlePickup(ICarrier _carrier)
	{
		MonoBehaviour x = this.m_item as MonoBehaviour;
		if (!(x != null))
		{
			return false;
		}
		if (this.m_allowPickupCallbacks.CallForResult(false))
		{
			return false;
		}
		IClientHandlePickup clientHandlePickup = this.m_item.AccessGameObject().RequireInterface<IClientHandlePickup>();
		return clientHandlePickup.CanHandlePickup(_carrier);
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x0006D552 File Offset: 0x0006B952
	public int GetPickupPriority()
	{
		if (this.m_item != null)
		{
			return this.m_station.m_pickupPriority;
		}
		return int.MinValue;
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x0006D570 File Offset: 0x0006B970
	public bool CanHandlePlacement(ICarrier _iCarrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = (this.m_item == null) ? null : this.m_item.AccessGameObject();
		IClientHandlePlacement placementHandler = null;
		if (gameObject != null)
		{
			placementHandler = gameObject.RequestInterface<IClientHandlePlacement>();
		}
		return this.m_station.CalculatePlacementType<IClientHandlePlacement>(gameObject, _context, _iCarrier, _directionXZ, placementHandler, this.m_allowPlacementCallbacks, this.GetHolder()) != PlacementType.NotValid;
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x0006D5D1 File Offset: 0x0006B9D1
	public int GetPlacementPriority()
	{
		if (this.m_item == null && this.m_station != null)
		{
			return this.m_station.m_placementPriority;
		}
		return int.MinValue;
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x0006D600 File Offset: 0x0006BA00
	private ICarrier GetHolder()
	{
		return this.m_holderAdapter;
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x0006D608 File Offset: 0x0006BA08
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_station.GetAttachPoint(gameObject);
	}

	// Token: 0x04000F5A RID: 3930
	private ClientAttachStation.HolderAdapter m_holderAdapter;

	// Token: 0x04000F5B RID: 3931
	private AttachStation m_station;

	// Token: 0x04000F5C RID: 3932
	private IClientAttachment m_item;

	// Token: 0x04000F5D RID: 3933
	private ClientAttachStation.OnItemAdded m_itemAdded = delegate(IClientAttachment _iHoldable)
	{
	};

	// Token: 0x04000F5E RID: 3934
	private ClientAttachStation.OnItemRemoved m_itemRemoved = delegate(IClientAttachment _iHoldable)
	{
	};

	// Token: 0x04000F5F RID: 3935
	private List<Generic<bool>> m_allowPickupCallbacks = new List<Generic<bool>>();

	// Token: 0x04000F60 RID: 3936
	private List<Generic<bool, GameObject, PlacementContext>> m_allowPlacementCallbacks = new List<Generic<bool, GameObject, PlacementContext>>();

	// Token: 0x02000435 RID: 1077
	public class HolderAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x060013C7 RID: 5063 RVA: 0x0006D61A File Offset: 0x0006BA1A
		public HolderAdapter(ClientAttachStation _station)
		{
			this.m_attachStation = _station;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0006D629 File Offset: 0x0006BA29
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0006D62B File Offset: 0x0006BA2B
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0006D62D File Offset: 0x0006BA2D
		public GameObject InspectCarriedItem()
		{
			return this.m_attachStation.InspectItem();
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0006D63A File Offset: 0x0006BA3A
		public GameObject AccessGameObject()
		{
			return this.m_attachStation.gameObject;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0006D647 File Offset: 0x0006BA47
		public void DestroyCarriedItem()
		{
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0006D649 File Offset: 0x0006BA49
		public void CarryItem(GameObject _object)
		{
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0006D64B File Offset: 0x0006BA4B
		public GameObject TakeItem()
		{
			return null;
		}

		// Token: 0x04000F63 RID: 3939
		private ClientAttachStation m_attachStation;
	}

	// Token: 0x02000436 RID: 1078
	// (Invoke) Token: 0x060013D0 RID: 5072
	public delegate void OnItemAdded(IClientAttachment _iHoldable);

	// Token: 0x02000437 RID: 1079
	// (Invoke) Token: 0x060013D4 RID: 5076
	public delegate void OnItemRemoved(IClientAttachment _iHoldable);
}
