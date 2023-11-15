using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A52 RID: 2642
public class BuildMenuBuildingsScreen : KIconToggleMenu
{
	// Token: 0x06004F91 RID: 20369 RVA: 0x001C189D File Offset: 0x001BFA9D
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06004F92 RID: 20370 RVA: 0x001C18A4 File Offset: 0x001BFAA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateBuildableStates();
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		base.onSelect += this.OnClickBuilding;
		Game.Instance.Subscribe(-1190690038, new Action<object>(this.OnBuildToolDeactivated));
	}

	// Token: 0x06004F93 RID: 20371 RVA: 0x001C1908 File Offset: 0x001BFB08
	public void Configure(HashedString category, IList<BuildMenu.BuildingInfo> building_infos)
	{
		this.ClearButtons();
		this.SetHasFocus(true);
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		string text = HashCache.Get().Get(category).ToUpper();
		text = text.Replace(" ", "");
		this.titleLabel.text = Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".BUILDMENUTITLE");
		foreach (BuildMenu.BuildingInfo buildingInfo in building_infos)
		{
			BuildingDef def = Assets.GetBuildingDef(buildingInfo.id);
			if (def.ShouldShowInBuildMenu() && def.IsAvailable())
			{
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(def.Name, new BuildMenuBuildingsScreen.UserData(def, PlanScreen.RequirementsState.Tech), def.HotKey, () => def.GetUISprite("ui", false));
				list.Add(item);
			}
		}
		base.Setup(list);
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			this.RefreshToggle(this.toggleInfo[i]);
		}
		int num = 0;
		using (IEnumerator enumerator2 = this.gridSizer.transform.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((Transform)enumerator2.Current).gameObject.activeSelf)
				{
					num++;
				}
			}
		}
		this.gridSizer.constraintCount = Mathf.Min(num, 3);
		int num2 = Mathf.Min(num, this.gridSizer.constraintCount);
		int num3 = (num + this.gridSizer.constraintCount - 1) / this.gridSizer.constraintCount;
		int num4 = num2 - 1;
		int num5 = num3 - 1;
		Vector2 vector = new Vector2((float)num2 * this.gridSizer.cellSize.x + (float)num4 * this.gridSizer.spacing.x + (float)this.gridSizer.padding.left + (float)this.gridSizer.padding.right, (float)num3 * this.gridSizer.cellSize.y + (float)num5 * this.gridSizer.spacing.y + (float)this.gridSizer.padding.top + (float)this.gridSizer.padding.bottom);
		this.contentSizeLayout.minWidth = vector.x;
		this.contentSizeLayout.minHeight = vector.y;
	}

	// Token: 0x06004F94 RID: 20372 RVA: 0x001C1BC8 File Offset: 0x001BFDC8
	private void ConfigureToolTip(ToolTip tooltip, BuildingDef def)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(def.Name, this.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(def.Effect, this.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x06004F95 RID: 20373 RVA: 0x001C1C00 File Offset: 0x001BFE00
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06004F96 RID: 20374 RVA: 0x001C1C68 File Offset: 0x001BFE68
	private void RefreshToggle(KIconToggleMenu.ToggleInfo info)
	{
		if (info == null || info.toggle == null)
		{
			return;
		}
		BuildingDef def = (info.userData as BuildMenuBuildingsScreen.UserData).def;
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		bool flag = DebugHandler.InstantBuildMode || techItem == null || techItem.IsComplete();
		bool flag2 = flag || techItem == null || techItem.ParentTech.ArePrerequisitesComplete();
		KToggle toggle = info.toggle;
		if (toggle.gameObject.activeSelf != flag2)
		{
			toggle.gameObject.SetActive(flag2);
		}
		if (toggle.bgImage == null)
		{
			return;
		}
		Image image = toggle.bgImage.GetComponentsInChildren<Image>()[1];
		Sprite uisprite = def.GetUISprite("ui", false);
		image.sprite = uisprite;
		image.SetNativeSize();
		image.rectTransform().sizeDelta /= 4f;
		ToolTip component = toggle.gameObject.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		string text = def.Name;
		string effect = def.Effect;
		if (def.HotKey != global::Action.NumActions)
		{
			text += GameUtil.GetHotkeyString(def.HotKey);
		}
		component.AddMultiStringTooltip(text, this.buildingToolTipSettings.BuildButtonName);
		component.AddMultiStringTooltip(effect, this.buildingToolTipSettings.BuildButtonDescription);
		LocText componentInChildren = toggle.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = def.Name;
		}
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		int num = (requirementsState == PlanScreen.RequirementsState.Complete) ? 1 : 0;
		ImageToggleState.State state;
		if (def == this.selectedBuilding && (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode))
		{
			state = ImageToggleState.State.Active;
		}
		else
		{
			state = ((requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode) ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		if (def == this.selectedBuilding && state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.DisabledActive;
		}
		else if (state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.Disabled;
		}
		toggle.GetComponent<ImageToggleState>().SetState(state);
		Material material;
		Color color;
		if (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode)
		{
			material = this.defaultUIMaterial;
			color = Color.white;
		}
		else
		{
			material = this.desaturatedUIMaterial;
			Color color3;
			if (!flag)
			{
				Graphic graphic = image;
				Color color2 = new Color(1f, 1f, 1f, 0.15f);
				graphic.color = color2;
				color3 = color2;
			}
			else
			{
				color3 = new Color(1f, 1f, 1f, 0.6f);
			}
			color = color3;
		}
		if (image.material != material)
		{
			image.material = material;
			image.color = color;
		}
		Image fgImage = toggle.gameObject.GetComponent<KToggle>().fgImage;
		fgImage.gameObject.SetActive(false);
		if (!flag)
		{
			fgImage.sprite = this.Overlay_NeedTech;
			fgImage.gameObject.SetActive(true);
			string newString = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			component.AddMultiStringTooltip(newString, this.buildingToolTipSettings.ResearchRequirement);
			return;
		}
		if (requirementsState != PlanScreen.RequirementsState.Complete)
		{
			fgImage.gameObject.SetActive(false);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			string newString2 = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
			component.AddMultiStringTooltip(newString2, this.buildingToolTipSettings.ResearchRequirement);
			foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
			{
				string newString3 = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				component.AddMultiStringTooltip(newString3, this.buildingToolTipSettings.ResearchRequirement);
			}
			component.AddMultiStringTooltip("", this.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x06004F97 RID: 20375 RVA: 0x001C2058 File Offset: 0x001C0258
	public void ClearUI()
	{
		this.Show(false);
		this.ClearButtons();
	}

	// Token: 0x06004F98 RID: 20376 RVA: 0x001C2068 File Offset: 0x001C0268
	private void ClearButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.gameObject.SetActive(false);
			ktoggle.gameObject.transform.SetParent(null);
			UnityEngine.Object.DestroyImmediate(ktoggle.gameObject);
		}
		if (this.toggles != null)
		{
			this.toggles.Clear();
		}
		if (this.toggleInfo != null)
		{
			this.toggleInfo.Clear();
		}
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x001C2100 File Offset: 0x001C0300
	private void OnClickBuilding(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuBuildingsScreen.UserData userData = toggle_info.userData as BuildMenuBuildingsScreen.UserData;
		this.OnSelectBuilding(userData.def);
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x001C2128 File Offset: 0x001C0328
	private void OnSelectBuilding(BuildingDef def)
	{
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (def != this.selectedBuilding)
			{
				this.selectedBuilding = def;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedBuilding = null;
				this.ClearSelection();
				this.CloseRecipe(true);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedBuilding = null;
			this.ClearSelection();
			this.CloseRecipe(true);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x001C21CC File Offset: 0x001C03CC
	public void UpdateBuildableStates()
	{
		if (this.toggleInfo == null || this.toggleInfo.Count <= 0)
		{
			return;
		}
		BuildingDef buildingDef = null;
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			this.RefreshToggle(toggleInfo);
			BuildMenuBuildingsScreen.UserData userData = toggleInfo.userData as BuildMenuBuildingsScreen.UserData;
			BuildingDef def = userData.def;
			if (def.IsAvailable())
			{
				PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
				if (requirementsState != userData.requirementsState)
				{
					if (def == BuildMenu.Instance.SelectedBuildingDef)
					{
						buildingDef = def;
					}
					this.RefreshToggle(toggleInfo);
					userData.requirementsState = requirementsState;
				}
			}
		}
		if (buildingDef != null)
		{
			BuildMenu.Instance.RefreshProductInfoScreen(buildingDef);
		}
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001C22A0 File Offset: 0x001C04A0
	private void OnResearchComplete(object data)
	{
		this.UpdateBuildableStates();
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001C22A8 File Offset: 0x001C04A8
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || typeof(PrebuildTool).IsAssignableFrom(type))
			{
				activeTool.DeactivateTool(null);
			}
		}
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001C2310 File Offset: 0x001C0510
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll && !e.TryConsume(global::Action.ZoomIn))
		{
			e.TryConsume(global::Action.ZoomOut);
		}
		if (!this.HasFocus)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			Game.Instance.Trigger(288942073, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			return;
		}
		base.OnKeyDown(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06004F9F RID: 20383 RVA: 0x001C2394 File Offset: 0x001C0594
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!this.HasFocus)
		{
			return;
		}
		if (this.selectedBuilding != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			Game.Instance.Trigger(288942073, null);
			return;
		}
		base.OnKeyUp(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001C240C File Offset: 0x001C060C
	public override void Close()
	{
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.ClearButtons();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06004FA1 RID: 20385 RVA: 0x001C2462 File Offset: 0x001C0662
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001C249A File Offset: 0x001C069A
	private void OnBuildToolDeactivated(object data)
	{
		this.CloseRecipe(false);
	}

	// Token: 0x0400340B RID: 13323
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x0400340C RID: 13324
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x0400340D RID: 13325
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x0400340E RID: 13326
	public Action<BuildingDef> onBuildingSelected;

	// Token: 0x0400340F RID: 13327
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04003410 RID: 13328
	[SerializeField]
	private BuildMenuBuildingsScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04003411 RID: 13329
	[SerializeField]
	private LayoutElement contentSizeLayout;

	// Token: 0x04003412 RID: 13330
	[SerializeField]
	private GridLayoutGroup gridSizer;

	// Token: 0x04003413 RID: 13331
	[SerializeField]
	private Sprite Overlay_NeedTech;

	// Token: 0x04003414 RID: 13332
	[SerializeField]
	private Material defaultUIMaterial;

	// Token: 0x04003415 RID: 13333
	[SerializeField]
	private Material desaturatedUIMaterial;

	// Token: 0x04003416 RID: 13334
	private BuildingDef selectedBuilding;

	// Token: 0x020018E1 RID: 6369
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x04007341 RID: 29505
		public TextStyleSetting BuildButtonName;

		// Token: 0x04007342 RID: 29506
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x04007343 RID: 29507
		public TextStyleSetting MaterialRequirement;

		// Token: 0x04007344 RID: 29508
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x020018E2 RID: 6370
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x04007345 RID: 29509
		public TextStyleSetting ActiveSelected;

		// Token: 0x04007346 RID: 29510
		public TextStyleSetting ActiveDeselected;

		// Token: 0x04007347 RID: 29511
		public TextStyleSetting InactiveSelected;

		// Token: 0x04007348 RID: 29512
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x020018E3 RID: 6371
	private class UserData
	{
		// Token: 0x06009323 RID: 37667 RVA: 0x0032E2AB File Offset: 0x0032C4AB
		public UserData(BuildingDef def, PlanScreen.RequirementsState state)
		{
			this.def = def;
			this.requirementsState = state;
		}

		// Token: 0x04007349 RID: 29513
		public BuildingDef def;

		// Token: 0x0400734A RID: 29514
		public PlanScreen.RequirementsState requirementsState;
	}
}
