using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public static class GameLoader
{
	// Token: 0x060006E6 RID: 1766 RVA: 0x00012E44 File Offset: 0x00011044
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Execute()
	{
		Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Prefabs/Game")));
	}
}
