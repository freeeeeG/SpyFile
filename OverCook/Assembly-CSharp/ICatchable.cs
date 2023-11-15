using System;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
public interface ICatchable
{
	// Token: 0x06001670 RID: 5744
	bool AllowCatch(IHandleCatch _catcher, Vector2 _directionXZ);

	// Token: 0x06001671 RID: 5745
	GameObject AccessGameObject();
}
