using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000129 RID: 297
[EntityConfigOrder(2)]
public class BabyStaterpillarConfig : IEntityConfig
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x0002670D File Offset: 0x0002490D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00026714 File Offset: 0x00024914
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarConfig.CreateStaterpillar("StaterpillarBaby", CREATURES.SPECIES.STATERPILLAR.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Staterpillar", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00026752 File Offset: 0x00024952
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00026754 File Offset: 0x00024954
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003FE RID: 1022
	public const string ID = "StaterpillarBaby";
}
