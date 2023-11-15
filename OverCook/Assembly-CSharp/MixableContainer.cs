using System;
using UnityEngine;

// Token: 0x0200051F RID: 1311
[AddComponentMenu("Scripts/Game/Environment/MixableContainer")]
[RequireComponent(typeof(IngredientContainer))]
[RequireComponent(typeof(PlacementContainer))]
[RequireComponent(typeof(PhysicalAttachment))]
[RequireComponent(typeof(MixingHandler))]
public class MixableContainer : MonoBehaviour
{
	// Token: 0x0600186C RID: 6252 RVA: 0x0007C300 File Offset: 0x0007A700
	public AssembledDefinitionNode GetOrderComposition(IIngredientContents _ingredientContents, float _recordedProgress, MixedCompositeOrderNode.MixingProgress _mixingProgress)
	{
		return new MixedCompositeAssembledNode
		{
			m_composition = _ingredientContents.GetContents(),
			m_recordedProgress = new float?(_recordedProgress),
			m_progress = _mixingProgress
		};
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x0007C334 File Offset: 0x0007A734
	public bool AllowItemPlacement(GameObject _object, PlacementContext _context, OrderDefinitionNode[] _orderDefinitionNodes, bool _overMixed, float _cookingProgress)
	{
		if (_overMixed)
		{
			return false;
		}
		IOrderDefinition orderDefinition = _object.RequestInterface<IOrderDefinition>();
		if (orderDefinition == null)
		{
			return false;
		}
		if (_object.RequestComponent<MixableContainer>() != null && _cookingProgress == 0f)
		{
			MixedCompositeOrderNode.MixingProgress mixingProgress = MixedCompositeOrderNode.MixingProgress.Unmixed;
			ServerMixableContainer serverMixableContainer = _object.RequestComponent<ServerMixableContainer>();
			if (serverMixableContainer != null)
			{
				ServerMixingHandler mixingHandler = serverMixableContainer.GetMixingHandler();
				if (mixingHandler != null)
				{
					mixingProgress = mixingHandler.GetMixedOrderState();
				}
			}
			else
			{
				ClientMixableContainer clientMixableContainer = _object.RequestComponent<ClientMixableContainer>();
				if (clientMixableContainer != null)
				{
					ClientMixingHandler mixingHandler2 = clientMixableContainer.GetMixingHandler();
					if (mixingHandler2 != null)
					{
						mixingProgress = mixingHandler2.GetMixedOrderState();
					}
				}
			}
			if (mixingProgress == MixedCompositeOrderNode.MixingProgress.OverMixed)
			{
				return false;
			}
			if (base.transform.parent.GetComponentInParent<CookingStation>() != null && mixingProgress != MixedCompositeOrderNode.MixingProgress.Mixed)
			{
				return false;
			}
			CookedCompositeOrderNode.CookingProgress cookingProgress = CookedCompositeOrderNode.CookingProgress.Raw;
			ServerCookableContainer serverCookableContainer = _object.RequestComponent<ServerCookableContainer>();
			if (serverCookableContainer != null)
			{
				ServerCookingHandler cookingHandler = serverCookableContainer.GetCookingHandler();
				if (cookingHandler != null)
				{
					cookingProgress = cookingHandler.GetCookedOrderState();
				}
			}
			else
			{
				ClientCookableContainer clientCookableContainer = _object.RequestComponent<ClientCookableContainer>();
				if (clientCookableContainer != null)
				{
					ClientCookingHandler cookingHandler2 = clientCookableContainer.GetCookingHandler();
					if (cookingHandler2 != null)
					{
						cookingProgress = cookingHandler2.GetCookedOrderState();
					}
				}
			}
			if (cookingProgress == CookedCompositeOrderNode.CookingProgress.Raw)
			{
				IIngredientContents ingredientContents = _object.RequestInterface<IIngredientContents>();
				if (ingredientContents != null)
				{
					IIngredientContents ingredientContents2 = base.gameObject.RequestInterface<IIngredientContents>();
					if (ingredientContents2 != null && ingredientContents2.CanTakeContents(ingredientContents.GetContents()))
					{
						return true;
					}
				}
			}
		}
		else if (base.transform.parent.GetComponentInParent<CookingStation>() != null)
		{
			return false;
		}
		if (_orderDefinitionNodes != null && _cookingProgress == 0f)
		{
			for (int i = 0; i < _orderDefinitionNodes.Length; i++)
			{
				if (AssembledDefinitionNode.Matching(orderDefinition.GetOrderComposition(), _orderDefinitionNodes[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040013AB RID: 5035
	[SerializeField]
	public OrderDefinitionNode[] m_ApprovedIngredients;
}
