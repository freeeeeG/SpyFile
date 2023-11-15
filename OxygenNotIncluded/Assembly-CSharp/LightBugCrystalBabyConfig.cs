using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000101 RID: 257
[EntityConfigOrder(2)]
public class LightBugCrystalBabyConfig : IEntityConfig
{
	// Token: 0x060004D3 RID: 1235 RVA: 0x00022A17 File Offset: 0x00020C17
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00022A1E File Offset: 0x00020C1E
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystalBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugCrystal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00022A5C File Offset: 0x00020C5C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00022A5E File Offset: 0x00020C5E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000349 RID: 841
	public const string ID = "LightBugCrystalBaby";
}
