using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200028F RID: 655
	public class LoadingAssetBundle
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x0003E1C0 File Offset: 0x0003C5C0
		public LoadingAssetBundle(AssetBundleCreateRequest assetBundleCreateRequest)
		{
			this.m_AssetBundleCreateRequest = assetBundleCreateRequest;
			this.m_ReferencedCount = 0;
		}

		// Token: 0x0400091B RID: 2331
		public AssetBundleCreateRequest m_AssetBundleCreateRequest;

		// Token: 0x0400091C RID: 2332
		public int m_ReferencedCount;
	}
}
