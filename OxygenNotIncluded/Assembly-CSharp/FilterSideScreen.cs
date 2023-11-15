using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1A RID: 3098
public class FilterSideScreen : SideScreenContent
{
	// Token: 0x06006206 RID: 25094 RVA: 0x00242D85 File Offset: 0x00240F85
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006207 RID: 25095 RVA: 0x00242D90 File Offset: 0x00240F90
	public override bool IsValidForTarget(GameObject target)
	{
		bool flag;
		if (this.isLogicFilter)
		{
			flag = (target.GetComponent<ConduitElementSensor>() != null || target.GetComponent<LogicElementSensor>() != null);
		}
		else
		{
			flag = (target.GetComponent<ElementFilter>() != null || target.GetComponent<RocketConduitStorageAccess>() != null || target.GetComponent<DevPump>() != null);
		}
		return flag && target.GetComponent<Filterable>() != null;
	}

	// Token: 0x06006208 RID: 25096 RVA: 0x00242E04 File Offset: 0x00241004
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetFilterable = target.GetComponent<Filterable>();
		if (this.targetFilterable == null)
		{
			return;
		}
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
			goto IL_87;
		case Filterable.ElementState.Gas:
			this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
			goto IL_87;
		}
		this.everythingElseHeaderLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
		IL_87:
		this.Configure(this.targetFilterable);
		this.SetFilterTag(this.targetFilterable.SelectedTag);
	}

	// Token: 0x06006209 RID: 25097 RVA: 0x00242EB8 File Offset: 0x002410B8
	private void ToggleCategory(Tag tag, bool forceOn = false)
	{
		HierarchyReferences hierarchyReferences = this.categoryToggles[tag];
		if (hierarchyReferences != null)
		{
			MultiToggle reference = hierarchyReferences.GetReference<MultiToggle>("Toggle");
			if (!forceOn)
			{
				reference.NextState();
			}
			else
			{
				reference.ChangeState(1);
			}
			hierarchyReferences.GetReference<RectTransform>("Entries").gameObject.SetActive(reference.CurrentState != 0);
		}
	}

	// Token: 0x0600620A RID: 25098 RVA: 0x00242F18 File Offset: 0x00241118
	private void Configure(Filterable filterable)
	{
		Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
		using (Dictionary<Tag, HashSet<Tag>>.Enumerator enumerator = tagOptions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, HashSet<Tag>> category_tags = enumerator.Current;
				if (!this.filterRowMap.ContainsKey(category_tags.Key))
				{
					if (category_tags.Key != GameTags.Void)
					{
						HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.categoryFoldoutPrefab.gameObject, this.elementEntryContainer.gameObject, false);
						hierarchyReferences.GetReference<LocText>("Label").text = category_tags.Key.ProperName();
						hierarchyReferences.GetReference<MultiToggle>("Toggle").onClick = delegate()
						{
							this.ToggleCategory(category_tags.Key, false);
						};
						this.categoryToggles.Add(category_tags.Key, hierarchyReferences);
					}
					this.filterRowMap[category_tags.Key] = new SortedDictionary<Tag, FilterSideScreenRow>(FilterSideScreen.comparer);
				}
				else if (category_tags.Key == GameTags.Void && !this.filterRowMap.ContainsKey(category_tags.Key))
				{
					this.filterRowMap[category_tags.Key] = new SortedDictionary<Tag, FilterSideScreenRow>(FilterSideScreen.comparer);
				}
				foreach (Tag tag in category_tags.Value)
				{
					if (!this.filterRowMap[category_tags.Key].ContainsKey(tag))
					{
						RectTransform rectTransform = (category_tags.Key != GameTags.Void) ? this.categoryToggles[category_tags.Key].GetReference<RectTransform>("Entries") : this.elementEntryContainer;
						FilterSideScreenRow row = Util.KInstantiateUI<FilterSideScreenRow>(this.elementEntryPrefab.gameObject, rectTransform.gameObject, false);
						row.SetTag(tag);
						row.button.onClick += delegate()
						{
							this.SetFilterTag(row.tag);
						};
						this.filterRowMap[category_tags.Key].Add(row.tag, row);
					}
				}
			}
		}
		int num = 0;
		this.filterRowMap[GameTags.Void][GameTags.Void].transform.SetSiblingIndex(num++);
		foreach (KeyValuePair<Tag, SortedDictionary<Tag, FilterSideScreenRow>> keyValuePair in this.filterRowMap)
		{
			if (tagOptions.ContainsKey(keyValuePair.Key) && tagOptions[keyValuePair.Key].Count > 0)
			{
				if (keyValuePair.Key != GameTags.Void)
				{
					this.categoryToggles[keyValuePair.Key].name = "CATE " + num.ToString();
					this.categoryToggles[keyValuePair.Key].transform.SetSiblingIndex(num++);
					this.categoryToggles[keyValuePair.Key].gameObject.SetActive(true);
				}
				int num2 = 0;
				using (SortedDictionary<Tag, FilterSideScreenRow>.Enumerator enumerator4 = keyValuePair.Value.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						KeyValuePair<Tag, FilterSideScreenRow> keyValuePair2 = enumerator4.Current;
						keyValuePair2.Value.name = "ELE " + num2.ToString();
						keyValuePair2.Value.transform.SetSiblingIndex(num2++);
						keyValuePair2.Value.gameObject.SetActive(tagOptions[keyValuePair.Key].Contains(keyValuePair2.Value.tag));
						if (keyValuePair2.Key != GameTags.Void && keyValuePair2.Key == this.targetFilterable.SelectedTag)
						{
							this.ToggleCategory(keyValuePair.Key, true);
						}
					}
					continue;
				}
			}
			if (keyValuePair.Key != GameTags.Void)
			{
				this.categoryToggles[keyValuePair.Key].gameObject.SetActive(false);
			}
		}
		this.RefreshUI();
	}

	// Token: 0x0600620B RID: 25099 RVA: 0x00243438 File Offset: 0x00241638
	private void SetFilterTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		if (tag.IsValid)
		{
			this.targetFilterable.SelectedTag = tag;
		}
		this.RefreshUI();
	}

	// Token: 0x0600620C RID: 25100 RVA: 0x00243464 File Offset: 0x00241664
	private void RefreshUI()
	{
		LocString loc_string;
		switch (this.targetFilterable.filterElementState)
		{
		case Filterable.ElementState.Solid:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
			goto IL_38;
		case Filterable.ElementState.Gas:
			loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
			goto IL_38;
		}
		loc_string = UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
		IL_38:
		this.currentSelectionLabel.text = string.Format(loc_string, UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
		foreach (KeyValuePair<Tag, SortedDictionary<Tag, FilterSideScreenRow>> keyValuePair in this.filterRowMap)
		{
			foreach (KeyValuePair<Tag, FilterSideScreenRow> keyValuePair2 in keyValuePair.Value)
			{
				bool flag = keyValuePair2.Key == this.targetFilterable.SelectedTag;
				keyValuePair2.Value.SetSelected(flag);
				if (flag)
				{
					if (keyValuePair2.Value.tag != GameTags.Void)
					{
						this.currentSelectionLabel.text = string.Format(loc_string, this.targetFilterable.SelectedTag.ProperName());
					}
					else
					{
						this.currentSelectionLabel.text = UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
					}
				}
			}
		}
	}

	// Token: 0x040042C4 RID: 17092
	public HierarchyReferences categoryFoldoutPrefab;

	// Token: 0x040042C5 RID: 17093
	public FilterSideScreenRow elementEntryPrefab;

	// Token: 0x040042C6 RID: 17094
	public RectTransform elementEntryContainer;

	// Token: 0x040042C7 RID: 17095
	public Image outputIcon;

	// Token: 0x040042C8 RID: 17096
	public Image everythingElseIcon;

	// Token: 0x040042C9 RID: 17097
	public LocText outputElementHeaderLabel;

	// Token: 0x040042CA RID: 17098
	public LocText everythingElseHeaderLabel;

	// Token: 0x040042CB RID: 17099
	public LocText selectElementHeaderLabel;

	// Token: 0x040042CC RID: 17100
	public LocText currentSelectionLabel;

	// Token: 0x040042CD RID: 17101
	private static TagNameComparer comparer = new TagNameComparer(GameTags.Void);

	// Token: 0x040042CE RID: 17102
	public Dictionary<Tag, HierarchyReferences> categoryToggles = new Dictionary<Tag, HierarchyReferences>();

	// Token: 0x040042CF RID: 17103
	public SortedDictionary<Tag, SortedDictionary<Tag, FilterSideScreenRow>> filterRowMap = new SortedDictionary<Tag, SortedDictionary<Tag, FilterSideScreenRow>>(FilterSideScreen.comparer);

	// Token: 0x040042D0 RID: 17104
	public bool isLogicFilter;

	// Token: 0x040042D1 RID: 17105
	private Filterable targetFilterable;
}
