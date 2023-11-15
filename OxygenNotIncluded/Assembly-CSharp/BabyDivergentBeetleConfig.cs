using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000EA RID: 234
[EntityConfigOrder(2)]
public class BabyDivergentBeetleConfig : IEntityConfig
{
	// Token: 0x06000449 RID: 1097 RVA: 0x00020893 File Offset: 0x0001EA93
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0002089A File Offset: 0x0001EA9A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetleBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.DESC, "baby_critter_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentBeetle", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000208D8 File Offset: 0x0001EAD8
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x000208DA File Offset: 0x0001EADA
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040002D6 RID: 726
	public const string ID = "DivergentBeetleBaby";
}
