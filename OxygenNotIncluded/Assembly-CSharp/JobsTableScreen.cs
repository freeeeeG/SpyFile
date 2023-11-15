using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B6B RID: 2923
public class JobsTableScreen : TableScreen
{
	// Token: 0x06005A5C RID: 23132 RVA: 0x00211EAC File Offset: 0x002100AC
	public override float GetSortKey()
	{
		return 22f;
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06005A5D RID: 23133 RVA: 0x00211EB4 File Offset: 0x002100B4
	public static List<JobsTableScreen.PriorityInfo> priorityInfo
	{
		get
		{
			if (JobsTableScreen._priorityInfo == null)
			{
				JobsTableScreen._priorityInfo = new List<JobsTableScreen.PriorityInfo>
				{
					new JobsTableScreen.PriorityInfo(0, Assets.GetSprite("icon_priority_disabled"), UI.JOBSSCREEN.PRIORITY.DISABLED),
					new JobsTableScreen.PriorityInfo(1, Assets.GetSprite("icon_priority_down_2"), UI.JOBSSCREEN.PRIORITY.VERYLOW),
					new JobsTableScreen.PriorityInfo(2, Assets.GetSprite("icon_priority_down"), UI.JOBSSCREEN.PRIORITY.LOW),
					new JobsTableScreen.PriorityInfo(3, Assets.GetSprite("icon_priority_flat"), UI.JOBSSCREEN.PRIORITY.STANDARD),
					new JobsTableScreen.PriorityInfo(4, Assets.GetSprite("icon_priority_up"), UI.JOBSSCREEN.PRIORITY.HIGH),
					new JobsTableScreen.PriorityInfo(5, Assets.GetSprite("icon_priority_up_2"), UI.JOBSSCREEN.PRIORITY.VERYHIGH),
					new JobsTableScreen.PriorityInfo(5, Assets.GetSprite("icon_priority_automatic"), UI.JOBSSCREEN.PRIORITY.VERYHIGH)
				};
			}
			return JobsTableScreen._priorityInfo;
		}
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x00211FBC File Offset: 0x002101BC
	protected override void OnActivate()
	{
		this.title = UI.JOBSSCREEN.TITLE;
		base.OnActivate();
		this.resetSettingsButton.onClick += this.OnResetSettingsClicked;
		this.prioritySprites = new List<Sprite>();
		foreach (JobsTableScreen.PriorityInfo priorityInfo in JobsTableScreen.priorityInfo)
		{
			this.prioritySprites.Add(priorityInfo.sprite);
		}
		base.AddPortraitColumn("Portrait", new Action<IAssignableIdentity, GameObject>(base.on_load_portrait), null, true);
		base.AddButtonLabelColumn("Names", new Action<IAssignableIdentity, GameObject>(this.ConfigureNameLabel), new Func<IAssignableIdentity, GameObject, string>(base.get_value_name_label), delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectMinion();
		}, delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectAndFocusMinion();
		}, new Comparison<IAssignableIdentity>(base.compare_rows_alphabetical), null, new Action<IAssignableIdentity, GameObject, ToolTip>(base.on_tooltip_sort_alphabetically), false);
		List<ChoreGroup> list = new List<ChoreGroup>(Db.Get().ChoreGroups.resources);
		from @group in list
		orderby @group.DefaultPersonalPriority descending, @group.Name
		select @group;
		foreach (ChoreGroup choreGroup in list)
		{
			if (choreGroup.userPrioritizable)
			{
				PrioritizationGroupTableColumn new_column = new PrioritizationGroupTableColumn(choreGroup, new Action<IAssignableIdentity, GameObject>(this.LoadValue), new Action<object, int>(this.ChangePersonalPriority), new Func<object, string>(this.HoverPersonalPriority), new Action<object, int>(this.ChangeColumnPriority), new Func<object, string>(this.HoverChangeColumnPriorityButton), new Action<object>(this.OnSortClicked), new Func<object, string>(this.OnSortHovered));
				base.RegisterColumn(choreGroup.Id, new_column);
			}
		}
		PrioritizeRowTableColumn new_column2 = new PrioritizeRowTableColumn(null, new Action<object, int>(this.ChangeRowPriority), new Func<object, int, string>(this.HoverChangeRowPriorityButton));
		base.RegisterColumn("prioritize_row", new_column2);
		this.settingsButton.onClick += this.OnSettingsButtonClicked;
		this.resetSettingsButton.onClick += this.OnResetSettingsClicked;
		this.toggleAdvancedModeButton.onClick += this.OnAdvancedModeToggleClicked;
		this.toggleAdvancedModeButton.fgImage.gameObject.SetActive(Game.Instance.advancedPersonalPriorities);
		this.RefreshEffectListeners();
	}

	// Token: 0x06005A5F RID: 23135 RVA: 0x00212264 File Offset: 0x00210464
	private string HoverPersonalPriority(object widget_go_obj)
	{
		GameObject gameObject = widget_go_obj as GameObject;
		ChoreGroup choreGroup = (base.GetWidgetColumn(gameObject) as PrioritizationGroupTableColumn).userData as ChoreGroup;
		string text = null;
		TableRow widgetRow = base.GetWidgetRow(gameObject);
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
		{
			string text2 = UI.JOBSSCREEN.HEADER_TOOLTIP.ToString();
			text2 = text2.Replace("{Job}", choreGroup.Name);
			string text3 = UI.JOBSSCREEN.HEADER_DETAILS_TOOLTIP.ToString();
			text3 = text3.Replace("{Description}", choreGroup.description);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ChoreType choreType in choreGroup.choreTypes)
			{
				hashSet.Add(choreType.Name);
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in hashSet)
			{
				stringBuilder.Append(value);
				if (num < hashSet.Count - 1)
				{
					stringBuilder.Append(", ");
				}
				num++;
			}
			text3 = text3.Replace("{ChoreList}", stringBuilder.ToString());
			text2 = text2.Replace("{Details}", text3);
			return text2;
		}
		case TableRow.RowType.Default:
			text = UI.JOBSSCREEN.NEW_MINION_ITEM_TOOLTIP.ToString();
			break;
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			text = UI.JOBSSCREEN.ITEM_TOOLTIP.ToString();
			text = text.Replace("{Name}", widgetRow.name);
			break;
		}
		ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
		IAssignableIdentity identity = widgetRow.GetIdentity();
		MinionIdentity minionIdentity = identity as MinionIdentity;
		if (minionIdentity != null)
		{
			IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
			int personalPriority = priorityManager.GetPersonalPriority(choreGroup);
			string newValue = this.GetPriorityStr(personalPriority);
			string priorityValue = this.GetPriorityValue(personalPriority);
			if (priorityManager.IsChoreGroupDisabled(choreGroup))
			{
				Trait trait;
				minionIdentity.GetComponent<Traits>().IsChoreGroupDisabled(choreGroup, out trait);
				text = UI.JOBSSCREEN.TRAIT_DISABLED.ToString();
				text = text.Replace("{Name}", minionIdentity.GetProperName());
				text = text.Replace("{Job}", choreGroup.Name);
				text = text.Replace("{Trait}", trait.Name);
				componentInChildren.ClearMultiStringTooltip();
				componentInChildren.AddMultiStringTooltip(text, null);
			}
			else
			{
				text = text.Replace("{Job}", choreGroup.Name);
				text = text.Replace("{Priority}", newValue);
				text = text.Replace("{PriorityValue}", priorityValue);
				componentInChildren.ClearMultiStringTooltip();
				componentInChildren.AddMultiStringTooltip(text, null);
				if (minionIdentity != null)
				{
					text = "\n" + UI.JOBSSCREEN.MINION_SKILL_TOOLTIP.ToString();
					text = text.Replace("{Name}", minionIdentity.GetProperName());
					text = text.Replace("{Attribute}", choreGroup.attribute.Name);
					float totalValue = minionIdentity.GetAttributes().Get(choreGroup.attribute).GetTotalValue();
					TextStyleSetting tooltipTextStyle_Ability = this.TooltipTextStyle_Ability;
					text += GameUtil.ColourizeString(tooltipTextStyle_Ability.textColor, totalValue.ToString());
					componentInChildren.AddMultiStringTooltip(text, null);
				}
				componentInChildren.AddMultiStringTooltip(UI.HORIZONTAL_RULE + "\n" + this.GetUsageString(), null);
			}
		}
		else if (identity as StoredMinionIdentity != null)
		{
			componentInChildren.AddMultiStringTooltip(string.Format(UI.JOBSSCREEN.CANNOT_ADJUST_PRIORITY, identity.GetProperName(), (identity as StoredMinionIdentity).GetStorageReason()), null);
		}
		return "";
	}

	// Token: 0x06005A60 RID: 23136 RVA: 0x00212604 File Offset: 0x00210804
	private string HoverChangeColumnPriorityButton(object widget_go_obj)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		ChoreGroup choreGroup = (base.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
		return UI.JOBSSCREEN.HEADER_CHANGE_TOOLTIP.ToString().Replace("{Job}", choreGroup.Name);
	}

	// Token: 0x06005A61 RID: 23137 RVA: 0x00212649 File Offset: 0x00210849
	private string GetUsageString()
	{
		return GameUtil.ReplaceHotkeyString(UI.JOBSSCREEN.INCREASE_PRIORITY_TUTORIAL, global::Action.MouseLeft) + "\n" + GameUtil.ReplaceHotkeyString(UI.JOBSSCREEN.DECREASE_PRIORITY_TUTORIAL, global::Action.MouseRight);
	}

	// Token: 0x06005A62 RID: 23138 RVA: 0x00212678 File Offset: 0x00210878
	private string HoverChangeRowPriorityButton(object widget_go_obj, int delta)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		LocString locString = null;
		LocString locString2 = null;
		string text = null;
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		switch (widgetRow.rowType)
		{
		case TableRow.RowType.Header:
			global::Debug.Assert(false);
			return null;
		case TableRow.RowType.Default:
			locString = UI.JOBSSCREEN.INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP;
			locString2 = UI.JOBSSCREEN.DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP;
			break;
		case TableRow.RowType.Minion:
			locString = UI.JOBSSCREEN.INCREASE_ROW_PRIORITY_MINION_TOOLTIP;
			locString2 = UI.JOBSSCREEN.DECREASE_ROW_PRIORITY_MINION_TOOLTIP;
			text = widgetRow.GetIdentity().GetProperName();
			break;
		case TableRow.RowType.StoredMinon:
		{
			StoredMinionIdentity storedMinionIdentity = widgetRow.GetIdentity() as StoredMinionIdentity;
			if (storedMinionIdentity != null)
			{
				return string.Format(UI.JOBSSCREEN.CANNOT_ADJUST_PRIORITY, storedMinionIdentity.GetProperName(), storedMinionIdentity.GetStorageReason());
			}
			break;
		}
		}
		string text2 = ((delta > 0) ? locString : locString2).ToString();
		if (text != null)
		{
			text2 = text2.Replace("{Name}", text);
		}
		return text2;
	}

	// Token: 0x06005A63 RID: 23139 RVA: 0x00212748 File Offset: 0x00210948
	private void OnSortClicked(object widget_go_obj)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		PrioritizationGroupTableColumn prioritizationGroupTableColumn = base.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn;
		ChoreGroup chore_group = prioritizationGroupTableColumn.userData as ChoreGroup;
		if (this.active_sort_column == prioritizationGroupTableColumn)
		{
			this.sort_is_reversed = !this.sort_is_reversed;
		}
		this.active_sort_column = prioritizationGroupTableColumn;
		this.active_sort_method = delegate(IAssignableIdentity a, IAssignableIdentity b)
		{
			MinionIdentity minionIdentity = a as MinionIdentity;
			MinionIdentity minionIdentity2 = b as MinionIdentity;
			if (minionIdentity == null && minionIdentity2 == null)
			{
				return 0;
			}
			if (minionIdentity == null)
			{
				return -1;
			}
			if (minionIdentity2 == null)
			{
				return 1;
			}
			ChoreConsumer component = minionIdentity.GetComponent<ChoreConsumer>();
			ChoreConsumer component2 = minionIdentity2.GetComponent<ChoreConsumer>();
			if (component.IsChoreGroupDisabled(chore_group))
			{
				return 1;
			}
			if (component2.IsChoreGroupDisabled(chore_group))
			{
				return -1;
			}
			int personalPriority = component.GetPersonalPriority(chore_group);
			int personalPriority2 = component2.GetPersonalPriority(chore_group);
			if (personalPriority == personalPriority2)
			{
				return minionIdentity.name.CompareTo(minionIdentity2.name);
			}
			return personalPriority2 - personalPriority;
		};
		base.SortRows();
	}

	// Token: 0x06005A64 RID: 23140 RVA: 0x002127B8 File Offset: 0x002109B8
	private string OnSortHovered(object widget_go_obj)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		ChoreGroup choreGroup = (base.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
		return UI.JOBSSCREEN.SORT_TOOLTIP.ToString().Replace("{Job}", choreGroup.Name);
	}

	// Token: 0x06005A65 RID: 23141 RVA: 0x00212800 File Offset: 0x00210A00
	private IPersonalPriorityManager GetPriorityManager(TableRow row)
	{
		IPersonalPriorityManager result = null;
		switch (row.rowType)
		{
		case TableRow.RowType.Default:
			result = Immigration.Instance;
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = row.GetIdentity() as MinionIdentity;
			if (minionIdentity != null)
			{
				result = minionIdentity.GetComponent<ChoreConsumer>();
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			result = (row.GetIdentity() as StoredMinionIdentity);
			break;
		}
		return result;
	}

	// Token: 0x06005A66 RID: 23142 RVA: 0x00212860 File Offset: 0x00210A60
	private LocString GetPriorityStr(int priority)
	{
		priority = Mathf.Clamp(priority, 0, 5);
		LocString result = null;
		foreach (JobsTableScreen.PriorityInfo priorityInfo in JobsTableScreen.priorityInfo)
		{
			if (priorityInfo.priority == priority)
			{
				result = priorityInfo.name;
			}
		}
		return result;
	}

	// Token: 0x06005A67 RID: 23143 RVA: 0x002128C8 File Offset: 0x00210AC8
	private string GetPriorityValue(int priority)
	{
		return (priority * 10).ToString();
	}

	// Token: 0x06005A68 RID: 23144 RVA: 0x002128E4 File Offset: 0x00210AE4
	private void LoadValue(IAssignableIdentity minion, GameObject widget_go)
	{
		if (widget_go == null)
		{
			return;
		}
		ChoreGroup choreGroup = (base.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		TableRow.RowType rowType = widgetRow.rowType;
		if (rowType != TableRow.RowType.Header)
		{
			if (rowType - TableRow.RowType.Default <= 2)
			{
				bool flag = this.GetPriorityManager(widgetRow).IsChoreGroupDisabled(choreGroup);
				HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
				(component.GetReference("FG") as KImage).raycastTarget = flag;
				(component.GetReference("FGToolTip") as ToolTip).enabled = flag;
			}
		}
		else
		{
			this.InitializeHeader(choreGroup, widget_go);
		}
		IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
		if (priorityManager != null)
		{
			this.UpdateWidget(widget_go, choreGroup, priorityManager);
		}
	}

	// Token: 0x06005A69 RID: 23145 RVA: 0x0021298C File Offset: 0x00210B8C
	private JobsTableScreen.PriorityInfo GetPriorityInfo(int priority)
	{
		JobsTableScreen.PriorityInfo result = default(JobsTableScreen.PriorityInfo);
		for (int i = 0; i < JobsTableScreen.priorityInfo.Count; i++)
		{
			if (JobsTableScreen.priorityInfo[i].priority == priority)
			{
				result = JobsTableScreen.priorityInfo[i];
				break;
			}
		}
		return result;
	}

	// Token: 0x06005A6A RID: 23146 RVA: 0x002129D8 File Offset: 0x00210BD8
	private void ChangePersonalPriority(object widget_go_obj, int delta)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		if (widget_go_obj == null)
		{
			return;
		}
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Header)
		{
			global::Debug.Assert(false);
		}
		ChoreGroup chore_group = (base.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
		IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
		this.ChangePersonalPriority(priorityManager, chore_group, delta, true);
		this.UpdateWidget(widget_go, chore_group, priorityManager);
	}

	// Token: 0x06005A6B RID: 23147 RVA: 0x00212A38 File Offset: 0x00210C38
	private void ChangeColumnPriority(object widget_go_obj, int new_priority)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		if (widget_go_obj == null)
		{
			return;
		}
		if (base.GetWidgetRow(widget_go).rowType != TableRow.RowType.Header)
		{
			global::Debug.Assert(false);
		}
		PrioritizationGroupTableColumn prioritizationGroupTableColumn = base.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn;
		ChoreGroup choreGroup = prioritizationGroupTableColumn.userData as ChoreGroup;
		foreach (TableRow tableRow in this.rows)
		{
			IPersonalPriorityManager priorityManager = this.GetPriorityManager(tableRow);
			if (priorityManager != null)
			{
				priorityManager.SetPersonalPriority(choreGroup, new_priority);
				GameObject widget = tableRow.GetWidget(prioritizationGroupTableColumn);
				this.UpdateWidget(widget, choreGroup, priorityManager);
			}
		}
	}

	// Token: 0x06005A6C RID: 23148 RVA: 0x00212AEC File Offset: 0x00210CEC
	private void ChangeRowPriority(object widget_go_obj, int delta)
	{
		GameObject widget_go = widget_go_obj as GameObject;
		if (widget_go_obj == null)
		{
			return;
		}
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Header)
		{
			global::Debug.Assert(false);
			return;
		}
		IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
		foreach (TableColumn tableColumn in this.columns.Values)
		{
			PrioritizationGroupTableColumn prioritizationGroupTableColumn = tableColumn as PrioritizationGroupTableColumn;
			if (prioritizationGroupTableColumn != null)
			{
				ChoreGroup chore_group = prioritizationGroupTableColumn.userData as ChoreGroup;
				GameObject widget = widgetRow.GetWidget(prioritizationGroupTableColumn);
				this.ChangePersonalPriority(priorityManager, chore_group, delta, false);
				this.UpdateWidget(widget, chore_group, priorityManager);
			}
		}
	}

	// Token: 0x06005A6D RID: 23149 RVA: 0x00212BA0 File Offset: 0x00210DA0
	private void ChangePersonalPriority(IPersonalPriorityManager priority_mgr, ChoreGroup chore_group, int delta, bool wrap_around)
	{
		if (priority_mgr.IsChoreGroupDisabled(chore_group))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		int num = priority_mgr.GetPersonalPriority(chore_group);
		num += delta;
		if (wrap_around)
		{
			num %= 6;
			if (num < 0)
			{
				num += 6;
			}
		}
		num = Mathf.Clamp(num, 0, 5);
		priority_mgr.SetPersonalPriority(chore_group, num);
		if (delta > 0)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
	}

	// Token: 0x06005A6E RID: 23150 RVA: 0x00212C1C File Offset: 0x00210E1C
	private void UpdateWidget(GameObject widget_go, ChoreGroup chore_group, IPersonalPriorityManager priority_mgr)
	{
		int fgIndex = 0;
		int num = 0;
		bool flag = priority_mgr.IsChoreGroupDisabled(chore_group);
		if (!flag)
		{
			num = priority_mgr.GetPersonalPriority(chore_group);
		}
		num = Mathf.Clamp(num, 0, 5);
		for (int i = 0; i < JobsTableScreen.priorityInfo.Count - 1; i++)
		{
			if (JobsTableScreen.priorityInfo[i].priority == num)
			{
				fgIndex = i;
				break;
			}
		}
		OptionSelector component = widget_go.GetComponent<OptionSelector>();
		int num2 = (priority_mgr != null) ? priority_mgr.GetAssociatedSkillLevel(chore_group) : 0;
		Color32 fillColour = GlobalAssets.Instance.colorSet.PrioritiesNeutralColor;
		if (num2 > 0)
		{
			float num3 = (float)(num2 - this.skillLevelLow);
			num3 /= (float)(this.skillLevelHigh - this.skillLevelLow);
			fillColour = Color32.Lerp(GlobalAssets.Instance.colorSet.PrioritiesLowColor, GlobalAssets.Instance.colorSet.PrioritiesHighColor, num3);
		}
		component.ConfigureItem(flag, new OptionSelector.DisplayOptionInfo
		{
			bgOptions = null,
			fgOptions = this.prioritySprites,
			bgIndex = 0,
			fgIndex = fgIndex,
			fillColour = fillColour
		});
		ToolTip componentInChildren = widget_go.transform.GetComponentInChildren<ToolTip>();
		if (componentInChildren != null)
		{
			componentInChildren.toolTip = this.HoverPersonalPriority(widget_go);
			componentInChildren.forceRefresh = true;
		}
	}

	// Token: 0x06005A6F RID: 23151 RVA: 0x00212D4C File Offset: 0x00210F4C
	public void ToggleColumnSortWidgets(bool show)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in this.columns)
		{
			if (keyValuePair.Value.column_sort_toggle != null)
			{
				keyValuePair.Value.column_sort_toggle.gameObject.SetActive(show);
			}
		}
	}

	// Token: 0x06005A70 RID: 23152 RVA: 0x00212DC4 File Offset: 0x00210FC4
	public void Refresh(MinionResume minion_resume)
	{
		if (this == null)
		{
			return;
		}
		foreach (TableRow tableRow in this.rows)
		{
			IAssignableIdentity identity = tableRow.GetIdentity();
			if (!(identity as MinionIdentity == null) && !((identity as MinionIdentity).gameObject != minion_resume.gameObject))
			{
				foreach (TableColumn tableColumn in this.columns.Values)
				{
					PrioritizationGroupTableColumn prioritizationGroupTableColumn = tableColumn as PrioritizationGroupTableColumn;
					if (prioritizationGroupTableColumn != null)
					{
						GameObject widget = tableRow.GetWidget(prioritizationGroupTableColumn);
						this.UpdateWidget(widget, prioritizationGroupTableColumn.userData as ChoreGroup, (identity as MinionIdentity).GetComponent<ChoreConsumer>());
					}
				}
			}
		}
	}

	// Token: 0x06005A71 RID: 23153 RVA: 0x00212EC0 File Offset: 0x002110C0
	protected override void RefreshRows()
	{
		base.RefreshRows();
		this.RefreshEffectListeners();
		if (this.dynamicRowSpacing)
		{
			this.SizeRows();
		}
	}

	// Token: 0x06005A72 RID: 23154 RVA: 0x00212EDC File Offset: 0x002110DC
	private void SizeRows()
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < this.header_row.transform.childCount; i++)
		{
			Transform child = this.header_row.transform.GetChild(i);
			LayoutElement component = child.GetComponent<LayoutElement>();
			if (component != null && !component.ignoreLayout)
			{
				num2++;
				num += component.minWidth;
			}
			else
			{
				HorizontalOrVerticalLayoutGroup component2 = child.GetComponent<HorizontalOrVerticalLayoutGroup>();
				if (component2 != null)
				{
					float x = component2.rectTransform().sizeDelta.x;
					num += x;
					num2++;
				}
			}
		}
		float width = base.gameObject.rectTransform().rect.width;
		float spacing = 0f;
		HorizontalLayoutGroup component3 = this.header_row.GetComponent<HorizontalLayoutGroup>();
		component3.spacing = spacing;
		component3.childAlignment = TextAnchor.MiddleLeft;
		foreach (TableRow tableRow in this.rows)
		{
			tableRow.transform.GetComponentInChildren<HorizontalLayoutGroup>().spacing = spacing;
		}
	}

	// Token: 0x06005A73 RID: 23155 RVA: 0x0021300C File Offset: 0x0021120C
	private void RefreshEffectListeners()
	{
		for (int i = 0; i < this.EffectListeners.Count; i++)
		{
			this.EffectListeners[i].Key.Unsubscribe(this.EffectListeners[i].Value.level_up);
			this.EffectListeners[i].Key.Unsubscribe(this.EffectListeners[i].Value.effect_added);
			this.EffectListeners[i].Key.Unsubscribe(this.EffectListeners[i].Value.effect_removed);
			this.EffectListeners[i].Key.Unsubscribe(this.EffectListeners[i].Value.disease_added);
			this.EffectListeners[i].Key.Unsubscribe(this.EffectListeners[i].Value.effect_added);
		}
		this.EffectListeners.Clear();
		for (int j = 0; j < Components.LiveMinionIdentities.Count; j++)
		{
			JobsTableScreen.SkillEventHandlerID skillEventHandlerID = default(JobsTableScreen.SkillEventHandlerID);
			MinionIdentity id = Components.LiveMinionIdentities[j];
			Action<object> handler = delegate(object o)
			{
				this.MarkSingleMinionRowDirty(id);
			};
			skillEventHandlerID.level_up = Components.LiveMinionIdentities[j].gameObject.Subscribe(-110704193, handler);
			skillEventHandlerID.effect_added = Components.LiveMinionIdentities[j].gameObject.Subscribe(-1901442097, handler);
			skillEventHandlerID.effect_removed = Components.LiveMinionIdentities[j].gameObject.Subscribe(-1157678353, handler);
			skillEventHandlerID.disease_added = Components.LiveMinionIdentities[j].gameObject.Subscribe(1592732331, handler);
			skillEventHandlerID.disease_cured = Components.LiveMinionIdentities[j].gameObject.Subscribe(77635178, handler);
		}
		for (int k = 0; k < Components.LiveMinionIdentities.Count; k++)
		{
			MinionIdentity id = Components.LiveMinionIdentities[k];
			Components.LiveMinionIdentities[k].gameObject.Subscribe(540773776, delegate(object new_role)
			{
				this.MarkSingleMinionRowDirty(id);
			});
		}
	}

	// Token: 0x06005A74 RID: 23156 RVA: 0x0021329C File Offset: 0x0021149C
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.dirty_single_minion_rows.Count != 0)
		{
			foreach (MinionIdentity minionIdentity in this.dirty_single_minion_rows)
			{
				if (!(minionIdentity == null))
				{
					this.RefreshSingleMinionRow(minionIdentity);
				}
			}
			this.dirty_single_minion_rows.Clear();
		}
	}

	// Token: 0x06005A75 RID: 23157 RVA: 0x00213318 File Offset: 0x00211518
	protected void MarkSingleMinionRowDirty(MinionIdentity id)
	{
		this.dirty_single_minion_rows.Add(id);
	}

	// Token: 0x06005A76 RID: 23158 RVA: 0x00213328 File Offset: 0x00211528
	private void RefreshSingleMinionRow(IAssignableIdentity id)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in this.columns)
		{
			if (keyValuePair.Value != null && keyValuePair.Value.on_load_action != null)
			{
				foreach (KeyValuePair<TableRow, GameObject> keyValuePair2 in keyValuePair.Value.widgets_by_row)
				{
					if (!(keyValuePair2.Value == null) && keyValuePair2.Key.GetIdentity() == id)
					{
						keyValuePair.Value.on_load_action(id, keyValuePair2.Value);
					}
				}
				keyValuePair.Value.on_load_action(null, this.rows[0].GetWidget(keyValuePair.Value));
			}
		}
	}

	// Token: 0x06005A77 RID: 23159 RVA: 0x0021343C File Offset: 0x0021163C
	protected override void OnCmpDisable()
	{
		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
		base.OnCmpDisable();
		foreach (TableColumn column in this.columns.Values)
		{
			foreach (TableRow tableRow in this.rows)
			{
				GameObject widget = tableRow.GetWidget(column);
				if (!(widget == null))
				{
					GroupSelectorWidget[] componentsInChildren = widget.GetComponentsInChildren<GroupSelectorWidget>();
					if (componentsInChildren != null)
					{
						GroupSelectorWidget[] array = componentsInChildren;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].CloseSubPanel();
						}
					}
					GroupSelectorHeaderWidget[] componentsInChildren2 = widget.GetComponentsInChildren<GroupSelectorHeaderWidget>();
					if (componentsInChildren2 != null)
					{
						GroupSelectorHeaderWidget[] array2 = componentsInChildren2;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].CloseSubPanel();
						}
					}
					SelectablePanel[] componentsInChildren3 = widget.GetComponentsInChildren<SelectablePanel>();
					if (componentsInChildren3 != null)
					{
						SelectablePanel[] array3 = componentsInChildren3;
						for (int i = 0; i < array3.Length; i++)
						{
							array3[i].gameObject.SetActive(false);
						}
					}
				}
			}
		}
		this.optionsPanel.gameObject.SetActive(false);
	}

	// Token: 0x06005A78 RID: 23160 RVA: 0x00213598 File Offset: 0x00211798
	private void GetMouseHoverInfo(out bool is_hovering_screen, out bool is_hovering_button)
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null)
		{
			is_hovering_button = false;
			is_hovering_screen = false;
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		current.RaycastAll(new PointerEventData(current)
		{
			position = KInputManager.GetMousePos()
		}, list);
		bool flag = false;
		bool flag2 = false;
		foreach (RaycastResult raycastResult in list)
		{
			if (raycastResult.gameObject.GetComponent<OptionSelector>() != null || (raycastResult.gameObject.transform.parent != null && raycastResult.gameObject.transform.parent.GetComponent<OptionSelector>() != null))
			{
				flag = true;
				flag2 = true;
				break;
			}
			if (this.HasParent(raycastResult.gameObject, base.gameObject))
			{
				flag2 = true;
			}
		}
		is_hovering_screen = flag2;
		is_hovering_button = flag;
	}

	// Token: 0x06005A79 RID: 23161 RVA: 0x00213694 File Offset: 0x00211894
	public override void OnKeyDown(KButtonEvent e)
	{
		bool flag = false;
		if (e.IsAction(global::Action.MouseRight))
		{
			bool flag2;
			bool flag3;
			this.GetMouseHoverInfo(out flag2, out flag3);
			if (flag3)
			{
				flag = true;
				if (!e.Consumed)
				{
					e.TryConsume(global::Action.MouseRight);
				}
			}
		}
		if (!flag)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x002136D8 File Offset: 0x002118D8
	public override void OnKeyUp(KButtonEvent e)
	{
		bool flag = false;
		if (e.IsAction(global::Action.MouseRight))
		{
			bool flag2;
			bool flag3;
			this.GetMouseHoverInfo(out flag2, out flag3);
			if (flag3)
			{
				e.TryConsume(global::Action.MouseRight);
				flag = true;
			}
		}
		if (!flag)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x00213714 File Offset: 0x00211914
	private bool HasParent(GameObject obj, GameObject parent)
	{
		bool result = false;
		Transform transform = parent.transform;
		Transform transform2 = obj.transform;
		while (transform2 != null)
		{
			if (transform2 == transform)
			{
				result = true;
				break;
			}
			transform2 = transform2.parent;
		}
		return result;
	}

	// Token: 0x06005A7C RID: 23164 RVA: 0x00213754 File Offset: 0x00211954
	private void ConfigureNameLabel(IAssignableIdentity identity, GameObject widget_go)
	{
		base.on_load_name_label(identity, widget_go);
		if (identity == null)
		{
			return;
		}
		string result = "";
		ToolTip component = widget_go.GetComponent<ToolTip>();
		if (component != null)
		{
			ToolTip toolTip = component;
			toolTip.OnToolTip = (Func<string>)Delegate.Combine(toolTip.OnToolTip, new Func<string>(delegate()
			{
				MinionIdentity minionIdentity = identity as MinionIdentity;
				if (minionIdentity != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("<b>" + UI.DETAILTABS.STATS.NAME + "</b>");
					foreach (AttributeInstance attributeInstance in minionIdentity.GetAttributes())
					{
						if (attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
						{
							string text = UIConstants.ColorPrefixWhite;
							if (attributeInstance.GetTotalValue() > 0f)
							{
								text = UIConstants.ColorPrefixGreen;
							}
							else if (attributeInstance.GetTotalValue() < 0f)
							{
								text = UIConstants.ColorPrefixRed;
							}
							stringBuilder.Append(string.Concat(new string[]
							{
								"\n    • ",
								attributeInstance.Name,
								": ",
								text,
								attributeInstance.GetTotalValue().ToString(),
								UIConstants.ColorSuffix
							}));
						}
					}
					result = stringBuilder.ToString();
				}
				else if (identity as StoredMinionIdentity != null)
				{
					result = string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (identity as StoredMinionIdentity).GetStorageReason(), identity.GetProperName());
				}
				return result;
			}));
		}
	}

	// Token: 0x06005A7D RID: 23165 RVA: 0x002137C4 File Offset: 0x002119C4
	private void InitializeHeader(ChoreGroup chore_group, GameObject widget_go)
	{
		HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
		HierarchyReferences hierarchyReferences = component.GetReference("PrioritizationWidget") as HierarchyReferences;
		GameObject items_root = hierarchyReferences.GetReference("ItemPanel").gameObject;
		if (items_root.transform.childCount > 0)
		{
			return;
		}
		items_root.SetActive(false);
		(component.GetReference("Label") as LocText).text = chore_group.Name;
		KButton kbutton = component.GetReference("PrioritizeButton") as KButton;
		Selectable selectable = items_root.GetComponent<Selectable>();
		kbutton.onClick += delegate()
		{
			selectable.Select();
			items_root.SetActive(true);
		};
		GameObject gameObject = hierarchyReferences.GetReference("ItemTemplate").gameObject;
		for (int i = 5; i >= 0; i--)
		{
			JobsTableScreen.PriorityInfo priorityInfo = this.GetPriorityInfo(i);
			if (priorityInfo.name != null)
			{
				GameObject gameObject2 = Util.KInstantiateUI(gameObject, items_root, true);
				KButton component2 = gameObject2.GetComponent<KButton>();
				HierarchyReferences component3 = gameObject2.GetComponent<HierarchyReferences>();
				KImage kimage = component3.GetReference("Icon") as KImage;
				TMP_Text tmp_Text = component3.GetReference("Label") as LocText;
				int new_priority = i;
				component2.onClick += delegate()
				{
					this.ChangeColumnPriority(widget_go, new_priority);
					UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
				};
				kimage.sprite = priorityInfo.sprite;
				tmp_Text.text = priorityInfo.name;
			}
		}
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x0021394B File Offset: 0x00211B4B
	private void OnSettingsButtonClicked()
	{
		this.optionsPanel.gameObject.SetActive(true);
		this.optionsPanel.GetComponent<Selectable>().Select();
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x00213970 File Offset: 0x00211B70
	private void OnResetSettingsClicked()
	{
		if (Game.Instance.advancedPersonalPriorities)
		{
			if (Immigration.Instance != null)
			{
				Immigration.Instance.ResetPersonalPriorities();
			}
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity minionIdentity = enumerator.Current;
					if (!(minionIdentity == null))
					{
						Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
					}
				}
				goto IL_101;
			}
		}
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			if (!(minionIdentity2 == null))
			{
				ChoreConsumer component = minionIdentity2.GetComponent<ChoreConsumer>();
				foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
				{
					if (choreGroup.userPrioritizable)
					{
						component.SetPersonalPriority(choreGroup, 3);
					}
				}
			}
		}
		IL_101:
		base.MarkRowsDirty();
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x00213AAC File Offset: 0x00211CAC
	private void OnAdvancedModeToggleClicked()
	{
		Game.Instance.advancedPersonalPriorities = !Game.Instance.advancedPersonalPriorities;
		this.toggleAdvancedModeButton.fgImage.gameObject.SetActive(Game.Instance.advancedPersonalPriorities);
	}

	// Token: 0x04003D2A RID: 15658
	[SerializeField]
	private int skillLevelLow = 1;

	// Token: 0x04003D2B RID: 15659
	[SerializeField]
	private int skillLevelHigh = 10;

	// Token: 0x04003D2C RID: 15660
	[SerializeField]
	private KButton settingsButton;

	// Token: 0x04003D2D RID: 15661
	[SerializeField]
	private KButton resetSettingsButton;

	// Token: 0x04003D2E RID: 15662
	[SerializeField]
	private KButton toggleAdvancedModeButton;

	// Token: 0x04003D2F RID: 15663
	[SerializeField]
	private KImage optionsPanel;

	// Token: 0x04003D30 RID: 15664
	[SerializeField]
	private bool dynamicRowSpacing = true;

	// Token: 0x04003D31 RID: 15665
	public TextStyleSetting TooltipTextStyle_Ability;

	// Token: 0x04003D32 RID: 15666
	public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;

	// Token: 0x04003D33 RID: 15667
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04003D34 RID: 15668
	private HashSet<MinionIdentity> dirty_single_minion_rows = new HashSet<MinionIdentity>();

	// Token: 0x04003D35 RID: 15669
	private static List<JobsTableScreen.PriorityInfo> _priorityInfo;

	// Token: 0x04003D36 RID: 15670
	private List<Sprite> prioritySprites;

	// Token: 0x04003D37 RID: 15671
	private List<KeyValuePair<GameObject, JobsTableScreen.SkillEventHandlerID>> EffectListeners = new List<KeyValuePair<GameObject, JobsTableScreen.SkillEventHandlerID>>();

	// Token: 0x02001A91 RID: 6801
	public struct PriorityInfo
	{
		// Token: 0x06009772 RID: 38770 RVA: 0x0033F48F File Offset: 0x0033D68F
		public PriorityInfo(int priority, Sprite sprite, LocString name)
		{
			this.priority = priority;
			this.sprite = sprite;
			this.name = name;
		}

		// Token: 0x040079DA RID: 31194
		public int priority;

		// Token: 0x040079DB RID: 31195
		public Sprite sprite;

		// Token: 0x040079DC RID: 31196
		public LocString name;
	}

	// Token: 0x02001A92 RID: 6802
	private struct SkillEventHandlerID
	{
		// Token: 0x040079DD RID: 31197
		public int level_up;

		// Token: 0x040079DE RID: 31198
		public int effect_added;

		// Token: 0x040079DF RID: 31199
		public int effect_removed;

		// Token: 0x040079E0 RID: 31200
		public int disease_added;

		// Token: 0x040079E1 RID: 31201
		public int disease_cured;
	}
}
