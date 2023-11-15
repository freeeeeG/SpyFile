using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000CF RID: 207
	public class ArrowRetrievalPoint : MonoBehaviour
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x0001D39B File Offset: 0x0001B59B
		private void Start()
		{
			ArrowRetrieveOnReload.RegisterRetrievalPoint(this);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001D3A3 File Offset: 0x0001B5A3
		private void OnDestroy()
		{
			ArrowRetrieveOnReload.RemoveRetrievalPoint(this);
		}
	}
}
