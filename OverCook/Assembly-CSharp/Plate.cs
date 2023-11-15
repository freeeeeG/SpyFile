using System;
using UnityEngine;

// Token: 0x02000548 RID: 1352
[AddComponentMenu("Scripts/Game/Environment/Plate")]
[RequireComponent(typeof(IngredientContainer))]
[RequireComponent(typeof(HandlePlacementReferral))]
[RequireComponent(typeof(PhysicalAttachment))]
[RequireComponent(typeof(PlacementContainer))]
public class Plate : MonoBehaviour
{
	// Token: 0x06001970 RID: 6512 RVA: 0x00080008 File Offset: 0x0007E408
	private void Awake()
	{
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x0008000C File Offset: 0x0007E40C
	public bool CanPlaceOnPlate(GameObject _gameObject, IIngredientContents _ingredientContentsAdapter)
	{
		IContainerTransferBehaviour containerTransferBehaviour = _gameObject.RequestInterface<IContainerTransferBehaviour>();
		if (containerTransferBehaviour == null)
		{
			return false;
		}
		ItemCarrierAdapter carrier = new ItemCarrierAdapter(_gameObject);
		ServerIngredientContainer component = base.gameObject.GetComponent<ServerIngredientContainer>();
		if (component != null)
		{
			IIngredientContents component2 = component.GetComponent<IIngredientContents>();
			if (component2 != null && component2.GetContentsCount() == 1)
			{
				for (int i = 0; i < component2.GetContentsCount(); i++)
				{
					AssembledDefinitionNode assembledDefinitionNode = component2.GetContents()[i];
					if (assembledDefinitionNode.m_freeObject != null)
					{
						IHandleOrderModification handleOrderModification = assembledDefinitionNode.m_freeObject.RequestInterface<IHandleOrderModification>();
						IOrderDefinition orderDefinition = _gameObject.RequestInterface<IOrderDefinition>();
						if (handleOrderModification != null && orderDefinition != null)
						{
							return handleOrderModification.CanAddOrderContents(new AssembledDefinitionNode[]
							{
								orderDefinition.GetOrderComposition()
							});
						}
					}
				}
			}
		}
		if (containerTransferBehaviour.CanTransferToContainer(_ingredientContentsAdapter))
		{
			containerTransferBehaviour.TransferToContainer(carrier, _ingredientContentsAdapter, true);
			AssembledDefinitionNode orderComposition = this.GetOrderComposition(_ingredientContentsAdapter.GetContents());
			return GameUtils.GetOrderPlatingPrefab(orderComposition, this.m_platingStep) != null;
		}
		return false;
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x0008010C File Offset: 0x0007E50C
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

	// Token: 0x0400143E RID: 5182
	[SerializeField]
	public PlatingStepData m_platingStep;
}
