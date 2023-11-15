using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000EE RID: 238
[EntityConfigOrder(2)]
public class BabyDreckoConfig : IEntityConfig
{
	// Token: 0x06000461 RID: 1121 RVA: 0x00020F61 File Offset: 0x0001F161
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00020F68 File Offset: 0x0001F168
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoConfig.CreateDrecko("DreckoBaby", CREATURES.SPECIES.DRECKO.BABY.NAME, CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Drecko", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00020FA6 File Offset: 0x0001F1A6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00020FA8 File Offset: 0x0001F1A8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002F3 RID: 755
	public const string ID = "DreckoBaby";
}
