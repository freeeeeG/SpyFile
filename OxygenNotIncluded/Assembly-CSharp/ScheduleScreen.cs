using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BE5 RID: 3045
public class ScheduleScreen : KScreen
{
	// Token: 0x0600605A RID: 24666 RVA: 0x00239C65 File Offset: 0x00237E65
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x0600605B RID: 24667 RVA: 0x00239C6C File Offset: 0x00237E6C
	protected override void OnPrefabInit()
	{
		base.ConsumeMouseScroll = true;
		this.entries = new List<ScheduleScreenEntry>();
		this.paintStyles = new Dictionary<string, ColorStyleSetting>();
		this.paintStyles["Hygene"] = this.hygene_color;
		this.paintStyles["Worktime"] = this.work_color;
		this.paintStyles["Recreation"] = this.recreation_color;
		this.paintStyles["Sleep"] = this.sleep_color;
	}

	// Token: 0x0600605C RID: 24668 RVA: 0x00239CF0 File Offset: 0x00237EF0
	protected override void OnSpawn()
	{
		this.paintButtons = new List<SchedulePaintButton>();
		foreach (ScheduleGroup group in Db.Get().ScheduleGroups.allGroups)
		{
			this.AddPaintButton(group);
		}
		foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
		{
			this.AddScheduleEntry(schedule);
		}
		this.addScheduleButton.onClick += this.OnAddScheduleClick;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		ScheduleManager.Instance.onSchedulesChanged += this.OnSchedulesChanged;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshWidgetWorldData));
	}

	// Token: 0x0600605D RID: 24669 RVA: 0x00239E10 File Offset: 0x00238010
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		ScheduleManager.Instance.onSchedulesChanged -= this.OnSchedulesChanged;
	}

	// Token: 0x0600605E RID: 24670 RVA: 0x00239E2E File Offset: 0x0023802E
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			base.Activate();
		}
	}

	// Token: 0x0600605F RID: 24671 RVA: 0x00239E40 File Offset: 0x00238040
	private void AddPaintButton(ScheduleGroup group)
	{
		SchedulePaintButton schedulePaintButton = Util.KInstantiateUI<SchedulePaintButton>(this.paintButtonPrefab.gameObject, this.paintButtonContainer, true);
		schedulePaintButton.SetGroup(group, this.paintStyles, new Action<SchedulePaintButton>(this.OnPaintButtonClick));
		schedulePaintButton.SetToggle(false);
		this.paintButtons.Add(schedulePaintButton);
	}

	// Token: 0x06006060 RID: 24672 RVA: 0x00239E91 File Offset: 0x00238091
	private void OnAddScheduleClick()
	{
		ScheduleManager.Instance.AddDefaultSchedule(false);
	}

	// Token: 0x06006061 RID: 24673 RVA: 0x00239EA0 File Offset: 0x002380A0
	private void OnPaintButtonClick(SchedulePaintButton clicked)
	{
		if (this.selectedPaint != clicked)
		{
			foreach (SchedulePaintButton schedulePaintButton in this.paintButtons)
			{
				schedulePaintButton.SetToggle(schedulePaintButton == clicked);
			}
			this.selectedPaint = clicked;
			return;
		}
		clicked.SetToggle(false);
		this.selectedPaint = null;
	}

	// Token: 0x06006062 RID: 24674 RVA: 0x00239F1C File Offset: 0x0023811C
	private void OnPaintDragged(ScheduleScreenEntry entry, float ratio)
	{
		if (this.selectedPaint == null)
		{
			return;
		}
		int idx = Mathf.FloorToInt(ratio * (float)entry.schedule.GetBlocks().Count);
		entry.schedule.SetGroup(idx, this.selectedPaint.group);
	}

	// Token: 0x06006063 RID: 24675 RVA: 0x00239F68 File Offset: 0x00238168
	private void AddScheduleEntry(Schedule schedule)
	{
		ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
		scheduleScreenEntry.Setup(schedule, this.paintStyles, new Action<ScheduleScreenEntry, float>(this.OnPaintDragged));
		this.entries.Add(scheduleScreenEntry);
	}

	// Token: 0x06006064 RID: 24676 RVA: 0x00239FB4 File Offset: 0x002381B4
	private void OnSchedulesChanged(List<Schedule> schedules)
	{
		foreach (ScheduleScreenEntry original in this.entries)
		{
			Util.KDestroyGameObject(original);
		}
		this.entries.Clear();
		foreach (Schedule schedule in schedules)
		{
			this.AddScheduleEntry(schedule);
		}
	}

	// Token: 0x06006065 RID: 24677 RVA: 0x0023A04C File Offset: 0x0023824C
	private void RefreshWidgetWorldData(object data = null)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.entries)
		{
			scheduleScreenEntry.RefreshWidgetWorldData();
		}
	}

	// Token: 0x06006066 RID: 24678 RVA: 0x0023A09C File Offset: 0x0023829C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006067 RID: 24679 RVA: 0x0023A0C0 File Offset: 0x002382C0
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (ScheduleScreenEntry scheduleScreenEntry in this.entries)
				{
					if (currentSelectedGameObject == scheduleScreenEntry.GetNameInputField())
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04004198 RID: 16792
	[SerializeField]
	private SchedulePaintButton paintButtonPrefab;

	// Token: 0x04004199 RID: 16793
	[SerializeField]
	private GameObject paintButtonContainer;

	// Token: 0x0400419A RID: 16794
	[SerializeField]
	private ScheduleScreenEntry scheduleEntryPrefab;

	// Token: 0x0400419B RID: 16795
	[SerializeField]
	private GameObject scheduleEntryContainer;

	// Token: 0x0400419C RID: 16796
	[SerializeField]
	private KButton addScheduleButton;

	// Token: 0x0400419D RID: 16797
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400419E RID: 16798
	[SerializeField]
	private ColorStyleSetting hygene_color;

	// Token: 0x0400419F RID: 16799
	[SerializeField]
	private ColorStyleSetting work_color;

	// Token: 0x040041A0 RID: 16800
	[SerializeField]
	private ColorStyleSetting recreation_color;

	// Token: 0x040041A1 RID: 16801
	[SerializeField]
	private ColorStyleSetting sleep_color;

	// Token: 0x040041A2 RID: 16802
	private Dictionary<string, ColorStyleSetting> paintStyles;

	// Token: 0x040041A3 RID: 16803
	private List<ScheduleScreenEntry> entries;

	// Token: 0x040041A4 RID: 16804
	private List<SchedulePaintButton> paintButtons;

	// Token: 0x040041A5 RID: 16805
	private SchedulePaintButton selectedPaint;
}
