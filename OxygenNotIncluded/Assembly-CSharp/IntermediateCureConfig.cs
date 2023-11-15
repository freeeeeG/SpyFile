using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class IntermediateCureConfig : IEntityConfig
{
	// Token: 0x06000BC9 RID: 3017 RVA: 0x00041EDE File Offset: 0x000400DE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00041EE8 File Offset: 0x000400E8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IntermediateCure", ITEMS.PILLS.INTERMEDIATECURE.NAME, ITEMS.PILLS.INTERMEDIATECURE.DESC, 1f, true, Assets.GetAnim("iv_slimelung_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.INTERMEDIATECURE);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SwampLilyFlowerConfig.ID, 1f),
			new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("IntermediateCure", 1f)
		};
		string text = "Apothecary";
		IntermediateCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(text, array, array2), array, array2)
		{
			time = 100f,
			description = ITEMS.PILLS.INTERMEDIATECURE.RECIPEDESC,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
			fabricators = new List<Tag>
			{
				text
			},
			sortOrder = 10,
			requiredTech = "MedicineII"
		};
		return gameObject;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00042009 File Offset: 0x00040209
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0004200B File Offset: 0x0004020B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006DF RID: 1759
	public const string ID = "IntermediateCure";

	// Token: 0x040006E0 RID: 1760
	public static ComplexRecipe recipe;
}
