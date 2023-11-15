using System;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BC0 RID: 3008
public class PlanScreen : KIconToggleMenu
{
	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06005E2E RID: 24110 RVA: 0x00228248 File Offset: 0x00226448
	// (set) Token: 0x06005E2F RID: 24111 RVA: 0x0022824F File Offset: 0x0022644F
	public static PlanScreen Instance { get; private set; }

	// Token: 0x06005E30 RID: 24112 RVA: 0x00228257 File Offset: 0x00226457
	public static void DestroyInstance()
	{
		PlanScreen.Instance = null;
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06005E31 RID: 24113 RVA: 0x0022825F File Offset: 0x0022645F
	public static Dictionary<HashedString, string> IconNameMap
	{
		get
		{
			return PlanScreen.iconNameMap;
		}
	}

	// Token: 0x06005E32 RID: 24114 RVA: 0x00228266 File Offset: 0x00226466
	private static HashedString CacheHashedString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06005E33 RID: 24115 RVA: 0x00228273 File Offset: 0x00226473
	// (set) Token: 0x06005E34 RID: 24116 RVA: 0x0022827B File Offset: 0x0022647B
	public ProductInfoScreen ProductInfoScreen { get; private set; }

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06005E35 RID: 24117 RVA: 0x00228284 File Offset: 0x00226484
	public KIconToggleMenu.ToggleInfo ActiveCategoryToggleInfo
	{
		get
		{
			return this.activeCategoryInfo;
		}
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06005E36 RID: 24118 RVA: 0x0022828C File Offset: 0x0022648C
	// (set) Token: 0x06005E37 RID: 24119 RVA: 0x00228294 File Offset: 0x00226494
	public GameObject SelectedBuildingGameObject { get; private set; }

	// Token: 0x06005E38 RID: 24120 RVA: 0x0022829D File Offset: 0x0022649D
	public override float GetSortKey()
	{
		return 2f;
	}

	// Token: 0x06005E39 RID: 24121 RVA: 0x002282A4 File Offset: 0x002264A4
	public PlanScreen.RequirementsState GetBuildableState(BuildingDef def)
	{
		if (def == null)
		{
			return PlanScreen.RequirementsState.Materials;
		}
		return this._buildableStatesByID[def.PrefabID];
	}

	// Token: 0x06005E3A RID: 24122 RVA: 0x002282C4 File Offset: 0x002264C4
	private bool IsDefResearched(BuildingDef def)
	{
		bool result = false;
		if (!this._researchedDefs.TryGetValue(def, out result))
		{
			result = this.UpdateDefResearched(def);
		}
		return result;
	}

	// Token: 0x06005E3B RID: 24123 RVA: 0x002282EC File Offset: 0x002264EC
	private bool UpdateDefResearched(BuildingDef def)
	{
		return this._researchedDefs[def] = Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
	}

	// Token: 0x06005E3C RID: 24124 RVA: 0x00228320 File Offset: 0x00226520
	protected override void OnPrefabInit()
	{
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.OnPrefabInit();
			PlanScreen.Instance = this;
			this.ProductInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent, false);
			this.ProductInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.ProductInfoScreen.rectTransform().SetLocalPosition(new Vector3(326f, 0f, 0f));
			this.ProductInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			KInputManager.InputChange.AddListener(new UnityAction(this.RefreshToolTip));
			this.planScreenScrollRect = base.transform.parent.GetComponentInParent<KScrollRect>();
			Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
			Game.Instance.Subscribe(1174281782, new Action<object>(this.OnActiveToolChanged));
			Game.Instance.Subscribe(1557339983, new Action<object>(this.ForceUpdateAllCategoryToggles));
		}
		this.buildingGroupsRoot.gameObject.SetActive(false);
	}

	// Token: 0x06005E3D RID: 24125 RVA: 0x00228458 File Offset: 0x00226658
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.useSubCategoryLayout = (KPlayerPrefs.GetInt("usePlanScreenListView") == 1);
		this.initTime = KTime.Instance.UnscaledGameTime;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			this._buildableStatesByID.Add(buildingDef.PrefabID, PlanScreen.RequirementsState.Materials);
		}
		if (BuildMenu.UseHotkeyBuildMenu())
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.onSelect += this.OnClickCategory;
			this.Refresh();
			foreach (KToggle ktoggle in this.toggles)
			{
				ktoggle.group = base.GetComponent<ToggleGroup>();
			}
			this.RefreshBuildableStates(true);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
		}
		this.copyBuildingButton.GetComponent<MultiToggle>().onClick = delegate()
		{
			this.OnClickCopyBuilding();
		};
		this.RefreshCopyBuildingButton(null);
		Game.Instance.Subscribe(-1503271301, new Action<object>(this.RefreshCopyBuildingButton));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.CloseRecipe(false);
		});
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.PointerEnter));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.PointerExit));
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
		this.RefreshScale(null);
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.BuildButtonList();
		this.gridViewButton.onClick += this.OnClickGridView;
		this.listViewButton.onClick += this.OnClickListView;
	}

	// Token: 0x06005E3E RID: 24126 RVA: 0x0022869C File Offset: 0x0022689C
	private void RefreshScale(object data = null)
	{
		base.GetComponent<GridLayoutGroup>().cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(54f, 50f) : new Vector2(45f, 45f));
		this.toggles.ForEach(delegate(KToggle to)
		{
			to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
		});
		LayoutElement component = this.copyBuildingButton.GetComponent<LayoutElement>();
		component.minWidth = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		component.minHeight = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? 58 : 54);
		base.gameObject.rectTransform().anchoredPosition = new Vector2(0f, (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? -68 : -74));
		this.adjacentPinnedButtons.GetComponent<HorizontalLayoutGroup>().padding.bottom = (ScreenResolutionMonitor.UsingGamepadUIMode() ? 14 : 6);
		Vector2 sizeDelta = this.buildingGroupsRoot.rectTransform().sizeDelta;
		Vector2 vector = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(320f, sizeDelta.y) : new Vector2(264f, sizeDelta.y);
		this.buildingGroupsRoot.rectTransform().sizeDelta = vector;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (this.useSubCategoryLayout)
			{
				componentInChildren.constraintCount = 1;
				componentInChildren.cellSize = new Vector2(vector.x - 24f, 36f);
			}
			else
			{
				componentInChildren.constraintCount = 3;
				componentInChildren.cellSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize : PlanScreen.standarduildingButtonSize);
			}
		}
		this.ProductInfoScreen.rectTransform().anchoredPosition = new Vector2(vector.x + 8f, this.ProductInfoScreen.rectTransform().anchoredPosition.y);
	}

	// Token: 0x06005E3F RID: 24127 RVA: 0x002288A4 File Offset: 0x00226AA4
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x06005E40 RID: 24128 RVA: 0x002288C2 File Offset: 0x00226AC2
	protected override void OnCleanUp()
	{
		if (Game.Instance != null)
		{
			Game.Instance.Unsubscribe(this.refreshScaleHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06005E41 RID: 24129 RVA: 0x002288E8 File Offset: 0x00226AE8
	private void OnClickCopyBuilding()
	{
		if (!this.LastSelectedBuilding.IsNullOrDestroyed() && this.LastSelectedBuilding.gameObject.activeInHierarchy)
		{
			PlanScreen.Instance.CopyBuildingOrder(this.LastSelectedBuilding);
			return;
		}
		if (this.lastSelectedBuildingDef != null)
		{
			PlanScreen.Instance.CopyBuildingOrder(this.lastSelectedBuildingDef, this.LastSelectedBuildingFacade);
		}
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x00228949 File Offset: 0x00226B49
	private void OnClickListView()
	{
		this.useSubCategoryLayout = true;
		this.ForceRefreshAllBuildingToggles();
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 1);
	}

	// Token: 0x06005E43 RID: 24131 RVA: 0x00228977 File Offset: 0x00226B77
	private void OnClickGridView()
	{
		this.useSubCategoryLayout = false;
		this.ForceRefreshAllBuildingToggles();
		this.BuildButtonList();
		this.ConfigurePanelSize(null);
		this.RefreshScale(null);
		KPlayerPrefs.SetInt("usePlanScreenListView", 0);
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06005E44 RID: 24132 RVA: 0x002289A5 File Offset: 0x00226BA5
	// (set) Token: 0x06005E45 RID: 24133 RVA: 0x002289B0 File Offset: 0x00226BB0
	private Building LastSelectedBuilding
	{
		get
		{
			return this.lastSelectedBuilding;
		}
		set
		{
			this.lastSelectedBuilding = value;
			if (this.lastSelectedBuilding != null)
			{
				this.lastSelectedBuildingDef = this.lastSelectedBuilding.Def;
				if (this.lastSelectedBuilding.gameObject.activeInHierarchy)
				{
					this.LastSelectedBuildingFacade = this.lastSelectedBuilding.GetComponent<BuildingFacade>().CurrentFacade;
				}
			}
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06005E46 RID: 24134 RVA: 0x00228A0B File Offset: 0x00226C0B
	// (set) Token: 0x06005E47 RID: 24135 RVA: 0x00228A13 File Offset: 0x00226C13
	public string LastSelectedBuildingFacade
	{
		get
		{
			return this.lastSelectedBuildingFacade;
		}
		set
		{
			this.lastSelectedBuildingFacade = value;
		}
	}

	// Token: 0x06005E48 RID: 24136 RVA: 0x00228A1C File Offset: 0x00226C1C
	public void RefreshCopyBuildingButton(object data = null)
	{
		this.adjacentPinnedButtons.rectTransform().anchoredPosition = new Vector2(Mathf.Min(base.gameObject.rectTransform().sizeDelta.x, base.transform.parent.rectTransform().rect.width), 0f);
		MultiToggle component = this.copyBuildingButton.GetComponent<MultiToggle>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
			if (component2 != null && component2.Def.ShouldShowInBuildMenu() && component2.Def.IsAvailable())
			{
				this.LastSelectedBuilding = component2;
			}
		}
		if (this.lastSelectedBuildingDef != null)
		{
			component.gameObject.SetActive(PlanScreen.Instance.gameObject.activeInHierarchy);
			Sprite sprite = this.lastSelectedBuildingDef.GetUISprite("ui", false);
			if (this.LastSelectedBuildingFacade != null && this.LastSelectedBuildingFacade != "DEFAULT_FACADE")
			{
				sprite = Def.GetFacadeUISprite(this.LastSelectedBuildingFacade);
			}
			component.transform.Find("FG").GetComponent<Image>().sprite = sprite;
			component.transform.Find("FG").GetComponent<Image>().color = Color.white;
			component.ChangeState(1);
			return;
		}
		component.gameObject.SetActive(false);
		component.ChangeState(0);
	}

	// Token: 0x06005E49 RID: 24137 RVA: 0x00228B98 File Offset: 0x00226D98
	public void RefreshToolTip()
	{
		for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
		{
			PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
			if (DlcManager.IsContentActive(planInfo.RequiredDlcId))
			{
				global::Action action = (i < 14) ? (global::Action.Plan1 + i) : global::Action.NumActions;
				string str = HashCache.Get().Get(planInfo.category).ToUpper();
				this.toggleInfo[i].tooltip = GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action);
			}
		}
		this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.COPY_BUILDING_TOOLTIP, global::Action.CopyBuilding));
	}

	// Token: 0x06005E4A RID: 24138 RVA: 0x00228C50 File Offset: 0x00226E50
	public void Refresh()
	{
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (this.tagCategoryMap == null)
		{
			int num = 0;
			this.tagCategoryMap = new Dictionary<Tag, HashedString>();
			this.tagOrderMap = new Dictionary<Tag, int>();
			if (TUNING.BUILDINGS.PLANORDER.Count > 15)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Insufficient keys to cover root plan menu",
					"Max of 14 keys supported but TUNING.BUILDINGS.PLANORDER has " + TUNING.BUILDINGS.PLANORDER.Count.ToString()
				});
			}
			this.toggleEntries.Clear();
			for (int i = 0; i < TUNING.BUILDINGS.PLANORDER.Count; i++)
			{
				PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER[i];
				if (DlcManager.IsContentActive(planInfo.RequiredDlcId))
				{
					global::Action action = (i < 15) ? (global::Action.Plan1 + i) : global::Action.NumActions;
					string icon = PlanScreen.iconNameMap[planInfo.category];
					string str = HashCache.Get().Get(planInfo.category).ToUpper();
					KIconToggleMenu.ToggleInfo toggleInfo = new KIconToggleMenu.ToggleInfo(UI.StripLinkFormatting(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".NAME")), icon, planInfo.category, action, GameUtil.ReplaceHotkeyString(Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str + ".TOOLTIP"), action), "");
					list.Add(toggleInfo);
					PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, this.tagCategoryMap, this.tagOrderMap, ref num);
					List<BuildingDef> list2 = new List<BuildingDef>();
					foreach (BuildingDef buildingDef in Assets.BuildingDefs)
					{
						HashedString x;
						if (buildingDef.IsAvailable() && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out x) && !(x != planInfo.category))
						{
							list2.Add(buildingDef);
						}
					}
					this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggleInfo, planInfo.category, list2, planInfo.hideIfNotResearched));
				}
			}
			base.Setup(list);
			this.toggleBouncers.Clear();
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
				to.GetComponentInChildren<LocText>().fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
				this.toggleBouncers.Add(to, to.GetComponent<Bouncer>());
			});
			for (int j = 0; j < this.toggleEntries.Count; j++)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[j];
				toggleEntry.CollectToggleImages();
				this.toggleEntries[j] = toggleEntry;
			}
			this.ForceUpdateAllCategoryToggles(null);
		}
	}

	// Token: 0x06005E4B RID: 24139 RVA: 0x00228EE0 File Offset: 0x002270E0
	private void ForceUpdateAllCategoryToggles(object data = null)
	{
		this.forceUpdateAllCategoryToggles = true;
	}

	// Token: 0x06005E4C RID: 24140 RVA: 0x00228EE9 File Offset: 0x002270E9
	public void ForceRefreshAllBuildingToggles()
	{
		this.forceRefreshAllBuildings = true;
	}

	// Token: 0x06005E4D RID: 24141 RVA: 0x00228EF4 File Offset: 0x002270F4
	public void CopyBuildingOrder(BuildingDef buildingDef, string facadeID)
	{
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				if (buildingDef.PrefabID == keyValuePair.Key)
				{
					this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
					this.OnSelectBuilding(this.activeCategoryBuildingToggles[buildingDef].gameObject, buildingDef, facadeID);
					this.ProductInfoScreen.ToggleExpandedInfo(true);
					break;
				}
			}
		}
	}

	// Token: 0x06005E4E RID: 24142 RVA: 0x00228FD4 File Offset: 0x002271D4
	public void CopyBuildingOrder(Building building)
	{
		this.CopyBuildingOrder(building.Def, building.GetComponent<BuildingFacade>().CurrentFacade);
		if (this.ProductInfoScreen.materialSelectionPanel == null)
		{
			DebugUtil.DevLogError(building.Def.name + " def likely needs to be marked def.ShowInBuildMenu = false");
			return;
		}
		this.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
		Rotatable component = building.GetComponent<Rotatable>();
		if (component != null)
		{
			BuildTool.Instance.SetToolOrientation(component.GetOrientation());
		}
	}

	// Token: 0x06005E4F RID: 24143 RVA: 0x00229058 File Offset: 0x00227258
	private static void PopulateOrderInfo(HashedString category, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index)
	{
		if (data.GetType() == typeof(PlanScreen.PlanInfo))
		{
			PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo)data;
			PlanScreen.PopulateOrderInfo(planInfo.category, planInfo.buildingAndSubcategoryData, category_map, order_map, ref building_index);
			return;
		}
		foreach (KeyValuePair<string, string> keyValuePair in ((List<KeyValuePair<string, string>>)data))
		{
			Tag key = new Tag(keyValuePair.Key);
			category_map[key] = category;
			order_map[key] = building_index;
			building_index++;
		}
	}

	// Token: 0x06005E50 RID: 24144 RVA: 0x00229100 File Offset: 0x00227300
	protected override void OnCmpEnable()
	{
		this.Refresh();
		this.RefreshCopyBuildingButton(null);
	}

	// Token: 0x06005E51 RID: 24145 RVA: 0x0022910F File Offset: 0x0022730F
	protected override void OnCmpDisable()
	{
		this.ClearButtons();
	}

	// Token: 0x06005E52 RID: 24146 RVA: 0x00229118 File Offset: 0x00227318
	private void ClearButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
		}
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair2 in this.allBuildingToggles)
		{
			keyValuePair2.Value.gameObject.SetActive(false);
		}
		this.activeCategoryBuildingToggles.Clear();
		this.copyBuildingButton.gameObject.SetActive(false);
		this.copyBuildingButton.GetComponent<MultiToggle>().ChangeState(0);
	}

	// Token: 0x06005E53 RID: 24147 RVA: 0x002291E0 File Offset: 0x002273E0
	public void OnSelectBuilding(GameObject button_go, BuildingDef def, string facadeID = null)
	{
		if (button_go == null)
		{
			global::Debug.Log("Button gameObject is null", base.gameObject);
			return;
		}
		if (button_go == this.SelectedBuildingGameObject)
		{
			this.CloseRecipe(true);
			return;
		}
		this.ignoreToolChangeMessages++;
		PlanBuildingToggle planBuildingToggle = null;
		if (this.currentlySelectedToggle != null)
		{
			planBuildingToggle = this.currentlySelectedToggle.GetComponent<PlanBuildingToggle>();
		}
		this.SelectedBuildingGameObject = button_go;
		this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		HashedString category = this.tagCategoryMap[def.Tag];
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
		{
			toggleEntry.pendingResearchAttentions.Remove(def.Tag);
			button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			if (toggleEntry.pendingResearchAttentions.Count == 0)
			{
				toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
			}
		}
		this.ProductInfoScreen.ClearProduct(false);
		if (planBuildingToggle != null)
		{
			planBuildingToggle.Refresh();
		}
		ToolMenu.Instance.ClearSelection();
		PrebuildTool.Instance.Activate(def, this.GetTooltipForBuildable(def));
		this.LastSelectedBuilding = def.BuildingComplete.GetComponent<Building>();
		this.RefreshCopyBuildingButton(null);
		this.ProductInfoScreen.Show(true);
		this.ProductInfoScreen.ConfigureScreen(def, facadeID);
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x06005E54 RID: 24148 RVA: 0x00229354 File Offset: 0x00227554
	private void RefreshBuildableStates(bool force_update)
	{
		if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
		{
			return;
		}
		if (this.timeSinceNotificationPing < this.specialNotificationEmbellishDelay)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= this.notificationPingExpire)
		{
			this.notificationPingCount = 0;
		}
		int num = 10;
		if (force_update)
		{
			num = Assets.BuildingDefs.Count;
			this.buildable_state_update_idx = 0;
		}
		ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
		for (int i = 0; i < num; i++)
		{
			this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
			BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
			PlanScreen.RequirementsState buildableStateForDef = this.GetBuildableStateForDef(buildingDef);
			HashedString hashedString;
			if (this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && this._buildableStatesByID[buildingDef.PrefabID] != buildableStateForDef)
			{
				this._buildableStatesByID[buildingDef.PrefabID] = buildableStateForDef;
				if (this.ProductInfoScreen.currentDef == buildingDef)
				{
					this.ignoreToolChangeMessages++;
					this.ProductInfoScreen.ClearProduct(false);
					this.ProductInfoScreen.Show(true);
					this.ProductInfoScreen.ConfigureScreen(buildingDef);
					this.ignoreToolChangeMessages--;
				}
				if (buildableStateForDef == PlanScreen.RequirementsState.Complete)
				{
					foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
					{
						if ((HashedString)toggleInfo.userData == hashedString)
						{
							Bouncer bouncer = this.toggleBouncers[toggleInfo.toggle];
							if (bouncer != null && !bouncer.IsBouncing() && !pooledList.Contains(hashedString))
							{
								pooledList.Add(hashedString);
								bouncer.Bounce();
								if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
								{
									if (this.timeSinceNotificationPing >= this.specialNotificationEmbellishDelay)
									{
										string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
										if (sound != null)
										{
											SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
										}
									}
									string sound2 = GlobalAssets.GetSound("NewBuildable", false);
									if (sound2 != null)
									{
										EventInstance instance = SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
										instance.setParameterByName("playCount", (float)this.notificationPingCount, false);
										SoundEvent.EndOneShot(instance);
									}
								}
								this.timeSinceNotificationPing = 0f;
								this.notificationPingCount++;
							}
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06005E55 RID: 24149 RVA: 0x00229624 File Offset: 0x00227824
	private PlanScreen.RequirementsState GetBuildableStateForDef(BuildingDef def)
	{
		if (!def.IsAvailable())
		{
			return PlanScreen.RequirementsState.Invalid;
		}
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !this.IsDefResearched(def))
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (def.BuildingComplete.HasTag(GameTags.Telepad) && ClusterUtil.ActiveWorldHasPrinter())
		{
			result = PlanScreen.RequirementsState.TelepadBuilt;
		}
		else if (def.BuildingComplete.HasTag(GameTags.RocketInteriorBuilding) && !ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorOnly;
		}
		else if (def.BuildingComplete.HasTag(GameTags.NotRocketInteriorBuilding) && ClusterUtil.ActiveWorldIsRocketInterior())
		{
			result = PlanScreen.RequirementsState.RocketInteriorForbidden;
		}
		else if (def.BuildingComplete.HasTag(GameTags.UniquePerWorld) && BuildingInventory.Instance.BuildingCountForWorld_BAD_PERF(def.Tag, ClusterManager.Instance.activeWorldId) > 0)
		{
			result = PlanScreen.RequirementsState.UniquePerWorld;
		}
		else if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !ProductInfoScreen.MaterialsMet(def.CraftRecipe))
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		return result;
	}

	// Token: 0x06005E56 RID: 24150 RVA: 0x00229710 File Offset: 0x00227910
	private void SetCategoryButtonState()
	{
		this.nextCategoryToUpdateIDX = (this.nextCategoryToUpdateIDX + 1) % this.toggleEntries.Count;
		for (int i = 0; i < this.toggleEntries.Count; i++)
		{
			if (this.forceUpdateAllCategoryToggles || i == this.nextCategoryToUpdateIDX)
			{
				PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[i];
				KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
				toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
				bool flag = false;
				bool flag2 = true;
				if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
				{
					flag = true;
					flag2 = false;
				}
				else
				{
					foreach (BuildingDef def in toggleEntry.buildingDefs)
					{
						if (this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete)
						{
							flag = true;
							flag2 = false;
							break;
						}
					}
					if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
					{
						flag2 = false;
					}
				}
				this.CategoryInteractive[toggleInfo] = !flag2;
				GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
				if (!flag)
				{
					if (flag2 && toggleEntry.hideIfNotResearched)
					{
						toggleInfo.toggle.gameObject.SetActive(false);
					}
					else if (flag2)
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(true);
					}
					else
					{
						toggleInfo.toggle.gameObject.SetActive(true);
						gameObject.gameObject.SetActive(false);
					}
					ImageToggleState.State state = (this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state);
					}
				}
				else
				{
					toggleInfo.toggle.gameObject.SetActive(true);
					gameObject.gameObject.SetActive(false);
					ImageToggleState.State state2 = (this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
					ImageToggleState[] toggleImages = toggleEntry.toggleImages;
					for (int j = 0; j < toggleImages.Length; j++)
					{
						toggleImages[j].SetState(state2);
					}
				}
			}
		}
		this.RefreshCopyBuildingButton(null);
		this.forceUpdateAllCategoryToggles = false;
	}

	// Token: 0x06005E57 RID: 24151 RVA: 0x0022997C File Offset: 0x00227B7C
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || type == typeof(PrebuildTool))
			{
				activeTool.DeactivateTool(null);
				PlayerController.Instance.ActivateTool(SelectTool.Instance);
			}
		}
	}

	// Token: 0x06005E58 RID: 24152 RVA: 0x002299F0 File Offset: 0x00227BF0
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
		this.SelectedBuildingGameObject = null;
	}

	// Token: 0x06005E59 RID: 24153 RVA: 0x00229A78 File Offset: 0x00227C78
	public void SoftCloseRecipe()
	{
		this.ignoreToolChangeMessages++;
		if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.DeactivateBuildTools();
		if (this.ProductInfoScreen != null)
		{
			this.ProductInfoScreen.ClearProduct(true);
		}
		this.currentlySelectedToggle = null;
		this.SelectedBuildingGameObject = null;
		this.ignoreToolChangeMessages--;
	}

	// Token: 0x06005E5A RID: 24154 RVA: 0x00229AFC File Offset: 0x00227CFC
	public void CloseCategoryPanel(bool playSound = true)
	{
		this.activeCategoryInfo = null;
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
		{
			this.ClearButtons();
			this.buildingGroupsRoot.gameObject.SetActive(false);
			this.ForceUpdateAllCategoryToggles(null);
		});
		this.PlanCategoryLabel.text = "";
		this.ForceUpdateAllCategoryToggles(null);
	}

	// Token: 0x06005E5B RID: 24155 RVA: 0x00229B58 File Offset: 0x00227D58
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.CloseRecipe(false);
		if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
		{
			this.CloseCategoryPanel(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		if (this.activeCategoryInfo == toggle_info)
		{
			this.CloseCategoryPanel(true);
		}
		else
		{
			this.OpenCategoryPanel(toggle_info, true);
		}
		this.ConfigurePanelSize(null);
		this.SetScrollPoint(0f);
	}

	// Token: 0x06005E5C RID: 24156 RVA: 0x00229BCC File Offset: 0x00227DCC
	private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
	{
		HashedString hashedString = (HashedString)toggle_info.userData;
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen.Instance.ClearSearch();
		}
		this.ClearButtons();
		this.buildingGroupsRoot.gameObject.SetActive(true);
		this.activeCategoryInfo = toggle_info;
		if (play_sound)
		{
			UISounds.PlaySound(UISounds.Sound.ClickObject);
		}
		this.BuildButtonList();
		this.ForceRefreshAllBuildingToggles();
		this.UpdateBuildingButtonList(this.activeCategoryInfo);
		this.RefreshCategoryPanelTitle();
		this.ForceUpdateAllCategoryToggles(null);
		this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand(null);
	}

	// Token: 0x06005E5D RID: 24157 RVA: 0x00229C5C File Offset: 0x00227E5C
	public void RefreshCategoryPanelTitle()
	{
		if (this.activeCategoryInfo != null)
		{
			this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
		}
		if (!BuildingGroupScreen.SearchIsEmpty)
		{
			this.PlanCategoryLabel.text = UI.BUILDMENU.SEARCH_RESULTS_HEADER;
		}
	}

	// Token: 0x06005E5E RID: 24158 RVA: 0x00229CA8 File Offset: 0x00227EA8
	public void OpenCategoryByName(string category)
	{
		PlanScreen.ToggleEntry toggleEntry;
		if (this.GetToggleEntryForCategory(category, out toggleEntry))
		{
			this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
			this.ConfigurePanelSize(null);
		}
	}

	// Token: 0x06005E5F RID: 24159 RVA: 0x00229CDC File Offset: 0x00227EDC
	private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
	{
		KToggle toggle = toggle_info.toggle;
		if (toggle == null)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				if (toggleInfo.userData == toggle_info.userData)
				{
					toggle = toggleInfo.toggle;
					break;
				}
			}
		}
		if (toggle != null && this.allBuildingToggles.Count != 0)
		{
			if (this.forceRefreshAllBuildings)
			{
				for (int i = 0; i < this.allBuildingToggles.Count; i++)
				{
					PlanBuildingToggle value = this.allBuildingToggles.ElementAt(i).Value;
					this.categoryPanelSizeNeedsRefresh = (value.Refresh() || this.categoryPanelSizeNeedsRefresh);
					this.forceRefreshAllBuildings = false;
					value.SwitchViewMode(this.useSubCategoryLayout);
				}
			}
			else
			{
				for (int j = 0; j < this.maxToggleRefreshPerFrame; j++)
				{
					if (this.building_button_refresh_idx >= this.allBuildingToggles.Count)
					{
						this.building_button_refresh_idx = 0;
					}
					PlanBuildingToggle value2 = this.allBuildingToggles.ElementAt(this.building_button_refresh_idx).Value;
					this.categoryPanelSizeNeedsRefresh = (value2.Refresh() || this.categoryPanelSizeNeedsRefresh);
					value2.SwitchViewMode(this.useSubCategoryLayout);
					this.building_button_refresh_idx++;
				}
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.allSubCategoryObjects)
		{
			GridLayoutGroup componentInChildren = keyValuePair.Value.GetComponentInChildren<GridLayoutGroup>(true);
			if (!(componentInChildren == null))
			{
				int num = 0;
				for (int k = 0; k < componentInChildren.transform.childCount; k++)
				{
					if (componentInChildren.transform.GetChild(k).gameObject.activeSelf)
					{
						num++;
					}
				}
				if (keyValuePair.Value.gameObject.activeSelf != num > 0)
				{
					keyValuePair.Value.gameObject.SetActive(num > 0);
				}
			}
		}
		if (this.categoryPanelSizeNeedsRefresh && this.building_button_refresh_idx >= this.activeCategoryBuildingToggles.Count)
		{
			this.categoryPanelSizeNeedsRefresh = false;
			this.ConfigurePanelSize(null);
		}
		if (this.ProductInfoScreen.gameObject.activeSelf)
		{
			this.ProductInfoScreen.materialSelectionPanel.UpdateResourceToggleValues();
		}
	}

	// Token: 0x06005E60 RID: 24160 RVA: 0x00229F5C File Offset: 0x0022815C
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.RefreshBuildableStates(false);
		this.SetCategoryButtonState();
		if (this.activeCategoryInfo != null)
		{
			this.UpdateBuildingButtonList(this.activeCategoryInfo);
		}
	}

	// Token: 0x06005E61 RID: 24161 RVA: 0x00229F88 File Offset: 0x00228188
	private void BuildButtonList()
	{
		this.activeCategoryBuildingToggles.Clear();
		Dictionary<string, HashedString> dictionary = new Dictionary<string, HashedString>();
		Dictionary<string, List<BuildingDef>> dictionary2 = new Dictionary<string, List<BuildingDef>>();
		if (!dictionary2.ContainsKey("default"))
		{
			dictionary2.Add("default", new List<BuildingDef>());
		}
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu())
				{
					dictionary.Add(buildingDef.PrefabID, planInfo.category);
					if (!dictionary2.ContainsKey(keyValuePair.Value))
					{
						dictionary2.Add(keyValuePair.Value, new List<BuildingDef>());
					}
					dictionary2[keyValuePair.Value].Add(buildingDef);
				}
			}
		}
		if (!this.allSubCategoryObjects.ContainsKey("default"))
		{
			this.allSubCategoryObjects.Add("default", global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true));
		}
		GameObject gameObject = this.allSubCategoryObjects["default"].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
		foreach (KeyValuePair<string, List<BuildingDef>> keyValuePair2 in dictionary2)
		{
			if (!this.allSubCategoryObjects.ContainsKey(keyValuePair2.Key))
			{
				this.allSubCategoryObjects.Add(keyValuePair2.Key, global::Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true));
			}
			if (keyValuePair2.Key == "default")
			{
				this.allSubCategoryObjects[keyValuePair2.Key].SetActive(this.useSubCategoryLayout);
			}
			HierarchyReferences component = this.allSubCategoryObjects[keyValuePair2.Key].GetComponent<HierarchyReferences>();
			GameObject parent;
			if (this.useSubCategoryLayout)
			{
				component.GetReference<RectTransform>("Header").gameObject.SetActive(true);
				parent = this.allSubCategoryObjects[keyValuePair2.Key].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
				StringEntry entry;
				if (Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + keyValuePair2.Key.ToUpper() + ".BUILDMENUTITLE", out entry))
				{
					component.GetReference<LocText>("HeaderLabel").SetText(entry);
				}
			}
			else
			{
				component.GetReference<RectTransform>("Header").gameObject.SetActive(false);
				parent = gameObject;
			}
			foreach (BuildingDef buildingDef2 in keyValuePair2.Value)
			{
				HashedString hashedString = dictionary[buildingDef2.PrefabID];
				GameObject gameObject2 = this.CreateButton(buildingDef2, parent, hashedString);
				PlanScreen.ToggleEntry toggleEntry = null;
				this.GetToggleEntryForCategory(hashedString, out toggleEntry);
				if (toggleEntry != null && toggleEntry.pendingResearchAttentions.Contains(buildingDef2.PrefabID))
				{
					gameObject2.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
				}
			}
		}
		this.RefreshScale(null);
	}

	// Token: 0x06005E62 RID: 24162 RVA: 0x0022A344 File Offset: 0x00228544
	public void ConfigurePanelSize(object data = null)
	{
		if (this.useSubCategoryLayout)
		{
			this.buildGrid_bg_rowHeight = 48f;
		}
		else
		{
			this.buildGrid_bg_rowHeight = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize.y : PlanScreen.standarduildingButtonSize.y);
		}
		GridLayoutGroup reference = this.subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
		this.buildGrid_bg_rowHeight += reference.spacing.y;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.GroupsTransform.childCount; i++)
		{
			int num3 = 0;
			HierarchyReferences component = this.GroupsTransform.GetChild(i).GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				GridLayoutGroup reference2 = component.GetReference<GridLayoutGroup>("Grid");
				if (!(reference2 == null))
				{
					bool flag = false;
					for (int j = 0; j < reference2.transform.childCount; j++)
					{
						if (reference2.transform.GetChild(j).gameObject.activeSelf)
						{
							flag = true;
							num3++;
						}
					}
					if (flag)
					{
						num2 += 24;
					}
					num += num3 / reference2.constraintCount;
					if (num3 % reference2.constraintCount != 0)
					{
						num++;
					}
				}
			}
		}
		num2 = Math.Min(72, num2);
		this.noResultMessage.SetActive(num == 0);
		int num4 = num;
		int num5 = Math.Max(1, Screen.height / (int)this.buildGrid_bg_rowHeight - 3);
		num5 = Math.Min(num5, this.useSubCategoryLayout ? 12 : 6);
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num4 = Mathf.Min(num5, this.useSubCategoryLayout ? 8 : 4);
		}
		this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num4 >= num5 - 1);
		float num6 = this.buildGrid_bg_borderHeight + (float)num2 + 36f + (float)Mathf.Clamp(num4, 0, num5) * this.buildGrid_bg_rowHeight;
		if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
		{
			num6 = Mathf.Max(num6, this.buildingGroupsRoot.sizeDelta.y);
		}
		this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, num6);
		this.RefreshScale(null);
	}

	// Token: 0x06005E63 RID: 24163 RVA: 0x0022A571 File Offset: 0x00228771
	private void SetScrollPoint(float targetY)
	{
		this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
	}

	// Token: 0x06005E64 RID: 24164 RVA: 0x0022A594 File Offset: 0x00228794
	private GameObject CreateButton(BuildingDef def, GameObject parent, HashedString plan_category)
	{
		GameObject gameObject;
		PlanBuildingToggle planBuildingToggle;
		if (this.allBuildingToggles.ContainsKey(def.PrefabID))
		{
			gameObject = this.allBuildingToggles[def.PrefabID].gameObject;
			planBuildingToggle = this.allBuildingToggles[def.PrefabID];
			planBuildingToggle.Refresh();
		}
		else
		{
			gameObject = global::Util.KInstantiateUI(this.planButtonPrefab, parent, false);
			gameObject.name = UI.StripLinkFormatting(def.name) + " Group:" + plan_category.ToString();
			planBuildingToggle = gameObject.GetComponentInChildren<PlanBuildingToggle>();
			planBuildingToggle.Config(def, this, plan_category);
			planBuildingToggle.soundPlayer.Enabled = false;
			planBuildingToggle.SwitchViewMode(this.useSubCategoryLayout);
			this.allBuildingToggles.Add(def.PrefabID, planBuildingToggle);
		}
		if (gameObject.transform.parent != parent)
		{
			gameObject.transform.SetParent(parent.transform);
		}
		this.activeCategoryBuildingToggles.Add(def, planBuildingToggle);
		return gameObject;
	}

	// Token: 0x06005E65 RID: 24165 RVA: 0x0022A68C File Offset: 0x0022888C
	public static bool TechRequirementsMet(TechItem techItem)
	{
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x06005E66 RID: 24166 RVA: 0x0022A6AC File Offset: 0x002288AC
	private static bool TechRequirementsUpcoming(TechItem techItem)
	{
		return PlanScreen.TechRequirementsMet(techItem);
	}

	// Token: 0x06005E67 RID: 24167 RVA: 0x0022A6B4 File Offset: 0x002288B4
	private bool GetToggleEntryForCategory(HashedString category, out PlanScreen.ToggleEntry toggleEntry)
	{
		toggleEntry = null;
		foreach (PlanScreen.ToggleEntry toggleEntry2 in this.toggleEntries)
		{
			if (toggleEntry2.planCategory == category)
			{
				toggleEntry = toggleEntry2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005E68 RID: 24168 RVA: 0x0022A71C File Offset: 0x0022891C
	public bool IsDefBuildable(BuildingDef def)
	{
		return this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete;
	}

	// Token: 0x06005E69 RID: 24169 RVA: 0x0022A728 File Offset: 0x00228928
	public string GetTooltipForBuildable(BuildingDef def)
	{
		PlanScreen.RequirementsState buildableState = this.GetBuildableState(def);
		return PlanScreen.GetTooltipForRequirementsState(def, buildableState);
	}

	// Token: 0x06005E6A RID: 24170 RVA: 0x0022A744 File Offset: 0x00228944
	public static string GetTooltipForRequirementsState(BuildingDef def, PlanScreen.RequirementsState state)
	{
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		string text = null;
		if (Game.Instance.SandboxModeActive)
		{
			text = UIConstants.ColorPrefixYellow + UI.SANDBOXTOOLS.SETTINGS.INSTANT_BUILD.NAME + UIConstants.ColorSuffix;
		}
		else if (DebugHandler.InstantBuildMode)
		{
			text = UIConstants.ColorPrefixYellow + UI.DEBUG_TOOLS.DEBUG_ACTIVE + UIConstants.ColorSuffix;
		}
		else
		{
			switch (state)
			{
			case PlanScreen.RequirementsState.Tech:
				text = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
				break;
			case PlanScreen.RequirementsState.Materials:
				text = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
				foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
				{
					string str = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					text = text + "\n" + str;
				}
				break;
			case PlanScreen.RequirementsState.TelepadBuilt:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.UniquePerWorld:
				text = UI.PRODUCTINFO_UNIQUE_PER_WORLD;
				break;
			case PlanScreen.RequirementsState.RocketInteriorOnly:
				text = UI.PRODUCTINFO_ROCKET_INTERIOR;
				break;
			case PlanScreen.RequirementsState.RocketInteriorForbidden:
				text = UI.PRODUCTINFO_ROCKET_NOT_INTERIOR;
				break;
			}
		}
		return text;
	}

	// Token: 0x06005E6B RID: 24171 RVA: 0x0022A8D0 File Offset: 0x00228AD0
	private void PointerEnter(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = true;
	}

	// Token: 0x06005E6C RID: 24172 RVA: 0x0022A8DE File Offset: 0x00228ADE
	private void PointerExit(PointerEventData data)
	{
		this.planScreenScrollRect.mouseIsOver = false;
	}

	// Token: 0x06005E6D RID: 24173 RVA: 0x0022A8EC File Offset: 0x00228AEC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyDown(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.IsAction(global::Action.CopyBuilding) && e.TryConsume(global::Action.CopyBuilding))
		{
			this.OnClickCopyBuilding();
		}
		if (this.toggles == null)
		{
			return;
		}
		if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(global::Action.Escape))
		{
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			this.ClearSelection();
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06005E6E RID: 24174 RVA: 0x0022A9B4 File Offset: 0x00228BB4
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut))
				{
					this.planScreenScrollRect.OnKeyUp(e);
				}
			}
			else if (!e.TryConsume(global::Action.ZoomIn))
			{
				e.TryConsume(global::Action.ZoomOut);
			}
		}
		if (e.Consumed)
		{
			return;
		}
		if (this.SelectedBuildingGameObject != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.CloseRecipe(false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06005E6F RID: 24175 RVA: 0x0022AA74 File Offset: 0x00228C74
	private void OnRecipeElementsFullySelected()
	{
		BuildingDef buildingDef = null;
		foreach (KeyValuePair<string, PlanBuildingToggle> keyValuePair in this.allBuildingToggles)
		{
			if (keyValuePair.Value == this.currentlySelectedToggle)
			{
				buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				break;
			}
		}
		DebugUtil.DevAssert(buildingDef, "def is null", null);
		if (buildingDef)
		{
			if (buildingDef.isKAnimTile && buildingDef.isUtility)
			{
				IList<Tag> getSelectedElementAsList = this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
				((buildingDef.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(buildingDef, getSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
				return;
			}
			BuildTool.Instance.Activate(buildingDef, this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
		}
	}

	// Token: 0x06005E70 RID: 24176 RVA: 0x0022AB84 File Offset: 0x00228D84
	public void OnResearchComplete(object tech)
	{
		foreach (TechItem techItem in ((Tech)tech).unlockedItems)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef != null)
			{
				this.UpdateDefResearched(buildingDef);
				if (this.tagCategoryMap.ContainsKey(buildingDef.Tag))
				{
					HashedString category = this.tagCategoryMap[buildingDef.Tag];
					PlanScreen.ToggleEntry toggleEntry;
					if (this.GetToggleEntryForCategory(category, out toggleEntry))
					{
						toggleEntry.pendingResearchAttentions.Add(buildingDef.Tag);
						toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
						toggleEntry.Refresh();
					}
				}
			}
		}
	}

	// Token: 0x06005E71 RID: 24177 RVA: 0x0022AC54 File Offset: 0x00228E54
	private void OnUIClear(object data)
	{
		if (this.activeCategoryInfo != null)
		{
			this.selected = -1;
			this.OnClickCategory(this.activeCategoryInfo);
			SelectTool.Instance.Activate();
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x06005E72 RID: 24178 RVA: 0x0022ACA4 File Offset: 0x00228EA4
	private void OnActiveToolChanged(object data)
	{
		if (data == null)
		{
			return;
		}
		if (this.ignoreToolChangeMessages > 0)
		{
			return;
		}
		Type type = data.GetType();
		if (!typeof(BuildTool).IsAssignableFrom(type) && !typeof(PrebuildTool).IsAssignableFrom(type) && !typeof(BaseUtilityBuildTool).IsAssignableFrom(type))
		{
			this.CloseRecipe(false);
			this.CloseCategoryPanel(false);
		}
	}

	// Token: 0x06005E73 RID: 24179 RVA: 0x0022AD0A File Offset: 0x00228F0A
	public PrioritySetting GetBuildingPriority()
	{
		return this.ProductInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x04003F73 RID: 16243
	[SerializeField]
	private GameObject planButtonPrefab;

	// Token: 0x04003F74 RID: 16244
	[SerializeField]
	private GameObject recipeInfoScreenParent;

	// Token: 0x04003F75 RID: 16245
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x04003F76 RID: 16246
	[SerializeField]
	private GameObject copyBuildingButton;

	// Token: 0x04003F77 RID: 16247
	[SerializeField]
	private KButton gridViewButton;

	// Token: 0x04003F78 RID: 16248
	[SerializeField]
	private KButton listViewButton;

	// Token: 0x04003F79 RID: 16249
	private bool useSubCategoryLayout;

	// Token: 0x04003F7A RID: 16250
	private int refreshScaleHandle = -1;

	// Token: 0x04003F7B RID: 16251
	[SerializeField]
	private GameObject adjacentPinnedButtons;

	// Token: 0x04003F7C RID: 16252
	private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>
	{
		{
			PlanScreen.CacheHashedString("Base"),
			"icon_category_base"
		},
		{
			PlanScreen.CacheHashedString("Oxygen"),
			"icon_category_oxygen"
		},
		{
			PlanScreen.CacheHashedString("Power"),
			"icon_category_electrical"
		},
		{
			PlanScreen.CacheHashedString("Food"),
			"icon_category_food"
		},
		{
			PlanScreen.CacheHashedString("Plumbing"),
			"icon_category_plumbing"
		},
		{
			PlanScreen.CacheHashedString("HVAC"),
			"icon_category_ventilation"
		},
		{
			PlanScreen.CacheHashedString("Refining"),
			"icon_category_refinery"
		},
		{
			PlanScreen.CacheHashedString("Medical"),
			"icon_category_medical"
		},
		{
			PlanScreen.CacheHashedString("Furniture"),
			"icon_category_furniture"
		},
		{
			PlanScreen.CacheHashedString("Equipment"),
			"icon_category_misc"
		},
		{
			PlanScreen.CacheHashedString("Utilities"),
			"icon_category_utilities"
		},
		{
			PlanScreen.CacheHashedString("Automation"),
			"icon_category_automation"
		},
		{
			PlanScreen.CacheHashedString("Conveyance"),
			"icon_category_shipping"
		},
		{
			PlanScreen.CacheHashedString("Rocketry"),
			"icon_category_rocketry"
		},
		{
			PlanScreen.CacheHashedString("HEP"),
			"icon_category_radiation"
		}
	};

	// Token: 0x04003F7D RID: 16253
	private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();

	// Token: 0x04003F7F RID: 16255
	[SerializeField]
	public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04003F80 RID: 16256
	public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;

	// Token: 0x04003F81 RID: 16257
	private KIconToggleMenu.ToggleInfo activeCategoryInfo;

	// Token: 0x04003F82 RID: 16258
	public Dictionary<BuildingDef, PlanBuildingToggle> activeCategoryBuildingToggles = new Dictionary<BuildingDef, PlanBuildingToggle>();

	// Token: 0x04003F83 RID: 16259
	private float timeSinceNotificationPing;

	// Token: 0x04003F84 RID: 16260
	private float notificationPingExpire = 0.5f;

	// Token: 0x04003F85 RID: 16261
	private float specialNotificationEmbellishDelay = 8f;

	// Token: 0x04003F86 RID: 16262
	private int notificationPingCount;

	// Token: 0x04003F87 RID: 16263
	private Dictionary<KToggle, Bouncer> toggleBouncers = new Dictionary<KToggle, Bouncer>();

	// Token: 0x04003F88 RID: 16264
	public const string DEFAULT_SUBCATEGORY_KEY = "default";

	// Token: 0x04003F89 RID: 16265
	private Dictionary<string, GameObject> allSubCategoryObjects = new Dictionary<string, GameObject>();

	// Token: 0x04003F8A RID: 16266
	private Dictionary<string, PlanBuildingToggle> allBuildingToggles = new Dictionary<string, PlanBuildingToggle>();

	// Token: 0x04003F8B RID: 16267
	private static Vector2 bigBuildingButtonSize = new Vector2(98f, 123f);

	// Token: 0x04003F8C RID: 16268
	private static Vector2 standarduildingButtonSize = PlanScreen.bigBuildingButtonSize * 0.8f;

	// Token: 0x04003F8D RID: 16269
	public static int fontSizeBigMode = 16;

	// Token: 0x04003F8E RID: 16270
	public static int fontSizeStandardMode = 14;

	// Token: 0x04003F90 RID: 16272
	[SerializeField]
	private GameObject subgroupPrefab;

	// Token: 0x04003F91 RID: 16273
	public Transform GroupsTransform;

	// Token: 0x04003F92 RID: 16274
	public Sprite Overlay_NeedTech;

	// Token: 0x04003F93 RID: 16275
	public RectTransform buildingGroupsRoot;

	// Token: 0x04003F94 RID: 16276
	public RectTransform BuildButtonBGPanel;

	// Token: 0x04003F95 RID: 16277
	public RectTransform BuildingGroupContentsRect;

	// Token: 0x04003F96 RID: 16278
	public Sprite defaultBuildingIconSprite;

	// Token: 0x04003F97 RID: 16279
	private KScrollRect planScreenScrollRect;

	// Token: 0x04003F98 RID: 16280
	public Material defaultUIMaterial;

	// Token: 0x04003F99 RID: 16281
	public Material desaturatedUIMaterial;

	// Token: 0x04003F9A RID: 16282
	public LocText PlanCategoryLabel;

	// Token: 0x04003F9B RID: 16283
	public GameObject noResultMessage;

	// Token: 0x04003F9C RID: 16284
	private int nextCategoryToUpdateIDX = -1;

	// Token: 0x04003F9D RID: 16285
	private bool forceUpdateAllCategoryToggles;

	// Token: 0x04003F9E RID: 16286
	private bool forceRefreshAllBuildings = true;

	// Token: 0x04003F9F RID: 16287
	private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();

	// Token: 0x04003FA0 RID: 16288
	private int ignoreToolChangeMessages;

	// Token: 0x04003FA1 RID: 16289
	private Dictionary<string, PlanScreen.RequirementsState> _buildableStatesByID = new Dictionary<string, PlanScreen.RequirementsState>();

	// Token: 0x04003FA2 RID: 16290
	private Dictionary<Def, bool> _researchedDefs = new Dictionary<Def, bool>();

	// Token: 0x04003FA3 RID: 16291
	[SerializeField]
	private TextStyleSetting[] CategoryLabelTextStyles;

	// Token: 0x04003FA4 RID: 16292
	private float initTime;

	// Token: 0x04003FA5 RID: 16293
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x04003FA6 RID: 16294
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x04003FA7 RID: 16295
	private BuildingDef lastSelectedBuildingDef;

	// Token: 0x04003FA8 RID: 16296
	private Building lastSelectedBuilding;

	// Token: 0x04003FA9 RID: 16297
	private string lastSelectedBuildingFacade = "DEFAULT_FACADE";

	// Token: 0x04003FAA RID: 16298
	private int buildable_state_update_idx;

	// Token: 0x04003FAB RID: 16299
	private int building_button_refresh_idx;

	// Token: 0x04003FAC RID: 16300
	private int maxToggleRefreshPerFrame = 10;

	// Token: 0x04003FAD RID: 16301
	private bool categoryPanelSizeNeedsRefresh;

	// Token: 0x04003FAE RID: 16302
	private float buildGrid_bg_width = 320f;

	// Token: 0x04003FAF RID: 16303
	private float buildGrid_bg_borderHeight = 48f;

	// Token: 0x04003FB0 RID: 16304
	private const float BUILDGRID_SEARCHBAR_HEIGHT = 36f;

	// Token: 0x04003FB1 RID: 16305
	private const int SUBCATEGORY_HEADER_HEIGHT = 24;

	// Token: 0x04003FB2 RID: 16306
	private float buildGrid_bg_rowHeight;

	// Token: 0x02001B04 RID: 6916
	public struct PlanInfo
	{
		// Token: 0x060098CC RID: 39116 RVA: 0x00342C44 File Offset: 0x00340E44
		public PlanInfo(HashedString category, bool hideIfNotResearched, List<string> listData, string RequiredDlcId = "")
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (string key in listData)
			{
				list.Add(new KeyValuePair<string, string>(key, TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(key) ? TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[key] : "uncategorized"));
			}
			this.category = category;
			this.hideIfNotResearched = hideIfNotResearched;
			this.data = listData;
			this.buildingAndSubcategoryData = list;
			this.RequiredDlcId = RequiredDlcId;
		}

		// Token: 0x04007B5D RID: 31581
		public HashedString category;

		// Token: 0x04007B5E RID: 31582
		public bool hideIfNotResearched;

		// Token: 0x04007B5F RID: 31583
		[Obsolete("Modders: Use ModUtil.AddBuildingToPlanScreen")]
		public List<string> data;

		// Token: 0x04007B60 RID: 31584
		public List<KeyValuePair<string, string>> buildingAndSubcategoryData;

		// Token: 0x04007B61 RID: 31585
		public string RequiredDlcId;
	}

	// Token: 0x02001B05 RID: 6917
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x04007B62 RID: 31586
		public TextStyleSetting BuildButtonName;

		// Token: 0x04007B63 RID: 31587
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x04007B64 RID: 31588
		public TextStyleSetting MaterialRequirement;

		// Token: 0x04007B65 RID: 31589
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001B06 RID: 6918
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x04007B66 RID: 31590
		public TextStyleSetting ActiveSelected;

		// Token: 0x04007B67 RID: 31591
		public TextStyleSetting ActiveDeselected;

		// Token: 0x04007B68 RID: 31592
		public TextStyleSetting InactiveSelected;

		// Token: 0x04007B69 RID: 31593
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001B07 RID: 6919
	private class ToggleEntry
	{
		// Token: 0x060098CD RID: 39117 RVA: 0x00342CE0 File Offset: 0x00340EE0
		public ToggleEntry(KIconToggleMenu.ToggleInfo toggle_info, HashedString plan_category, List<BuildingDef> building_defs, bool hideIfNotResearched)
		{
			this.toggleInfo = toggle_info;
			this.planCategory = plan_category;
			this.buildingDefs = building_defs;
			this.hideIfNotResearched = hideIfNotResearched;
			this.pendingResearchAttentions = new List<Tag>();
			this.requiredTechItems = new List<TechItem>();
			this.toggleImages = null;
			foreach (BuildingDef buildingDef in building_defs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
				if (techItem == null)
				{
					this.requiredTechItems.Clear();
					break;
				}
				if (!this.requiredTechItems.Contains(techItem))
				{
					this.requiredTechItems.Add(techItem);
				}
			}
			this._areAnyRequiredTechItemsAvailable = false;
			this.Refresh();
		}

		// Token: 0x060098CE RID: 39118 RVA: 0x00342DB4 File Offset: 0x00340FB4
		public bool AreAnyRequiredTechItemsAvailable()
		{
			return this._areAnyRequiredTechItemsAvailable;
		}

		// Token: 0x060098CF RID: 39119 RVA: 0x00342DBC File Offset: 0x00340FBC
		public void Refresh()
		{
			if (this._areAnyRequiredTechItemsAvailable)
			{
				return;
			}
			if (this.requiredTechItems.Count == 0)
			{
				this._areAnyRequiredTechItemsAvailable = true;
				return;
			}
			using (List<TechItem>.Enumerator enumerator = this.requiredTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PlanScreen.TechRequirementsUpcoming(enumerator.Current))
					{
						this._areAnyRequiredTechItemsAvailable = true;
						break;
					}
				}
			}
		}

		// Token: 0x060098D0 RID: 39120 RVA: 0x00342E38 File Offset: 0x00341038
		public void CollectToggleImages()
		{
			this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
		}

		// Token: 0x04007B6A RID: 31594
		public KIconToggleMenu.ToggleInfo toggleInfo;

		// Token: 0x04007B6B RID: 31595
		public HashedString planCategory;

		// Token: 0x04007B6C RID: 31596
		public List<BuildingDef> buildingDefs;

		// Token: 0x04007B6D RID: 31597
		public List<Tag> pendingResearchAttentions;

		// Token: 0x04007B6E RID: 31598
		private List<TechItem> requiredTechItems;

		// Token: 0x04007B6F RID: 31599
		public ImageToggleState[] toggleImages;

		// Token: 0x04007B70 RID: 31600
		public bool hideIfNotResearched;

		// Token: 0x04007B71 RID: 31601
		private bool _areAnyRequiredTechItemsAvailable;
	}

	// Token: 0x02001B08 RID: 6920
	public enum RequirementsState
	{
		// Token: 0x04007B73 RID: 31603
		Invalid,
		// Token: 0x04007B74 RID: 31604
		Tech,
		// Token: 0x04007B75 RID: 31605
		Materials,
		// Token: 0x04007B76 RID: 31606
		Complete,
		// Token: 0x04007B77 RID: 31607
		TelepadBuilt,
		// Token: 0x04007B78 RID: 31608
		UniquePerWorld,
		// Token: 0x04007B79 RID: 31609
		RocketInteriorOnly,
		// Token: 0x04007B7A RID: 31610
		RocketInteriorForbidden
	}
}
