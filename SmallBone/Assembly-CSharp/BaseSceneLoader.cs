using System;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200007F RID: 127
public class BaseSceneLoader : MonoBehaviour
{
	// Token: 0x06000251 RID: 593 RVA: 0x00009B1F File Offset: 0x00007D1F
	private void Awake()
	{
		if (Scene<Base>.instance == null)
		{
			SceneManager.LoadScene("Base", LoadSceneMode.Additive);
		}
	}
}
