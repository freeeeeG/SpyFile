using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x02000287 RID: 647
	public abstract class AssetBundleLoadLevelOperationBase : AssetBundleLoadOperation
	{
		// Token: 0x06000BEA RID: 3050
		public abstract float GetProgress();

		// Token: 0x0400090D RID: 2317
		public AssetBundleLoadLevelOperationBase.StartAsyncOperationDelegate OnAsyncOperationStarted;

		// Token: 0x02000288 RID: 648
		// (Invoke) Token: 0x06000BEC RID: 3052
		public delegate void StartAsyncOperationDelegate(AsyncOperation op);
	}
}
