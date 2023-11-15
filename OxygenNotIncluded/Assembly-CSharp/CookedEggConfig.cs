using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class CookedEggConfig : IEntityConfig
{
	// Token: 0x06000724 RID: 1828 RVA: 0x0002D99C File Offset: 0x0002BB9C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedEgg", ITEMS.FOOD.COOKEDEGG.NAME, ITEMS.FOOD.COOKEDEGG.DESC, 1f, false, Assets.GetAnim("cookedegg_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_EGG);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0002DA08 File Offset: 0x0002BC08
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0002DA0A File Offset: 0x0002BC0A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004EC RID: 1260
	public const string ID = "CookedEgg";

	// Token: 0x040004ED RID: 1261
	public static ComplexRecipe recipe;
}
