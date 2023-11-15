using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class IntermediateRadPillConfig : IEntityConfig
{
	// Token: 0x06000BCE RID: 3022 RVA: 0x00042015 File Offset: 0x00040215
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0004201C File Offset: 0x0004021C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateRadPill", ITEMS.PILLS.INTERMEDIATERADPILL.NAME, ITEMS.PILLS.INTERMEDIATERADPILL.DESC, 1f, true, Assets.GetAnim("vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATERADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedApothecary", array, array2), array, array2)
		{
			time = 50f,
			description = ITEMS.PILLS.INTERMEDIATERADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"AdvancedApothecary"
			},
			sortOrder = 21
		};
		return gameObject;
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x0004211F File Offset: 0x0004031F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00042121 File Offset: 0x00040321
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006E1 RID: 1761
	public const string ID = "IntermediateRadPill";

	// Token: 0x040006E2 RID: 1762
	public static ComplexRecipe recipe;
}
