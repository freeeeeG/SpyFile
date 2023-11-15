using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200028A RID: 650
	public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
	{
		// Token: 0x06000BF4 RID: 3060
		public abstract T GetAsset<T>() where T : UnityEngine.Object;
	}
}
