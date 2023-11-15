using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F7 RID: 247
[EntityConfigOrder(2)]
public class BabyHatchMetalConfig : IEntityConfig
{
	// Token: 0x06000497 RID: 1175 RVA: 0x00021D31 File Offset: 0x0001FF31
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00021D38 File Offset: 0x0001FF38
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchMetalConfig.CreateHatch("HatchMetalBaby", CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchMetal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00021D76 File Offset: 0x0001FF76
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00021D78 File Offset: 0x0001FF78
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000324 RID: 804
	public const string ID = "HatchMetalBaby";
}
