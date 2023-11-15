using System;
using UnityEngine;

// Token: 0x02000488 RID: 1160
[AddComponentMenu("Scripts/Game/Environment/FoodObjects/PreparationContainer")]
[RequireComponent(typeof(PhysicalAttachment))]
[RequireComponent(typeof(IngredientContainer))]
[RequireComponent(typeof(HandlePlacementReferral))]
public class PreparationContainer : MonoBehaviour, ISpawnableItem
{
	// Token: 0x060015A4 RID: 5540 RVA: 0x00073120 File Offset: 0x00071520
	public SubTexture2D GetSubTexture()
	{
		return this.m_ingredientOrderNode.m_crateLid;
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x0007312D File Offset: 0x0007152D
	public Sprite GetUIIcon()
	{
		return this.m_ingredientOrderNode.m_iconSprite;
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x0007313C File Offset: 0x0007153C
	public AssembledDefinitionNode[] GetOrderDefinitionOfCarriedItem(GameObject _carriedItem, IIngredientContents _itemContainer, IBaseCookable _cookingHandler)
	{
		AssembledDefinitionNode[] result = null;
		if (_carriedItem.GetComponent<CookableContainer>() != null)
		{
			ClientIngredientContainer component = _carriedItem.GetComponent<ClientIngredientContainer>();
			IContainerTransferBehaviour containerTransferBehaviour = _carriedItem.RequireInterface<IContainerTransferBehaviour>();
			if (component.HasContents() && containerTransferBehaviour.CanTransferToContainer(_itemContainer))
			{
				CookableContainer component2 = _carriedItem.GetComponent<CookableContainer>();
				ClientMixableContainer component3 = _carriedItem.GetComponent<ClientMixableContainer>();
				AssembledDefinitionNode cookableMixableContents = null;
				bool isMixed = false;
				if (component3 != null)
				{
					cookableMixableContents = component3.GetOrderComposition();
					isMixed = component3.GetMixingHandler().IsMixed();
				}
				CookedCompositeAssembledNode cookedCompositeAssembledNode = component2.GetOrderComposition(_itemContainer, _cookingHandler, cookableMixableContents, isMixed) as CookedCompositeAssembledNode;
				cookedCompositeAssembledNode.m_composition = new AssembledDefinitionNode[]
				{
					component.GetContentsElement(0)
				};
				result = new AssembledDefinitionNode[]
				{
					cookedCompositeAssembledNode
				};
			}
		}
		else if (_carriedItem.GetComponent<IngredientPropertiesComponent>() != null)
		{
			IngredientPropertiesComponent component4 = _carriedItem.GetComponent<IngredientPropertiesComponent>();
			result = new AssembledDefinitionNode[]
			{
				component4.GetOrderComposition()
			};
		}
		else if (_carriedItem.GetComponent<Plate>() != null)
		{
			ClientIngredientContainer component5 = _carriedItem.GetComponent<ClientIngredientContainer>();
			result = component5.GetContents();
		}
		return result;
	}

	// Token: 0x04001074 RID: 4212
	[SerializeField]
	public OrderToPrefabLookup m_containerRestrictions;

	// Token: 0x04001075 RID: 4213
	[SerializeField]
	public IngredientOrderNode m_ingredientOrderNode;

	// Token: 0x04001076 RID: 4214
	[SerializeField]
	public GameObject m_cosmeticsPrefab;
}
