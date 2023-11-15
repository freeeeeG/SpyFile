using System;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class TargetLocator : IEntityConfig
{
	// Token: 0x06000D09 RID: 3337 RVA: 0x00048C7E File Offset: 0x00046E7E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x00048C85 File Offset: 0x00046E85
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x00048CA2 File Offset: 0x00046EA2
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x00048CA4 File Offset: 0x00046EA4
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000793 RID: 1939
	public static readonly string ID = "TargetLocator";
}
