using System;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class ItemHeatTransferBehaviour : MonoBehaviour, IHeatTransferBehaviour
{
	// Token: 0x06001762 RID: 5986 RVA: 0x00077C68 File Offset: 0x00076068
	private void EnsureOrderDefinition()
	{
		if (this.m_orderDefinition == null)
		{
			this.m_orderDefinition = base.gameObject.RequestInterface<IOrderDefinition>();
		}
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x00077C88 File Offset: 0x00076088
	public bool CanTransferToContainer(IHeatContainer _container)
	{
		this.EnsureOrderDefinition();
		AssembledDefinitionNode orderComposition = this.m_orderDefinition.GetOrderComposition();
		return orderComposition is ItemAssembledNode && (orderComposition as ItemAssembledNode).m_itemOrderNode.m_heatValue > 0f;
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x00077CCC File Offset: 0x000760CC
	public void TransferToContainer(ICarrierPlacement _carrier, IHeatContainer _container)
	{
		this.EnsureOrderDefinition();
		AssembledDefinitionNode orderComposition = this.m_orderDefinition.GetOrderComposition();
		if (orderComposition is ItemAssembledNode)
		{
			ItemAssembledNode itemAssembledNode = orderComposition as ItemAssembledNode;
			_container.IncreaseHeat(itemAssembledNode.m_itemOrderNode.m_heatValue);
		}
		_carrier.DestroyCarriedItem();
	}

	// Token: 0x04001129 RID: 4393
	private IOrderDefinition m_orderDefinition;
}
