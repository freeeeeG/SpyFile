using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000121 RID: 289
[EntityConfigOrder(2)]
public class BabyPuftOxyliteConfig : IEntityConfig
{
	// Token: 0x06000591 RID: 1425 RVA: 0x000251AB File Offset: 0x000233AB
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x000251B2 File Offset: 0x000233B2
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftOxylite", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x000251F0 File Offset: 0x000233F0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x000251F2 File Offset: 0x000233F2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003D8 RID: 984
	public const string ID = "PuftOxyliteBaby";
}
