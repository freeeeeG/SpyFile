using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C46 RID: 3142
public class SelectedRecipeQueueScreen : KScreen
{
	// Token: 0x06006398 RID: 25496 RVA: 0x0024D550 File Offset: 0x0024B750
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DecrementButton.onClick = delegate()
		{
			this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.IncrementButton.onClick = delegate()
		{
			this.target.IncrementRecipeQueueCount(this.selectedRecipe);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.InfiniteButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER;
		this.InfiniteButton.onClick += delegate()
		{
			if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
			}
			else
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
			}
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onEndEdit += delegate()
		{
			base.isEditing = false;
			this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onStartEdit += delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
		};
		MultiToggle multiToggle = this.previousRecipeButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.CyclePreviousRecipe));
		MultiToggle multiToggle2 = this.nextRecipeButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.CycleNextRecipe));
	}

	// Token: 0x06006399 RID: 25497 RVA: 0x0024D640 File Offset: 0x0024B840
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedRecipe != null)
		{
			GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
			Equippable equippable = (prefab != null) ? prefab.GetComponent<Equippable>() : null;
			if (equippable != null && equippable.GetBuildOverride() != null)
			{
				this.minionWidget.RemoveEquipment(equippable);
			}
		}
	}

	// Token: 0x0600639A RID: 25498 RVA: 0x0024D6AC File Offset: 0x0024B8AC
	public void SetRecipe(ComplexFabricatorSideScreen owner, ComplexFabricator target, ComplexRecipe recipe)
	{
		this.ownerScreen = owner;
		this.target = target;
		this.selectedRecipe = recipe;
		this.recipeName.text = recipe.GetUIName(false);
		global::Tuple<Sprite, Color> tuple = (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? Def.GetUISprite(recipe.ingredients[0].material, "ui", false) : Def.GetUISprite(recipe.results[0].material, recipe.results[0].facadeID);
		if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
		{
			this.recipeIcon.sprite = owner.radboltSprite;
		}
		else
		{
			this.recipeIcon.sprite = tuple.first;
			this.recipeIcon.color = tuple.second;
		}
		string text = (recipe.time.ToString() + " " + UI.UNITSUFFIXES.SECONDS).ToLower();
		this.recipeMainDescription.SetText(recipe.description);
		this.recipeDuration.SetText(text);
		string simpleTooltip = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPE_WORKTIME, text);
		this.recipeDurationTooltip.SetSimpleTooltip(simpleTooltip);
		this.RefreshIngredientDescriptors();
		this.RefreshResultDescriptors();
		this.RefreshQueueCountDisplay();
		this.ToggleAndRefreshMinionDisplay();
	}

	// Token: 0x0600639B RID: 25499 RVA: 0x0024D7DC File Offset: 0x0024B9DC
	private void CyclePreviousRecipe()
	{
		this.ownerScreen.CycleRecipe(-1);
	}

	// Token: 0x0600639C RID: 25500 RVA: 0x0024D7EA File Offset: 0x0024B9EA
	private void CycleNextRecipe()
	{
		this.ownerScreen.CycleRecipe(1);
	}

	// Token: 0x0600639D RID: 25501 RVA: 0x0024D7F8 File Offset: 0x0024B9F8
	private void ToggleAndRefreshMinionDisplay()
	{
		this.minionWidget.gameObject.SetActive(this.RefreshMinionDisplayAnim());
	}

	// Token: 0x0600639E RID: 25502 RVA: 0x0024D810 File Offset: 0x0024BA10
	private bool RefreshMinionDisplayAnim()
	{
		GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
		if (prefab == null)
		{
			return false;
		}
		Equippable component = prefab.GetComponent<Equippable>();
		if (component == null)
		{
			return false;
		}
		KAnimFile buildOverride = component.GetBuildOverride();
		if (buildOverride == null)
		{
			return false;
		}
		this.minionWidget.SetDefaultPortraitAnimator();
		KAnimFile animFile = buildOverride;
		if (!this.selectedRecipe.results[0].facadeID.IsNullOrWhiteSpace())
		{
			EquippableFacadeResource equippableFacadeResource = Db.GetEquippableFacades().TryGet(this.selectedRecipe.results[0].facadeID);
			if (equippableFacadeResource != null)
			{
				animFile = Assets.GetAnim(equippableFacadeResource.BuildOverride);
			}
		}
		this.minionWidget.UpdateEquipment(component, animFile);
		return true;
	}

	// Token: 0x0600639F RID: 25503 RVA: 0x0024D8CC File Offset: 0x0024BACC
	private void RefreshQueueCountDisplay()
	{
		bool flag = this.target.GetRecipeQueueCount(this.selectedRecipe) == ComplexFabricator.QUEUE_INFINITE;
		if (!flag)
		{
			this.QueueCount.SetAmount((float)this.target.GetRecipeQueueCount(this.selectedRecipe));
		}
		else
		{
			this.QueueCount.SetDisplayValue("");
		}
		this.InfiniteIcon.gameObject.SetActive(flag);
	}

	// Token: 0x060063A0 RID: 25504 RVA: 0x0024D938 File Offset: 0x0024BB38
	private void RefreshResultDescriptors()
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		list.AddRange(this.GetResultDescriptions(this.selectedRecipe));
		foreach (Descriptor desc in this.target.AdditionalEffectsForRecipe(this.selectedRecipe))
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
		}
		if (list.Count > 0)
		{
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeEffectsDescriptorRows)
			{
				Util.KDestroyGameObject(keyValuePair.Value);
			}
			this.recipeEffectsDescriptorRows.Clear();
			foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list)
			{
				GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.EffectsDescriptorPanel.gameObject, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
				component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
				component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
				component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
				component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
				this.recipeEffectsDescriptorRows.Add(descriptorWithSprite, gameObject);
			}
		}
	}

	// Token: 0x060063A1 RID: 25505 RVA: 0x0024DB38 File Offset: 0x0024BD38
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetResultDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		if (recipe.producedHEP > 0)
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.producedHEP), string.Format("<b>{0}</b>: {1}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.producedHEP), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.results)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, recipeElement.facadeID), false));
			Element element = ElementLoader.GetElement(recipeElement.material);
			if (element != null)
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list2 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc in GameUtil.GetMaterialDescriptors(element))
				{
					list2.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list2)
				{
					descriptorWithSprite.descriptor.IncreaseIndent();
				}
				list.AddRange(list2);
			}
			else
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list3 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc2 in GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab, false)))
				{
					list3.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc2, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite2 in list3)
				{
					descriptorWithSprite2.descriptor.IncreaseIndent();
				}
				list.AddRange(list3);
			}
		}
		return list;
	}

	// Token: 0x060063A2 RID: 25506 RVA: 0x0024DE08 File Offset: 0x0024C008
	private void RefreshIngredientDescriptors()
	{
		new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> ingredientDescriptions = this.GetIngredientDescriptions(this.selectedRecipe);
		this.IngredientsDescriptorPanel.gameObject.SetActive(true);
		foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeIngredientDescriptorRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.recipeIngredientDescriptorRows.Clear();
		foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in ingredientDescriptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.IngredientsDescriptorPanel.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
			component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
			component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
			component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
			component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
			this.recipeIngredientDescriptorRows.Add(descriptorWithSprite, gameObject);
		}
	}

	// Token: 0x060063A3 RID: 25507 RVA: 0x0024DFA0 File Offset: 0x0024C1A0
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetIngredientDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			float amount = this.target.GetMyWorld().worldInventory.GetAmount(recipeElement.material, true);
			string formattedByTag2 = GameUtil.GetFormattedByTag(recipeElement.material, amount, GameUtil.TimeSlice.None);
			string text = (amount >= recipeElement.amount) ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) : ("<color=#F44A47>" + string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) + "</color>");
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(text, text, Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, "ui", false), Assets.GetPrefab(recipeElement.material).GetComponent<MutantPlant>() != null));
		}
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = this.target.GetComponent<HighEnergyParticleStorage>();
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1} / {2}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.consumedHEP, component.Particles), string.Format("<b>{0}</b>: {1} / {2}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.consumedHEP, component.Particles), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		return list;
	}

	// Token: 0x040043FB RID: 17403
	public Image recipeIcon;

	// Token: 0x040043FC RID: 17404
	public LocText recipeName;

	// Token: 0x040043FD RID: 17405
	public LocText recipeMainDescription;

	// Token: 0x040043FE RID: 17406
	public LocText recipeDuration;

	// Token: 0x040043FF RID: 17407
	public ToolTip recipeDurationTooltip;

	// Token: 0x04004400 RID: 17408
	public GameObject IngredientsDescriptorPanel;

	// Token: 0x04004401 RID: 17409
	public GameObject EffectsDescriptorPanel;

	// Token: 0x04004402 RID: 17410
	public KNumberInputField QueueCount;

	// Token: 0x04004403 RID: 17411
	public MultiToggle DecrementButton;

	// Token: 0x04004404 RID: 17412
	public MultiToggle IncrementButton;

	// Token: 0x04004405 RID: 17413
	public KButton InfiniteButton;

	// Token: 0x04004406 RID: 17414
	public GameObject InfiniteIcon;

	// Token: 0x04004407 RID: 17415
	private ComplexFabricator target;

	// Token: 0x04004408 RID: 17416
	private ComplexFabricatorSideScreen ownerScreen;

	// Token: 0x04004409 RID: 17417
	private ComplexRecipe selectedRecipe;

	// Token: 0x0400440A RID: 17418
	[SerializeField]
	private GameObject recipeElementDescriptorPrefab;

	// Token: 0x0400440B RID: 17419
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeIngredientDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x0400440C RID: 17420
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeEffectsDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x0400440D RID: 17421
	[SerializeField]
	private FullBodyUIMinionWidget minionWidget;

	// Token: 0x0400440E RID: 17422
	[SerializeField]
	private MultiToggle previousRecipeButton;

	// Token: 0x0400440F RID: 17423
	[SerializeField]
	private MultiToggle nextRecipeButton;

	// Token: 0x02001B88 RID: 7048
	private class DescriptorWithSprite
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06009A31 RID: 39473 RVA: 0x00346367 File Offset: 0x00344567
		public Descriptor descriptor { get; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06009A32 RID: 39474 RVA: 0x0034636F File Offset: 0x0034456F
		public global::Tuple<Sprite, Color> tintedSprite { get; }

		// Token: 0x06009A33 RID: 39475 RVA: 0x00346377 File Offset: 0x00344577
		public DescriptorWithSprite(Descriptor desc, global::Tuple<Sprite, Color> sprite, bool filterRowVisible = false)
		{
			this.descriptor = desc;
			this.tintedSprite = sprite;
			this.showFilterRow = filterRowVisible;
		}

		// Token: 0x04007D05 RID: 32005
		public bool showFilterRow;
	}
}
