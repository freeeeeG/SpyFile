using System;
using System.Collections;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000064 RID: 100
public class LoadTestChapter : MonoBehaviour
{
	// Token: 0x060001D4 RID: 468 RVA: 0x0000865A File Offset: 0x0000685A
	private IEnumerator Start()
	{
		GameData.Initialize();
		SceneManager.LoadScene("Base", LoadSceneMode.Additive);
		yield return null;
		GameResourceLoader.Load();
		GameResourceLoader.instance.WaitForCompletion();
		yield return null;
		Singleton<Service>.Instance.levelManager.Load(Chapter.Type.Test);
		yield break;
	}
}
