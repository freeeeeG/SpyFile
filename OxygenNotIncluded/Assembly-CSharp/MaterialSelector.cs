using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B74 RID: 2932
public class MaterialSelector : KScreen
{
	// Token: 0x06005B0B RID: 23307 RVA: 0x002179DC File Offset: 0x00215BDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleGroup = base.GetComponent<ToggleGroup>();
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x002179F0 File Offset: 0x00215BF0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005B0D RID: 23309 RVA: 0x00217A04 File Offset: 0x00215C04
	public void ClearMaterialToggles()
	{
		this.CurrentSelectedElement = null;
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			keyValuePair.Value.gameObject.SetActive(false);
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.ElementToggles.Clear();
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x00217A9C File Offset: 0x00215C9C
	public static List<Tag> GetValidMaterials(Tag materialTypeTag, bool omitDisabledElements = false)
	{
		List<Tag> list = new List<Tag>();
		foreach (Element element in ElementLoader.elements)
		{
			if ((!element.disabled || !omitDisabledElements) && element.IsSolid && (element.tag == materialTypeTag || element.HasTag(materialTypeTag)))
			{
				list.Add(element.tag);
			}
		}
		foreach (Tag tag in GameTags.MaterialBuildingElements)
		{
			if (tag == materialTypeTag)
			{
				foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (component != null && !list.Contains(component.PrefabTag))
					{
						list.Add(component.PrefabTag);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06005B0F RID: 23311 RVA: 0x00217BCC File Offset: 0x00215DCC
	public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
	{
		this.ClearMaterialToggles();
		this.activeIngredient = ingredient;
		this.activeRecipe = recipe;
		this.activeMass = ingredient.amount;
		foreach (Tag tag in MaterialSelector.GetValidMaterials(ingredient.tag, false))
		{
			if (!this.ElementToggles.ContainsKey(tag))
			{
				GameObject gameObject = Util.KInstantiate(this.TogglePrefab, this.LayoutContainer, "MaterialSelection_" + tag.ProperName());
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				KToggle component = gameObject.GetComponent<KToggle>();
				this.ElementToggles.Add(tag, component);
				component.group = this.toggleGroup;
				gameObject.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
			}
		}
		this.ConfigureMaterialTooltips();
		this.RefreshToggleContents();
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x00217CCC File Offset: 0x00215ECC
	private void SetToggleBGImage(KToggle toggle, Tag elem)
	{
		if (toggle == this.selectedToggle)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponent<ImageToggleState>().SetActive();
			return;
		}
		if (ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
			toggle.GetComponent<ImageToggleState>().SetInactive();
			return;
		}
		toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
		toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
		if (!MaterialSelector.AllowInsufficientMaterialBuild())
		{
			toggle.GetComponent<ImageToggleState>().SetDisabled();
		}
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x00217DC0 File Offset: 0x00215FC0
	public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
	{
		KToggle x = this.ElementToggles[elem];
		if (x != this.selectedToggle)
		{
			this.selectedToggle = x;
			if (recipe != null)
			{
				SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, recipe.Result, elem);
			}
			this.CurrentSelectedElement = elem;
			if (this.selectMaterialActions != null)
			{
				this.selectMaterialActions();
			}
			this.UpdateHeader();
			this.SetDescription(elem);
			this.SetEffects(elem);
			if (!this.MaterialDescriptionPane.gameObject.activeSelf && !this.MaterialEffectsPane.gameObject.activeSelf)
			{
				this.DescriptorsPanel.SetActive(false);
			}
			else
			{
				this.DescriptorsPanel.SetActive(true);
			}
		}
		if (focusScrollRect && this.ElementToggles.Count > 1)
		{
			List<Tag> list = new List<Tag>();
			foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
			{
				list.Add(keyValuePair.Key);
			}
			list.Sort(new Comparison<Tag>(this.ElementSorter));
			float num = (float)list.IndexOf(elem);
			int constraintCount = this.LayoutContainer.GetComponent<GridLayoutGroup>().constraintCount;
			float num2 = num / (float)constraintCount / (float)Math.Max((list.Count - 1) / constraintCount, 1);
			this.ScrollRect.normalizedPosition = new Vector2(0f, 1f - num2);
		}
		this.RefreshToggleContents();
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x00217F58 File Offset: 0x00216158
	public void RefreshToggleContents()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			KToggle value = keyValuePair.Value;
			Tag elem = keyValuePair.Key;
			GameObject gameObject = value.gameObject;
			bool flag = DiscoveredResources.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
			if (gameObject.activeSelf != flag)
			{
				gameObject.SetActive(flag);
			}
			if (flag)
			{
				LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
				LocText locText = componentsInChildren[0];
				TMP_Text tmp_Text = componentsInChildren[1];
				Image image = gameObject.GetComponentsInChildren<Image>()[1];
				tmp_Text.text = Util.FormatWholeNumber(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true));
				locText.text = Util.FormatWholeNumber(this.activeMass);
				GameObject gameObject2 = Assets.TryGetPrefab(keyValuePair.Key);
				if (gameObject2 != null)
				{
					KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
					image.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
				}
				this.SetToggleBGImage(keyValuePair.Value, keyValuePair.Key);
				value.soundPlayer.AcceptClickCondition = (() => this.IsEnoughMass(elem));
				value.ClearOnClick();
				if (this.IsEnoughMass(elem))
				{
					value.onClick += delegate()
					{
						this.OnSelectMaterial(elem, this.activeRecipe, false);
					};
				}
			}
		}
		this.SortElementToggles();
		this.UpdateHeader();
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x00218114 File Offset: 0x00216314
	private bool IsEnoughMass(Tag t)
	{
		return ClusterManager.Instance.activeWorld.worldInventory.GetAmount(t, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || MaterialSelector.AllowInsufficientMaterialBuild();
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x00218150 File Offset: 0x00216350
	public bool AutoSelectAvailableMaterial()
	{
		if (this.activeRecipe == null || this.ElementToggles.Count == 0)
		{
			return false;
		}
		Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, this.activeRecipe.Result);
		if (previousElement != null)
		{
			KToggle x;
			this.ElementToggles.TryGetValue(previousElement, out x);
			if (x != null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || ClusterManager.Instance.activeWorld.worldInventory.GetAmount(previousElement, true) >= this.activeMass))
			{
				this.OnSelectMaterial(previousElement, this.activeRecipe, true);
				return true;
			}
		}
		float num = -1f;
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort(new Comparison<Tag>(this.ElementSorter));
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			this.OnSelectMaterial(list[0], this.activeRecipe, true);
			return true;
		}
		Tag tag = null;
		foreach (Tag tag2 in list)
		{
			float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
			if (amount >= this.activeMass && amount > num)
			{
				num = amount;
				tag = tag2;
			}
		}
		if (tag != null)
		{
			this.OnSelectMaterial(tag, this.activeRecipe, true);
			return true;
		}
		return false;
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x00218328 File Offset: 0x00216528
	private void SortElementToggles()
	{
		bool flag = false;
		int num = -1;
		this.elementsToSort.Clear();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.elementsToSort.Add(keyValuePair.Key);
			}
		}
		this.elementsToSort.Sort(new Comparison<Tag>(this.ElementSorter));
		for (int i = 0; i < this.elementsToSort.Count; i++)
		{
			int siblingIndex = this.ElementToggles[this.elementsToSort[i]].transform.GetSiblingIndex();
			if (siblingIndex <= num)
			{
				flag = true;
				break;
			}
			num = siblingIndex;
		}
		if (flag)
		{
			foreach (Tag key in this.elementsToSort)
			{
				this.ElementToggles[key].transform.SetAsLastSibling();
			}
		}
		this.UpdateScrollBar();
	}

	// Token: 0x06005B16 RID: 23318 RVA: 0x00218468 File Offset: 0x00216668
	private void ConfigureMaterialTooltips()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			ToolTip component = keyValuePair.Value.gameObject.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = GameUtil.GetMaterialTooltips(keyValuePair.Key);
			}
		}
	}

	// Token: 0x06005B17 RID: 23319 RVA: 0x002184E4 File Offset: 0x002166E4
	private void UpdateScrollBar()
	{
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		if (this.Scrollbar.activeSelf != num > 5)
		{
			this.Scrollbar.SetActive(num > 5);
		}
		this.ScrollRect.GetComponent<LayoutElement>().minHeight = (float)(74 * ((num <= 5) ? 1 : 2));
	}

	// Token: 0x06005B18 RID: 23320 RVA: 0x00218588 File Offset: 0x00216788
	private void UpdateHeader()
	{
		if (this.activeIngredient == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		LocText componentInChildren = this.Headerbar.GetComponentInChildren<LocText>();
		if (num == 0)
		{
			componentInChildren.text = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, this.activeIngredient.tag.ProperName(), GameUtil.GetFormattedMass(this.activeIngredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string text = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_DESC, this.activeIngredient.tag.ProperName());
			this.NoMaterialDiscovered.text = text;
			this.NoMaterialDiscovered.gameObject.SetActive(true);
			this.NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
			this.BadBG.SetActive(true);
			this.Scrollbar.SetActive(false);
			this.LayoutContainer.SetActive(false);
			return;
		}
		componentInChildren.text = string.Format(UI.PRODUCTINFO_SELECTMATERIAL, this.activeIngredient.tag.ProperName());
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		this.BadBG.SetActive(false);
		this.LayoutContainer.SetActive(true);
		this.UpdateScrollBar();
	}

	// Token: 0x06005B19 RID: 23321 RVA: 0x00218710 File Offset: 0x00216910
	public void ToggleShowDescriptorsPanel(bool show)
	{
		this.DescriptorsPanel.gameObject.SetActive(show);
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x00218724 File Offset: 0x00216924
	private void SetDescription(Tag element)
	{
		StringEntry stringEntry = null;
		if (Strings.TryGet(new StringKey("STRINGS.ELEMENTS." + element.ToString().ToUpper() + ".BUILD_DESC"), out stringEntry))
		{
			this.MaterialDescriptionText.text = stringEntry.ToString();
			this.MaterialDescriptionPane.SetActive(true);
			return;
		}
		this.MaterialDescriptionPane.SetActive(false);
	}

	// Token: 0x06005B1B RID: 23323 RVA: 0x0021878C File Offset: 0x0021698C
	private void SetEffects(Tag element)
	{
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.MaterialEffectsPane.gameObject.SetActive(true);
			this.MaterialEffectsPane.SetDescriptors(materialDescriptors);
			return;
		}
		this.MaterialEffectsPane.gameObject.SetActive(false);
	}

	// Token: 0x06005B1C RID: 23324 RVA: 0x00218804 File Offset: 0x00216A04
	public static bool AllowInsufficientMaterialBuild()
	{
		return GenericGameSettings.instance.allowInsufficientMaterialBuild;
	}

	// Token: 0x06005B1D RID: 23325 RVA: 0x00218810 File Offset: 0x00216A10
	private int ElementSorter(Tag at, Tag bt)
	{
		GameObject gameObject = Assets.TryGetPrefab(at);
		IHasSortOrder hasSortOrder = (gameObject != null) ? gameObject.GetComponent<IHasSortOrder>() : null;
		GameObject gameObject2 = Assets.TryGetPrefab(bt);
		IHasSortOrder hasSortOrder2 = (gameObject2 != null) ? gameObject2.GetComponent<IHasSortOrder>() : null;
		if (hasSortOrder == null || hasSortOrder2 == null)
		{
			return 0;
		}
		Element element = ElementLoader.GetElement(at);
		Element element2 = ElementLoader.GetElement(bt);
		if (element != null && element2 != null && element.buildMenuSort == element2.buildMenuSort)
		{
			return element.idx.CompareTo(element2.idx);
		}
		return hasSortOrder.sortOrder.CompareTo(hasSortOrder2.sortOrder);
	}

	// Token: 0x04003D81 RID: 15745
	public Tag CurrentSelectedElement;

	// Token: 0x04003D82 RID: 15746
	public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();

	// Token: 0x04003D83 RID: 15747
	public int selectorIndex;

	// Token: 0x04003D84 RID: 15748
	public MaterialSelector.SelectMaterialActions selectMaterialActions;

	// Token: 0x04003D85 RID: 15749
	public MaterialSelector.SelectMaterialActions deselectMaterialActions;

	// Token: 0x04003D86 RID: 15750
	private ToggleGroup toggleGroup;

	// Token: 0x04003D87 RID: 15751
	public GameObject TogglePrefab;

	// Token: 0x04003D88 RID: 15752
	public GameObject LayoutContainer;

	// Token: 0x04003D89 RID: 15753
	public KScrollRect ScrollRect;

	// Token: 0x04003D8A RID: 15754
	public GameObject Scrollbar;

	// Token: 0x04003D8B RID: 15755
	public GameObject Headerbar;

	// Token: 0x04003D8C RID: 15756
	public GameObject BadBG;

	// Token: 0x04003D8D RID: 15757
	public LocText NoMaterialDiscovered;

	// Token: 0x04003D8E RID: 15758
	public GameObject MaterialDescriptionPane;

	// Token: 0x04003D8F RID: 15759
	public LocText MaterialDescriptionText;

	// Token: 0x04003D90 RID: 15760
	public DescriptorPanel MaterialEffectsPane;

	// Token: 0x04003D91 RID: 15761
	public GameObject DescriptorsPanel;

	// Token: 0x04003D92 RID: 15762
	private KToggle selectedToggle;

	// Token: 0x04003D93 RID: 15763
	private Recipe.Ingredient activeIngredient;

	// Token: 0x04003D94 RID: 15764
	private Recipe activeRecipe;

	// Token: 0x04003D95 RID: 15765
	private float activeMass;

	// Token: 0x04003D96 RID: 15766
	private List<Tag> elementsToSort = new List<Tag>();

	// Token: 0x02001AAB RID: 6827
	// (Invoke) Token: 0x060097B3 RID: 38835
	public delegate void SelectMaterialActions();
}
