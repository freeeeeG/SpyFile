using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public interface IHandleCatch
{
	// Token: 0x0600167F RID: 5759
	bool CanHandleCatch(ICatchable _object, Vector2 _directionXZ);

	// Token: 0x06001680 RID: 5760
	void HandleCatch(ICatchable _object, Vector2 _directionXZ);

	// Token: 0x06001681 RID: 5761
	void AlertToThrownItem(ICatchable _thrown, IThrower _thrower, Vector2 _directionXZ);

	// Token: 0x06001682 RID: 5762
	int GetCatchingPriority();
}
