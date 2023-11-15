using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000119 RID: 281
[EntityConfigOrder(2)]
public class BabyPacuTropicalConfig : IEntityConfig
{
	// Token: 0x06000561 RID: 1377 RVA: 0x000246E5 File Offset: 0x000228E5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x000246EC File Offset: 0x000228EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuTropicalConfig.CreatePacu("PacuTropicalBaby", CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuTropical", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0002472A File Offset: 0x0002292A
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0002472C File Offset: 0x0002292C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003AC RID: 940
	public const string ID = "PacuTropicalBaby";
}
