using System;
using UnityEngine;

// Token: 0x02000286 RID: 646
public class SleepLocator : IEntityConfig
{
	// Token: 0x06000D15 RID: 3349 RVA: 0x00048CFD File Offset: 0x00046EFD
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00048D04 File Offset: 0x00046F04
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SleepLocator.ID, SleepLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Sleepable>();
		return gameObject;
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00048D2F File Offset: 0x00046F2F
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00048D31 File Offset: 0x00046F31
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000795 RID: 1941
	public static readonly string ID = "SleepLocator";
}
