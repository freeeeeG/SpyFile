using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200028E RID: 654
	public class LoadedAssetBundle
	{
		// Token: 0x06000BFF RID: 3071 RVA: 0x0003E1AA File Offset: 0x0003C5AA
		public LoadedAssetBundle(AssetBundle assetBundle)
		{
			this.m_AssetBundle = assetBundle;
			this.m_ReferencedCount = 1;
		}

		// Token: 0x04000919 RID: 2329
		public AssetBundle m_AssetBundle;

		// Token: 0x0400091A RID: 2330
		public int m_ReferencedCount;
	}
}
