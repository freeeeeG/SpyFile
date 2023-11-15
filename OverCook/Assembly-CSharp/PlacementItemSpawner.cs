using System;
using UnityEngine;

// Token: 0x0200053E RID: 1342
[RequireComponent(typeof(HandlePlacementReferral))]
[RequireComponent(typeof(IOrderDefinition))]
public class PlacementItemSpawner : MonoBehaviour, IContainerTransferBehaviour
{
	// Token: 0x0600192A RID: 6442 RVA: 0x0007F6DB File Offset: 0x0007DADB
	private void Awake()
	{
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0007F6EE File Offset: 0x0007DAEE
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		return AssembledNodeTransfer.CanCombineWithContents(this.m_orderDefinition.GetOrderComposition(), _container);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x0007F704 File Offset: 0x0007DB04
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove)
	{
		AssembledDefinitionNode orderComposition = this.m_orderDefinition.GetOrderComposition();
		AssembledNodeTransfer.CombineWithContents(orderComposition, _container, _dontRemove);
	}

	// Token: 0x0400141F RID: 5151
	[SerializeField]
	public int m_pickupPriority;

	// Token: 0x04001420 RID: 5152
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04001421 RID: 5153
	public StatValidationList m_condimentAchievementFilter;
}
