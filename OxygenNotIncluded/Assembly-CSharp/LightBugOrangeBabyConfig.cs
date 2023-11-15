using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000103 RID: 259
[EntityConfigOrder(2)]
public class LightBugOrangeBabyConfig : IEntityConfig
{
	// Token: 0x060004DF RID: 1247 RVA: 0x00022C8B File Offset: 0x00020E8B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00022C92 File Offset: 0x00020E92
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrangeBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugOrange", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00022CD0 File Offset: 0x00020ED0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00022CD2 File Offset: 0x00020ED2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000350 RID: 848
	public const string ID = "LightBugOrangeBaby";
}
