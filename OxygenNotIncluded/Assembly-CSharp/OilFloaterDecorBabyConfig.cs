using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000110 RID: 272
[EntityConfigOrder(2)]
public class OilFloaterDecorBabyConfig : IEntityConfig
{
	// Token: 0x06000530 RID: 1328 RVA: 0x00023F95 File Offset: 0x00022195
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00023F9C File Offset: 0x0002219C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecorBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterDecor", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00023FDA File Offset: 0x000221DA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00023FDC File Offset: 0x000221DC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400038E RID: 910
	public const string ID = "OilfloaterDecorBaby";
}
