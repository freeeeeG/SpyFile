using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class WormBasicFoodConfig : IEntityConfig
{
	// Token: 0x060007DF RID: 2015 RVA: 0x0002EEA3 File Offset: 0x0002D0A3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFood", ITEMS.FOOD.WORMBASICFOOD.NAME, ITEMS.FOOD.WORMBASICFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_roast_nuts_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMBASICFOOD);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0002EF10 File Offset: 0x0002D110
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0002EF12 File Offset: 0x0002D112
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400052B RID: 1323
	public const string ID = "WormBasicFood";

	// Token: 0x0400052C RID: 1324
	public static ComplexRecipe recipe;
}
