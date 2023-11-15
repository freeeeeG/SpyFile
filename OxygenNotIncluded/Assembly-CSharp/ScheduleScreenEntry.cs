using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BE8 RID: 3048
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleScreenEntry")]
public class ScheduleScreenEntry : KMonoBehaviour
{
	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x0600606E RID: 24686 RVA: 0x0023A189 File Offset: 0x00238389
	// (set) Token: 0x0600606F RID: 24687 RVA: 0x0023A191 File Offset: 0x00238391
	public Schedule schedule { get; private set; }

	// Token: 0x06006070 RID: 24688 RVA: 0x0023A19C File Offset: 0x0023839C
	public void Setup(Schedule schedule, Dictionary<string, ColorStyleSetting> paintStyles, Action<ScheduleScreenEntry, float> onPaintDragged)
	{
		this.schedule = schedule;
		base.gameObject.name = "Schedule_" + schedule.name;
		this.title.SetTitle(schedule.name);
		this.title.OnNameChanged += this.OnNameChanged;
		this.blockButtonContainer.Setup(delegate(float f)
		{
			onPaintDragged(this, f);
		});
		int num = 0;
		this.blockButtons = new List<ScheduleBlockButton>();
		int count = schedule.GetBlocks().Count;
		foreach (ScheduleBlock scheduleBlock in schedule.GetBlocks())
		{
			ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, this.blockButtonContainer.gameObject, true);
			scheduleBlockButton.Setup(num++, paintStyles, count);
			scheduleBlockButton.SetBlockTypes(scheduleBlock.allowed_types);
			this.blockButtons.Add(scheduleBlockButton);
		}
		this.minionWidgets = new List<ScheduleMinionWidget>();
		this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, false);
		this.blankMinionWidget.SetupBlank(schedule);
		this.RebuildMinionWidgets();
		this.RefreshNotes();
		this.RefreshAlarmButton();
		this.optionsButton.onClick += this.OnOptionsClicked;
		HierarchyReferences component = this.optionsPanel.GetComponent<HierarchyReferences>();
		MultiToggle reference = component.GetReference<MultiToggle>("AlarmButton");
		reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(this.OnAlarmClicked));
		component.GetReference<KButton>("ResetButton").onClick += this.OnResetClicked;
		component.GetReference<KButton>("DeleteButton").onClick += this.OnDeleteClicked;
		schedule.onChanged = (Action<Schedule>)Delegate.Combine(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
	}

	// Token: 0x06006071 RID: 24689 RVA: 0x0023A3AC File Offset: 0x002385AC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.schedule != null)
		{
			Schedule schedule = this.schedule;
			schedule.onChanged = (Action<Schedule>)Delegate.Remove(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		}
	}

	// Token: 0x06006072 RID: 24690 RVA: 0x0023A3E3 File Offset: 0x002385E3
	public GameObject GetNameInputField()
	{
		return this.title.inputField.gameObject;
	}

	// Token: 0x06006073 RID: 24691 RVA: 0x0023A3F8 File Offset: 0x002385F8
	private void RebuildMinionWidgets()
	{
		if (!this.MinionWidgetsNeedRebuild())
		{
			return;
		}
		foreach (ScheduleMinionWidget original in this.minionWidgets)
		{
			Util.KDestroyGameObject(original);
		}
		this.minionWidgets.Clear();
		foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
		{
			ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
			scheduleMinionWidget.Setup(@ref.Get());
			this.minionWidgets.Add(scheduleMinionWidget);
		}
		if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
		{
			this.blankMinionWidget.transform.SetAsLastSibling();
			this.blankMinionWidget.gameObject.SetActive(true);
			return;
		}
		this.blankMinionWidget.gameObject.SetActive(false);
	}

	// Token: 0x06006074 RID: 24692 RVA: 0x0023A51C File Offset: 0x0023871C
	private bool MinionWidgetsNeedRebuild()
	{
		List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
		if (assigned.Count != this.minionWidgets.Count)
		{
			return true;
		}
		if (assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
		{
			return true;
		}
		for (int i = 0; i < assigned.Count; i++)
		{
			if (assigned[i].Get() != this.minionWidgets[i].schedulable)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006075 RID: 24693 RVA: 0x0023A5AC File Offset: 0x002387AC
	public void RefreshWidgetWorldData()
	{
		foreach (ScheduleMinionWidget scheduleMinionWidget in this.minionWidgets)
		{
			if (!scheduleMinionWidget.IsNullOrDestroyed())
			{
				scheduleMinionWidget.RefreshWidgetWorldData();
			}
		}
	}

	// Token: 0x06006076 RID: 24694 RVA: 0x0023A608 File Offset: 0x00238808
	private void OnNameChanged(string newName)
	{
		this.schedule.name = newName;
		base.gameObject.name = "Schedule_" + this.schedule.name;
	}

	// Token: 0x06006077 RID: 24695 RVA: 0x0023A636 File Offset: 0x00238836
	private void OnOptionsClicked()
	{
		this.optionsPanel.gameObject.SetActive(!this.optionsPanel.gameObject.activeSelf);
		this.optionsPanel.GetComponent<Selectable>().Select();
	}

	// Token: 0x06006078 RID: 24696 RVA: 0x0023A66B File Offset: 0x0023886B
	private void OnAlarmClicked()
	{
		this.schedule.alarmActivated = !this.schedule.alarmActivated;
		this.RefreshAlarmButton();
	}

	// Token: 0x06006079 RID: 24697 RVA: 0x0023A68C File Offset: 0x0023888C
	private void RefreshAlarmButton()
	{
		MultiToggle reference = this.optionsPanel.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("AlarmButton");
		reference.ChangeState(this.schedule.alarmActivated ? 1 : 0);
		ToolTip component = reference.GetComponent<ToolTip>();
		component.SetSimpleTooltip(this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP : UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP);
		ToolTipScreen.Instance.MarkTooltipDirty(component);
		this.alarmField.text = (this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED : UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED);
	}

	// Token: 0x0600607A RID: 24698 RVA: 0x0023A723 File Offset: 0x00238923
	private void OnResetClicked()
	{
		this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
	}

	// Token: 0x0600607B RID: 24699 RVA: 0x0023A73F File Offset: 0x0023893F
	private void OnDeleteClicked()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x0600607C RID: 24700 RVA: 0x0023A754 File Offset: 0x00238954
	private void OnScheduleChanged(Schedule changedSchedule)
	{
		foreach (ScheduleBlockButton scheduleBlockButton in this.blockButtons)
		{
			scheduleBlockButton.SetBlockTypes(changedSchedule.GetBlock(scheduleBlockButton.idx).allowed_types);
		}
		this.RefreshNotes();
		this.RebuildMinionWidgets();
	}

	// Token: 0x0600607D RID: 24701 RVA: 0x0023A7C4 File Offset: 0x002389C4
	private void RefreshNotes()
	{
		this.blockTypeCounts.Clear();
		foreach (ScheduleBlockType scheduleBlockType in Db.Get().ScheduleBlockTypes.resources)
		{
			this.blockTypeCounts[scheduleBlockType.Id] = 0;
		}
		foreach (ScheduleBlock scheduleBlock in this.schedule.GetBlocks())
		{
			foreach (ScheduleBlockType scheduleBlockType2 in scheduleBlock.allowed_types)
			{
				Dictionary<string, int> dictionary = this.blockTypeCounts;
				string id = scheduleBlockType2.Id;
				int num = dictionary[id];
				dictionary[id] = num + 1;
			}
		}
		if (this.noteEntryRight == null)
		{
			return;
		}
		ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		int num2 = 0;
		foreach (KeyValuePair<string, int> keyValuePair in this.blockTypeCounts)
		{
			if (keyValuePair.Value == 0)
			{
				num2++;
				component.AddMultiStringTooltip(string.Format(UI.SCHEDULEGROUPS.NOTIME, Db.Get().ScheduleBlockTypes.Get(keyValuePair.Key).Name), null);
			}
		}
		if (num2 > 0)
		{
			this.noteEntryRight.text = string.Format(UI.SCHEDULEGROUPS.MISSINGBLOCKS, num2);
		}
		else
		{
			this.noteEntryRight.text = "";
		}
		string breakBonus = QualityOfLifeNeed.GetBreakBonus(this.blockTypeCounts[Db.Get().ScheduleBlockTypes.Recreation.Id]);
		if (breakBonus != null)
		{
			Effect effect = Db.Get().effects.Get(breakBonus);
			if (effect != null)
			{
				foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
				{
					if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
					{
						this.noteEntryLeft.text = string.Format(UI.SCHEDULESCREEN.DOWNTIME_MORALE, attributeModifier.GetFormattedString());
						this.noteEntryLeft.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.SCHEDULESCREEN.SCHEDULE_DOWNTIME_MORALE, attributeModifier.GetFormattedString()));
					}
				}
			}
		}
	}

	// Token: 0x040041A9 RID: 16809
	[SerializeField]
	private ScheduleBlockButton blockButtonPrefab;

	// Token: 0x040041AA RID: 16810
	[SerializeField]
	private ScheduleBlockPainter blockButtonContainer;

	// Token: 0x040041AB RID: 16811
	[SerializeField]
	private ScheduleMinionWidget minionWidgetPrefab;

	// Token: 0x040041AC RID: 16812
	[SerializeField]
	private GameObject minionWidgetContainer;

	// Token: 0x040041AD RID: 16813
	private ScheduleMinionWidget blankMinionWidget;

	// Token: 0x040041AE RID: 16814
	[SerializeField]
	private EditableTitleBar title;

	// Token: 0x040041AF RID: 16815
	[SerializeField]
	private LocText alarmField;

	// Token: 0x040041B0 RID: 16816
	[SerializeField]
	private KButton optionsButton;

	// Token: 0x040041B1 RID: 16817
	[SerializeField]
	private DialogPanel optionsPanel;

	// Token: 0x040041B2 RID: 16818
	[SerializeField]
	private LocText noteEntryLeft;

	// Token: 0x040041B3 RID: 16819
	[SerializeField]
	private LocText noteEntryRight;

	// Token: 0x040041B4 RID: 16820
	private List<ScheduleBlockButton> blockButtons;

	// Token: 0x040041B5 RID: 16821
	private List<ScheduleMinionWidget> minionWidgets;

	// Token: 0x040041B7 RID: 16823
	private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();
}
