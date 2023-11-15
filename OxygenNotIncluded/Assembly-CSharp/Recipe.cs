using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000905 RID: 2309
[DebuggerDisplay("{Name}")]
public class Recipe : IHasSortOrder
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x060042F9 RID: 17145 RVA: 0x001769B5 File Offset: 0x00174BB5
	// (set) Token: 0x060042FA RID: 17146 RVA: 0x001769BD File Offset: 0x00174BBD
	public int sortOrder { get; set; }

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x060042FC RID: 17148 RVA: 0x001769CF File Offset: 0x00174BCF
	// (set) Token: 0x060042FB RID: 17147 RVA: 0x001769C6 File Offset: 0x00174BC6
	public string Name
	{
		get
		{
			if (this.nameOverride != null)
			{
				return this.nameOverride;
			}
			return this.Result.ProperName();
		}
		set
		{
			this.nameOverride = value;
		}
	}

	// Token: 0x060042FD RID: 17149 RVA: 0x001769EB File Offset: 0x00174BEB
	public Recipe()
	{
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x00176A00 File Offset: 0x00174C00
	public Recipe(string prefabId, float outputUnits = 1f, SimHashes elementOverride = (SimHashes)0, string nameOverride = null, string recipeDescription = null, int sortOrder = 0)
	{
		global::Debug.Assert(prefabId != null);
		this.Result = TagManager.Create(prefabId);
		this.ResultElementOverride = elementOverride;
		this.nameOverride = nameOverride;
		this.OutputUnits = outputUnits;
		this.Ingredients = new List<Recipe.Ingredient>();
		this.recipeDescription = recipeDescription;
		this.sortOrder = sortOrder;
		this.FabricationVisualizer = null;
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x00176A6B File Offset: 0x00174C6B
	public Recipe SetFabricator(string fabricator, float fabricationTime)
	{
		this.fabricators = new string[]
		{
			fabricator
		};
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x06004300 RID: 17152 RVA: 0x00176A90 File Offset: 0x00174C90
	public Recipe SetFabricators(string[] fabricators, float fabricationTime)
	{
		this.fabricators = fabricators;
		this.FabricationTime = fabricationTime;
		RecipeManager.Get().Add(this);
		return this;
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x00176AAC File Offset: 0x00174CAC
	public Recipe SetIcon(Sprite Icon)
	{
		this.Icon = Icon;
		this.IconColor = Color.white;
		return this;
	}

	// Token: 0x06004302 RID: 17154 RVA: 0x00176AC1 File Offset: 0x00174CC1
	public Recipe SetIcon(Sprite Icon, Color IconColor)
	{
		this.Icon = Icon;
		this.IconColor = IconColor;
		return this;
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x00176AD2 File Offset: 0x00174CD2
	public Recipe AddIngredient(Recipe.Ingredient ingredient)
	{
		this.Ingredients.Add(ingredient);
		return this;
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x00176AE4 File Offset: 0x00174CE4
	public Recipe.Ingredient[] GetAllIngredients(IList<Tag> selectedTags)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			float amount = this.Ingredients[i].amount;
			if (i < selectedTags.Count)
			{
				list.Add(new Recipe.Ingredient(selectedTags[i], amount));
			}
			else
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, amount));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x00176B60 File Offset: 0x00174D60
	public Recipe.Ingredient[] GetAllIngredients(IList<Element> selected_elements)
	{
		List<Recipe.Ingredient> list = new List<Recipe.Ingredient>();
		for (int i = 0; i < this.Ingredients.Count; i++)
		{
			int num = (int)this.Ingredients[i].amount;
			bool flag = false;
			if (i < selected_elements.Count)
			{
				Element element = selected_elements[i];
				if (element != null && element.HasTag(this.Ingredients[i].tag))
				{
					list.Add(new Recipe.Ingredient(GameTagExtensions.Create(element.id), (float)num));
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(new Recipe.Ingredient(this.Ingredients[i].tag, (float)num));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x00176C18 File Offset: 0x00174E18
	public GameObject Craft(Storage resource_storage, IList<Tag> selectedTags)
	{
		Recipe.Ingredient[] allIngredients = this.GetAllIngredients(selectedTags);
		return this.CraftRecipe(resource_storage, allIngredients);
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x00176C38 File Offset: 0x00174E38
	private GameObject CraftRecipe(Storage resource_storage, Recipe.Ingredient[] ingredientTags)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		float num = 0f;
		float num2 = 0f;
		foreach (Recipe.Ingredient ingredient in ingredientTags)
		{
			GameObject gameObject = resource_storage.FindFirst(ingredient.tag);
			if (gameObject != null)
			{
				Edible component = gameObject.GetComponent<Edible>();
				if (component)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			SimUtil.DiseaseInfo b;
			float temp;
			resource_storage.ConsumeAndGetDisease(ingredient, out b, out temp);
			diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
			num = SimUtil.CalculateFinalTemperature(num2, num, ingredient.amount, temp);
			num2 += ingredient.amount;
		}
		GameObject prefab = Assets.GetPrefab(this.Result);
		GameObject gameObject2 = null;
		if (prefab != null)
		{
			gameObject2 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
			gameObject2.GetComponent<KSelectable>().entityName = this.Name;
			if (component2 != null)
			{
				gameObject2.GetComponent<KPrefabID>().RemoveTag(TagManager.Create("Vacuum"));
				if (this.ResultElementOverride != (SimHashes)0)
				{
					if (component2.GetComponent<ElementChunk>() != null)
					{
						component2.SetElement(this.ResultElementOverride, true);
					}
					else
					{
						component2.ElementID = this.ResultElementOverride;
					}
				}
				component2.Temperature = num;
				component2.Units = this.OutputUnits;
			}
			Edible component3 = gameObject2.GetComponent<Edible>();
			if (component3)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component3.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED, "{0}", component3.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
			}
			gameObject2.SetActive(true);
			if (component2 != null)
			{
				component2.AddDisease(diseaseInfo.idx, diseaseInfo.count, "Recipe.CraftRecipe");
			}
			gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
		}
		return gameObject2;
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06004308 RID: 17160 RVA: 0x00176E40 File Offset: 0x00175040
	public string[] MaterialOptionNames
	{
		get
		{
			List<string> list = new List<string>();
			foreach (Element element in ElementLoader.elements)
			{
				if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
				{
					list.Add(element.id.ToString());
				}
			}
			return list.ToArray();
		}
	}

	// Token: 0x06004309 RID: 17161 RVA: 0x00176ED0 File Offset: 0x001750D0
	public Element[] MaterialOptions()
	{
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (Array.IndexOf<Tag>(element.oreTags, this.Ingredients[0].tag) >= 0)
			{
				list.Add(element);
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x00176F50 File Offset: 0x00175150
	public BuildingDef GetBuildingDef()
	{
		BuildingComplete component = Assets.GetPrefab(this.Result).GetComponent<BuildingComplete>();
		if (component != null)
		{
			return component.Def;
		}
		return null;
	}

	// Token: 0x0600430B RID: 17163 RVA: 0x00176F80 File Offset: 0x00175180
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		if (this.Icon != null)
		{
			result = this.Icon;
		}
		else
		{
			KBatchedAnimController component = Assets.GetPrefab(this.Result).GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x0600430C RID: 17164 RVA: 0x00176FDA File Offset: 0x001751DA
	public Color GetUIColor()
	{
		if (!(this.Icon != null))
		{
			return Color.white;
		}
		return this.IconColor;
	}

	// Token: 0x04002BAA RID: 11178
	private string nameOverride;

	// Token: 0x04002BAB RID: 11179
	public string HotKey;

	// Token: 0x04002BAC RID: 11180
	public string Type;

	// Token: 0x04002BAD RID: 11181
	public List<Recipe.Ingredient> Ingredients;

	// Token: 0x04002BAE RID: 11182
	public string recipeDescription;

	// Token: 0x04002BAF RID: 11183
	public Tag Result;

	// Token: 0x04002BB0 RID: 11184
	public GameObject FabricationVisualizer;

	// Token: 0x04002BB1 RID: 11185
	public SimHashes ResultElementOverride;

	// Token: 0x04002BB2 RID: 11186
	public Sprite Icon;

	// Token: 0x04002BB3 RID: 11187
	public Color IconColor = Color.white;

	// Token: 0x04002BB4 RID: 11188
	public string[] fabricators;

	// Token: 0x04002BB5 RID: 11189
	public float OutputUnits;

	// Token: 0x04002BB6 RID: 11190
	public float FabricationTime;

	// Token: 0x04002BB7 RID: 11191
	public string TechUnlock;

	// Token: 0x0200175A RID: 5978
	[DebuggerDisplay("{tag} {amount}")]
	[Serializable]
	public class Ingredient
	{
		// Token: 0x06008E24 RID: 36388 RVA: 0x0031EB24 File Offset: 0x0031CD24
		public Ingredient(string tag, float amount)
		{
			this.tag = TagManager.Create(tag);
			this.amount = amount;
		}

		// Token: 0x06008E25 RID: 36389 RVA: 0x0031EB3F File Offset: 0x0031CD3F
		public Ingredient(Tag tag, float amount)
		{
			this.tag = tag;
			this.amount = amount;
		}

		// Token: 0x06008E26 RID: 36390 RVA: 0x0031EB58 File Offset: 0x0031CD58
		public List<Element> GetElementOptions()
		{
			List<Element> list = new List<Element>(ElementLoader.elements);
			list.RemoveAll((Element e) => !e.IsSolid);
			list.RemoveAll((Element e) => !e.HasTag(this.tag));
			return list;
		}

		// Token: 0x04006E75 RID: 28277
		public Tag tag;

		// Token: 0x04006E76 RID: 28278
		public float amount;
	}
}
