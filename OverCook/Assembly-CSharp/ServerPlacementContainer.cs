using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000614 RID: 1556
public class ServerPlacementContainer : ServerSynchroniserBase, IHandlePlacement, IPlacementSupression, IBaseHandlePlacement
{
	// Token: 0x06001D6B RID: 7531 RVA: 0x0009012E File Offset: 0x0008E52E
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_ingredientContainer = base.gameObject.RequireComponent<ServerIngredientContainer>();
		this.m_placementContainer = (PlacementContainer)synchronisedObject;
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x0009014D File Offset: 0x0008E54D
	public virtual int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x00090150 File Offset: 0x0008E550
	protected virtual bool CanCombine(GameObject _placingObject, PlacementContext _context)
	{
		return this.m_placementContainer.CanCombine(_placingObject, this.m_allowPlacementCallbacks, this.m_ingredientContainer, _context);
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x0009016C File Offset: 0x0008E56C
	public virtual bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		if (this.CanCombine(gameObject, _context))
		{
			return true;
		}
		ServerPlacementContainer serverPlacementContainer = gameObject.RequestComponent<ServerPlacementContainer>();
		if (serverPlacementContainer != null && serverPlacementContainer.CanCombine(base.gameObject, _context))
		{
			return true;
		}
		if (_carrier is ServerPlayerAttachmentCarrier)
		{
			ServerPreparationContainer serverPreparationContainer = gameObject.RequestComponent<ServerPreparationContainer>();
			if (serverPreparationContainer != null)
			{
				IOrderDefinition orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
				if (serverPreparationContainer.CanAddOrderContents(new AssembledDefinitionNode[]
				{
					orderDefinition.GetOrderComposition()
				}))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x00090200 File Offset: 0x0008E600
	public virtual void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		if (this.CanCombine(gameObject, _context))
		{
			IContainerTransferBehaviour containerTransferBehaviour = gameObject.RequireInterface<IContainerTransferBehaviour>();
			containerTransferBehaviour.TransferToContainer(_carrier, this.m_ingredientContainer, false);
			this.m_ingredientContainer.InformOfInternalChange();
			return;
		}
		ServerIngredientContainer serverIngredientContainer = gameObject.RequireComponent<ServerIngredientContainer>();
		IContainerTransferBehaviour containerTransferBehaviour2 = base.gameObject.RequireInterface<IContainerTransferBehaviour>();
		if (containerTransferBehaviour2.CanTransferToContainer(serverIngredientContainer))
		{
			containerTransferBehaviour2.TransferToContainer(null, serverIngredientContainer, false);
			serverIngredientContainer.InformOfInternalChange();
		}
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x00090270 File Offset: 0x0008E670
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x00090272 File Offset: 0x0008E672
	public void RegisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallbacks.Add(_allowPlacementCallback);
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x00090280 File Offset: 0x0008E680
	public void UnregisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallbacks.Remove(_allowPlacementCallback);
	}

	// Token: 0x040016C9 RID: 5833
	protected ServerIngredientContainer m_ingredientContainer;

	// Token: 0x040016CA RID: 5834
	protected PlacementContainer m_placementContainer;

	// Token: 0x040016CB RID: 5835
	protected List<Generic<bool, GameObject, PlacementContext>> m_allowPlacementCallbacks = new List<Generic<bool, GameObject, PlacementContext>>();
}
