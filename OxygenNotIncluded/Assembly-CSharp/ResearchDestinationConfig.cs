using System;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class ResearchDestinationConfig : IEntityConfig
{
	// Token: 0x06000D74 RID: 3444 RVA: 0x0004B93F File Offset: 0x00049B3F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0004B946 File Offset: 0x00049B46
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ResearchDestination", "ResearchDestination", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ResearchDestination>();
		return gameObject;
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x0004B966 File Offset: 0x00049B66
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x0004B968 File Offset: 0x00049B68
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007C8 RID: 1992
	public const string ID = "ResearchDestination";
}
