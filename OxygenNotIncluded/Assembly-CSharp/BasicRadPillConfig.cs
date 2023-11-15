using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class BasicRadPillConfig : IEntityConfig
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x00041CAE File Offset: 0x0003FEAE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00041CB8 File Offset: 0x0003FEB8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("BasicRadPill", ITEMS.PILLS.BASICRADPILL.NAME, ITEMS.PILLS.BASICRADPILL.DESC, 1f, true, Assets.GetAnim("pill_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.BASICRADPILL);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Carbon", 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("BasicRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		BasicRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2), array, array2)
		{
			time = 50f,
			description = ITEMS.PILLS.BASICRADPILL.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				"Apothecary"
			},
			sortOrder = 10
		};
		return gameObject;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00041DBB File Offset: 0x0003FFBB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00041DBD File Offset: 0x0003FFBD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006DB RID: 1755
	public const string ID = "BasicRadPill";

	// Token: 0x040006DC RID: 1756
	public static ComplexRecipe recipe;
}
