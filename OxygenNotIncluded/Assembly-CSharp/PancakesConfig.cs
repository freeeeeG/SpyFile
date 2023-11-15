using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class PancakesConfig : IEntityConfig
{
	// Token: 0x0600077C RID: 1916 RVA: 0x0002E38E File Offset: 0x0002C58E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0002E398 File Offset: 0x0002C598
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", ITEMS.FOOD.PANCAKES.NAME, ITEMS.FOOD.PANCAKES.DESC, 1f, false, Assets.GetAnim("stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PANCAKES);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0002E3FC File Offset: 0x0002C5FC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0002E3FE File Offset: 0x0002C5FE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000509 RID: 1289
	public const string ID = "Pancakes";

	// Token: 0x0400050A RID: 1290
	public static ComplexRecipe recipe;
}
