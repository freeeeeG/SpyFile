using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class ClientPreparationContainer : ClientSynchroniserBase, IClientOrderDefinition, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x060015BD RID: 5565 RVA: 0x00073998 File Offset: 0x00071D98
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_preparationContainer = (PreparationContainer)synchronisedObject;
		this.m_itemContainer = base.gameObject.GetComponent<ClientIngredientContainer>();
		this.m_itemContainer.RegisterContentsChangedCallback(delegate(AssembledDefinitionNode[] _contents)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		});
		if (this.m_preparationContainer.m_cosmeticsPrefab != null)
		{
			Transform parent = NetworkUtils.FindVisualRoot(base.gameObject);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_preparationContainer.m_cosmeticsPrefab, parent);
			if (gameObject != null)
			{
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		}
		ClientThrowableItem clientThrowableItem = base.gameObject.RequestComponent<ClientThrowableItem>();
		if (clientThrowableItem != null)
		{
			clientThrowableItem.RegisterCanThrowCallback(new Generic<bool>(this.AllowThrowing));
		}
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x00073A63 File Offset: 0x00071E63
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.GetAsOrderComposite();
	}

	// Token: 0x060015BF RID: 5567 RVA: 0x00073A6C File Offset: 0x00071E6C
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

	// Token: 0x060015C0 RID: 5568 RVA: 0x00073AE9 File Offset: 0x00071EE9
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x00073B02 File Offset: 0x00071F02
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x00073B1C File Offset: 0x00071F1C
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

	// Token: 0x060015C3 RID: 5571 RVA: 0x00073B64 File Offset: 0x00071F64
	private bool CanAddIngredient(AssembledDefinitionNode _toAdd)
	{
		CompositeAssembledNode asOrderComposite = this.GetAsOrderComposite();
		return asOrderComposite.CanAddOrderNode(_toAdd, true);
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x00073B80 File Offset: 0x00071F80
	private AssembledDefinitionNode[] GetOrderDefinitionOfCarriedItem(GameObject _carriedItem)
	{
		IBaseCookable cookingHandler = null;
		ClientCookableContainer component = _carriedItem.GetComponent<ClientCookableContainer>();
		if (component != null)
		{
			cookingHandler = component.GetCookingHandler();
		}
		return this.m_preparationContainer.GetOrderDefinitionOfCarriedItem(_carriedItem, this.m_itemContainer, cookingHandler);
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x00073BBC File Offset: 0x00071FBC
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject carriedItem = _carrier.InspectCarriedItem();
		AssembledDefinitionNode[] orderDefinitionOfCarriedItem = this.GetOrderDefinitionOfCarriedItem(carriedItem);
		return this.CanAddOrderContents(orderDefinitionOfCarriedItem);
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x00073BDF File Offset: 0x00071FDF
	public int GetPlacementPriority()
	{
		return int.MinValue;
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x00073BE6 File Offset: 0x00071FE6
	public bool AllowThrowing()
	{
		return this.m_itemContainer.GetContentsCount() == 0;
	}

	// Token: 0x0400107B RID: 4219
	private PreparationContainer m_preparationContainer;

	// Token: 0x0400107C RID: 4220
	protected OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x0400107D RID: 4221
	private ClientIngredientContainer m_itemContainer;
}
