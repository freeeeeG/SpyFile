using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000109 RID: 265
[EntityConfigOrder(2)]
public class BabyMoleConfig : IEntityConfig
{
	// Token: 0x06000504 RID: 1284 RVA: 0x00023501 File Offset: 0x00021701
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00023508 File Offset: 0x00021708
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleConfig.CreateMole("MoleBaby", CREATURES.SPECIES.MOLE.BABY.NAME, CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Mole", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00023546 File Offset: 0x00021746
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00023548 File Offset: 0x00021748
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x04000365 RID: 869
	public const string ID = "MoleBaby";
}
