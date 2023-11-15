using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024A RID: 586
public class IntermediateBoosterConfig : IEntityConfig
{
	// Token: 0x06000BC4 RID: 3012 RVA: 0x00041DC7 File Offset: 0x0003FFC7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00041DD0 File Offset: 0x0003FFD0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateBooster", ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME, ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC, 1f, true, Assets.GetAnim("pill_3_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATEBOOSTER);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateBooster", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		IntermediateBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 100f,
			description = ITEMS.PILLS.INTERMEDIATEBOOSTER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 5
		};
		return gameObject;
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00041ED2 File Offset: 0x000400D2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00041ED4 File Offset: 0x000400D4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006DD RID: 1757
	public const string ID = "IntermediateBooster";

	// Token: 0x040006DE RID: 1758
	public static ComplexRecipe recipe;
}
