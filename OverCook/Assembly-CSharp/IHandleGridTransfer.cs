using System;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public interface IHandleGridTransfer
{
	// Token: 0x06001817 RID: 6167
	bool CanHandleTransfer(GridIndex _index, GameObject _object);

	// Token: 0x06001818 RID: 6168
	void HandleTransfer(GridIndex _index, GameObject _object);
}
