using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200010E RID: 270
[EntityConfigOrder(2)]
public class OilFloaterBabyConfig : IEntityConfig
{
	// Token: 0x06000524 RID: 1316 RVA: 0x00023D49 File Offset: 0x00021F49
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00023D50 File Offset: 0x00021F50
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", CREATURES.SPECIES.OILFLOATER.BABY.NAME, CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Oilfloater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00023D8E File Offset: 0x00021F8E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00023D90 File Offset: 0x00021F90
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000386 RID: 902
	public const string ID = "OilfloaterBaby";
}
