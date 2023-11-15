using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200028C RID: 652
	public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
	{
		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003E090 File Offset: 0x0003C490
		public AssetBundleLoadAssetOperationFull(string bundleName, string assetName, Type type)
		{
			this.m_AssetBundleName = bundleName;
			this.m_AssetName = assetName;
			this.m_Type = type;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003E0AD File Offset: 0x0003C4AD
		public override T GetAsset<T>()
		{
			if (this.m_Request != null && this.m_Request.isDone)
			{
				return this.m_Request.asset as T;
			}
			return (T)((object)null);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0003E0E8 File Offset: 0x0003C4E8
		public override bool Update()
		{
			if (this.m_Request != null)
			{
				return false;
			}
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(this.m_AssetBundleName, out this.m_DownloadingError);
			if (loadedAssetBundle != null)
			{
				this.m_Request = loadedAssetBundle.m_AssetBundle.LoadAssetAsync(this.m_AssetName, this.m_Type);
				return false;
			}
			return true;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0003E13A File Offset: 0x0003C53A
		public override bool IsDone()
		{
			return (this.m_Request == null && this.m_DownloadingError != null) || (this.m_Request != null && this.m_Request.isDone);
		}

		// Token: 0x04000914 RID: 2324
		protected string m_AssetBundleName;

		// Token: 0x04000915 RID: 2325
		protected string m_AssetName;

		// Token: 0x04000916 RID: 2326
		protected string m_DownloadingError;

		// Token: 0x04000917 RID: 2327
		protected Type m_Type;

		// Token: 0x04000918 RID: 2328
		protected AssetBundleRequest m_Request;
	}
}
