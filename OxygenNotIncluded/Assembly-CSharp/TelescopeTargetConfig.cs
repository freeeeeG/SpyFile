using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class TelescopeTargetConfig : IEntityConfig
{
	// Token: 0x06000D85 RID: 3461 RVA: 0x0004BA24 File Offset: 0x00049C24
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0004BA2B File Offset: 0x00049C2B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TelescopeTarget", "TelescopeTarget", true);
		gameObject.AddOrGet<TelescopeTarget>();
		return gameObject;
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0004BA44 File Offset: 0x00049C44
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0004BA46 File Offset: 0x00049C46
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007CB RID: 1995
	public const string ID = "TelescopeTarget";
}
