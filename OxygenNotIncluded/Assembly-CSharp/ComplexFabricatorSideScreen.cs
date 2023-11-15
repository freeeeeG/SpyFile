using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0A RID: 3082
public class ComplexFabricatorSideScreen : SideScreenContent
{
	// Token: 0x0600619B RID: 24987 RVA: 0x00240674 File Offset: 0x0023E874
	public override string GetTitle()
	{
		if (this.targetFab == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetFab.GetProperName());
	}

	// Token: 0x0600619C RID: 24988 RVA: 0x002406D0 File Offset: 0x0023E8D0
	public override bool IsValidForTarget(GameObject target)
	{
		ComplexFabricator component = target.GetComponent<ComplexFabricator>();
		return component != null && component.enabled;
	}

	// Token: 0x0600619D RID: 24989 RVA: 0x002406F8 File Offset: 0x0023E8F8
	public override void SetTarget(GameObject target)
	{
		ComplexFabricator component = target.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a ComplexFabricator!");
			return;
		}
		if (this.targetOrdersUpdatedSubHandle != -1)
		{
			base.Unsubscribe(this.targetOrdersUpdatedSubHandle);
		}
		this.Initialize(component);
		this.targetOrdersUpdatedSubHandle = this.targetFab.Subscribe(1721324763, new Action<object>(this.UpdateQueueCountLabels));
		this.UpdateQueueCountLabels(null);
	}

	// Token: 0x0600619E RID: 24990 RVA: 0x00240768 File Offset: 0x0023E968
	private void UpdateQueueCountLabels(object data = null)
	{
		ComplexRecipe[] recipes = this.targetFab.GetRecipes();
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe r = recipes[i];
			GameObject gameObject = this.recipeToggles.Find((GameObject match) => this.recipeMap[match] == r);
			if (gameObject != null)
			{
				this.RefreshQueueCountDisplay(gameObject, this.targetFab);
			}
		}
		if (this.targetFab.CurrentWorkingOrder != null)
		{
			this.currentOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, this.targetFab.CurrentWorkingOrder.GetUIName(false));
		}
		else
		{
			this.currentOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
		}
		if (this.targetFab.NextOrder != null)
		{
			this.nextOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, this.targetFab.NextOrder.GetUIName(false));
			return;
		}
		this.nextOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
	}

	// Token: 0x0600619F RID: 24991 RVA: 0x00240884 File Offset: 0x0023EA84
	protected override void OnShow(bool show)
	{
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.selectedRecipe = null;
			this.selectedToggle = null;
		}
		base.OnShow(show);
	}

	// Token: 0x060061A0 RID: 24992 RVA: 0x002408E0 File Offset: 0x0023EAE0
	public void Initialize(ComplexFabricator target)
	{
		if (target == null)
		{
			global::Debug.LogError("ComplexFabricator provided was null.");
			return;
		}
		this.targetFab = target;
		base.gameObject.SetActive(true);
		this.recipeMap = new Dictionary<GameObject, ComplexRecipe>();
		this.recipeToggles.ForEach(delegate(GameObject rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.recipeToggles.Clear();
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.recipeCategories)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.transform.parent.gameObject);
		}
		this.recipeCategories.Clear();
		int num = 0;
		ComplexRecipe[] recipes = this.targetFab.GetRecipes();
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe recipe = recipes[i];
			bool flag = false;
			if (DebugHandler.InstantBuildMode)
			{
				flag = true;
			}
			else if (recipe.RequiresTechUnlock())
			{
				if (recipe.IsRequiredTechUnlocked())
				{
					flag = true;
				}
			}
			else if (target.GetRecipeQueueCount(recipe) != 0)
			{
				flag = true;
			}
			else if (this.AnyRecipeRequirementsDiscovered(recipe))
			{
				flag = true;
			}
			else if (this.HasAnyRecipeRequirements(recipe))
			{
				flag = true;
			}
			if (flag)
			{
				num++;
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(recipe.ingredients[0].material, "ui", false);
				global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(recipe.results[0].material, recipe.results[0].facadeID);
				KToggle newToggle = null;
				ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = target.sideScreenStyle;
				GameObject entryGO;
				if (sideScreenStyle - ComplexFabricatorSideScreen.StyleSetting.ListInputOutput > 1)
				{
					if (sideScreenStyle != ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
					{
						newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButton, this.recipeGrid, false);
						entryGO = newToggle.gameObject;
						Image componentInChildrenOnly = newToggle.gameObject.GetComponentInChildrenOnly<Image>();
						if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.GridInput || target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListInput)
						{
							componentInChildrenOnly.sprite = uisprite.first;
							componentInChildrenOnly.color = uisprite.second;
						}
						else
						{
							componentInChildrenOnly.sprite = uisprite2.first;
							componentInChildrenOnly.color = uisprite2.second;
						}
					}
					else
					{
						newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButtonQueueHybrid, this.recipeGrid, false);
						entryGO = newToggle.gameObject;
						this.recipeMap.Add(entryGO, recipe);
						if (recipe.recipeCategoryID != "")
						{
							if (!this.recipeCategories.ContainsKey(recipe.recipeCategoryID))
							{
								GameObject gameObject = global::Util.KInstantiateUI(this.recipeCategoryHeader, this.recipeGrid, true);
								gameObject.GetComponentInChildren<LocText>().SetText(Strings.Get("STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_CATEGORIES." + recipe.recipeCategoryID.ToUpper()).String);
								HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
								RectTransform categoryContent = component.GetReference<RectTransform>("content");
								component.GetReference<Image>("icon").sprite = recipe.GetUIIcon();
								categoryContent.gameObject.SetActive(false);
								MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
								MultiToggle toggle2 = toggle;
								toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
								{
									categoryContent.gameObject.SetActive(!categoryContent.gameObject.activeSelf);
									toggle.ChangeState(categoryContent.gameObject.activeSelf ? 1 : 0);
								}));
								this.recipeCategories.Add(recipe.recipeCategoryID, categoryContent.gameObject);
							}
							newToggle.transform.SetParent(this.recipeCategories[recipe.recipeCategoryID].rectTransform());
						}
						Image image = entryGO.GetComponentsInChildrenOnly<Image>()[2];
						if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
						{
							image.sprite = uisprite.first;
							image.color = uisprite.second;
						}
						else if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
						{
							image.sprite = this.radboltSprite;
						}
						else
						{
							image.sprite = uisprite2.first;
							image.color = uisprite2.second;
						}
						entryGO.GetComponentInChildren<LocText>().text = recipe.GetUIName(false);
						bool flag2 = this.HasAllRecipeRequirements(recipe);
						image.material = (flag2 ? Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial);
						this.RefreshQueueCountDisplay(entryGO, this.targetFab);
						entryGO.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("DecrementButton").onClick = delegate()
						{
							target.DecrementRecipeQueueCount(recipe, false);
							this.RefreshQueueCountDisplay(entryGO, target);
						};
						entryGO.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("IncrementButton").onClick = delegate()
						{
							target.IncrementRecipeQueueCount(recipe);
							this.RefreshQueueCountDisplay(entryGO, target);
						};
						entryGO.gameObject.SetActive(true);
					}
				}
				else
				{
					newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButtonMultiple, this.recipeGrid, false);
					entryGO = newToggle.gameObject;
					HierarchyReferences component2 = newToggle.GetComponent<HierarchyReferences>();
					foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
					{
						GameObject gameObject2 = global::Util.KInstantiateUI(component2.GetReference("FromIconPrefab").gameObject, component2.GetReference("FromIcons").gameObject, true);
						gameObject2.GetComponent<Image>().sprite = Def.GetUISprite(recipeElement.material, "ui", false).first;
						gameObject2.GetComponent<Image>().color = Def.GetUISprite(recipeElement.material, "ui", false).second;
						gameObject2.gameObject.name = recipeElement.material.Name;
					}
					foreach (ComplexRecipe.RecipeElement recipeElement2 in recipe.results)
					{
						GameObject gameObject3 = global::Util.KInstantiateUI(component2.GetReference("ToIconPrefab").gameObject, component2.GetReference("ToIcons").gameObject, true);
						gameObject3.GetComponent<Image>().sprite = Def.GetUISprite(recipeElement2.material, "ui", false).first;
						gameObject3.GetComponent<Image>().color = Def.GetUISprite(recipeElement2.material, "ui", false).second;
						gameObject3.gameObject.name = recipeElement2.material.Name;
					}
				}
				if (this.targetFab.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ClassicFabricator)
				{
					newToggle.GetComponentInChildren<LocText>().text = recipe.results[0].material.ProperName();
				}
				else if (this.targetFab.sideScreenStyle != ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
				{
					newToggle.GetComponentInChildren<LocText>().text = string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_WITH_NEWLINES, recipe.ingredients[0].material.ProperName(), recipe.results[0].material.ProperName());
				}
				ToolTip component3 = entryGO.GetComponent<ToolTip>();
				component3.toolTipPosition = ToolTip.TooltipPosition.Custom;
				component3.parentPositionAnchor = new Vector2(0f, 0.5f);
				component3.tooltipPivot = new Vector2(1f, 1f);
				component3.tooltipPositionOffset = new Vector2(-24f, 20f);
				component3.ClearMultiStringTooltip();
				component3.AddMultiStringTooltip(recipe.GetUIName(false), this.styleTooltipHeader);
				component3.AddMultiStringTooltip(recipe.description, this.styleTooltipBody);
				newToggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				entryGO.SetActive(true);
				this.recipeToggles.Add(entryGO);
			}
		}
		if (this.recipeToggles.Count > 0)
		{
			VerticalLayoutGroup component4 = this.buttonContentContainer.GetComponent<VerticalLayoutGroup>();
			this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = Mathf.Min(451f, (float)(component4.padding.top + component4.padding.bottom) + (float)num * this.recipeButtonQueueHybrid.GetComponent<LayoutElement>().minHeight + (float)(num - 1) * component4.spacing);
			this.subtitleLabel.SetText(this.targetFab.SideScreenSubtitleLabel);
			this.noRecipesDiscoveredLabel.gameObject.SetActive(false);
		}
		else
		{
			this.subtitleLabel.SetText(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED);
			this.noRecipesDiscoveredLabel.SetText(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED_BODY);
			this.noRecipesDiscoveredLabel.gameObject.SetActive(true);
			this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = this.noRecipesDiscoveredLabel.GetComponent<LayoutElement>().minHeight + 10f;
		}
		this.RefreshIngredientAvailabilityVis();
	}

	// Token: 0x060061A1 RID: 24993 RVA: 0x002412AC File Offset: 0x0023F4AC
	public void RefreshQueueCountDisplayForRecipe(ComplexRecipe recipe, ComplexFabricator fabricator)
	{
		GameObject gameObject = this.recipeToggles.Find((GameObject match) => this.recipeMap[match] == recipe);
		if (gameObject != null)
		{
			this.RefreshQueueCountDisplay(gameObject, fabricator);
		}
	}

	// Token: 0x060061A2 RID: 24994 RVA: 0x002412F8 File Offset: 0x0023F4F8
	private void RefreshQueueCountDisplay(GameObject entryGO, ComplexFabricator fabricator)
	{
		HierarchyReferences component = entryGO.GetComponent<HierarchyReferences>();
		bool flag = fabricator.GetRecipeQueueCount(this.recipeMap[entryGO]) == ComplexFabricator.QUEUE_INFINITE;
		component.GetReference<LocText>("CountLabel").text = (flag ? "" : fabricator.GetRecipeQueueCount(this.recipeMap[entryGO]).ToString());
		component.GetReference<RectTransform>("InfiniteIcon").gameObject.SetActive(flag);
	}

	// Token: 0x060061A3 RID: 24995 RVA: 0x00241370 File Offset: 0x0023F570
	private void ToggleClicked(KToggle toggle)
	{
		if (!this.recipeMap.ContainsKey(toggle.gameObject))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedToggle == toggle)
		{
			this.selectedToggle.isOn = false;
			this.selectedToggle = null;
			this.selectedRecipe = null;
		}
		else
		{
			this.selectedToggle = toggle;
			this.selectedToggle.isOn = true;
			this.selectedRecipe = this.recipeMap[toggle.gameObject];
			this.selectedRecipeFabricatorMap[this.targetFab] = this.recipeToggles.IndexOf(toggle.gameObject);
		}
		this.RefreshIngredientAvailabilityVis();
		if (toggle.isOn)
		{
			this.recipeScreen = (SelectedRecipeQueueScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.recipeScreenPrefab, this.targetFab.SideScreenRecipeScreenTitle);
			this.recipeScreen.SetRecipe(this, this.targetFab, this.selectedRecipe);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x060061A4 RID: 24996 RVA: 0x00241468 File Offset: 0x0023F668
	public void CycleRecipe(int increment)
	{
		int num = 0;
		if (this.selectedToggle != null)
		{
			num = this.recipeToggles.IndexOf(this.selectedToggle.gameObject);
		}
		int num2 = (num + increment) % this.recipeToggles.Count;
		if (num2 < 0)
		{
			num2 = this.recipeToggles.Count + num2;
		}
		this.ToggleClicked(this.recipeToggles[num2].GetComponent<KToggle>());
	}

	// Token: 0x060061A5 RID: 24997 RVA: 0x002414D8 File Offset: 0x0023F6D8
	private bool HasAnyRecipeRequirements(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(recipeElement.material, true, this.targetFab.ForbiddenTags) + this.targetFab.inStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) + this.targetFab.buildStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) >= recipeElement.amount)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060061A6 RID: 24998 RVA: 0x00241578 File Offset: 0x0023F778
	private bool HasAllRecipeRequirements(ComplexRecipe recipe)
	{
		bool result = true;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(recipeElement.material, true, this.targetFab.ForbiddenTags) + this.targetFab.inStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) + this.targetFab.buildStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) < recipeElement.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060061A7 RID: 24999 RVA: 0x0024161C File Offset: 0x0023F81C
	private bool AnyRecipeRequirementsDiscovered(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (DiscoveredResources.Instance.IsDiscovered(recipeElement.material))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060061A8 RID: 25000 RVA: 0x00241657 File Offset: 0x0023F857
	private void Update()
	{
		this.RefreshIngredientAvailabilityVis();
	}

	// Token: 0x060061A9 RID: 25001 RVA: 0x00241660 File Offset: 0x0023F860
	private void RefreshIngredientAvailabilityVis()
	{
		foreach (KeyValuePair<GameObject, ComplexRecipe> keyValuePair in this.recipeMap)
		{
			HierarchyReferences component = keyValuePair.Key.GetComponent<HierarchyReferences>();
			bool flag = this.HasAllRecipeRequirements(keyValuePair.Value);
			KToggle component2 = keyValuePair.Key.GetComponent<KToggle>();
			if (flag)
			{
				if (this.selectedRecipe == keyValuePair.Value)
				{
					component2.ActivateFlourish(true, ImageToggleState.State.Active);
				}
				else
				{
					component2.ActivateFlourish(false, ImageToggleState.State.Inactive);
				}
			}
			else if (this.selectedRecipe == keyValuePair.Value)
			{
				component2.ActivateFlourish(true, ImageToggleState.State.DisabledActive);
			}
			else
			{
				component2.ActivateFlourish(false, ImageToggleState.State.Disabled);
			}
			component.GetReference<LocText>("Label").color = (flag ? Color.black : new Color(0.22f, 0.22f, 0.22f, 1f));
		}
	}

	// Token: 0x060061AA RID: 25002 RVA: 0x00241754 File Offset: 0x0023F954
	private Element[] GetRecipeElements(Recipe recipe)
	{
		Element[] array = new Element[recipe.Ingredients.Count];
		for (int i = 0; i < recipe.Ingredients.Count; i++)
		{
			Tag tag = recipe.Ingredients[i].tag;
			foreach (Element element in ElementLoader.elements)
			{
				if (GameTagExtensions.Create(element.id) == tag)
				{
					array[i] = element;
					break;
				}
			}
		}
		return array;
	}

	// Token: 0x04004274 RID: 17012
	[Header("Recipe List")]
	[SerializeField]
	private GameObject recipeGrid;

	// Token: 0x04004275 RID: 17013
	[Header("Recipe button variants")]
	[SerializeField]
	private GameObject recipeButton;

	// Token: 0x04004276 RID: 17014
	[SerializeField]
	private GameObject recipeButtonMultiple;

	// Token: 0x04004277 RID: 17015
	[SerializeField]
	private GameObject recipeButtonQueueHybrid;

	// Token: 0x04004278 RID: 17016
	[SerializeField]
	private GameObject recipeCategoryHeader;

	// Token: 0x04004279 RID: 17017
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x0400427A RID: 17018
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x0400427B RID: 17019
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x0400427C RID: 17020
	[SerializeField]
	public Sprite radboltSprite;

	// Token: 0x0400427D RID: 17021
	private KToggle selectedToggle;

	// Token: 0x0400427E RID: 17022
	public LayoutElement buttonScrollContainer;

	// Token: 0x0400427F RID: 17023
	public RectTransform buttonContentContainer;

	// Token: 0x04004280 RID: 17024
	[SerializeField]
	private GameObject elementContainer;

	// Token: 0x04004281 RID: 17025
	[SerializeField]
	private LocText currentOrderLabel;

	// Token: 0x04004282 RID: 17026
	[SerializeField]
	private LocText nextOrderLabel;

	// Token: 0x04004283 RID: 17027
	private Dictionary<ComplexFabricator, int> selectedRecipeFabricatorMap = new Dictionary<ComplexFabricator, int>();

	// Token: 0x04004284 RID: 17028
	public EventReference createOrderSound;

	// Token: 0x04004285 RID: 17029
	[SerializeField]
	private RectTransform content;

	// Token: 0x04004286 RID: 17030
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x04004287 RID: 17031
	[SerializeField]
	private LocText noRecipesDiscoveredLabel;

	// Token: 0x04004288 RID: 17032
	public TextStyleSetting styleTooltipHeader;

	// Token: 0x04004289 RID: 17033
	public TextStyleSetting styleTooltipBody;

	// Token: 0x0400428A RID: 17034
	private ComplexFabricator targetFab;

	// Token: 0x0400428B RID: 17035
	private ComplexRecipe selectedRecipe;

	// Token: 0x0400428C RID: 17036
	private Dictionary<GameObject, ComplexRecipe> recipeMap;

	// Token: 0x0400428D RID: 17037
	private Dictionary<string, GameObject> recipeCategories = new Dictionary<string, GameObject>();

	// Token: 0x0400428E RID: 17038
	private List<GameObject> recipeToggles = new List<GameObject>();

	// Token: 0x0400428F RID: 17039
	public SelectedRecipeQueueScreen recipeScreenPrefab;

	// Token: 0x04004290 RID: 17040
	private SelectedRecipeQueueScreen recipeScreen;

	// Token: 0x04004291 RID: 17041
	private int targetOrdersUpdatedSubHandle = -1;

	// Token: 0x02001B5B RID: 7003
	public enum StyleSetting
	{
		// Token: 0x04007C8D RID: 31885
		GridResult,
		// Token: 0x04007C8E RID: 31886
		ListResult,
		// Token: 0x04007C8F RID: 31887
		GridInput,
		// Token: 0x04007C90 RID: 31888
		ListInput,
		// Token: 0x04007C91 RID: 31889
		ListInputOutput,
		// Token: 0x04007C92 RID: 31890
		GridInputOutput,
		// Token: 0x04007C93 RID: 31891
		ClassicFabricator,
		// Token: 0x04007C94 RID: 31892
		ListQueueHybrid
	}
}
