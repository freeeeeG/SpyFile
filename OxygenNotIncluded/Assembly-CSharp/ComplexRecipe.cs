using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A4D RID: 2637
public class ComplexRecipe
{
	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06004F63 RID: 20323 RVA: 0x001C0A32 File Offset: 0x001BEC32
	// (set) Token: 0x06004F64 RID: 20324 RVA: 0x001C0A3A File Offset: 0x001BEC3A
	public bool ProductHasFacade { get; set; }

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06004F65 RID: 20325 RVA: 0x001C0A43 File Offset: 0x001BEC43
	public Tag FirstResult
	{
		get
		{
			return this.results[0].material;
		}
	}

	// Token: 0x06004F66 RID: 20326 RVA: 0x001C0A52 File Offset: 0x001BEC52
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results)
	{
		this.id = id;
		this.ingredients = ingredients;
		this.results = results;
		ComplexRecipeManager.Get().Add(this);
	}

	// Token: 0x06004F67 RID: 20327 RVA: 0x001C0A85 File Offset: 0x001BEC85
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP) : this(id, ingredients, results)
	{
		this.consumedHEP = consumedHEP;
		this.producedHEP = producedHEP;
	}

	// Token: 0x06004F68 RID: 20328 RVA: 0x001C0AA0 File Offset: 0x001BECA0
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP) : this(id, ingredients, results, consumedHEP, 0)
	{
	}

	// Token: 0x06004F69 RID: 20329 RVA: 0x001C0AB0 File Offset: 0x001BECB0
	public float TotalResultUnits()
	{
		float num = 0f;
		foreach (ComplexRecipe.RecipeElement recipeElement in this.results)
		{
			num += recipeElement.amount;
		}
		return num;
	}

	// Token: 0x06004F6A RID: 20330 RVA: 0x001C0AE6 File Offset: 0x001BECE6
	public bool RequiresTechUnlock()
	{
		return !string.IsNullOrEmpty(this.requiredTech);
	}

	// Token: 0x06004F6B RID: 20331 RVA: 0x001C0AF6 File Offset: 0x001BECF6
	public bool IsRequiredTechUnlocked()
	{
		return string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();
	}

	// Token: 0x06004F6C RID: 20332 RVA: 0x001C0B24 File Offset: 0x001BED24
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		KBatchedAnimController component = Assets.GetPrefab((this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? this.ingredients[0].material : this.results[0].material).GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
		}
		return result;
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x001C0B85 File Offset: 0x001BED85
	public Color GetUIColor()
	{
		return Color.white;
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001C0B8C File Offset: 0x001BED8C
	public string GetUIName(bool includeAmounts)
	{
		string text = this.results[0].facadeID.IsNullOrWhiteSpace() ? this.results[0].material.ProperName() : this.results[0].facadeID.ProperName();
		switch (this.nameDisplay)
		{
		case ComplexRecipe.RecipeNameDisplay.Result:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, text, this.results[0].amount);
			}
			return text;
		case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Composite:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.results[0].amount,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE, this.ingredients[0].material.ProperName(), text, this.results[1].material.ProperName());
		case ComplexRecipe.RecipeNameDisplay.HEP:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.producedHEP,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Custom:
			return this.customName;
		}
		if (includeAmounts)
		{
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, this.ingredients[0].material.ProperName(), this.ingredients[0].amount);
		}
		return this.ingredients[0].material.ProperName();
	}

	// Token: 0x040033F1 RID: 13297
	public string id;

	// Token: 0x040033F2 RID: 13298
	public ComplexRecipe.RecipeElement[] ingredients;

	// Token: 0x040033F3 RID: 13299
	public ComplexRecipe.RecipeElement[] results;

	// Token: 0x040033F4 RID: 13300
	public float time;

	// Token: 0x040033F5 RID: 13301
	public GameObject FabricationVisualizer;

	// Token: 0x040033F6 RID: 13302
	public int consumedHEP;

	// Token: 0x040033F7 RID: 13303
	public int producedHEP;

	// Token: 0x040033F8 RID: 13304
	public string recipeCategoryID = "";

	// Token: 0x040033FA RID: 13306
	public ComplexRecipe.RecipeNameDisplay nameDisplay;

	// Token: 0x040033FB RID: 13307
	public string customName;

	// Token: 0x040033FC RID: 13308
	public string description;

	// Token: 0x040033FD RID: 13309
	public List<Tag> fabricators;

	// Token: 0x040033FE RID: 13310
	public int sortOrder;

	// Token: 0x040033FF RID: 13311
	public string requiredTech;

	// Token: 0x020018DD RID: 6365
	public enum RecipeNameDisplay
	{
		// Token: 0x0400732E RID: 29486
		Ingredient,
		// Token: 0x0400732F RID: 29487
		Result,
		// Token: 0x04007330 RID: 29488
		IngredientToResult,
		// Token: 0x04007331 RID: 29489
		ResultWithIngredient,
		// Token: 0x04007332 RID: 29490
		Composite,
		// Token: 0x04007333 RID: 29491
		HEP,
		// Token: 0x04007334 RID: 29492
		Custom
	}

	// Token: 0x020018DE RID: 6366
	public class RecipeElement
	{
		// Token: 0x06009315 RID: 37653 RVA: 0x0032E1AB File Offset: 0x0032C3AB
		public RecipeElement(Tag material, float amount, bool inheritElement)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
			this.inheritElement = inheritElement;
		}

		// Token: 0x06009316 RID: 37654 RVA: 0x0032E1CF File Offset: 0x0032C3CF
		public RecipeElement(Tag material, float amount)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x06009317 RID: 37655 RVA: 0x0032E1EC File Offset: 0x0032C3EC
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
		}

		// Token: 0x06009318 RID: 37656 RVA: 0x0032E211 File Offset: 0x0032C411
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06009319 RID: 37657 RVA: 0x0032E23E File Offset: 0x0032C43E
		// (set) Token: 0x0600931A RID: 37658 RVA: 0x0032E246 File Offset: 0x0032C446
		public float amount { get; private set; }

		// Token: 0x04007335 RID: 29493
		public Tag material;

		// Token: 0x04007337 RID: 29495
		public ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation;

		// Token: 0x04007338 RID: 29496
		public bool storeElement;

		// Token: 0x04007339 RID: 29497
		public bool inheritElement;

		// Token: 0x0400733A RID: 29498
		public string facadeID;

		// Token: 0x0200220D RID: 8717
		public enum TemperatureOperation
		{
			// Token: 0x04009875 RID: 39029
			AverageTemperature,
			// Token: 0x04009876 RID: 39030
			Heated,
			// Token: 0x04009877 RID: 39031
			Melted
		}
	}
}
