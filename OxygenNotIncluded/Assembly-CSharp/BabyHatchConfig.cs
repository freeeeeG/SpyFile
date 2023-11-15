using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F3 RID: 243
[EntityConfigOrder(2)]
public class BabyHatchConfig : IEntityConfig
{
	// Token: 0x0600047E RID: 1150 RVA: 0x0002180F File Offset: 0x0001FA0F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00021816 File Offset: 0x0001FA16
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchConfig.CreateHatch("HatchBaby", CREATURES.SPECIES.HATCH.BABY.NAME, CREATURES.SPECIES.HATCH.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Hatch", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00021854 File Offset: 0x0001FA54
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00021856 File Offset: 0x0001FA56
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000313 RID: 787
	public const string ID = "HatchBaby";
}
