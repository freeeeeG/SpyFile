using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class ServerAttachStation : ServerSynchroniserBase, IHandlePlacement, IHandlePickup, IPlacementSupression, IHandleCatch, IBaseHandlePlacement, IBaseHandlePickup
{
	// Token: 0x0600136D RID: 4973 RVA: 0x0006C59D File Offset: 0x0006A99D
	public override EntityType GetEntityType()
	{
		return EntityType.AttachStation;
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x0006C5A4 File Offset: 0x0006A9A4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_station = (AttachStation)synchronisedObject;
		this.m_pendingInit = true;
		this.m_attachmentCatchingProxy = base.gameObject.RequestComponent<ServerAttachmentCatchingProxy>();
		if (this.m_attachmentCatchingProxy != null)
		{
			this.m_attachmentCatchingProxy.RegisterUncatchableItemCallback(new GenericVoid<GameObject, Vector2>(this.HandleUncatchableItem));
		}
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x0006C604 File Offset: 0x0006AA04
	private void SendItemEvent()
	{
		this.m_data.m_item = ((this.m_item == null) ? null : this.m_item.AccessGameObject());
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0006C63C File Offset: 0x0006AA3C
	public bool CanHandlePickup(ICarrier _carrier)
	{
		if (this.m_item == null)
		{
			return false;
		}
		if (this.m_allowPickupCallbacks.CallForResult(false))
		{
			return false;
		}
		IHandlePickup handlePickup = this.m_item.AccessGameObject().RequireInterface<IHandlePickup>();
		return handlePickup.CanHandlePickup(_carrier);
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0006C684 File Offset: 0x0006AA84
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		IHandlePickup handlePickup = this.m_item.AccessGameObject().RequireInterface<IHandlePickup>();
		bool itemTaken = false;
		AttachChangedCallback callback = delegate(IParentable _parentable)
		{
			itemTaken = true;
		};
		this.m_item.RegisterAttachChangedCallback(callback);
		handlePickup.HandlePickup(_carrier, _directionXZ);
		this.m_item.UnregisterAttachChangedCallback(callback);
		if (itemTaken)
		{
			this.OnItemTaken();
		}
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x0006C6ED File Offset: 0x0006AAED
	public int GetPickupPriority()
	{
		if (this.m_item != null)
		{
			return this.m_station.m_pickupPriority;
		}
		return int.MinValue;
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x0006C70C File Offset: 0x0006AB0C
	public bool CanHandlePlacement(ICarrier _iCarrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = (this.m_item == null) ? null : this.m_item.AccessGameObject();
		IHandlePlacement placementHandler = null;
		if (gameObject != null)
		{
			placementHandler = gameObject.RequestInterface<IHandlePlacement>();
		}
		return this.m_station.CalculatePlacementType<IHandlePlacement>(gameObject, _context, _iCarrier, _directionXZ, placementHandler, this.m_allowPlacementCallbacks, this.GetHolder()) != PlacementType.NotValid;
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x0006C76D File Offset: 0x0006AB6D
	public bool CanAttachToSelf(GameObject _item, PlacementContext _context = default(PlacementContext))
	{
		return this.m_station.CanAttachToSelf((this.m_item == null) ? null : this.m_item.AccessGameObject(), _item, _context, this.m_allowPlacementCallbacks);
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x0006C7A0 File Offset: 0x0006ABA0
	public void HandlePlacement(ICarrier _iCarrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = (this.m_item == null) ? null : this.m_item.AccessGameObject();
		IHandlePlacement placementHandler = null;
		if (gameObject != null)
		{
			placementHandler = gameObject.RequestInterface<IHandlePlacement>();
		}
		switch (this.m_station.CalculatePlacementType<IHandlePlacement>(gameObject, _context, _iCarrier, _directionXZ, placementHandler, this.m_allowPlacementCallbacks, this.GetHolder()))
		{
		case PlacementType.OntoEmpty:
			this.OnItemPlaced(_iCarrier.TakeItem(), _directionXZ, _context);
			return;
		case PlacementType.ContentsOntoEmpty:
			this.OnContentsPlaced(_iCarrier.InspectCarriedItem(), _directionXZ, _context);
			return;
		case PlacementType.OntoOccupant:
			this.PlaceOntoOccupant(_iCarrier, _directionXZ, _context);
			return;
		case PlacementType.UnderOccupant:
			this.PlaceUnderOccupant(_iCarrier, _directionXZ, _context);
			return;
		case PlacementType.OntoAndUnderOccupant:
			this.PlaceOntoOccupant(_iCarrier, _directionXZ, _context);
			this.PlaceUnderOccupant(_iCarrier, _directionXZ, _context);
			return;
		default:
			return;
		}
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0006C864 File Offset: 0x0006AC64
	private void OnContentsPlaced(GameObject _container, Vector2 _directionXZ, PlacementContext _context)
	{
		ServerIngredientContainer serverIngredientContainer = _container.RequireComponent<ServerIngredientContainer>();
		if (serverIngredientContainer != null)
		{
			AssembledDefinitionNode assembledDefinitionNode = serverIngredientContainer.GetContents()[0];
			GameObject freeObject = assembledDefinitionNode.m_freeObject;
			serverIngredientContainer.Empty();
			this.OnItemPlaced(freeObject, _directionXZ, _context);
			freeObject.SetActive(true);
		}
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x0006C8AA File Offset: 0x0006ACAA
	public void RegisterFailedToPlace(VoidGeneric<GameObject> _callback)
	{
		this.m_failedToPlaceCallback = (VoidGeneric<GameObject>)Delegate.Combine(this.m_failedToPlaceCallback, _callback);
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x0006C8C3 File Offset: 0x0006ACC3
	public void UnregisterFailedToPlace(VoidGeneric<GameObject> _callback)
	{
		this.m_failedToPlaceCallback = (VoidGeneric<GameObject>)Delegate.Remove(this.m_failedToPlaceCallback, _callback);
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x0006C8DC File Offset: 0x0006ACDC
	public void OnFailedToPlace(GameObject _item)
	{
		this.m_failedToPlaceCallback(_item);
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x0006C8EC File Offset: 0x0006ACEC
	private void PlaceOntoOccupant(ICarrier _iCarrier, Vector2 _directionXZ, PlacementContext _context)
	{
		IHandlePlacement handlePlacement = this.m_item.AccessGameObject().RequireInterface<IHandlePlacement>();
		handlePlacement.HandlePlacement(_iCarrier, _directionXZ, _context);
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x0006C914 File Offset: 0x0006AD14
	private void PlaceUnderOccupant(ICarrier _iCarrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject obj = _iCarrier.InspectCarriedItem();
		ICarrier holder = this.GetHolder();
		if (holder.InspectCarriedItem() != null)
		{
			IHandlePlacement handlePlacement = obj.RequireInterface<IHandlePlacement>();
			handlePlacement.HandlePlacement(this.GetHolder(), _directionXZ, _context);
		}
		this.OnItemPlaced(_iCarrier.TakeItem(), _directionXZ, _context);
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x0006C963 File Offset: 0x0006AD63
	public int GetPlacementPriority()
	{
		if (this.m_item == null)
		{
			return this.m_station.m_placementPriority;
		}
		return int.MinValue;
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x0006C981 File Offset: 0x0006AD81
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_station.GetAttachPoint(gameObject);
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x0006C98F File Offset: 0x0006AD8F
	public bool HasItem()
	{
		return this.m_item != null;
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x0006C99D File Offset: 0x0006AD9D
	public GameObject InspectItem()
	{
		return (this.m_item == null) ? null : this.m_item.AccessGameObject();
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x0006C9BC File Offset: 0x0006ADBC
	public GameObject TakeItem()
	{
		GameObject result = this.m_item.AccessGameObject();
		this.m_item.Detach();
		this.OnItemTaken();
		return result;
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0006C9E7 File Offset: 0x0006ADE7
	public void AddItem(GameObject _item, Vector2 _directionXZ, PlacementContext _context = default(PlacementContext))
	{
		this.OnItemPlaced(_item, _directionXZ, _context);
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0006C9F4 File Offset: 0x0006ADF4
	private void OnItemTaken()
	{
		ServerHandlePickupReferral component = this.m_item.AccessGameObject().GetComponent<ServerHandlePickupReferral>();
		if (component && component.GetHandlePickupReferree() == this)
		{
			component.SetHandlePickupReferree(null);
		}
		ServerHandlePlacementReferral component2 = this.m_item.AccessGameObject().GetComponent<ServerHandlePlacementReferral>();
		if (component2 && component2.GetHandlePlacementReferree() == this)
		{
			component2.SetHandlePlacementReferree(null);
		}
		ServerAttachmentCatchingProxy component3 = this.m_item.AccessGameObject().GetComponent<ServerAttachmentCatchingProxy>();
		if (component3 && component3.GetHandleCatchingReferree() == this)
		{
			component3.SetHandleCatchingReferree(null);
		}
		foreach (ISurfacePlacementNotified surfacePlacementNotified in this.m_item.AccessGameObject().RequestInterfaces<ISurfacePlacementNotified>())
		{
			surfacePlacementNotified.OnSurfaceDeplacement(this);
		}
		IAttachment item = this.m_item;
		ServerLimitedQuantityItem component4 = this.m_item.AccessGameObject().GetComponent<ServerLimitedQuantityItem>();
		if (null != component4)
		{
			component4.UnregisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		this.m_item = null;
		this.SendItemEvent();
		this.m_itemRemoved(item);
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0006CB1C File Offset: 0x0006AF1C
	private void OnItemPlaced(GameObject _objectToPlace, Vector2 _directionXZ, PlacementContext _context)
	{
		IAttachment component = _objectToPlace.GetComponent<IAttachment>();
		component.Attach(this.m_station);
		component.AccessGameObject().transform.localRotation = this.GetRotationOnSurface(_directionXZ);
		this.m_item = component;
		ServerLimitedQuantityItem component2 = this.m_item.AccessGameObject().GetComponent<ServerLimitedQuantityItem>();
		if (null != component2)
		{
			component2.RegisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		ServerHandlePickupReferral component3 = this.m_item.AccessGameObject().GetComponent<ServerHandlePickupReferral>();
		if (component3)
		{
			component3.SetHandlePickupReferree(this);
		}
		ServerHandlePlacementReferral component4 = this.m_item.AccessGameObject().GetComponent<ServerHandlePlacementReferral>();
		if (component4)
		{
			component4.SetHandlePlacementReferree(this);
		}
		ServerAttachmentCatchingProxy component5 = this.m_item.AccessGameObject().GetComponent<ServerAttachmentCatchingProxy>();
		if (component5)
		{
			component5.SetHandleCatchingReferree(this);
		}
		foreach (ISurfacePlacementNotified surfacePlacementNotified in this.m_item.AccessGameObject().RequestInterfaces<ISurfacePlacementNotified>())
		{
			surfacePlacementNotified.OnSurfacePlacement(this);
		}
		this.m_itemAdded(component);
		this.SendItemEvent();
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x0006CC41 File Offset: 0x0006B041
	public void RegisterAllowItemPickup(Generic<bool> _allowPickupCallback)
	{
		this.m_allowPickupCallbacks.Add(_allowPickupCallback);
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x0006CC4F File Offset: 0x0006B04F
	public void UnregisterAllowItemPickup(Generic<bool> _allowPickupCallback)
	{
		this.m_allowPickupCallbacks.Remove(_allowPickupCallback);
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x0006CC5E File Offset: 0x0006B05E
	public void RegisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallbacks.Add(_allowPlacementCallback);
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x0006CC6C File Offset: 0x0006B06C
	public void UnregisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallbacks.Remove(_allowPlacementCallback);
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x0006CC7B File Offset: 0x0006B07B
	public void RegisterOnItemAdded(ServerAttachStation.OnItemAdded _added)
	{
		this.m_itemAdded = (ServerAttachStation.OnItemAdded)Delegate.Combine(this.m_itemAdded, _added);
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x0006CC94 File Offset: 0x0006B094
	public void UnregisterOnItemAdded(ServerAttachStation.OnItemAdded _added)
	{
		this.m_itemAdded = (ServerAttachStation.OnItemAdded)Delegate.Remove(this.m_itemAdded, _added);
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x0006CCAD File Offset: 0x0006B0AD
	public void RegisterOnItemRemoved(ServerAttachStation.OnItemRemoved _removed)
	{
		this.m_itemRemoved = (ServerAttachStation.OnItemRemoved)Delegate.Combine(this.m_itemRemoved, _removed);
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x0006CCC6 File Offset: 0x0006B0C6
	public void UnregisterOnItemRemoved(ServerAttachStation.OnItemRemoved _removed)
	{
		this.m_itemRemoved = (ServerAttachStation.OnItemRemoved)Delegate.Remove(this.m_itemRemoved, _removed);
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x0006CCDF File Offset: 0x0006B0DF
	public void RotateForDirection(Vector2 _playerDirectionXZ)
	{
		this.m_item.AccessGameObject().transform.localRotation = this.GetRotationOnSurface(_playerDirectionXZ);
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x0006CD00 File Offset: 0x0006B100
	private Quaternion GetRotationOnSurface(Vector2 _playerDirectionXZ)
	{
		Quaternion rot = Quaternion.LookRotation(VectorUtils.FromXZ(_playerDirectionXZ, 0f), Vector3.up);
		TransformHelper trans = new TransformHelper(rot, Vector3.zero);
		TransformHelper transformHelper = new TransformHelper(this.m_station.GetAttachPoint(base.gameObject));
		return transformHelper.ToLocal(trans).Rotation;
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x0006CD58 File Offset: 0x0006B158
	private void AttachInitialObjects()
	{
		Collider collider = base.gameObject.RequireComponent<Collider>();
		Bounds bounds = collider.bounds;
		float magnitude = bounds.extents.magnitude;
		foreach (Collider collider2 in Physics.OverlapSphere(bounds.center, magnitude))
		{
			if (collider2.gameObject != collider.gameObject && bounds.Contains(collider2.bounds.center))
			{
				IAttachment attachment = collider2.gameObject.RequestInterface<IAttachment>();
				if (attachment != null && !attachment.IsAttached() && this.CanAttachToSelf(collider2.gameObject, default(PlacementContext)))
				{
					this.AddItem(collider2.gameObject, collider2.transform.forward.XZ(), default(PlacementContext));
				}
			}
		}
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x0006CE4F File Offset: 0x0006B24F
	private void Awake()
	{
		this.m_holderAdapter = new ServerAttachStation.HolderAdapter(this);
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x0006CE5D File Offset: 0x0006B25D
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachmentCatchingProxy != null)
		{
			this.m_attachmentCatchingProxy.UnRegisterUncatchableItemCallback(new GenericVoid<GameObject, Vector2>(this.HandleUncatchableItem));
		}
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x0006CE8D File Offset: 0x0006B28D
	public override void UpdateSynchronising()
	{
		if (this.m_pendingInit)
		{
			this.m_pendingInit = false;
			this.AttachInitialObjects();
		}
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x0006CEA8 File Offset: 0x0006B2A8
	private void AttachObject(GameObject _object, PlacementContext _context = default(PlacementContext))
	{
		if (this.CanAttachToSelf(_object.gameObject, _context))
		{
			this.AddItem(_object.gameObject, (_object.transform.position - base.transform.position).SafeNormalised(base.transform.forward).XZ(), _context);
		}
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x0006CF04 File Offset: 0x0006B304
	private void HandleUncatchableItem(GameObject _object, Vector2 _directionXZ)
	{
		this.AttachObject(_object, default(PlacementContext));
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x0006CF24 File Offset: 0x0006B324
	public bool CanHandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		if (!_object.AllowCatch(this, _directionXZ))
		{
			return false;
		}
		if (this.HasItem())
		{
			IHandleCatch handleCatch = this.m_item.AccessGameObject().RequestInterface<IHandleCatch>();
			if (handleCatch != null)
			{
				return handleCatch.CanHandleCatch(_object, _directionXZ);
			}
		}
		IThrowable throwable = _object.AccessGameObject().RequestInterface<IThrowable>();
		return throwable != null && !throwable.IsFlying() && this.m_station.m_canCatch && this.CanAttachToSelf(_object.AccessGameObject(), new PlacementContext(PlacementContext.Source.Game));
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x0006CFB0 File Offset: 0x0006B3B0
	public void HandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		if (this.HasItem())
		{
			IHandleCatch handleCatch = this.m_item.AccessGameObject().RequestInterface<IHandleCatch>();
			if (handleCatch != null)
			{
				handleCatch.HandleCatch(_object, _directionXZ);
			}
		}
		else
		{
			this.AttachObject(_object.AccessGameObject(), default(PlacementContext));
		}
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x0006D001 File Offset: 0x0006B401
	public void AlertToThrownItem(ICatchable _thrown, IThrower _thrower, Vector2 _directionXZ)
	{
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x0006D003 File Offset: 0x0006B403
	public int GetCatchingPriority()
	{
		if (this.m_item != null)
		{
			return this.m_station.m_catchingPriority;
		}
		return int.MinValue;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x0006D024 File Offset: 0x0006B424
	private void OnAttachmentDestroyed(GameObject toDeDestroyed)
	{
		IAttachment attachment = toDeDestroyed.RequestInterface<IAttachment>();
		if (this.m_item != null && attachment == this.m_item)
		{
			this.TakeItem();
		}
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x0006D056 File Offset: 0x0006B456
	private ICarrier GetHolder()
	{
		return this.m_holderAdapter;
	}

	// Token: 0x04000F4B RID: 3915
	private AttachStation m_station;

	// Token: 0x04000F4C RID: 3916
	private AttachStationMessage m_data = new AttachStationMessage();

	// Token: 0x04000F4D RID: 3917
	private bool m_pendingInit;

	// Token: 0x04000F4E RID: 3918
	private ServerAttachmentCatchingProxy m_attachmentCatchingProxy;

	// Token: 0x04000F4F RID: 3919
	private IAttachment m_item;

	// Token: 0x04000F50 RID: 3920
	private ServerAttachStation.OnItemAdded m_itemAdded = delegate(IAttachment _iHoldable)
	{
	};

	// Token: 0x04000F51 RID: 3921
	private ServerAttachStation.OnItemRemoved m_itemRemoved = delegate(IAttachment _iHoldable)
	{
	};

	// Token: 0x04000F52 RID: 3922
	private List<Generic<bool, GameObject, PlacementContext>> m_allowPlacementCallbacks = new List<Generic<bool, GameObject, PlacementContext>>();

	// Token: 0x04000F53 RID: 3923
	private List<Generic<bool>> m_allowPickupCallbacks = new List<Generic<bool>>();

	// Token: 0x04000F54 RID: 3924
	private VoidGeneric<GameObject> m_failedToPlaceCallback = delegate(GameObject _object)
	{
	};

	// Token: 0x04000F55 RID: 3925
	private ServerAttachStation.HolderAdapter m_holderAdapter;

	// Token: 0x02000431 RID: 1073
	// (Invoke) Token: 0x0600139E RID: 5022
	public delegate void OnItemAdded(IAttachment _iHoldable);

	// Token: 0x02000432 RID: 1074
	// (Invoke) Token: 0x060013A2 RID: 5026
	public delegate void OnItemRemoved(IAttachment _iHoldable);

	// Token: 0x02000433 RID: 1075
	public class HolderAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x060013A5 RID: 5029 RVA: 0x0006D064 File Offset: 0x0006B464
		public HolderAdapter(ServerAttachStation _station)
		{
			this.m_attachStation = _station;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0006D073 File Offset: 0x0006B473
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0006D075 File Offset: 0x0006B475
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0006D077 File Offset: 0x0006B477
		public GameObject InspectCarriedItem()
		{
			return this.m_attachStation.InspectItem();
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0006D084 File Offset: 0x0006B484
		public GameObject AccessGameObject()
		{
			return this.m_attachStation.gameObject;
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0006D094 File Offset: 0x0006B494
		public void DestroyCarriedItem()
		{
			GameObject gameObject = this.TakeItem();
			NetworkUtils.DestroyObject(gameObject);
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0006D0B0 File Offset: 0x0006B4B0
		public void CarryItem(GameObject _object)
		{
			this.m_attachStation.AddItem(_object, this.m_attachStation.transform.forward.XZ(), default(PlacementContext));
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0006D0E7 File Offset: 0x0006B4E7
		public GameObject TakeItem()
		{
			return this.m_attachStation.TakeItem();
		}

		// Token: 0x04000F59 RID: 3929
		private ServerAttachStation m_attachStation;
	}
}
