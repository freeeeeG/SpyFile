using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundles
{
	// Token: 0x02000289 RID: 649
	public class AssetBundleLoadLevelOperation : AssetBundleLoadLevelOperationBase
	{
		// Token: 0x06000BEF RID: 3055 RVA: 0x0003DF29 File Offset: 0x0003C329
		public AssetBundleLoadLevelOperation(string assetbundleName, string levelName, bool isAdditive)
		{
			this.m_AssetBundleName = assetbundleName;
			this.m_LevelName = levelName;
			this.m_IsAdditive = isAdditive;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0003DF48 File Offset: 0x0003C348
		public override bool Update()
		{
			if (this.m_Request != null)
			{
				return false;
			}
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(this.m_AssetBundleName, out this.m_DownloadingError);
			if (loadedAssetBundle != null)
			{
				if (this.OnAsyncOperationStarted != null)
				{
					AsyncOperation asyncOperation;
					if (this.m_IsAdditive)
					{
						asyncOperation = SceneManager.LoadSceneAsync(this.m_LevelName, LoadSceneMode.Additive);
					}
					else
					{
						asyncOperation = SceneManager.LoadSceneAsync(this.m_LevelName);
					}
					this.OnAsyncOperationStarted(asyncOperation);
					this.m_Request = asyncOperation;
					this.OnAsyncOperationStarted = null;
				}
				else if (this.m_IsAdditive)
				{
					this.m_Request = SceneManager.LoadSceneAsync(this.m_LevelName, LoadSceneMode.Additive);
				}
				else
				{
					this.m_Request = SceneManager.LoadSceneAsync(this.m_LevelName);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0003E005 File Offset: 0x0003C405
		public override float GetProgress()
		{
			if (this.m_Request != null)
			{
				return this.m_Request.progress;
			}
			return 0f;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0003E023 File Offset: 0x0003C423
		public override bool IsDone()
		{
			if (this.m_Request == null && this.m_DownloadingError != null)
			{
				Debug.LogError(this.m_DownloadingError);
				return true;
			}
			return this.m_Request != null && this.m_Request.isDone;
		}

		// Token: 0x0400090E RID: 2318
		protected string m_AssetBundleName;

		// Token: 0x0400090F RID: 2319
		protected string m_LevelName;

		// Token: 0x04000910 RID: 2320
		protected bool m_IsAdditive;

		// Token: 0x04000911 RID: 2321
		protected string m_DownloadingError;

		// Token: 0x04000912 RID: 2322
		protected AsyncOperation m_Request;
	}
}
