using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameResources
{
	// Token: 0x0200018C RID: 396
	public abstract class Request<T> where T : Component
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00018DD1 File Offset: 0x00016FD1
		public AsyncOperationHandle<GameObject> handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00018DD9 File Offset: 0x00016FD9
		public T asset
		{
			get
			{
				return this._handle.Result.GetComponent<T>();
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00018DEB File Offset: 0x00016FEB
		public bool isDone
		{
			get
			{
				return this._handle.IsDone;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00018DF8 File Offset: 0x00016FF8
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x00018E00 File Offset: 0x00017000
		public bool releaseReserved { get; private set; }

		// Token: 0x060008AB RID: 2219 RVA: 0x00018E09 File Offset: 0x00017009
		public Request(string path)
		{
			this._handle = Addressables.LoadAssetAsync<GameObject>(path);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00018E1D File Offset: 0x0001701D
		public Request(AssetReference reference)
		{
			this._handle = reference.LoadAssetAsync<GameObject>();
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00018E31 File Offset: 0x00017031
		public T Instantiate(Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._handle.Result, position, rotation);
			ReleaseAddressableHandleOnDestroy.Reserve(gameObject, this._handle);
			this.releaseReserved = true;
			return gameObject.GetComponent<T>();
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00018E5D File Offset: 0x0001705D
		public void WaitForCompletion()
		{
			this._handle.WaitForCompletion();
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00018E6B File Offset: 0x0001706B
		public void SetReleaseReserved()
		{
			this.releaseReserved = true;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00018E74 File Offset: 0x00017074
		public void Release()
		{
			if (this.releaseReserved)
			{
				return;
			}
			if (!this._handle.IsValid())
			{
				return;
			}
			Addressables.Release<GameObject>(this._handle);
		}

		// Token: 0x040006EF RID: 1775
		private AsyncOperationHandle<GameObject> _handle;
	}
}
