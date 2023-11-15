using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000521 RID: 1313
public class ClientMixableContainer : ClientSynchroniserBase, IClientOrderDefinition
{
	// Token: 0x06001880 RID: 6272 RVA: 0x0007C900 File Offset: 0x0007AD00
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_mixableContainer = (MixableContainer)synchronisedObject;
		this.m_mixingHandler = base.gameObject.RequireComponent<ClientMixingHandler>();
		ClientMixingHandler mixingHandler = this.m_mixingHandler;
		mixingHandler.m_stateChangedCallback = (StateChanged)Delegate.Combine(mixingHandler.m_stateChangedCallback, new StateChanged(delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		}));
		this.m_itemContainer = base.gameObject.GetComponent<ClientIngredientContainer>();
		this.m_itemContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_PlacementContainer = base.gameObject.RequireComponent<ClientPlacementContainer>();
		this.m_PlacementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x0007C9A1 File Offset: 0x0007ADA1
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_mixableContainer.GetOrderComposition(this.m_itemContainer, this.m_mixingHandler.GetMixingProgress() / this.m_mixingHandler.AccessMixingTime, this.m_mixingHandler.GetMixedOrderState());
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x0007C9D6 File Offset: 0x0007ADD6
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x0007C9EF File Offset: 0x0007ADEF
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x0007CA08 File Offset: 0x0007AE08
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		float cookingProgress = 0f;
		ClientCookableContainer clientCookableContainer = base.gameObject.RequestComponent<ClientCookableContainer>();
		if (clientCookableContainer != null)
		{
			ClientCookingHandler cookingHandler = clientCookableContainer.GetCookingHandler();
			if (cookingHandler != null)
			{
				cookingProgress = cookingHandler.GetCookingProgress();
			}
		}
		return this.m_mixableContainer.AllowItemPlacement(_object, _context, this.m_mixableContainer.m_ApprovedIngredients, this.m_mixingHandler.IsOverMixed(), cookingProgress);
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x0007CA74 File Offset: 0x0007AE74
	private bool AllowItemCatching(GameObject _object)
	{
		return this.AllowItemPlacement(_object, default(PlacementContext));
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x0007CA91 File Offset: 0x0007AE91
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x0007CAA4 File Offset: 0x0007AEA4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_PlacementContainer != null)
		{
			this.m_PlacementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x0007CAD4 File Offset: 0x0007AED4
	public ClientMixingHandler GetMixingHandler()
	{
		return this.m_mixingHandler;
	}

	// Token: 0x040013B3 RID: 5043
	private MixableContainer m_mixableContainer;

	// Token: 0x040013B4 RID: 5044
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x040013B5 RID: 5045
	private ClientIngredientContainer m_itemContainer;

	// Token: 0x040013B6 RID: 5046
	private ClientMixingHandler m_mixingHandler;

	// Token: 0x040013B7 RID: 5047
	private ClientPlacementContainer m_PlacementContainer;
}
