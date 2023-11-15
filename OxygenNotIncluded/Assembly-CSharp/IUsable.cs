using System;
using UnityEngine;

// Token: 0x020006A7 RID: 1703
public interface IUsable
{
	// Token: 0x06002E21 RID: 11809
	bool IsUsable();

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06002E22 RID: 11810
	Transform transform { get; }
}
