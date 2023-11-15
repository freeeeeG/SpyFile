using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class ServerPreparationContainer : ServerSynchroniserBase, IHandlePlacement, IOrderDefinition, IContainerTransferBehaviour, IPlaceUnder, IHandleOrderModification, IBaseHandlePlacement
{
	// Token: 0x060015A8 RID: 5544 RVA: 0x0007327C File Offset: 0x0007167C
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_preparationContainer = (PreparationContainer)synchronisedObject;
		this.m_itemContainer = base.gameObject.GetComponent<ServerIngredientContainer>();
		this.m_itemContainer.RegisterContentsChangedCallback(delegate(AssembledDefinitionNode[] _contents)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		});
		ServerThrowableItem serverThrowableItem = base.gameObject.RequestComponent<ServerThrowableItem>();
		if (serverThrowableItem != null)
		{
			serverThrowableItem.RegisterCanThrowCallback(new Generic<bool>(this.AllowThrowing));
		}
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x000732E8 File Offset: 0x000716E8
	public virtual bool CanTransferToContainer(IIngredientContents _container)
	{
		if (_container.HasContents())
		{
			for (int i = 0; i < _container.GetContentsCount(); i++)
			{
				AssembledDefinitionNode contentsElement = _container.GetContentsElement(i);
				if (!this.CanAddIngredient(contentsElement))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x00073330 File Offset: 0x00071730
	public virtual void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove)
	{
		if (_dontRemove)
		{
			this.TestAddToOtherContainer(_container);
		}
		else
		{
			GameObject x = _carrier.InspectCarriedItem();
			if (x == base.gameObject)
			{
				CookedCompositeAssembledNode cookedCompositeAssembledNode = this.GetOrderComposition() as CookedCompositeAssembledNode;
				if (cookedCompositeAssembledNode != null)
				{
					_carrier.TakeItem();
					this.AddToOtherContainer(_container);
				}
				else
				{
					this.AddToOtherContainer(_container);
					_carrier.DestroyCarriedItem();
				}
			}
			else
			{
				ServerHandlePickupReferral serverHandlePickupReferral = base.gameObject.RequestComponent<ServerHandlePickupReferral>();
				if (serverHandlePickupReferral != null)
				{
					IHandlePickup handlePickupReferree = serverHandlePickupReferral.GetHandlePickupReferree();
					if (handlePickupReferree != null && handlePickupReferree as ServerAttachStation != null)
					{
						ServerAttachStation serverAttachStation = handlePickupReferree as ServerAttachStation;
						if (serverAttachStation.HasItem())
						{
							GameObject gameObject = serverAttachStation.TakeItem();
							this.AddToOtherContainer(_container);
							if (!(this.GetOrderComposition() is CookedCompositeAssembledNode))
							{
								NetworkUtils.DestroyObject(gameObject);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x00073414 File Offset: 0x00071814
	public void TestAddToOtherContainer(IIngredientContents _container)
	{
		CompositeAssembledNode asOrderComposite = this.GetAsOrderComposite();
		if (_container.HasContents())
		{
			for (int i = 0; i < _container.GetContentsCount(); i++)
			{
				AssembledDefinitionNode contentsElement = _container.GetContentsElement(i);
				asOrderComposite.AddOrderNode(contentsElement, true);
			}
			_container.Empty();
		}
		_container.AddIngredient(asOrderComposite);
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x00073468 File Offset: 0x00071868
	public void AddToOtherContainer(IIngredientContents _container)
	{
		if (_container.HasContents())
		{
			for (int i = 0; i < _container.GetContentsCount(); i++)
			{
				AssembledDefinitionNode contentsElement = _container.GetContentsElement(i);
				this.m_itemContainer.AddIngredient(contentsElement);
			}
			_container.Empty();
		}
		CompositeAssembledNode asOrderComposite = this.GetAsOrderComposite();
		_container.AddIngredient(asOrderComposite);
		asOrderComposite.m_freeObject = base.gameObject;
		ServerPhysicalAttachment component = base.GetComponent<ServerPhysicalAttachment>();
		if (component != null)
		{
			component.ManualDisable(true);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x000734F4 File Offset: 0x000718F4
	protected virtual bool CanAddIngredient(AssembledDefinitionNode _toAdd)
	{
		CompositeAssembledNode asOrderComposite = this.GetAsOrderComposite();
		return asOrderComposite.CanAddOrderNode(_toAdd, true);
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x00073510 File Offset: 0x00071910
	private AssembledDefinitionNode[] GetOrderDefinitionOfCarriedItem(GameObject _carriedItem)
	{
		IBaseCookable cookingHandler = null;
		ServerCookableContainer component = _carriedItem.GetComponent<ServerCookableContainer>();
		if (component != null)
		{
			cookingHandler = component.GetCookingHandler();
		}
		return this.m_preparationContainer.GetOrderDefinitionOfCarriedItem(_carriedItem, this.m_itemContainer, cookingHandler);
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0007354C File Offset: 0x0007194C
	public bool CanAddOrderContents(AssembledDefinitionNode[] _contents)
	{
		if (_contents != null)
		{
			foreach (AssembledDefinitionNode toAdd in _contents)
			{
				if (!this.CanAddIngredient(toAdd))
				{
					return false;
				}
			}
			return this.m_itemContainer.CanTakeContents(_contents);
		}
		return false;
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x00073593 File Offset: 0x00071993
	public virtual void AddOrderContents(AssembledDefinitionNode[] _contents)
	{
		this.m_itemContainer.CopyContents(_contents);
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x000735A4 File Offset: 0x000719A4
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		AssembledDefinitionNode[] orderDefinitionOfCarriedItem = this.GetOrderDefinitionOfCarriedItem(gameObject);
		ServerIngredientContainer component = gameObject.GetComponent<ServerIngredientContainer>();
		Plate component2 = gameObject.GetComponent<Plate>();
		if (!this.CanAddOrderContents(orderDefinitionOfCarriedItem))
		{
			Tray component3 = gameObject.GetComponent<Tray>();
			if (component3 != null)
			{
				return true;
			}
			if (!(component2 != null) || !(component != null) || !this.CanTransferToContainer(component))
			{
				return false;
			}
		}
		return !(component2 != null) || !(this.m_preparationContainer.m_ingredientOrderNode.m_platingStep != component2.m_platingStep);
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x00073648 File Offset: 0x00071A48
	public virtual void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		AssembledDefinitionNode[] orderDefinitionOfCarriedItem = this.GetOrderDefinitionOfCarriedItem(gameObject);
		if (gameObject.GetComponent<CookableContainer>())
		{
			ServerIngredientContainer component = gameObject.GetComponent<ServerIngredientContainer>();
			this.AddOrderContents(orderDefinitionOfCarriedItem);
			component.RemoveIngredient(0);
		}
		else if (gameObject.GetComponent<Plate>() != null)
		{
			ServerIngredientContainer component2 = gameObject.GetComponent<ServerIngredientContainer>();
			if (orderDefinitionOfCarriedItem.Length > 0 && gameObject.GetComponent<Tray>() == null)
			{
				this.AddOrderContents(orderDefinitionOfCarriedItem);
				component2.Empty();
			}
			else if (this.CanTransferToContainer(component2))
			{
				this.TransferToContainer(_carrier, component2, false);
				ServerPhysicalAttachment serverPhysicalAttachment = base.gameObject.RequestComponent<ServerPhysicalAttachment>();
				if (serverPhysicalAttachment != null)
				{
					serverPhysicalAttachment.ManualDisable(true);
				}
			}
		}
		else
		{
			this.AddOrderContents(orderDefinitionOfCarriedItem);
			_carrier.DestroyCarriedItem();
		}
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x0007371C File Offset: 0x00071B1C
	public bool AllowThrowing()
	{
		return this.m_itemContainer.GetContentsCount() == 0;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x0007372C File Offset: 0x00071B2C
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x0007372E File Offset: 0x00071B2E
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.GetAsOrderComposite();
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x00073738 File Offset: 0x00071B38
	protected virtual CompositeAssembledNode GetAsOrderComposite()
	{
		CompositeAssembledNode compositeAssembledNode = new CompositeAssembledNode();
		compositeAssembledNode.m_composition = this.m_itemContainer.GetContents();
		Array.Resize<AssembledDefinitionNode>(ref compositeAssembledNode.m_composition, compositeAssembledNode.m_composition.Length + 1);
		compositeAssembledNode.m_composition[compositeAssembledNode.m_composition.Length - 1] = new IngredientAssembledNode(this.m_preparationContainer.m_ingredientOrderNode);
		compositeAssembledNode.m_permittedEntries = this.m_preparationContainer.m_containerRestrictions.GetContentRestrictions();
		compositeAssembledNode.m_freeObject = base.gameObject;
		return compositeAssembledNode;
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x000737B5 File Offset: 0x00071BB5
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x000737CE File Offset: 0x00071BCE
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x000737E7 File Offset: 0x00071BE7
	public int GetPlacementPriority()
	{
		return int.MinValue;
	}

	// Token: 0x04001077 RID: 4215
	private PreparationContainer m_preparationContainer;

	// Token: 0x04001078 RID: 4216
	private ServerIngredientContainer m_itemContainer;

	// Token: 0x04001079 RID: 4217
	protected OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
