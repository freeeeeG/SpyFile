using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000114 RID: 276
[EntityConfigOrder(2)]
public class BabyPacuCleanerConfig : IEntityConfig
{
	// Token: 0x06000548 RID: 1352 RVA: 0x00024468 File Offset: 0x00022668
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002446F File Offset: 0x0002266F
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuCleaner", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x000244AD File Offset: 0x000226AD
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x000244AF File Offset: 0x000226AF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003A1 RID: 929
	public const string ID = "PacuCleanerBaby";
}
