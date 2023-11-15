using System;
using AssetBundles;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B9B RID: 2971
internal class SceneLoaderHelper : MonoBehaviour
{
	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x00123397 File Offset: 0x00121797
	// (set) Token: 0x06003CE4 RID: 15588 RVA: 0x0012339F File Offset: 0x0012179F
	public bool ActivateSceneWhenLoaded
	{
		get
		{
			return this.m_bAllowSceneActivation;
		}
		set
		{
			this.m_bAllowSceneActivation = value;
			if (this.m_asyncOp != null)
			{
				this.m_asyncOp.allowSceneActivation = this.m_bAllowSceneActivation;
			}
		}
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x001233C4 File Offset: 0x001217C4
	public void LoadLevelAsync(string sceneName, bool bActivateWhenLoaded, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
	{
		if (this.m_LastSceneLoaded != sceneName)
		{
			this.m_LastSceneLoaded = sceneName;
			this.m_bAllowSceneActivation = bActivateWhenLoaded;
			this.m_LoadOp = AssetBundleManager.LoadLevelAsync(sceneName.ToLowerInvariant(), sceneName, loadSceneMode == LoadSceneMode.Additive);
			AssetBundleLoadLevelOperationBase loadOp = this.m_LoadOp;
			loadOp.OnAsyncOperationStarted = (AssetBundleLoadLevelOperationBase.StartAsyncOperationDelegate)Delegate.Combine(loadOp.OnAsyncOperationStarted, new AssetBundleLoadLevelOperationBase.StartAsyncOperationDelegate(this.OnAsyncOperationStart));
			if (loadSceneMode != LoadSceneMode.Single || base.gameObject.GetComponent<LoadingScreenFlow>() == null)
			{
			}
			base.StartCoroutine(this.m_LoadOp);
			this.m_asyncOp = null;
		}
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x00123462 File Offset: 0x00121862
	private void OnAsyncOperationStart(AsyncOperation op)
	{
		this.m_asyncOp = op;
		this.m_asyncOp.allowSceneActivation = this.m_bAllowSceneActivation;
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x0012347C File Offset: 0x0012187C
	public float GetProgress()
	{
		if (this.m_asyncOp != null)
		{
			return this.m_asyncOp.progress;
		}
		if (this.m_LoadOp != null)
		{
			return this.m_LoadOp.GetProgress();
		}
		return 0f;
	}

	// Token: 0x04003102 RID: 12546
	private AssetBundleLoadLevelOperationBase m_LoadOp;

	// Token: 0x04003103 RID: 12547
	private AsyncOperation m_asyncOp;

	// Token: 0x04003104 RID: 12548
	private bool m_bAllowSceneActivation;

	// Token: 0x04003105 RID: 12549
	private string m_LastSceneLoaded = "null";
}
