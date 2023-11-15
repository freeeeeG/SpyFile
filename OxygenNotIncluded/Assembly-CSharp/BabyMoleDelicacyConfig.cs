using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200010B RID: 267
[EntityConfigOrder(2)]
public class BabyMoleDelicacyConfig : IEntityConfig
{
	// Token: 0x06000511 RID: 1297 RVA: 0x000238DA File Offset: 0x00021ADA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x000238E1 File Offset: 0x00021AE1
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleDelicacyConfig.CreateMole("MoleDelicacyBaby", CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.NAME, CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "MoleDelicacy", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0002391F File Offset: 0x00021B1F
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00023921 File Offset: 0x00021B21
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x04000373 RID: 883
	public const string ID = "MoleDelicacyBaby";
}
