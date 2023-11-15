using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F9 RID: 249
[EntityConfigOrder(2)]
public class BabyHatchVeggieConfig : IEntityConfig
{
	// Token: 0x060004A3 RID: 1187 RVA: 0x00021F95 File Offset: 0x00020195
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00021F9C File Offset: 0x0002019C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchVeggieConfig.CreateHatch("HatchVeggieBaby", CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchVeggie", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00021FDA File Offset: 0x000201DA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00021FDC File Offset: 0x000201DC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400032D RID: 813
	public const string ID = "HatchVeggieBaby";
}
