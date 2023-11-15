using System;
using System.Collections;
using AssetBundles;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000640 RID: 1600
public class BootFlow : MonoBehaviour
{
	// Token: 0x06001E73 RID: 7795 RVA: 0x00093343 File Offset: 0x00091743
	private void Awake()
	{
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x00093348 File Offset: 0x00091748
	private IEnumerator Start()
	{
		AssetBundleLoadManifestOperation request = AssetBundleManager.Initialize();
		while (request.MoveNext())
		{
			yield return null;
		}
		this.m_SLH = base.gameObject.AddComponent<SceneLoaderHelper>();
		this.m_SLH.LoadLevelAsync(this.m_SceneToLoad, true, LoadSceneMode.Single);
		yield break;
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x00093364 File Offset: 0x00091764
	private void Update()
	{
		if (this.m_ProgressBar != null)
		{
			if (this.m_SLH != null)
			{
				this.m_ProgressBar.SetValue(this.m_SLH.GetProgress());
			}
			else
			{
				this.m_ProgressBar.SetValue(0f);
			}
		}
	}

	// Token: 0x0400176A RID: 5994
	public string m_SceneToLoad = string.Empty;

	// Token: 0x0400176B RID: 5995
	public ProgressBarUI m_ProgressBar;

	// Token: 0x0400176C RID: 5996
	private SceneLoaderHelper m_SLH;

	// Token: 0x0400176D RID: 5997
	public GameObject m_splashScreen;
}
