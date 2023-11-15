using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011D RID: 285
[EntityConfigOrder(2)]
public class BabyPuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x06000579 RID: 1401 RVA: 0x00024CAF File Offset: 0x00022EAF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00024CB6 File Offset: 0x00022EB6
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby", CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftBleachstone", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00024CF4 File Offset: 0x00022EF4
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00024CF6 File Offset: 0x00022EF6
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040003C2 RID: 962
	public const string ID = "PuftBleachstoneBaby";
}
