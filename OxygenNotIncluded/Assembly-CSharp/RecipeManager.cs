using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A4B RID: 2635
public class RecipeManager
{
	// Token: 0x06004F55 RID: 20309 RVA: 0x001C0673 File Offset: 0x001BE873
	public static RecipeManager Get()
	{
		if (RecipeManager._Instance == null)
		{
			RecipeManager._Instance = new RecipeManager();
		}
		return RecipeManager._Instance;
	}

	// Token: 0x06004F56 RID: 20310 RVA: 0x001C068B File Offset: 0x001BE88B
	public static void DestroyInstance()
	{
		RecipeManager._Instance = null;
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x001C0693 File Offset: 0x001BE893
	public void Add(Recipe recipe)
	{
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x040033EC RID: 13292
	private static RecipeManager _Instance;

	// Token: 0x040033ED RID: 13293
	public List<Recipe> recipes = new List<Recipe>();
}
