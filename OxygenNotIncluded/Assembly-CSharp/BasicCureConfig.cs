using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000248 RID: 584
public class BasicCureConfig : IEntityConfig
{
	// Token: 0x06000BBA RID: 3002 RVA: 0x00041B7E File Offset: 0x0003FD7E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x00041B88 File Offset: 0x0003FD88
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicCure", ITEMS.PILLS.BASICCURE.NAME, ITEMS.PILLS.BASICCURE.DESC, 1f, true, Assets.GetAnim("pill_foodpoisoning_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICCURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Carbon.CreateTag(), 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = ITEMS.PILLS.BASICCURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00041CA2 File Offset: 0x0003FEA2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00041CA4 File Offset: 0x0003FEA4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D9 RID: 1753
	public const string ID = "BasicCure";

	// Token: 0x040006DA RID: 1754
	public static ComplexRecipe recipe;
}
