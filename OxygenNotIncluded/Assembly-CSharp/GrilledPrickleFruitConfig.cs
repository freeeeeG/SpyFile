using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class GrilledPrickleFruitConfig : IEntityConfig
{
	// Token: 0x06000756 RID: 1878 RVA: 0x0002DE74 File Offset: 0x0002C074
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0002DE7C File Offset: 0x0002C07C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0002DEE0 File Offset: 0x0002C0E0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0002DEE2 File Offset: 0x0002C0E2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004FD RID: 1277
	public const string ID = "GrilledPrickleFruit";

	// Token: 0x040004FE RID: 1278
	public static ComplexRecipe recipe;
}
