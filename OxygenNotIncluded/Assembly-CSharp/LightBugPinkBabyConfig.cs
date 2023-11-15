using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000105 RID: 261
[EntityConfigOrder(2)]
public class LightBugPinkBabyConfig : IEntityConfig
{
	// Token: 0x060004EB RID: 1259 RVA: 0x00022F1F File Offset: 0x0002111F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00022F26 File Offset: 0x00021126
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPink", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00022F64 File Offset: 0x00021164
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00022F66 File Offset: 0x00021166
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000357 RID: 855
	public const string ID = "LightBugPinkBaby";
}
