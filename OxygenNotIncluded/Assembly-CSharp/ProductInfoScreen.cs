using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BC8 RID: 3016
public class ProductInfoScreen : KScreen
{
	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06005EA2 RID: 24226 RVA: 0x0022BBC0 File Offset: 0x00229DC0
	public FacadeSelectionPanel FacadeSelectionPanel
	{
		get
		{
			return this.facadeSelectionPanel;
		}
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x0022BBC8 File Offset: 0x00229DC8
	private void RefreshScreen()
	{
		if (this.currentDef != null)
		{
			this.SetTitle(this.currentDef);
			return;
		}
		this.ClearProduct(true);
	}

	// Token: 0x06005EA4 RID: 24228 RVA: 0x0022BBEC File Offset: 0x00229DEC
	public void ClearProduct(bool deactivateTool = true)
	{
		if (this.materialSelectionPanel == null)
		{
			return;
		}
		this.currentDef = null;
		this.materialSelectionPanel.ClearMaterialToggles();
		if (PlayerController.Instance.ActiveTool == BuildTool.Instance && deactivateTool)
		{
			BuildTool.Instance.Deactivate();
		}
		if (PlayerController.Instance.ActiveTool == UtilityBuildTool.Instance || PlayerController.Instance.ActiveTool == WireBuildTool.Instance)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.ClearLabels();
		this.Show(false);
	}

	// Token: 0x06005EA5 RID: 24229 RVA: 0x0022BC80 File Offset: 0x00229E80
	public new void Awake()
	{
		base.Awake();
		this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab.gameObject, base.gameObject, false);
		FacadeSelectionPanel facadeSelectionPanel = this.facadeSelectionPanel;
		facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, new System.Action(this.OnFacadeSelectionChanged));
		this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x06005EA6 RID: 24230 RVA: 0x0022BCF4 File Offset: 0x00229EF4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen instance = BuildingGroupScreen.Instance;
			instance.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildingGroupScreen instance2 = BuildingGroupScreen.Instance;
			instance2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance2.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (PlanScreen.Instance != null)
		{
			PlanScreen instance3 = PlanScreen.Instance;
			instance3.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance3.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			PlanScreen instance4 = PlanScreen.Instance;
			instance4.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance4.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu instance5 = BuildMenu.Instance;
			instance5.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance5.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildMenu instance6 = BuildMenu.Instance;
			instance6.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance6.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		base.ConsumeMouseScroll = true;
		this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		MultiToggle multiToggle = this.sandboxInstantBuildToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SandboxToolParameterMenu.instance.settings.InstantBuild = !SandboxToolParameterMenu.instance.settings.InstantBuild;
			this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		}));
		this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		});
	}

	// Token: 0x06005EA7 RID: 24231 RVA: 0x0022BEDA File Offset: 0x0022A0DA
	public void ConfigureScreen(BuildingDef def)
	{
		this.ConfigureScreen(def, this.FacadeSelectionPanel.SelectedFacade);
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x0022BEF0 File Offset: 0x0022A0F0
	public void ConfigureScreen(BuildingDef def, string facadeID)
	{
		this.configuring = true;
		this.currentDef = def;
		this.SetTitle(def);
		this.SetDescription(def);
		this.SetEffects(def);
		this.facadeSelectionPanel.SetBuildingDef(def.PrefabID);
		BuildingFacadeResource buildingFacadeResource = null;
		if ("DEFAULT_FACADE" != facadeID)
		{
			buildingFacadeResource = Db.GetBuildingFacades().TryGet(facadeID);
		}
		if (buildingFacadeResource != null && buildingFacadeResource.PrefabID == def.PrefabID && buildingFacadeResource.IsUnlocked())
		{
			this.facadeSelectionPanel.SelectedFacade = facadeID;
		}
		else
		{
			this.facadeSelectionPanel.SelectedFacade = "DEFAULT_FACADE";
		}
		this.SetMaterials(def);
		this.configuring = false;
	}

	// Token: 0x06005EA9 RID: 24233 RVA: 0x0022BF96 File Offset: 0x0022A196
	private void ExpandInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(true);
	}

	// Token: 0x06005EAA RID: 24234 RVA: 0x0022BF9F File Offset: 0x0022A19F
	private void CollapseInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(false);
	}

	// Token: 0x06005EAB RID: 24235 RVA: 0x0022BFA8 File Offset: 0x0022A1A8
	public void ToggleExpandedInfo(bool state)
	{
		this.expandedInfo = state;
		if (this.ProductDescriptionPane != null)
		{
			this.ProductDescriptionPane.SetActive(this.expandedInfo);
		}
		if (this.ProductRequirementsPane != null)
		{
			this.ProductRequirementsPane.gameObject.SetActive(this.expandedInfo && this.ProductRequirementsPane.HasDescriptors());
		}
		if (this.ProductEffectsPane != null)
		{
			this.ProductEffectsPane.gameObject.SetActive(this.expandedInfo && this.ProductEffectsPane.HasDescriptors());
		}
		if (this.ProductFlavourPane != null)
		{
			this.ProductFlavourPane.SetActive(this.expandedInfo);
		}
		if (this.materialSelectionPanel != null && this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			this.materialSelectionPanel.ToggleShowDescriptorPanels(this.expandedInfo);
		}
	}

	// Token: 0x06005EAC RID: 24236 RVA: 0x0022C09C File Offset: 0x0022A29C
	private void CheckMouseOver(PointerEventData data)
	{
		bool state = base.GetMouseOver || (PlanScreen.Instance != null && ((PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.GetMouseOver) || BuildingGroupScreen.Instance.GetMouseOver)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.GetMouseOver);
		this.ToggleExpandedInfo(state);
	}

	// Token: 0x06005EAD RID: 24237 RVA: 0x0022C114 File Offset: 0x0022A314
	private void Update()
	{
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && this.currentDef != null && this.materialSelectionPanel.CurrentSelectedElement != null && !MaterialSelector.AllowInsufficientMaterialBuild() && this.currentDef.Mass[0] > ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.materialSelectionPanel.CurrentSelectedElement, true))
		{
			this.materialSelectionPanel.AutoSelectAvailableMaterial();
		}
	}

	// Token: 0x06005EAE RID: 24238 RVA: 0x0022C19C File Offset: 0x0022A39C
	private void SetTitle(BuildingDef def)
	{
		this.titleBar.SetTitle(def.Name);
		bool flag = (PlanScreen.Instance != null && PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.IsDefBuildable(def)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete);
		this.titleBar.GetComponentInChildren<KImage>().ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Disabled);
	}

	// Token: 0x06005EAF RID: 24239 RVA: 0x0022C228 File Offset: 0x0022A428
	private void SetDescription(BuildingDef def)
	{
		if (def == null)
		{
			return;
		}
		if (this.productFlavourText == null)
		{
			return;
		}
		string text = "";
		KPrefabID component = def.BuildingComplete.GetComponent<KPrefabID>();
		string text2 = "";
		foreach (Tag key in component.Tags)
		{
			string str;
			if (CodexEntryGenerator.room_constraint_to_building_label_dict.TryGetValue(key, out str))
			{
				text2 = text2 + "\n    • " + str;
			}
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			text += string.Format("<b>{0}</b>: {1}\n\n", CODEX.HEADERS.BUILDINGTYPE, text2);
		}
		text += def.Desc;
		Dictionary<Klei.AI.Attribute, float> dictionary = new Dictionary<Klei.AI.Attribute, float>();
		Dictionary<Klei.AI.Attribute, float> dictionary2 = new Dictionary<Klei.AI.Attribute, float>();
		foreach (Klei.AI.Attribute key2 in def.attributes)
		{
			if (!dictionary.ContainsKey(key2))
			{
				dictionary[key2] = 0f;
			}
		}
		foreach (AttributeModifier attributeModifier in def.attributeModifiers)
		{
			float num = 0f;
			Klei.AI.Attribute key3 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
			dictionary.TryGetValue(key3, out num);
			num += attributeModifier.Value;
			dictionary[key3] = num;
		}
		if (this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			Element element = ElementLoader.GetElement(this.materialSelectionPanel.CurrentSelectedElement);
			if (element != null)
			{
				using (List<AttributeModifier>.Enumerator enumerator3 = element.attributeModifiers.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						AttributeModifier attributeModifier2 = enumerator3.Current;
						float num2 = 0f;
						Klei.AI.Attribute key4 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId);
						dictionary2.TryGetValue(key4, out num2);
						num2 += attributeModifier2.Value;
						dictionary2[key4] = num2;
					}
					goto IL_2A8;
				}
			}
			PrefabAttributeModifiers component2 = Assets.TryGetPrefab(this.materialSelectionPanel.CurrentSelectedElement).GetComponent<PrefabAttributeModifiers>();
			if (component2 != null)
			{
				foreach (AttributeModifier attributeModifier3 in component2.descriptors)
				{
					float num3 = 0f;
					Klei.AI.Attribute key5 = Db.Get().BuildingAttributes.Get(attributeModifier3.AttributeId);
					dictionary2.TryGetValue(key5, out num3);
					num3 += attributeModifier3.Value;
					dictionary2[key5] = num3;
				}
			}
		}
		IL_2A8:
		if (dictionary.Count > 0)
		{
			text += "\n\n";
			foreach (KeyValuePair<Klei.AI.Attribute, float> keyValuePair in dictionary)
			{
				float num4 = 0f;
				dictionary.TryGetValue(keyValuePair.Key, out num4);
				float num5 = 0f;
				string text3 = "";
				if (dictionary2.TryGetValue(keyValuePair.Key, out num5))
				{
					num5 = Mathf.Abs(num4 * num5);
					text3 = "(+" + num5.ToString() + ")";
				}
				text = string.Concat(new string[]
				{
					text,
					"\n",
					keyValuePair.Key.Name,
					": ",
					(num4 + num5).ToString(),
					text3
				});
			}
		}
		this.productFlavourText.text = text;
	}

	// Token: 0x06005EB0 RID: 24240 RVA: 0x0022C620 File Offset: 0x0022A820
	private void SetEffects(BuildingDef def)
	{
		if (this.productDescriptionText.text != null)
		{
			this.productDescriptionText.text = string.Format("{0}", def.Effect);
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete, false);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONREQUIREMENTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONREQUIREMENTS, Descriptor.DescriptorType.Effect);
			requirementDescriptors.Insert(0, item);
			this.ProductRequirementsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductRequirementsPane.gameObject.SetActive(false);
		}
		this.ProductRequirementsPane.SetDescriptors(requirementDescriptors);
		List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
		if (effectDescriptors.Count > 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONEFFECTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONEFFECTS, Descriptor.DescriptorType.Effect);
			effectDescriptors.Insert(0, item2);
			this.ProductEffectsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductEffectsPane.gameObject.SetActive(false);
		}
		this.ProductEffectsPane.SetDescriptors(effectDescriptors);
	}

	// Token: 0x06005EB1 RID: 24241 RVA: 0x0022C738 File Offset: 0x0022A938
	public void ClearLabels()
	{
		List<string> list = new List<string>(this.descLabels.Keys);
		if (list.Count > 0)
		{
			foreach (string key in list)
			{
				GameObject gameObject = this.descLabels[key];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				this.descLabels.Remove(key);
			}
		}
	}

	// Token: 0x06005EB2 RID: 24242 RVA: 0x0022C7C4 File Offset: 0x0022A9C4
	public void SetMaterials(BuildingDef def)
	{
		this.materialSelectionPanel.gameObject.SetActive(true);
		Recipe craftRecipe = def.CraftRecipe;
		this.materialSelectionPanel.ClearSelectActions();
		this.materialSelectionPanel.ConfigureScreen(craftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(PlanScreen.Instance.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(PlanScreen.Instance.GetTooltipForBuildable));
		this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshScreen));
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.onMenuMaterialChanged));
		this.materialSelectionPanel.AutoSelectAvailableMaterial();
		this.ActivateAppropriateTool(def);
	}

	// Token: 0x06005EB3 RID: 24243 RVA: 0x0022C86D File Offset: 0x0022AA6D
	private void OnFacadeSelectionChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
	}

	// Token: 0x06005EB4 RID: 24244 RVA: 0x0022C88A File Offset: 0x0022AA8A
	private void onMenuMaterialChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
		this.SetDescription(this.currentDef);
	}

	// Token: 0x06005EB5 RID: 24245 RVA: 0x0022C8B4 File Offset: 0x0022AAB4
	private void ActivateAppropriateTool(BuildingDef def)
	{
		global::Debug.Assert(def != null, "def was null");
		if (((PlanScreen.Instance != null) ? PlanScreen.Instance.IsDefBuildable(def) : (BuildMenu.Instance != null && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete)) && this.materialSelectionPanel.AllSelectorsSelected() && this.facadeSelectionPanel.SelectedFacade != null)
		{
			this.onElementsFullySelected.Signal();
			return;
		}
		if (!MaterialSelector.AllowInsufficientMaterialBuild() && !DebugHandler.InstantBuildMode)
		{
			if (PlayerController.Instance.ActiveTool == BuildTool.Instance)
			{
				BuildTool.Instance.Deactivate();
			}
			PrebuildTool.Instance.Activate(def, PlanScreen.Instance.GetTooltipForBuildable(def));
		}
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x0022C978 File Offset: 0x0022AB78
	public static bool MaterialsMet(Recipe recipe)
	{
		if (recipe == null)
		{
			global::Debug.LogError("Trying to verify the materials on a null recipe!");
			return false;
		}
		if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
		{
			global::Debug.LogError("Trying to verify the materials on a recipe with no MaterialCategoryTags!");
			return false;
		}
		bool result = true;
		for (int i = 0; i < recipe.Ingredients.Count; i++)
		{
			if (MaterialSelectionPanel.Filter(recipe.Ingredients[i].tag).kgAvailable < recipe.Ingredients[i].amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x0022CA00 File Offset: 0x0022AC00
	public void Close()
	{
		if (this.configuring)
		{
			return;
		}
		this.ClearProduct(true);
		this.Show(false);
	}

	// Token: 0x04003FE8 RID: 16360
	public TitleBar titleBar;

	// Token: 0x04003FE9 RID: 16361
	public GameObject ProductDescriptionPane;

	// Token: 0x04003FEA RID: 16362
	public LocText productDescriptionText;

	// Token: 0x04003FEB RID: 16363
	public DescriptorPanel ProductRequirementsPane;

	// Token: 0x04003FEC RID: 16364
	public DescriptorPanel ProductEffectsPane;

	// Token: 0x04003FED RID: 16365
	public GameObject ProductFlavourPane;

	// Token: 0x04003FEE RID: 16366
	public LocText productFlavourText;

	// Token: 0x04003FEF RID: 16367
	public RectTransform BGPanel;

	// Token: 0x04003FF0 RID: 16368
	public MaterialSelectionPanel materialSelectionPanelPrefab;

	// Token: 0x04003FF1 RID: 16369
	public FacadeSelectionPanel facadeSelectionPanelPrefab;

	// Token: 0x04003FF2 RID: 16370
	private Dictionary<string, GameObject> descLabels = new Dictionary<string, GameObject>();

	// Token: 0x04003FF3 RID: 16371
	public MultiToggle sandboxInstantBuildToggle;

	// Token: 0x04003FF4 RID: 16372
	[NonSerialized]
	public MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x04003FF5 RID: 16373
	[SerializeField]
	private FacadeSelectionPanel facadeSelectionPanel;

	// Token: 0x04003FF6 RID: 16374
	[NonSerialized]
	public BuildingDef currentDef;

	// Token: 0x04003FF7 RID: 16375
	public System.Action onElementsFullySelected;

	// Token: 0x04003FF8 RID: 16376
	private bool expandedInfo = true;

	// Token: 0x04003FF9 RID: 16377
	private bool configuring;
}
