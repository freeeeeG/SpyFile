using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
[AddComponentMenu("Scripts/Game/Environment/CookableContainer")]
[RequireComponent(typeof(IngredientContainer))]
[RequireComponent(typeof(PlacementContainer))]
[RequireComponent(typeof(CookingHandler))]
public class CookableContainer : MonoBehaviour
{
	// Token: 0x06001490 RID: 5264 RVA: 0x000704C7 File Offset: 0x0006E8C7
	private void Start()
	{
		if (this.m_approvedContentsList != null)
		{
			this.m_approvedContentsList.CacheAssembledOrderNodes();
		}
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x000704E8 File Offset: 0x0006E8E8
	public AssembledDefinitionNode GetOrderComposition(IIngredientContents _itemContainer, IBaseCookable _cookingHandler, AssembledDefinitionNode _cookableMixableContents, bool _isMixed)
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = new CookedCompositeAssembledNode();
		if (_cookableMixableContents != null)
		{
			if (!_isMixed)
			{
				return _cookableMixableContents;
			}
			cookedCompositeAssembledNode.m_composition = new AssembledDefinitionNode[]
			{
				_cookableMixableContents
			};
		}
		else
		{
			cookedCompositeAssembledNode.m_composition = _itemContainer.GetContents();
		}
		cookedCompositeAssembledNode.m_cookingStep = _cookingHandler.AccessCookingType;
		cookedCompositeAssembledNode.m_recordedProgress = new float?(_cookingHandler.GetCookingProgress() / _cookingHandler.AccessCookingTime);
		cookedCompositeAssembledNode.m_progress = _cookingHandler.GetCookedOrderState();
		return cookedCompositeAssembledNode;
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x00070564 File Offset: 0x0006E964
	public bool AllowItemPlacement(GameObject _object, PlacementContext _context, IBaseCookable _iCookingHandler)
	{
		CookableProperties cookableProperties = _object.RequestComponent<CookableProperties>();
		if (cookableProperties == null || !cookableProperties.AllowsCookingStep(_iCookingHandler.AccessCookingType))
		{
			return false;
		}
		if (this.m_approvedContentsList != null)
		{
			IOrderDefinition orderDefinition = _object.RequestInterface<IOrderDefinition>();
			if (orderDefinition != null && this.m_approvedContentsList.GetPrefabForNode(orderDefinition.GetOrderComposition()) == null)
			{
				return false;
			}
		}
		return !_iCookingHandler.IsBurning();
	}

	// Token: 0x04000FD3 RID: 4051
	[SerializeField]
	public OrderToPrefabLookup m_approvedContentsList;

	// Token: 0x04000FD4 RID: 4052
	[SerializeField]
	public GameObject m_cosmeticsPrefab;
}
