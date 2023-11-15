using System;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public interface IThrowable
{
	// Token: 0x06001769 RID: 5993
	bool CanHandleThrow(IThrower _thrower, Vector2 _directionXZ);

	// Token: 0x0600176A RID: 5994
	void HandleThrow(IThrower _thrower, Vector2 _directionXZ);

	// Token: 0x0600176B RID: 5995
	bool IsFlying();

	// Token: 0x0600176C RID: 5996
	float GetFlightTime();

	// Token: 0x0600176D RID: 5997
	IThrower GetThrower();

	// Token: 0x0600176E RID: 5998
	IThrower GetPreviousThrower();

	// Token: 0x0600176F RID: 5999
	void RegisterLandedCallback(GenericVoid<GameObject> _callback);

	// Token: 0x06001770 RID: 6000
	void UnregisterLandedCallback(GenericVoid<GameObject> _callback);
}
