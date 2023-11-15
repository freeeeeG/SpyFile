using System;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public interface IClientThrowable
{
	// Token: 0x06001771 RID: 6001
	bool CanHandleThrow(IClientThrower _thrower, Vector2 _directionXZ);

	// Token: 0x06001772 RID: 6002
	bool IsFlying();

	// Token: 0x06001773 RID: 6003
	IClientThrower GetThrower();
}
