using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000FD RID: 253
[EntityConfigOrder(2)]
public class LightBugBlueBabyConfig : IEntityConfig
{
	// Token: 0x060004BB RID: 1211 RVA: 0x000224FF File Offset: 0x000206FF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00022506 File Offset: 0x00020706
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlueConfig.CreateLightBug("LightBugBlueBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlue", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00022544 File Offset: 0x00020744
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00022546 File Offset: 0x00020746
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400033B RID: 827
	public const string ID = "LightBugBlueBaby";
}
