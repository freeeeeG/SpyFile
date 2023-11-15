using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000A4C RID: 2636
public class ComplexRecipeManager
{
	// Token: 0x06004F59 RID: 20313 RVA: 0x001C06CD File Offset: 0x001BE8CD
	public static ComplexRecipeManager Get()
	{
		if (ComplexRecipeManager._Instance == null)
		{
			ComplexRecipeManager._Instance = new ComplexRecipeManager();
		}
		return ComplexRecipeManager._Instance;
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x001C06E5 File Offset: 0x001BE8E5
	public static void DestroyInstance()
	{
		ComplexRecipeManager._Instance = null;
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x001C06F0 File Offset: 0x001BE8F0
	public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
	{
		string str = "_";
		Tag tag = signatureElement;
		return fabricator + str + tag.ToString();
	}

	// Token: 0x06004F5C RID: 20316 RVA: 0x001C0718 File Offset: 0x001BE918
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x001C0800 File Offset: 0x001BEA00
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs, string facadeID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		stringBuilder.Append("_" + facadeID);
		return stringBuilder.ToString();
	}

	// Token: 0x06004F5E RID: 20318 RVA: 0x001C08F8 File Offset: 0x001BEAF8
	public void Add(ComplexRecipe recipe)
	{
		using (List<ComplexRecipe>.Enumerator enumerator = this.recipes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.id == recipe.id)
				{
					global::Debug.LogError(string.Format("DUPLICATE RECIPE ID! '{0}' is being added to the recipe manager multiple times. This will result in the failure to save/load certain queued recipes at fabricators.", recipe.id));
				}
			}
		}
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x06004F5F RID: 20319 RVA: 0x001C0990 File Offset: 0x001BEB90
	public ComplexRecipe GetRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.recipes.Find((ComplexRecipe r) => r.id == id);
	}

	// Token: 0x06004F60 RID: 20320 RVA: 0x001C09D0 File Offset: 0x001BEBD0
	public void AddObsoleteIDMapping(string obsolete_id, string new_id)
	{
		this.obsoleteIDMapping[obsolete_id] = new_id;
	}

	// Token: 0x06004F61 RID: 20321 RVA: 0x001C09E0 File Offset: 0x001BEBE0
	public ComplexRecipe GetObsoleteRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		ComplexRecipe result = null;
		string id2 = null;
		if (this.obsoleteIDMapping.TryGetValue(id, out id2))
		{
			result = this.GetRecipe(id2);
		}
		return result;
	}

	// Token: 0x040033EE RID: 13294
	private static ComplexRecipeManager _Instance;

	// Token: 0x040033EF RID: 13295
	public List<ComplexRecipe> recipes = new List<ComplexRecipe>();

	// Token: 0x040033F0 RID: 13296
	private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();
}
