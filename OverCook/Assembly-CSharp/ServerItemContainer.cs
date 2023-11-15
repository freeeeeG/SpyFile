using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class ServerItemContainer : ServerSynchroniserBase, IOrderDefinition, IContainerTransferBehaviour
{
	// Token: 0x06001748 RID: 5960 RVA: 0x00077834 File Offset: 0x00075C34
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_itemContainer = (ItemContainer)synchronisedObject;
		this.m_ingredientContainer = base.gameObject.GetComponent<ServerIngredientContainer>();
		this.m_ingredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_placementContainer = base.gameObject.RequireComponent<ServerPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		this.m_ingredientCatcher = base.gameObject.RequestComponent<ServerIngredientCatcher>();
		if (this.m_ingredientCatcher != null)
		{
			this.m_ingredientCatcher.RegisterAllowItemCatching(new QueryForCatching(this.AllowItemCatching));
		}
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x000778D6 File Offset: 0x00075CD6
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x000778EF File Offset: 0x00075CEF
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x00077908 File Offset: 0x00075D08
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_ingredientCatcher)
		{
			this.m_ingredientCatcher.UnregisterAllowItemCatching(new QueryForCatching(this.AllowItemCatching));
		}
		if (null != this.m_placementContainer)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
		if (this.m_ingredientContainer != null)
		{
			this.m_ingredientContainer.UnregisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		}
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x00077993 File Offset: 0x00075D93
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x000779A6 File Offset: 0x00075DA6
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return this.m_itemContainer.AllowItemPlacement(_object, _context);
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000779B5 File Offset: 0x00075DB5
	private bool AllowItemCatching(GameObject _object)
	{
		return this.m_itemContainer.AllowItemPlacement(_object, new PlacementContext(PlacementContext.Source.Game));
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x000779C9 File Offset: 0x00075DC9
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_itemContainer.GetOrderComposition(this.m_ingredientContainer);
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x000779DC File Offset: 0x00075DDC
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		return AssembledNodeTransfer.CanTransferFromContainer<ServerItemContainer>(this, _container);
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x000779E5 File Offset: 0x00075DE5
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove)
	{
		AssembledNodeTransfer.TransferFromContainer<ServerItemContainer>(this, _container, _dontRemove);
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x000779EF File Offset: 0x00075DEF
	public bool CanTransferItemContents(AssembledDefinitionNode[] _toAdd)
	{
		return this.m_ingredientContainer.CanTakeContents(_toAdd);
	}

	// Token: 0x0400111C RID: 4380
	private ItemContainer m_itemContainer;

	// Token: 0x0400111D RID: 4381
	private ServerIngredientContainer m_ingredientContainer;

	// Token: 0x0400111E RID: 4382
	private ServerPlacementContainer m_placementContainer;

	// Token: 0x0400111F RID: 4383
	private ServerIngredientCatcher m_ingredientCatcher;

	// Token: 0x04001120 RID: 4384
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
