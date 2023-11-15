using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class StoredMinionConfig : IEntityConfig
{
	// Token: 0x06000D7F RID: 3455 RVA: 0x0004B9AA File Offset: 0x00049BAA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x0004B9B4 File Offset: 0x00049BB4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(StoredMinionConfig.ID, StoredMinionConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<KPrefabID>();
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<Schedulable>();
		gameObject.AddOrGet<StoredMinionIdentity>();
		gameObject.AddOrGet<KSelectable>().IsSelectable = false;
		gameObject.AddOrGet<MinionModifiers>().addBaseTraits = false;
		return gameObject;
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x0004BA0C File Offset: 0x00049C0C
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x0004BA0E File Offset: 0x00049C0E
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007CA RID: 1994
	public static string ID = "StoredMinion";
}
