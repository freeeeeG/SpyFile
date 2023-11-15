using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012B RID: 299
[EntityConfigOrder(2)]
public class BabyStaterpillarGasConfig : IEntityConfig
{
	// Token: 0x060005CE RID: 1486 RVA: 0x00026AF1 File Offset: 0x00024CF1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00026AF8 File Offset: 0x00024CF8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarGas", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00026B36 File Offset: 0x00024D36
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00026B4E File Offset: 0x00024D4E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400040E RID: 1038
	public const string ID = "StaterpillarGasBaby";
}
