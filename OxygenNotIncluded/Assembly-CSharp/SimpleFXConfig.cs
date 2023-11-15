using System;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class SimpleFXConfig : IEntityConfig
{
	// Token: 0x06000D79 RID: 3449 RVA: 0x0004B972 File Offset: 0x00049B72
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x0004B979 File Offset: 0x00049B79
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SimpleFXConfig.ID, SimpleFXConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		return gameObject;
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x0004B992 File Offset: 0x00049B92
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x0004B994 File Offset: 0x00049B94
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007C9 RID: 1993
	public static readonly string ID = "SimpleFX";
}
