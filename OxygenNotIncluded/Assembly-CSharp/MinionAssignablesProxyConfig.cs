using System;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class MinionAssignablesProxyConfig : IEntityConfig
{
	// Token: 0x06000D36 RID: 3382 RVA: 0x000492AE File Offset: 0x000474AE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x000492B5 File Offset: 0x000474B5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<Ownables>();
		gameObject.AddOrGet<Equipment>();
		gameObject.AddOrGet<MinionAssignablesProxy>();
		return gameObject;
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x000492E3 File Offset: 0x000474E3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x000492E5 File Offset: 0x000474E5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007B4 RID: 1972
	public static string ID = "MinionAssignablesProxy";
}
