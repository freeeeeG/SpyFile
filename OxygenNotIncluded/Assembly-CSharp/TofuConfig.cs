using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class TofuConfig : IEntityConfig
{
	// Token: 0x060007DA RID: 2010 RVA: 0x0002EE06 File Offset: 0x0002D006
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0002EE10 File Offset: 0x0002D010
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Tofu", ITEMS.FOOD.TOFU.NAME, ITEMS.FOOD.TOFU.DESC, 1f, false, Assets.GetAnim("loafu_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.TOFU);
		ComplexRecipeManager.Get().GetRecipe(TofuConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0002EE97 File Offset: 0x0002D097
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002EE99 File Offset: 0x0002D099
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000529 RID: 1321
	public const string ID = "Tofu";

	// Token: 0x0400052A RID: 1322
	public static ComplexRecipe recipe;
}
