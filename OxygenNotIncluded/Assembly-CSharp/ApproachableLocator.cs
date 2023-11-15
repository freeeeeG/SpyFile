using System;
using UnityEngine;

// Token: 0x02000285 RID: 645
public class ApproachableLocator : IEntityConfig
{
	// Token: 0x06000D0F RID: 3343 RVA: 0x00048CBA File Offset: 0x00046EBA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00048CC1 File Offset: 0x00046EC1
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ApproachableLocator.ID, ApproachableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		return gameObject;
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x00048CE5 File Offset: 0x00046EE5
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x00048CE7 File Offset: 0x00046EE7
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000794 RID: 1940
	public static readonly string ID = "ApproachableLocator";
}
