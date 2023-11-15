using System;
using UnityEngine;

// Token: 0x020004E2 RID: 1250
public class ContainerHeatTransferBehaviour : MonoBehaviour, IHeatTransferBehaviour
{
	// Token: 0x06001744 RID: 5956 RVA: 0x000776FB File Offset: 0x00075AFB
	private void EnsureContainer()
	{
		if (this.m_ingredientContainer == null)
		{
			this.m_ingredientContainer = base.gameObject.RequestInterface<IIngredientContents>();
		}
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x0007771C File Offset: 0x00075B1C
	public bool CanTransferToContainer(IHeatContainer _container)
	{
		this.EnsureContainer();
		int contentsCount = this.m_ingredientContainer.GetContentsCount();
		for (int i = 0; i < contentsCount; i++)
		{
			AssembledDefinitionNode contentsElement = this.m_ingredientContainer.GetContentsElement(i);
			if (contentsElement is ItemAssembledNode)
			{
				ItemAssembledNode itemAssembledNode = contentsElement as ItemAssembledNode;
				if (itemAssembledNode.m_itemOrderNode.m_heatValue > 0f)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x00077784 File Offset: 0x00075B84
	public void TransferToContainer(ICarrierPlacement _carrier, IHeatContainer _container)
	{
		this.EnsureContainer();
		float num = 0f;
		int contentsCount = this.m_ingredientContainer.GetContentsCount();
		for (int i = contentsCount - 1; i >= 0; i--)
		{
			AssembledDefinitionNode contentsElement = this.m_ingredientContainer.GetContentsElement(i);
			if (contentsElement is ItemAssembledNode)
			{
				ItemAssembledNode itemAssembledNode = contentsElement as ItemAssembledNode;
				num += itemAssembledNode.m_itemOrderNode.m_heatValue;
			}
		}
		if (num > 0f)
		{
			_container.IncreaseHeat(num);
			this.m_ingredientContainer.Empty();
		}
	}

	// Token: 0x0400111B RID: 4379
	private IIngredientContents m_ingredientContainer;
}
