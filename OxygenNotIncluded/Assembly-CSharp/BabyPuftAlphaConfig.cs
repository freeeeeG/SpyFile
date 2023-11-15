using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011B RID: 283
[EntityConfigOrder(2)]
public class BabyPuftAlphaConfig : IEntityConfig
{
	// Token: 0x0600056D RID: 1389 RVA: 0x00024A26 File Offset: 0x00022C26
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00024A2D File Offset: 0x00022C2D
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftAlpha", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00024A6B File Offset: 0x00022C6B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00024A6D File Offset: 0x00022C6D
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040003B8 RID: 952
	public const string ID = "PuftAlphaBaby";
}
