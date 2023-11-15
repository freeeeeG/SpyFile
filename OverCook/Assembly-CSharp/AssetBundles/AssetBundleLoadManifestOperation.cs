using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200028D RID: 653
	public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperationFull
	{
		// Token: 0x06000BFD RID: 3069 RVA: 0x0003E16D File Offset: 0x0003C56D
		public AssetBundleLoadManifestOperation(string bundleName, string assetName, Type type) : base(bundleName, assetName, type)
		{
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0003E178 File Offset: 0x0003C578
		public override bool Update()
		{
			base.Update();
			if (this.m_Request != null && this.m_Request.isDone)
			{
				AssetBundleManager.AssetBundleManifestObject = this.GetAsset<AssetBundleManifest>();
				return false;
			}
			return true;
		}
	}
}
