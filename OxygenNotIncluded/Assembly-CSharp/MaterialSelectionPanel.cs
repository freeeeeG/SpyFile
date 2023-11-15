using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B73 RID: 2931
public class MaterialSelectionPanel : KScreen
{
	// Token: 0x06005AF5 RID: 23285 RVA: 0x00217167 File Offset: 0x00215367
	public static void ClearStatics()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06005AF6 RID: 23286 RVA: 0x00217173 File Offset: 0x00215373
	public Tag CurrentSelectedElement
	{
		get
		{
			return this.MaterialSelectors[0].CurrentSelectedElement;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06005AF7 RID: 23287 RVA: 0x00217188 File Offset: 0x00215388
	public IList<Tag> GetSelectedElementAsList
	{
		get
		{
			this.currentSelectedElements.Clear();
			foreach (MaterialSelector materialSelector in this.MaterialSelectors)
			{
				if (materialSelector.gameObject.activeSelf)
				{
					global::Debug.Assert(materialSelector.CurrentSelectedElement != null);
					this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
				}
			}
			return this.currentSelectedElements;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06005AF8 RID: 23288 RVA: 0x0021721C File Offset: 0x0021541C
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x06005AF9 RID: 23289 RVA: 0x00217224 File Offset: 0x00215424
	protected override void OnPrefabInit()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		for (int i = 0; i < 3; i++)
		{
			MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, base.gameObject, false);
			materialSelector.selectorIndex = i;
			this.MaterialSelectors.Add(materialSelector);
		}
		this.MaterialSelectors[0].gameObject.SetActive(true);
		this.MaterialSelectorTemplate.SetActive(false);
		this.ResearchRequired.SetActive(false);
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
		this.priorityScreenParent.transform.SetAsLastSibling();
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, delegate(object d)
		{
			this.RefreshSelectors();
		}));
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x00217314 File Offset: 0x00215514
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateOnSpawn = true;
	}

	// Token: 0x06005AFB RID: 23291 RVA: 0x00217324 File Offset: 0x00215524
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x0021738C File Offset: 0x0021558C
	public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
	{
		this.MaterialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions)Delegate.Combine(selector.selectMaterialActions, action);
		});
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x002173BD File Offset: 0x002155BD
	public void ClearSelectActions()
	{
		this.MaterialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = null;
		});
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x002173E9 File Offset: 0x002155E9
	public void ClearMaterialToggles()
	{
		this.MaterialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.ClearMaterialToggles();
		});
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x00217415 File Offset: 0x00215615
	public void ConfigureScreen(Recipe recipe, MaterialSelectionPanel.GetBuildableStateDelegate buildableStateCB, MaterialSelectionPanel.GetBuildableTooltipDelegate buildableTooltipCB)
	{
		this.activeRecipe = recipe;
		this.GetBuildableState = buildableStateCB;
		this.GetBuildableTooltip = buildableTooltipCB;
		this.RefreshSelectors();
	}

	// Token: 0x06005B00 RID: 23296 RVA: 0x00217434 File Offset: 0x00215634
	public bool AllSelectorsSelected()
	{
		bool flag = false;
		foreach (MaterialSelector materialSelector in this.MaterialSelectors)
		{
			flag = (flag || materialSelector.gameObject.activeInHierarchy);
			if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == null)
			{
				return false;
			}
		}
		return flag;
	}

	// Token: 0x06005B01 RID: 23297 RVA: 0x002174BC File Offset: 0x002156BC
	public void RefreshSelectors()
	{
		if (this.activeRecipe == null)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.MaterialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.gameObject.SetActive(false);
		});
		BuildingDef buildingDef = this.activeRecipe.GetBuildingDef();
		bool flag = this.GetBuildableState(buildingDef);
		string text = this.GetBuildableTooltip(buildingDef);
		if (!flag)
		{
			this.ResearchRequired.SetActive(true);
			LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
			componentsInChildren[0].text = "";
			componentsInChildren[1].text = text;
			componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
			this.priorityScreen.gameObject.SetActive(false);
			this.buildToolRotateButton.gameObject.SetActive(false);
			return;
		}
		this.ResearchRequired.SetActive(false);
		for (int i = 0; i < this.activeRecipe.Ingredients.Count; i++)
		{
			this.MaterialSelectors[i].gameObject.SetActive(true);
			this.MaterialSelectors[i].ConfigureScreen(this.activeRecipe.Ingredients[i], this.activeRecipe);
		}
		this.priorityScreen.gameObject.SetActive(true);
		this.priorityScreen.transform.SetAsLastSibling();
		this.buildToolRotateButton.gameObject.SetActive(true);
		this.buildToolRotateButton.transform.SetAsLastSibling();
	}

	// Token: 0x06005B02 RID: 23298 RVA: 0x00217635 File Offset: 0x00215835
	public void UpdateResourceToggleValues()
	{
		this.MaterialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			if (selector.gameObject.activeSelf)
			{
				selector.RefreshToggleContents();
			}
		});
	}

	// Token: 0x06005B03 RID: 23299 RVA: 0x00217664 File Offset: 0x00215864
	public bool AutoSelectAvailableMaterial()
	{
		bool result = true;
		for (int i = 0; i < this.MaterialSelectors.Count; i++)
		{
			if (!this.MaterialSelectors[i].AutoSelectAvailableMaterial())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06005B04 RID: 23300 RVA: 0x002176A0 File Offset: 0x002158A0
	public void SelectSourcesMaterials(Building building)
	{
		Tag[] array = null;
		Deconstructable component = building.gameObject.GetComponent<Deconstructable>();
		if (component != null)
		{
			array = component.constructionElements;
		}
		Constructable component2 = building.GetComponent<Constructable>();
		if (component2 != null)
		{
			array = component2.SelectedElementsTags.ToArray<Tag>();
		}
		if (array != null)
		{
			for (int i = 0; i < Mathf.Min(array.Length, this.MaterialSelectors.Count); i++)
			{
				if (this.MaterialSelectors[i].ElementToggles.ContainsKey(array[i]))
				{
					this.MaterialSelectors[i].OnSelectMaterial(array[i], this.activeRecipe, false);
				}
			}
		}
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x00217748 File Offset: 0x00215948
	public static MaterialSelectionPanel.SelectedElemInfo Filter(Tag materialCategoryTag)
	{
		MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = default(MaterialSelectionPanel.SelectedElemInfo);
		selectedElemInfo.element = null;
		selectedElemInfo.kgAvailable = 0f;
		if (DiscoveredResources.Instance == null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
		{
			return selectedElemInfo;
		}
		List<Tag> list = null;
		if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(materialCategoryTag, out list))
		{
			list = new List<Tag>();
			foreach (Element element in ElementLoader.elements)
			{
				if (element.tag == materialCategoryTag || element.HasTag(materialCategoryTag))
				{
					list.Add(element.tag);
				}
			}
			foreach (Tag tag in GameTags.MaterialBuildingElements)
			{
				if (tag == materialCategoryTag)
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
			MaterialSelectionPanel.elementsWithTag[materialCategoryTag] = list;
		}
		foreach (Tag tag2 in list)
		{
			float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
			if (amount > selectedElemInfo.kgAvailable)
			{
				selectedElemInfo.kgAvailable = amount;
				selectedElemInfo.element = tag2;
			}
		}
		return selectedElemInfo;
	}

	// Token: 0x06005B06 RID: 23302 RVA: 0x0021793C File Offset: 0x00215B3C
	public void ToggleShowDescriptorPanels(bool show)
	{
		for (int i = 0; i < this.MaterialSelectors.Count; i++)
		{
			if (this.MaterialSelectors[i] != null)
			{
				this.MaterialSelectors[i].ToggleShowDescriptorsPanel(show);
			}
		}
	}

	// Token: 0x06005B07 RID: 23303 RVA: 0x00217985 File Offset: 0x00215B85
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x04003D73 RID: 15731
	public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();

	// Token: 0x04003D74 RID: 15732
	private List<MaterialSelector> MaterialSelectors = new List<MaterialSelector>();

	// Token: 0x04003D75 RID: 15733
	private List<Tag> currentSelectedElements = new List<Tag>();

	// Token: 0x04003D76 RID: 15734
	[SerializeField]
	protected PriorityScreen priorityScreenPrefab;

	// Token: 0x04003D77 RID: 15735
	[SerializeField]
	protected GameObject priorityScreenParent;

	// Token: 0x04003D78 RID: 15736
	[SerializeField]
	protected BuildToolRotateButtonUI buildToolRotateButton;

	// Token: 0x04003D79 RID: 15737
	private PriorityScreen priorityScreen;

	// Token: 0x04003D7A RID: 15738
	public GameObject MaterialSelectorTemplate;

	// Token: 0x04003D7B RID: 15739
	public GameObject ResearchRequired;

	// Token: 0x04003D7C RID: 15740
	private Recipe activeRecipe;

	// Token: 0x04003D7D RID: 15741
	private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();

	// Token: 0x04003D7E RID: 15742
	private MaterialSelectionPanel.GetBuildableStateDelegate GetBuildableState;

	// Token: 0x04003D7F RID: 15743
	private MaterialSelectionPanel.GetBuildableTooltipDelegate GetBuildableTooltip;

	// Token: 0x04003D80 RID: 15744
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x02001AA5 RID: 6821
	// (Invoke) Token: 0x0600979F RID: 38815
	public delegate bool GetBuildableStateDelegate(BuildingDef def);

	// Token: 0x02001AA6 RID: 6822
	// (Invoke) Token: 0x060097A3 RID: 38819
	public delegate string GetBuildableTooltipDelegate(BuildingDef def);

	// Token: 0x02001AA7 RID: 6823
	// (Invoke) Token: 0x060097A7 RID: 38823
	public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

	// Token: 0x02001AA8 RID: 6824
	public struct SelectedElemInfo
	{
		// Token: 0x04007A18 RID: 31256
		public Tag element;

		// Token: 0x04007A19 RID: 31257
		public float kgAvailable;
	}
}
