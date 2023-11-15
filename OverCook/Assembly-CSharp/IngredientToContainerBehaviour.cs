using System;
using UnityEngine;

// Token: 0x02000482 RID: 1154
[RequireComponent(typeof(IOrderDefinition))]
public class IngredientToContainerBehaviour : MonoBehaviour, IContainerTransferBehaviour, IPlaceUnder
{
	// Token: 0x0600157E RID: 5502 RVA: 0x000746A1 File Offset: 0x00072AA1
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		this.EnsureOrderDefinition();
		return AssembledNodeTransfer.CanCombineWithContents(this.m_orderDefinition.GetOrderComposition(), _container);
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x000746BC File Offset: 0x00072ABC
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove)
	{
		this.EnsureOrderDefinition();
		AssembledDefinitionNode assembledDefinitionNode = this.m_orderDefinition.GetOrderComposition();
		if (_dontRemove)
		{
			assembledDefinitionNode = assembledDefinitionNode.Simpilfy();
		}
		AssembledNodeTransfer.CombineWithContents(assembledDefinitionNode, _container, _dontRemove);
		if (!_dontRemove)
		{
			_carrier.DestroyCarriedItem();
		}
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x000746FC File Offset: 0x00072AFC
	private void EnsureOrderDefinition()
	{
		if (this.m_orderDefinition == null)
		{
			this.m_orderDefinition = base.gameObject.RequestInterface<IOrderDefinition>();
		}
	}

	// Token: 0x04001064 RID: 4196
	private IOrderDefinition m_orderDefinition;
}
