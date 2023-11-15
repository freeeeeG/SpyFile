using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F5 RID: 245
[EntityConfigOrder(2)]
public class BabyHatchHardConfig : IEntityConfig
{
	// Token: 0x0600048A RID: 1162 RVA: 0x00021A71 File Offset: 0x0001FC71
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00021A78 File Offset: 0x0001FC78
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchHardConfig.CreateHatch("HatchHardBaby", CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchHard", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00021AB6 File Offset: 0x0001FCB6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00021AB8 File Offset: 0x0001FCB8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400031C RID: 796
	public const string ID = "HatchHardBaby";
}
