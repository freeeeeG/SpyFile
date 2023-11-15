using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B5A RID: 2906
public class CrewListScreen<EntryType> : KScreen where EntryType : CrewListEntry
{
	// Token: 0x060059E6 RID: 23014 RVA: 0x0020E744 File Offset: 0x0020C944
	protected override void OnActivate()
	{
		base.OnActivate();
		this.ClearEntries();
		this.SpawnEntries();
		this.PositionColumnTitles();
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060059E7 RID: 23015 RVA: 0x0020E773 File Offset: 0x0020C973
	protected override void OnCmpEnable()
	{
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		this.Reconstruct();
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x0020E78C File Offset: 0x0020C98C
	private void ClearEntries()
	{
		for (int i = this.EntryObjects.Count - 1; i > -1; i--)
		{
			Util.KDestroyGameObject(this.EntryObjects[i]);
		}
		this.EntryObjects.Clear();
	}

	// Token: 0x060059E9 RID: 23017 RVA: 0x0020E7D2 File Offset: 0x0020C9D2
	protected void RefreshCrewPortraitContent()
	{
		this.EntryObjects.ForEach(delegate(EntryType eo)
		{
			eo.RefreshCrewPortraitContent();
		});
	}

	// Token: 0x060059EA RID: 23018 RVA: 0x0020E7FE File Offset: 0x0020C9FE
	protected virtual void SpawnEntries()
	{
		if (this.EntryObjects.Count != 0)
		{
			this.ClearEntries();
		}
	}

	// Token: 0x060059EB RID: 23019 RVA: 0x0020E814 File Offset: 0x0020CA14
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		bool flag = false;
		List<MinionIdentity> liveIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		if (this.EntryObjects.Count != liveIdentities.Count || this.EntryObjects.FindAll((EntryType o) => liveIdentities.Contains(o.Identity)).Count != this.EntryObjects.Count)
		{
			flag = true;
		}
		if (flag)
		{
			this.Reconstruct();
		}
		this.UpdateScroll();
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x0020E8A5 File Offset: 0x0020CAA5
	public void Reconstruct()
	{
		this.ClearEntries();
		this.SpawnEntries();
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x0020E8B4 File Offset: 0x0020CAB4
	private void UpdateScroll()
	{
		if (this.PanelScrollbar)
		{
			if (this.EntryObjects.Count <= this.maxEntriesBeforeScroll)
			{
				this.PanelScrollbar.value = Mathf.Lerp(this.PanelScrollbar.value, 1f, 10f);
				this.PanelScrollbar.gameObject.SetActive(false);
				return;
			}
			this.PanelScrollbar.gameObject.SetActive(true);
		}
	}

	// Token: 0x060059EE RID: 23022 RVA: 0x0020E92C File Offset: 0x0020CB2C
	private void SetHeadersActive(bool state)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			this.ColumnTitlesContainer.GetChild(i).gameObject.SetActive(state);
		}
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x0020E968 File Offset: 0x0020CB68
	protected virtual void PositionColumnTitles()
	{
		if (this.ColumnTitlesContainer == null)
		{
			return;
		}
		if (this.EntryObjects.Count <= 0)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		int childCount = this.EntryObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiate(this.Prefab_ColumnTitle, null, null);
				gameObject.name = component.Column_DisplayName;
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				gameObject.transform.SetParent(this.ColumnTitlesContainer);
				componentInChildren.text = (component.StringLookup ? Strings.Get(component.Column_DisplayName) : component.Column_DisplayName);
				gameObject.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.SORTCOLUMN, componentInChildren.text);
				gameObject.rectTransform().anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x, 0f);
				OverviewColumnIdentity overviewColumnIdentity = gameObject.GetComponent<OverviewColumnIdentity>();
				if (overviewColumnIdentity == null)
				{
					overviewColumnIdentity = gameObject.AddComponent<OverviewColumnIdentity>();
				}
				overviewColumnIdentity.Column_DisplayName = component.Column_DisplayName;
				overviewColumnIdentity.columnID = component.columnID;
				overviewColumnIdentity.xPivot = component.xPivot;
				overviewColumnIdentity.Sortable = component.Sortable;
				if (overviewColumnIdentity.Sortable)
				{
					overviewColumnIdentity.GetComponentInChildren<ImageToggleState>(true).gameObject.SetActive(true);
				}
			}
		}
		this.UpdateColumnTitles();
		this.sortToggleGroup = base.gameObject.AddComponent<ToggleGroup>();
		this.sortToggleGroup.allowSwitchOff = true;
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x0020EB2C File Offset: 0x0020CD2C
	protected void SortByName(bool reverse)
	{
		List<EntryType> list = new List<EntryType>(this.EntryObjects);
		list.Sort(delegate(EntryType a, EntryType b)
		{
			string text = a.Identity.GetProperName() + a.gameObject.GetInstanceID().ToString();
			string strB = b.Identity.GetProperName() + b.gameObject.GetInstanceID().ToString();
			return text.CompareTo(strB);
		});
		this.ReorderEntries(list, reverse);
	}

	// Token: 0x060059F1 RID: 23025 RVA: 0x0020EB74 File Offset: 0x0020CD74
	protected void UpdateColumnTitles()
	{
		if (this.EntryObjects.Count <= 0 || !this.EntryObjects[0].gameObject.activeSelf)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			RectTransform rectTransform = this.ColumnTitlesContainer.GetChild(i).rectTransform();
			for (int j = 0; j < this.EntryObjects[0].transform.childCount; j++)
			{
				OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(j).GetComponent<OverviewColumnIdentity>();
				if (component != null && component.Column_DisplayName == rectTransform.name)
				{
					rectTransform.pivot = new Vector2(component.xPivot, rectTransform.pivot.y);
					rectTransform.anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x + this.columnTitleHorizontalOffset, 0f);
					rectTransform.sizeDelta = new Vector2(component.rectTransform().sizeDelta.x, rectTransform.sizeDelta.y);
					if (rectTransform.anchoredPosition.x == 0f)
					{
						rectTransform.gameObject.SetActive(false);
					}
					else
					{
						rectTransform.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x060059F2 RID: 23026 RVA: 0x0020ECF0 File Offset: 0x0020CEF0
	protected void ReorderEntries(List<EntryType> sortedEntries, bool reverse)
	{
		for (int i = 0; i < sortedEntries.Count; i++)
		{
			if (reverse)
			{
				sortedEntries[i].transform.SetSiblingIndex(sortedEntries.Count - 1 - i);
			}
			else
			{
				sortedEntries[i].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x04003CE3 RID: 15587
	public GameObject Prefab_CrewEntry;

	// Token: 0x04003CE4 RID: 15588
	public List<EntryType> EntryObjects = new List<EntryType>();

	// Token: 0x04003CE5 RID: 15589
	public Transform ScrollRectTransform;

	// Token: 0x04003CE6 RID: 15590
	public Transform EntriesPanelTransform;

	// Token: 0x04003CE7 RID: 15591
	protected Vector2 EntryRectSize = new Vector2(750f, 64f);

	// Token: 0x04003CE8 RID: 15592
	public int maxEntriesBeforeScroll = 5;

	// Token: 0x04003CE9 RID: 15593
	public Scrollbar PanelScrollbar;

	// Token: 0x04003CEA RID: 15594
	protected ToggleGroup sortToggleGroup;

	// Token: 0x04003CEB RID: 15595
	protected Toggle lastSortToggle;

	// Token: 0x04003CEC RID: 15596
	protected bool lastSortReversed;

	// Token: 0x04003CED RID: 15597
	public GameObject Prefab_ColumnTitle;

	// Token: 0x04003CEE RID: 15598
	public Transform ColumnTitlesContainer;

	// Token: 0x04003CEF RID: 15599
	public bool autoColumn;

	// Token: 0x04003CF0 RID: 15600
	public float columnTitleHorizontalOffset;
}
