using System;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
[RequireComponent(typeof(IOrderDefinition))]
public class ItemToContainerBehaviour : MonoBehaviour, IContainerTransferBehaviour, IPlaceUnder
{
	// Token: 0x06001766 RID: 5990 RVA: 0x00077D1C File Offset: 0x0007611C
	private void EnsureOrderDefinition()
	{
		if (this.m_orderDefinition == null)
		{
			this.m_orderDefinition = base.gameObject.RequestInterface<IOrderDefinition>();
		}
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x00077D3C File Offset: 0x0007613C
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		this.EnsureOrderDefinition();
		if (_container as MonoBehaviour != null)
		{
			GameObject gameObject = (_container as MonoBehaviour).gameObject;
			if (gameObject.RequestComponent<ItemContainer>() != null)
			{
				return AssembledNodeTransfer.CanCombineWithContents(this.m_orderDefinition.GetOrderComposition(), _container);
			}
		}
		return false;
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x00077D90 File Offset: 0x00076190
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove)
	{
		this.EnsureOrderDefinition();
		AssembledNodeTransfer.CombineWithContents(this.m_orderDefinition.GetOrderComposition(), _container, _dontRemove);
		if (!_dontRemove)
		{
			_carrier.DestroyCarriedItem();
		}
	}

	// Token: 0x0400112A RID: 4394
	private IOrderDefinition m_orderDefinition;
}
