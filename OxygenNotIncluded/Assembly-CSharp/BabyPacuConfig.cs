using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000117 RID: 279
[EntityConfigOrder(2)]
public class BabyPacuConfig : IEntityConfig
{
	// Token: 0x06000555 RID: 1365 RVA: 0x000245A5 File Offset: 0x000227A5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x000245AC File Offset: 0x000227AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuConfig.CreatePacu("PacuBaby", CREATURES.SPECIES.PACU.BABY.NAME, CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Pacu", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x000245EA File Offset: 0x000227EA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000245EC File Offset: 0x000227EC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003A6 RID: 934
	public const string ID = "PacuBaby";
}
