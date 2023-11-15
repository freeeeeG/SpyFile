using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000247 RID: 583
public class BasicBoosterConfig : IEntityConfig
{
	// Token: 0x06000BB5 RID: 2997 RVA: 0x00041A66 File Offset: 0x0003FC66
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00041A70 File Offset: 0x0003FC70
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicBooster", ITEMS.PILLS.BASICBOOSTER.NAME, ITEMS.PILLS.BASICBOOSTER.DESC, 1f, true, Assets.GetAnim("pill_2_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICBOOSTER);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicBooster".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = ITEMS.PILLS.BASICBOOSTER.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 1
		};
		return gameObject;
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00041B72 File Offset: 0x0003FD72
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00041B74 File Offset: 0x0003FD74
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D7 RID: 1751
	public const string ID = "BasicBooster";

	// Token: 0x040006D8 RID: 1752
	public static ComplexRecipe recipe;
}
