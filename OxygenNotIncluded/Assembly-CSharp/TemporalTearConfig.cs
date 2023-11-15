using System;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class TemporalTearConfig : IEntityConfig
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x0004BA50 File Offset: 0x00049C50
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0004BA57 File Offset: 0x00049C57
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TemporalTear", "TemporalTear", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<TemporalTear>();
		return gameObject;
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0004BA77 File Offset: 0x00049C77
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0004BA79 File Offset: 0x00049C79
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007CC RID: 1996
	public const string ID = "TemporalTear";
}
