using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000FB RID: 251
[EntityConfigOrder(2)]
public class LightBugBlackBabyConfig : IEntityConfig
{
	// Token: 0x060004AF RID: 1199 RVA: 0x0002224B File Offset: 0x0002044B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00022252 File Offset: 0x00020452
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlackBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlack", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00022290 File Offset: 0x00020490
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00022292 File Offset: 0x00020492
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000334 RID: 820
	public const string ID = "LightBugBlackBaby";
}
