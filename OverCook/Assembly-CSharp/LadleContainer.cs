using System;
using UnityEngine;

// Token: 0x020004EA RID: 1258
[RequireComponent(typeof(IngredientContainer))]
[RequireComponent(typeof(PlacementContainer))]
[RequireComponent(typeof(PhysicalAttachment))]
public class LadleContainer : MonoBehaviour
{
	// Token: 0x06001775 RID: 6005 RVA: 0x00077DC0 File Offset: 0x000761C0
	public AssembledDefinitionNode GetOrderComposition(AssembledDefinitionNode[] _contents)
	{
		if (_contents.Length == 1 && _contents[0].GetType() != typeof(IngredientAssembledNode))
		{
			return _contents[0];
		}
		return new CompositeAssembledNode
		{
			m_composition = _contents
		};
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x00077E00 File Offset: 0x00076200
	public bool AllowItemPlacement(GameObject _object, PlacementContext _context, IIngredientContents _contents)
	{
		IOrderDefinition orderDefinition = _object.RequestInterface<IOrderDefinition>();
		if (orderDefinition == null || _contents.HasContents())
		{
			return false;
		}
		CookingHandler cookingHandler = _object.RequestComponentRecursive<CookingHandler>();
		if (!(cookingHandler != null))
		{
			return false;
		}
		if (cookingHandler.m_cookingType != this.m_cookingStep)
		{
			return false;
		}
		CompositeAssembledNode compositeAssembledNode = orderDefinition.GetOrderComposition() as CompositeAssembledNode;
		return compositeAssembledNode == null || _contents.CanTakeContents(compositeAssembledNode.m_composition);
	}

	// Token: 0x0400112B RID: 4395
	[SerializeField]
	private CookingStepData m_cookingStep;
}
