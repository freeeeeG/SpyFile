using System;

// Token: 0x0200060C RID: 1548
public interface IContainerTransferBehaviour
{
	// Token: 0x06001D53 RID: 7507
	void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _container, bool _dontRemove);

	// Token: 0x06001D54 RID: 7508
	bool CanTransferToContainer(IIngredientContents _container);
}
