using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class CachedObject : MonoBehaviour
{
	// Token: 0x06000530 RID: 1328 RVA: 0x00029898 File Offset: 0x00027C98
	private void OnDestroy()
	{
		ComponentCacheRegistry.EraseObject(base.gameObject);
	}
}
