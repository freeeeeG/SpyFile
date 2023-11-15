using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class ActivationCallback : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600053B RID: 1339 RVA: 0x00029A74 File Offset: 0x00027E74
	// (remove) Token: 0x0600053C RID: 1340 RVA: 0x00029AAC File Offset: 0x00027EAC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<bool> StateChangeCallbacks = delegate(bool _state)
	{
	};

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600053D RID: 1341 RVA: 0x00029AE4 File Offset: 0x00027EE4
	// (remove) Token: 0x0600053E RID: 1342 RVA: 0x00029B1C File Offset: 0x00027F1C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid ActivateCallbacks = delegate()
	{
	};

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600053F RID: 1343 RVA: 0x00029B54 File Offset: 0x00027F54
	// (remove) Token: 0x06000540 RID: 1344 RVA: 0x00029B8C File Offset: 0x00027F8C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid DeactivateCallbacks = delegate()
	{
	};

	// Token: 0x06000541 RID: 1345 RVA: 0x00029BC2 File Offset: 0x00027FC2
	private void OnEnable()
	{
		this.StateChangeCallbacks(true);
		this.ActivateCallbacks();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00029BDB File Offset: 0x00027FDB
	private void OnDisable()
	{
		this.StateChangeCallbacks(false);
		this.DeactivateCallbacks();
	}
}
