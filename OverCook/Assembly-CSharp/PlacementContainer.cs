using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000613 RID: 1555
[RequireComponent(typeof(IngredientContainer))]
public class PlacementContainer : MonoBehaviour
{
	// Token: 0x06001D69 RID: 7529 RVA: 0x000900B4 File Offset: 0x0008E4B4
	public virtual bool CanCombine(GameObject _placingObject, List<Generic<bool, GameObject, PlacementContext>> m_allowPlacementCallbacks, IIngredientContents _container, PlacementContext _context)
	{
		if (!m_allowPlacementCallbacks.CallForResult(true, _placingObject, _context))
		{
			return false;
		}
		IOrderDefinition orderDefinition = _placingObject.RequestInterface<IOrderDefinition>();
		if (orderDefinition != null && orderDefinition.GetOrderComposition().Simpilfy() != AssembledDefinitionNode.NullNode)
		{
			IContainerTransferBehaviour containerTransferBehaviour = _placingObject.RequestInterface<IContainerTransferBehaviour>();
			if (containerTransferBehaviour != null && containerTransferBehaviour.CanTransferToContainer(_container))
			{
				return _container.CanAddIngredient(orderDefinition.GetOrderComposition());
			}
		}
		return false;
	}
}
