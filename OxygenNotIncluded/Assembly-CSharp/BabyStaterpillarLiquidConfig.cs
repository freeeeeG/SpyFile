using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012D RID: 301
[EntityConfigOrder(2)]
public class BabyStaterpillarLiquidConfig : IEntityConfig
{
	// Token: 0x060005DA RID: 1498 RVA: 0x00026EE9 File Offset: 0x000250E9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00026EF0 File Offset: 0x000250F0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00026F2E File Offset: 0x0002512E
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00026F46 File Offset: 0x00025146
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400041E RID: 1054
	public const string ID = "StaterpillarLiquidBaby";
}
