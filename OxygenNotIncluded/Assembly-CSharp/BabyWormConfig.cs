using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000EC RID: 236
[EntityConfigOrder(2)]
public class BabyWormConfig : IEntityConfig
{
	// Token: 0x06000455 RID: 1109 RVA: 0x00020C05 File Offset: 0x0001EE05
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00020C0C File Offset: 0x0001EE0C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentWormConfig.CreateWorm("DivergentWormBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.DESC, "baby_worm_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentWorm", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00020C4A File Offset: 0x0001EE4A
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00020C4C File Offset: 0x0001EE4C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002E5 RID: 741
	public const string ID = "DivergentWormBaby";
}
