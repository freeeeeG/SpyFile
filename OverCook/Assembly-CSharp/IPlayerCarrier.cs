using System;
using UnityEngine;

// Token: 0x020009EE RID: 2542
public interface IPlayerCarrier : ICarrier, ICarrierPlacement
{
	// Token: 0x0600319E RID: 12702
	GameObject InspectCarriedItem(PlayerAttachTarget playerAttachTarget);

	// Token: 0x0600319F RID: 12703
	GameObject TakeItem(PlayerAttachTarget playerAttachTarget);

	// Token: 0x060031A0 RID: 12704
	bool HasAttachment(PlayerAttachTarget playerAttachTarget);
}
