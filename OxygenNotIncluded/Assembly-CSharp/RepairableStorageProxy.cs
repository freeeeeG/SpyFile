using System;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class RepairableStorageProxy : IEntityConfig
{
	// Token: 0x06000D6E RID: 3438 RVA: 0x0004B8FC File Offset: 0x00049AFC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x0004B903 File Offset: 0x00049B03
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
		gameObject.AddOrGet<Storage>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x0004B927 File Offset: 0x00049B27
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x0004B929 File Offset: 0x00049B29
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040007C7 RID: 1991
	public static string ID = "RepairableStorageProxy";
}
