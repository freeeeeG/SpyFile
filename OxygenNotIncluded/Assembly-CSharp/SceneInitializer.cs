using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000953 RID: 2387
public class SceneInitializer : MonoBehaviour
{
	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06004602 RID: 17922 RVA: 0x0018C124 File Offset: 0x0018A324
	// (set) Token: 0x06004603 RID: 17923 RVA: 0x0018C12B File Offset: 0x0018A32B
	public static SceneInitializer Instance { get; private set; }

	// Token: 0x06004604 RID: 17924 RVA: 0x0018C134 File Offset: 0x0018A334
	private void Awake()
	{
		Localization.SwapToLocalizedFont();
		string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		string text = Application.dataPath + Path.DirectorySeparatorChar.ToString() + "Plugins";
		if (!environmentVariable.Contains(text))
		{
			Environment.SetEnvironmentVariable("PATH", environmentVariable + Path.PathSeparator.ToString() + text, EnvironmentVariableTarget.Process);
		}
		SceneInitializer.Instance = this;
		this.PreLoadPrefabs();
	}

	// Token: 0x06004605 RID: 17925 RVA: 0x0018C1A3 File Offset: 0x0018A3A3
	private void OnDestroy()
	{
		SceneInitializer.Instance = null;
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x0018C1AC File Offset: 0x0018A3AC
	private void PreLoadPrefabs()
	{
		foreach (GameObject gameObject in this.preloadPrefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, gameObject.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
			}
		}
	}

	// Token: 0x06004607 RID: 17927 RVA: 0x0018C224 File Offset: 0x0018A424
	public void NewSaveGamePrefab()
	{
		if (this.prefab_NewSaveGame != null && SaveGame.Instance == null)
		{
			Util.KInstantiate(this.prefab_NewSaveGame, base.gameObject, null);
		}
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x0018C254 File Offset: 0x0018A454
	public void PostLoadPrefabs()
	{
		foreach (GameObject gameObject in this.prefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
	}

	// Token: 0x04002E5E RID: 11870
	public const int MAXDEPTH = -30000;

	// Token: 0x04002E5F RID: 11871
	public const int SCREENDEPTH = -1000;

	// Token: 0x04002E61 RID: 11873
	public GameObject prefab_NewSaveGame;

	// Token: 0x04002E62 RID: 11874
	public List<GameObject> preloadPrefabs = new List<GameObject>();

	// Token: 0x04002E63 RID: 11875
	public List<GameObject> prefabs = new List<GameObject>();
}
