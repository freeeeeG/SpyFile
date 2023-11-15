using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011F RID: 287
[EntityConfigOrder(2)]
public class BabyPuftConfig : IEntityConfig
{
	// Token: 0x06000585 RID: 1413 RVA: 0x00024F29 File Offset: 0x00023129
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00024F30 File Offset: 0x00023130
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftConfig.CreatePuft("PuftBaby", CREATURES.SPECIES.PUFT.BABY.NAME, CREATURES.SPECIES.PUFT.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Puft", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00024F6E File Offset: 0x0002316E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00024F70 File Offset: 0x00023170
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003CE RID: 974
	public const string ID = "PuftBaby";
}
