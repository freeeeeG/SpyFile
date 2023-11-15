using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class ServerCookableIngredient : ServerSynchroniserBase, IOrderDefinition
{
	// Token: 0x06001527 RID: 5415 RVA: 0x0007301A File Offset: 0x0007141A
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cookableIngredient = (CookableIngredient)synchronisedObject;
		this.m_cookingHandler = base.gameObject.RequireComponent<ServerCookingHandler>();
		this.m_cookingHandler.CookingStateChangedCallback += delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		};
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x00073050 File Offset: 0x00071450
	public AssembledDefinitionNode GetOrderComposition()
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = new CookedCompositeAssembledNode();
		IngredientAssembledNode ingredientAssembledNode = new IngredientAssembledNode(this.m_cookableIngredient.m_ingredientOrderNode);
		cookedCompositeAssembledNode.m_composition = new AssembledDefinitionNode[]
		{
			ingredientAssembledNode
		};
		cookedCompositeAssembledNode.m_cookingStep = this.m_cookingHandler.AccessCookingType;
		cookedCompositeAssembledNode.m_recordedProgress = new float?(this.m_cookingHandler.GetCookingProgress() / this.m_cookingHandler.AccessCookingTime);
		cookedCompositeAssembledNode.m_progress = this.m_cookingHandler.GetCookedOrderState();
		return cookedCompositeAssembledNode;
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x000730C9 File Offset: 0x000714C9
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x000730E2 File Offset: 0x000714E2
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x04001046 RID: 4166
	private CookableIngredient m_cookableIngredient;

	// Token: 0x04001047 RID: 4167
	private ServerCookingHandler m_cookingHandler;

	// Token: 0x04001048 RID: 4168
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
