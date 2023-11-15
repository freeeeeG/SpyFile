using System;

// Token: 0x020004BC RID: 1212
public interface IBaseHandlePickup
{
	// Token: 0x06001683 RID: 5763
	bool CanHandlePickup(ICarrier _carrier);

	// Token: 0x06001684 RID: 5764
	int GetPickupPriority();
}
