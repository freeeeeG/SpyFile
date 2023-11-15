using System;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class OrbitalBGConfig : IEntityConfig
{
	// Token: 0x06000D68 RID: 3432 RVA: 0x0004B8B6 File Offset: 0x00049AB6
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0004B8BD File Offset: 0x00049ABD
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OrbitalBGConfig.ID, OrbitalBGConfig.ID, false);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OrbitalObject>();
		gameObject.AddOrGet<SaveLoadRoot>();
		return gameObject;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0004B8E4 File Offset: 0x00049AE4
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x0004B8E6 File Offset: 0x00049AE6
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007C6 RID: 1990
	public static string ID = "OrbitalBG";
}
