using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class AdvancedCureConfig : IEntityConfig
{
	// Token: 0x06000BAB RID: 2987 RVA: 0x00041801 File Offset: 0x0003FA01
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00041808 File Offset: 0x0003FA08
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("AdvancedCure", ITEMS.PILLS.ADVANCEDCURE.NAME, ITEMS.PILLS.ADVANCEDCURE.DESC, 1f, true, Assets.GetAnim("vial_spore_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ADVANCEDCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement("LightBugOrangeEgg", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("AdvancedCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string text = "Apothecary";
		AdvancedCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 200f,
			description = ITEMS.PILLS.ADVANCEDCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 20,
			requiredTech = "MedicineIV"
		};
		return gameObject;
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0004192B File Offset: 0x0003FB2B
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0004192D File Offset: 0x0003FB2D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D3 RID: 1747
	public const string ID = "AdvancedCure";

	// Token: 0x040006D4 RID: 1748
	public static ComplexRecipe recipe;
}
