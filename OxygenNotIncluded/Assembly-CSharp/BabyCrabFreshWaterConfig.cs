using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E6 RID: 230
[EntityConfigOrder(2)]
public class BabyCrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x06000431 RID: 1073 RVA: 0x0002038A File Offset: 0x0001E58A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00020391 File Offset: 0x0001E591
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWaterBaby", CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.DESC, "baby_pincher_kanim", true, null);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabFreshWater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000203D0 File Offset: 0x0001E5D0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x000203D2 File Offset: 0x0001E5D2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002C2 RID: 706
	public const string ID = "CrabFreshWaterBaby";
}
