using System;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public interface IHandlePickup : IBaseHandlePickup
{
	// Token: 0x06001685 RID: 5765
	void HandlePickup(ICarrier _carrier, Vector2 _directionXZ);
}
