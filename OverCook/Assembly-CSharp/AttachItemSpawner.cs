using System;
using UnityEngine;

// Token: 0x0200042A RID: 1066
[RequireComponent(typeof(HandlePlacementReferral))]
[RequireComponent(typeof(IOrderDefinition))]
public class AttachItemSpawner : MonoBehaviour, IContainerTransferBehaviour
{
	// Token: 0x0600135A RID: 4954 RVA: 0x0006C102 File Offset: 0x0006A502
	private void Awake()
	{
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x0006C115 File Offset: 0x0006A515
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		return AssembledNodeTransfer.CanCombineWithContents(this.m_orderDefinition.GetOrderComposition(), _container);
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0006C128 File Offset: 0x0006A528
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove)
	{
		AssembledDefinitionNode orderComposition = this.m_orderDefinition.GetOrderComposition();
		AssembledNodeTransfer.CombineWithContents(orderComposition, _container, _dontRemove);
	}

	// Token: 0x04000F3C RID: 3900
	private IOrderDefinition m_orderDefinition;
}
