using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000112 RID: 274
[EntityConfigOrder(2)]
public class OilFloaterHighTempBabyConfig : IEntityConfig
{
	// Token: 0x0600053C RID: 1340 RVA: 0x000241E3 File Offset: 0x000223E3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000241EA File Offset: 0x000223EA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTempBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterHighTemp", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00024228 File Offset: 0x00022428
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0002422A File Offset: 0x0002242A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000398 RID: 920
	public const string ID = "OilfloaterHighTempBaby";
}
