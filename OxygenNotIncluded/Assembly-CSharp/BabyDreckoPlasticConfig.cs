using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F0 RID: 240
[EntityConfigOrder(2)]
public class BabyDreckoPlasticConfig : IEntityConfig
{
	// Token: 0x0600046D RID: 1133 RVA: 0x0002129D File Offset: 0x0001F49D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x000212A4 File Offset: 0x0001F4A4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoPlasticConfig.CreateDrecko("DreckoPlasticBaby", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DreckoPlastic", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000212E2 File Offset: 0x0001F4E2
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000212E4 File Offset: 0x0001F4E4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000301 RID: 769
	public const string ID = "DreckoPlasticBaby";
}
