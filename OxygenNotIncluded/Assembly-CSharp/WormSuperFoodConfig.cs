using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class WormSuperFoodConfig : IEntityConfig
{
	// Token: 0x060007E9 RID: 2025 RVA: 0x0002EF94 File Offset: 0x0002D194
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0002EF9C File Offset: 0x0002D19C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFood", ITEMS.FOOD.WORMSUPERFOOD.NAME, ITEMS.FOOD.WORMSUPERFOOD.DESC, 1f, false, Assets.GetAnim("wormwood_preserved_berries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.WORMSUPERFOOD);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0002F000 File Offset: 0x0002D200
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0002F002 File Offset: 0x0002D202
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400052E RID: 1326
	public const string ID = "WormSuperFood";

	// Token: 0x0400052F RID: 1327
	public static ComplexRecipe recipe;
}
