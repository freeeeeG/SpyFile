using System;

// Token: 0x020004C5 RID: 1221
public interface IHeatTransferBehaviour
{
	// Token: 0x0600168C RID: 5772
	bool CanTransferToContainer(IHeatContainer _container);

	// Token: 0x0600168D RID: 5773
	void TransferToContainer(ICarrierPlacement _carrier, IHeatContainer _container);
}
