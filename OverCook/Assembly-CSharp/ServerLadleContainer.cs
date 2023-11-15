using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class ServerLadleContainer : ServerSynchroniserBase, IOrderDefinition, IContainerTransferBehaviour
{
	// Token: 0x06001778 RID: 6008 RVA: 0x00077EA8 File Offset: 0x000762A8
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_ladleContainer = (LadleContainer)synchronisedObject;
		this.m_placementContainer = base.gameObject.RequireComponent<ServerPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		this.m_ingredientContainer = base.gameObject.GetComponent<ServerIngredientContainer>();
		this.m_ingredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x00077F11 File Offset: 0x00076311
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return this.m_ladleContainer.AllowItemPlacement(_object, _context, this.m_ingredientContainer);
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x00077F26 File Offset: 0x00076326
	public void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x00077F39 File Offset: 0x00076339
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_ladleContainer.GetOrderComposition(this.m_ingredientContainer.GetContents());
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x00077F51 File Offset: 0x00076351
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x00077F6A File Offset: 0x0007636A
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x00077F83 File Offset: 0x00076383
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _plateContainer, bool _dontRemove)
	{
		AssembledNodeTransfer.TransferFromContainer<ServerLadleContainer>(this, _plateContainer, _dontRemove);
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x00077F8D File Offset: 0x0007638D
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		return AssembledNodeTransfer.CanTransferFromContainer<ServerLadleContainer>(this, _container);
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x00077F96 File Offset: 0x00076396
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_placementContainer != null)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x0400112C RID: 4396
	private LadleContainer m_ladleContainer;

	// Token: 0x0400112D RID: 4397
	private ServerIngredientContainer m_ingredientContainer;

	// Token: 0x0400112E RID: 4398
	private ServerPlacementContainer m_placementContainer;

	// Token: 0x0400112F RID: 4399
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
