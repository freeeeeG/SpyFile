using System;
using UnityEngine;

// Token: 0x020004E5 RID: 1253
[RequireComponent(typeof(IngredientContainer))]
[RequireComponent(typeof(PlacementContainer))]
public class ItemContainer : MonoBehaviour
{
	// Token: 0x0600175E RID: 5982 RVA: 0x00077BCB File Offset: 0x00075FCB
	private void Start()
	{
		if (this.m_approvedContentsList != null)
		{
			this.m_approvedContentsList.CacheAssembledOrderNodes();
		}
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x00077BEC File Offset: 0x00075FEC
	public AssembledDefinitionNode GetOrderComposition(IIngredientContents _ingredientContainer)
	{
		return new CompositeAssembledNode
		{
			m_composition = _ingredientContainer.GetContents()
		};
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x00077C0C File Offset: 0x0007600C
	public bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		IOrderDefinition orderDefinition = _object.RequestInterface<IOrderDefinition>();
		if (orderDefinition != null)
		{
			AssembledDefinitionNode orderComposition = orderDefinition.GetOrderComposition();
			if (orderComposition is ItemAssembledNode)
			{
				return this.m_approvedContentsList == null || this.m_approvedContentsList.GetPrefabForNode(orderComposition) != null;
			}
		}
		return false;
	}

	// Token: 0x04001127 RID: 4391
	[SerializeField]
	public OrderToPrefabLookup m_approvedContentsList;

	// Token: 0x04001128 RID: 4392
	[SerializeField]
	public GameObject m_cosmeticsPrefab;
}
