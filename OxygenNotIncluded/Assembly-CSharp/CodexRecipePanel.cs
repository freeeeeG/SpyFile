using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AD2 RID: 2770
public class CodexRecipePanel : CodexWidget<CodexRecipePanel>
{
	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06005542 RID: 21826 RVA: 0x001EFF74 File Offset: 0x001EE174
	// (set) Token: 0x06005543 RID: 21827 RVA: 0x001EFF7C File Offset: 0x001EE17C
	public string linkID { get; set; }

	// Token: 0x06005544 RID: 21828 RVA: 0x001EFF85 File Offset: 0x001EE185
	public CodexRecipePanel()
	{
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x001EFF8D File Offset: 0x001EE18D
	public CodexRecipePanel(ComplexRecipe recipe, bool shouldUseFabricatorForTitle = false)
	{
		this.complexRecipe = recipe;
		this.useFabricatorForTitle = shouldUseFabricatorForTitle;
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x001EFFA3 File Offset: 0x001EE1A3
	public CodexRecipePanel(Recipe rec)
	{
		this.recipe = rec;
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x001EFFB4 File Offset: 0x001EE1B4
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.title = component.GetReference<LocText>("Title");
		this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
		this.fabricatorPrefab = component.GetReference<RectTransform>("FabricatorPrefab").gameObject;
		this.ingredientsContainer = component.GetReference<RectTransform>("IngredientsContainer").gameObject;
		this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
		this.fabricatorContainer = component.GetReference<RectTransform>("FabricatorContainer").gameObject;
		this.ClearPanel();
		if (this.recipe != null)
		{
			this.ConfigureRecipe();
			return;
		}
		if (this.complexRecipe != null)
		{
			this.ConfigureComplexRecipe();
		}
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x001F006C File Offset: 0x001EE26C
	private void ConfigureRecipe()
	{
		this.title.text = this.recipe.Result.ProperName();
		foreach (Recipe.Ingredient ingredient in this.recipe.Ingredients)
		{
			GameObject gameObject = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(ingredient.tag, "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ingredient.tag, ingredient.amount, GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			string text = ingredient.tag.ProperName();
			GameObject prefab = Assets.GetPrefab(ingredient.tag);
			if (prefab.GetComponent<Edible>() != null)
			{
				text = text + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
			}
			gameObject.GetComponent<ToolTip>().toolTip = text;
		}
		GameObject gameObject2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true);
		HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(this.recipe.Result, "ui", false);
		component2.GetReference<Image>("Icon").sprite = uisprite2.first;
		component2.GetReference<Image>("Icon").color = uisprite2.second;
		component2.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(this.recipe.Result, this.recipe.OutputUnits, GameUtil.TimeSlice.None);
		component2.GetReference<LocText>("Amount").color = Color.black;
		string text2 = this.recipe.Result.ProperName();
		GameObject prefab2 = Assets.GetPrefab(this.recipe.Result);
		if (prefab2.GetComponent<Edible>() != null)
		{
			text2 = text2 + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab2.GetComponent<Edible>().GetQuality()));
		}
		gameObject2.GetComponent<ToolTip>().toolTip = text2;
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x001F02F0 File Offset: 0x001EE4F0
	private void ConfigureComplexRecipe()
	{
		ComplexRecipe.RecipeElement[] array = this.complexRecipe.ingredients;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement ing = array[i];
			HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true).GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(ing.material, "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ing.material, ing.amount, GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			string text = ing.material.ProperName();
			GameObject prefab = Assets.GetPrefab(ing.material);
			if (prefab.GetComponent<Edible>() != null)
			{
				text = text + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()));
			}
			component.GetReference<ToolTip>("Tooltip").toolTip = text;
			component.GetReference<KButton>("Button").onClick += delegate()
			{
				ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(Assets.GetPrefab(ing.material).GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
		}
		array = this.complexRecipe.results;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement res = array[i];
			HierarchyReferences component2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(res.material, "ui", false);
			component2.GetReference<Image>("Icon").sprite = uisprite2.first;
			component2.GetReference<Image>("Icon").color = uisprite2.second;
			component2.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(res.material, res.amount, GameUtil.TimeSlice.None);
			component2.GetReference<LocText>("Amount").color = Color.black;
			string text2 = res.material.ProperName();
			GameObject prefab2 = Assets.GetPrefab(res.material);
			if (prefab2.GetComponent<Edible>() != null)
			{
				text2 = text2 + "\n    • " + string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(prefab2.GetComponent<Edible>().GetQuality()));
			}
			component2.GetReference<ToolTip>("Tooltip").toolTip = text2;
			component2.GetReference<KButton>("Button").onClick += delegate()
			{
				ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(Assets.GetPrefab(res.material).GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
		}
		string text3 = this.complexRecipe.id.Substring(0, this.complexRecipe.id.IndexOf('_'));
		HierarchyReferences component3 = Util.KInstantiateUI(this.fabricatorPrefab, this.fabricatorContainer, true).GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite3 = Def.GetUISprite(text3, "ui", false);
		component3.GetReference<Image>("Icon").sprite = uisprite3.first;
		component3.GetReference<Image>("Icon").color = uisprite3.second;
		component3.GetReference<LocText>("Time").text = GameUtil.GetFormattedTime(this.complexRecipe.time, "F0");
		component3.GetReference<LocText>("Time").color = Color.black;
		GameObject fabricator = Assets.GetPrefab(text3.ToTag());
		component3.GetReference<ToolTip>("Tooltip").toolTip = fabricator.GetProperName();
		component3.GetReference<KButton>("Button").onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(fabricator.GetProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
		if (this.useFabricatorForTitle)
		{
			this.title.text = fabricator.GetProperName();
			return;
		}
		this.title.text = this.complexRecipe.results[0].material.ProperName();
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x001F0714 File Offset: 0x001EE914
	private void ClearPanel()
	{
		foreach (object obj in this.ingredientsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (object obj2 in this.resultsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
		}
		foreach (object obj3 in this.fabricatorContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj3).gameObject);
		}
	}

	// Token: 0x040038D8 RID: 14552
	private LocText title;

	// Token: 0x040038D9 RID: 14553
	private GameObject materialPrefab;

	// Token: 0x040038DA RID: 14554
	private GameObject fabricatorPrefab;

	// Token: 0x040038DB RID: 14555
	private GameObject ingredientsContainer;

	// Token: 0x040038DC RID: 14556
	private GameObject resultsContainer;

	// Token: 0x040038DD RID: 14557
	private GameObject fabricatorContainer;

	// Token: 0x040038DE RID: 14558
	private ComplexRecipe complexRecipe;

	// Token: 0x040038DF RID: 14559
	private Recipe recipe;

	// Token: 0x040038E0 RID: 14560
	private bool useFabricatorForTitle;
}
