using System;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public interface IKComponentManager
{
	// Token: 0x06002446 RID: 9286
	HandleVector<int>.Handle Add(GameObject go);

	// Token: 0x06002447 RID: 9287
	void Remove(GameObject go);
}
