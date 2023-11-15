using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000127 RID: 295
[EntityConfigOrder(2)]
public class BabySquirrelHugConfig : IEntityConfig
{
	// Token: 0x060005B6 RID: 1462 RVA: 0x00026481 File Offset: 0x00024681
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00026488 File Offset: 0x00024688
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelHugConfig.CreateSquirrelHug("SquirrelHugBaby", CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.NAME, CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "SquirrelHug", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x000264C6 File Offset: 0x000246C6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x000264C8 File Offset: 0x000246C8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003F7 RID: 1015
	public const string ID = "SquirrelHugBaby";
}
