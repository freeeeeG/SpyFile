using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000A5 RID: 165
[EntityConfigOrder(2)]
public class BabyBeeConfig : IEntityConfig
{
	// Token: 0x060002E0 RID: 736 RVA: 0x000174AC File Offset: 0x000156AC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x000174B4 File Offset: 0x000156B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BeeConfig.CreateBee("BeeBaby", CREATURES.SPECIES.BEE.BABY.NAME, CREATURES.SPECIES.BEE.BABY.DESC, "baby_blarva_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Bee", null, true, 2f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		return gameObject;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0001750E File Offset: 0x0001570E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00017510 File Offset: 0x00015710
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040001FE RID: 510
	public const string ID = "BeeBaby";
}
