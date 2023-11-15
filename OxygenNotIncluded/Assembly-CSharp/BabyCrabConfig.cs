using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E4 RID: 228
[EntityConfigOrder(2)]
public class BabyCrabConfig : IEntityConfig
{
	// Token: 0x06000425 RID: 1061 RVA: 0x0001FFB5 File Offset: 0x0001E1B5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001FFBC File Offset: 0x0001E1BC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("CrabBaby", CREATURES.SPECIES.CRAB.BABY.NAME, CREATURES.SPECIES.CRAB.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Crab", "BabyCrabShell", false, 5f);
		return gameObject;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0002000E File Offset: 0x0001E20E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00020010 File Offset: 0x0001E210
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002B8 RID: 696
	public const string ID = "CrabBaby";
}
