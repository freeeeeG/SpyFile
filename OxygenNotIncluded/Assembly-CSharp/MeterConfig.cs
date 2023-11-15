using System;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class MeterConfig : IEntityConfig
{
	// Token: 0x06000D30 RID: 3376 RVA: 0x0004926F File Offset: 0x0004746F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x00049276 File Offset: 0x00047476
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		gameObject.AddOrGet<KBatchedAnimTracker>();
		return gameObject;
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x00049296 File Offset: 0x00047496
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00049298 File Offset: 0x00047498
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007B3 RID: 1971
	public static readonly string ID = "Meter";
}
