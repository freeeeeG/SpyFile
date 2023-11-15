using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BBE RID: 3006
public class PlanBuildingToggle : KToggle
{
	// Token: 0x06005E1F RID: 24095 RVA: 0x00227AF4 File Offset: 0x00225CF4
	public void Config(BuildingDef def, PlanScreen planScreen, HashedString buildingCategory)
	{
		this.def = def;
		this.planScreen = planScreen;
		this.buildingCategory = buildingCategory;
		this.techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(1557339983, new Action<object>(this.CheckResearch)));
		this.sprite = def.GetUISprite("ui", false);
		base.onClick += delegate()
		{
			PlanScreen.Instance.OnSelectBuilding(this.gameObject, def, null);
			this.RefreshDisplay();
		};
		if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(def.PrefabID))
		{
			Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[def.PrefabID].ToUpper() + ".NAME", out this.subcategoryName);
		}
		else
		{
			global::Debug.LogWarning("Building " + def.PrefabID + " has not been added to plan screen subcategory organization in BuildingTuning.cs");
		}
		this.CheckResearch(null);
		this.Refresh();
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x00227C68 File Offset: 0x00225E68
	protected override void OnDestroy()
	{
		if (Game.Instance != null)
		{
			foreach (int id in this.gameSubscriptions)
			{
				Game.Instance.Unsubscribe(id);
			}
		}
		this.gameSubscriptions.Clear();
		base.OnDestroy();
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x00227CE0 File Offset: 0x00225EE0
	private void CheckResearch(object data = null)
	{
		this.researchComplete = PlanScreen.TechRequirementsMet(this.techItem);
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x00227CF4 File Offset: 0x00225EF4
	public bool CheckBuildingPassesSearchFilter(Def building)
	{
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			return this.StandardDisplayFilter();
		}
		string text = BuildingGroupScreen.Instance.inputField.text;
		string text2 = UI.StripLinkFormatting(building.Name).ToLower();
		text = text.ToUpper();
		return text2.ToUpper().Contains(text) || (this.subcategoryName != null && this.subcategoryName.String.ToUpper().Contains(text));
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x00227D68 File Offset: 0x00225F68
	private bool StandardDisplayFilter()
	{
		return (this.researchComplete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive) && (this.planScreen.ActiveCategoryToggleInfo == null || this.buildingCategory == (HashedString)this.planScreen.ActiveCategoryToggleInfo.userData);
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x00227DC4 File Offset: 0x00225FC4
	public bool Refresh()
	{
		bool flag;
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			flag = this.StandardDisplayFilter();
		}
		else
		{
			flag = this.CheckBuildingPassesSearchFilter(this.def);
		}
		bool result = false;
		if (base.gameObject.activeSelf != flag)
		{
			base.gameObject.SetActive(flag);
			result = true;
		}
		if (!base.gameObject.activeSelf)
		{
			return result;
		}
		this.PositionTooltip();
		this.RefreshLabel();
		this.RefreshDisplay();
		return result;
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x00227E30 File Offset: 0x00226030
	public void SwitchViewMode(bool listView)
	{
		this.text.gameObject.SetActive(!listView);
		this.text_listView.gameObject.SetActive(listView);
		this.buildingIcon.gameObject.SetActive(!listView);
		this.buildingIcon_listView.gameObject.SetActive(listView);
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x00227E88 File Offset: 0x00226088
	private void RefreshLabel()
	{
		if (this.text != null)
		{
			this.text.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text_listView.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text.text = this.def.Name;
			this.text_listView.text = this.def.Name;
		}
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x00227F10 File Offset: 0x00226110
	private void RefreshDisplay()
	{
		PlanScreen.RequirementsState buildableState = PlanScreen.Instance.GetBuildableState(this.def);
		bool flag = buildableState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		bool flag2 = base.gameObject == PlanScreen.Instance.SelectedBuildingGameObject;
		if (flag2 && flag)
		{
			this.toggle.ChangeState(1);
		}
		else if (!flag2 && flag)
		{
			this.toggle.ChangeState(0);
		}
		else if (flag2 && !flag)
		{
			this.toggle.ChangeState(3);
		}
		else if (!flag2 && !flag)
		{
			this.toggle.ChangeState(2);
		}
		this.RefreshBuildingButtonIconAndColors(flag);
		this.RefreshFG(buildableState);
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x00227FC0 File Offset: 0x002261C0
	private void PositionTooltip()
	{
		this.tooltip.overrideParentObject = (PlanScreen.Instance.ProductInfoScreen.gameObject.activeSelf ? PlanScreen.Instance.ProductInfoScreen.rectTransform() : PlanScreen.Instance.buildingGroupsRoot);
		this.tooltip.tooltipPivot = Vector2.zero;
		this.tooltip.parentPositionAnchor = new Vector2(1f, 0f);
		this.tooltip.tooltipPositionOffset = new Vector2(4f, 0f);
		this.tooltip.ClearMultiStringTooltip();
		string name = this.def.Name;
		string effect = this.def.Effect;
		this.tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		this.tooltip.AddMultiStringTooltip(effect, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x002280A8 File Offset: 0x002262A8
	private void RefreshBuildingButtonIconAndColors(bool buttonAvailable)
	{
		if (this.sprite == null)
		{
			this.sprite = PlanScreen.Instance.defaultBuildingIconSprite;
		}
		this.buildingIcon.sprite = this.sprite;
		this.buildingIcon.SetNativeSize();
		this.buildingIcon_listView.sprite = this.sprite;
		float d = ScreenResolutionMonitor.UsingGamepadUIMode() ? 3.25f : 4f;
		this.buildingIcon.rectTransform().sizeDelta /= d;
		Material material = buttonAvailable ? PlanScreen.Instance.defaultUIMaterial : PlanScreen.Instance.desaturatedUIMaterial;
		if (this.buildingIcon.material != material)
		{
			this.buildingIcon.material = material;
			this.buildingIcon_listView.material = material;
		}
	}

	// Token: 0x06005E2A RID: 24106 RVA: 0x00228178 File Offset: 0x00226378
	private void RefreshFG(PlanScreen.RequirementsState requirementsState)
	{
		if (requirementsState == PlanScreen.RequirementsState.Tech)
		{
			this.fgImage.sprite = PlanScreen.Instance.Overlay_NeedTech;
			this.fgImage.gameObject.SetActive(true);
		}
		else
		{
			this.fgImage.gameObject.SetActive(false);
		}
		string tooltipForRequirementsState = PlanScreen.GetTooltipForRequirementsState(this.def, requirementsState);
		if (tooltipForRequirementsState != null)
		{
			this.tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
			this.tooltip.AddMultiStringTooltip(tooltipForRequirementsState, PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x04003F62 RID: 16226
	private BuildingDef def;

	// Token: 0x04003F63 RID: 16227
	private HashedString buildingCategory;

	// Token: 0x04003F64 RID: 16228
	private TechItem techItem;

	// Token: 0x04003F65 RID: 16229
	private List<int> gameSubscriptions = new List<int>();

	// Token: 0x04003F66 RID: 16230
	private bool researchComplete;

	// Token: 0x04003F67 RID: 16231
	private Sprite sprite;

	// Token: 0x04003F68 RID: 16232
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04003F69 RID: 16233
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04003F6A RID: 16234
	[SerializeField]
	private LocText text;

	// Token: 0x04003F6B RID: 16235
	[SerializeField]
	private LocText text_listView;

	// Token: 0x04003F6C RID: 16236
	[SerializeField]
	private Image buildingIcon;

	// Token: 0x04003F6D RID: 16237
	[SerializeField]
	private Image buildingIcon_listView;

	// Token: 0x04003F6E RID: 16238
	[SerializeField]
	private Image fgIcon;

	// Token: 0x04003F6F RID: 16239
	[SerializeField]
	private PlanScreen planScreen;

	// Token: 0x04003F70 RID: 16240
	private StringEntry subcategoryName;
}
