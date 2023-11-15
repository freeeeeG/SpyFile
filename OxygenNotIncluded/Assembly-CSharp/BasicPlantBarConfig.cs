using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class BasicPlantBarConfig : IEntityConfig
{
	// Token: 0x0600070B RID: 1803 RVA: 0x0002D71D File Offset: 0x0002B91D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0002D724 File Offset: 0x0002B924
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicPlantBar", ITEMS.FOOD.BASICPLANTBAR.NAME, ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim("liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.BASICPLANTBAR);
		ComplexRecipeManager.Get().GetRecipe(BasicPlantBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0002D7AB File Offset: 0x0002B9AB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0002D7AD File Offset: 0x0002B9AD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004E3 RID: 1251
	public const string ID = "BasicPlantBar";

	// Token: 0x040004E4 RID: 1252
	public static ComplexRecipe recipe;
}
