using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000E8 RID: 232
[EntityConfigOrder(2)]
public class BabyCrabWoodConfig : IEntityConfig
{
	// Token: 0x0600043D RID: 1085 RVA: 0x0002062A File Offset: 0x0001E82A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00020634 File Offset: 0x0001E834
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWoodBaby", CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabWoodShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabWood", "BabyCrabWoodShell", false, 5f);
		return gameObject;
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00020686 File Offset: 0x0001E886
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00020688 File Offset: 0x0001E888
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002CC RID: 716
	public const string ID = "CrabWoodBaby";
}
