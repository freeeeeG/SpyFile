using System;
using UnityEngine;

// Token: 0x020009EC RID: 2540
public interface ICarrierPlacement
{
	// Token: 0x06003197 RID: 12695
	GameObject InspectCarriedItem();

	// Token: 0x06003198 RID: 12696
	void DestroyCarriedItem();

	// Token: 0x06003199 RID: 12697
	GameObject TakeItem();
}
