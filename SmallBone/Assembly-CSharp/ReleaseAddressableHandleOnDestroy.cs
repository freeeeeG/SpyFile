using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000075 RID: 117
public class ReleaseAddressableHandleOnDestroy : MonoBehaviour
{
	// Token: 0x0600021F RID: 543 RVA: 0x000092AE File Offset: 0x000074AE
	public static void Reserve(GameObject gameObject, AsyncOperationHandle<GameObject> handle)
	{
		gameObject.AddComponent<ReleaseAddressableHandleOnDestroy>()._handle = handle;
	}

	// Token: 0x06000220 RID: 544 RVA: 0x000092BC File Offset: 0x000074BC
	private void OnDestroy()
	{
		if (!this._handle.IsValid())
		{
			return;
		}
		Addressables.Release<GameObject>(this._handle);
	}

	// Token: 0x040001D5 RID: 469
	private AsyncOperationHandle<GameObject> _handle;
}
