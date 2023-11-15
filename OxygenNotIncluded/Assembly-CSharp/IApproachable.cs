using System;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public interface IApproachable
{
	// Token: 0x06001937 RID: 6455
	CellOffset[] GetOffsets();

	// Token: 0x06001938 RID: 6456
	int GetCell();

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06001939 RID: 6457
	Transform transform { get; }
}
