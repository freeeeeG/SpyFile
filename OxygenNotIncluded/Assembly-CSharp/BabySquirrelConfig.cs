using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000125 RID: 293
[EntityConfigOrder(2)]
public class BabySquirrelConfig : IEntityConfig
{
	// Token: 0x060005AA RID: 1450 RVA: 0x00026221 File Offset: 0x00024421
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00026228 File Offset: 0x00024428
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelConfig.CreateSquirrel("SquirrelBaby", CREATURES.SPECIES.SQUIRREL.BABY.NAME, CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Squirrel", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00026266 File Offset: 0x00024466
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00026268 File Offset: 0x00024468
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003EB RID: 1003
	public const string ID = "SquirrelBaby";
}
