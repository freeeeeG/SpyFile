using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B5D RID: 2909
public class CrewRationsScreen : CrewListScreen<CrewRationsEntry>
{
	// Token: 0x06005A0B RID: 23051 RVA: 0x0020F734 File Offset: 0x0020D934
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closebutton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
	}

	// Token: 0x06005A0C RID: 23052 RVA: 0x0020F766 File Offset: 0x0020D966
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		base.RefreshCrewPortraitContent();
		this.SortByPreviousSelected();
	}

	// Token: 0x06005A0D RID: 23053 RVA: 0x0020F77C File Offset: 0x0020D97C
	private void SortByPreviousSelected()
	{
		if (this.sortToggleGroup == null)
		{
			return;
		}
		if (this.lastSortToggle == null)
		{
			return;
		}
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>() == this.lastSortToggle)
			{
				if (component.columnID == "name")
				{
					base.SortByName(this.lastSortReversed);
				}
				if (component.columnID == "health")
				{
					this.SortByAmount("HitPoints", this.lastSortReversed);
				}
				if (component.columnID == "stress")
				{
					this.SortByAmount("Stress", this.lastSortReversed);
				}
				if (component.columnID == "calories")
				{
					this.SortByAmount("Calories", this.lastSortReversed);
				}
			}
		}
	}

	// Token: 0x06005A0E RID: 23054 RVA: 0x0020F880 File Offset: 0x0020DA80
	protected override void PositionColumnTitles()
	{
		base.PositionColumnTitles();
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component.Sortable)
			{
				Toggle toggle = this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
				toggle.group = this.sortToggleGroup;
				ImageToggleState toggleImage = toggle.GetComponentInChildren<ImageToggleState>(true);
				if (component.columnID == "name")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByName(!toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "health")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("HitPoints", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "stress")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("Stress", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "calories")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("Calories", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
			}
		}
	}

	// Token: 0x06005A0F RID: 23055 RVA: 0x0020F9CC File Offset: 0x0020DBCC
	protected override void SpawnEntries()
	{
		base.SpawnEntries();
		foreach (MinionIdentity identity in Components.LiveMinionIdentities.Items)
		{
			CrewRationsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewRationsEntry>();
			component.Populate(identity);
			this.EntryObjects.Add(component);
		}
		this.SortByPreviousSelected();
	}

	// Token: 0x06005A10 RID: 23056 RVA: 0x0020FA58 File Offset: 0x0020DC58
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		foreach (CrewRationsEntry crewRationsEntry in this.EntryObjects)
		{
			crewRationsEntry.Refresh();
		}
	}

	// Token: 0x06005A11 RID: 23057 RVA: 0x0020FAB0 File Offset: 0x0020DCB0
	private void SortByAmount(string amount_id, bool reverse)
	{
		List<CrewRationsEntry> list = new List<CrewRationsEntry>(this.EntryObjects);
		list.Sort(delegate(CrewRationsEntry a, CrewRationsEntry b)
		{
			float value = a.Identity.GetAmounts().GetValue(amount_id);
			float value2 = b.Identity.GetAmounts().GetValue(amount_id);
			return value.CompareTo(value2);
		});
		base.ReorderEntries(list, reverse);
	}

	// Token: 0x06005A12 RID: 23058 RVA: 0x0020FAF0 File Offset: 0x0020DCF0
	private void ResetSortToggles(Toggle exceptToggle)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			Toggle component = this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
			ImageToggleState componentInChildren = component.GetComponentInChildren<ImageToggleState>(true);
			if (component != exceptToggle)
			{
				componentInChildren.SetDisabled();
			}
		}
	}

	// Token: 0x04003D06 RID: 15622
	[SerializeField]
	private KButton closebutton;
}
