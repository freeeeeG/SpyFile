using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class FruitCakeConfig : IEntityConfig
{
	// Token: 0x0600074C RID: 1868 RVA: 0x0002DD60 File Offset: 0x0002BF60
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0002DD68 File Offset: 0x0002BF68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("FruitCake", ITEMS.FOOD.FRUITCAKE.NAME, ITEMS.FOOD.FRUITCAKE.DESC, 1f, false, Assets.GetAnim("fruitcake_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.FRUITCAKE);
		ComplexRecipeManager.Get().GetRecipe(FruitCakeConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0002DDEF File Offset: 0x0002BFEF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0002DDF1 File Offset: 0x0002BFF1
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004F9 RID: 1273
	public const string ID = "FruitCake";

	// Token: 0x040004FA RID: 1274
	public static ComplexRecipe recipe;
}
