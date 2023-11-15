using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020005FE RID: 1534
[AddComponentMenu("KMonoBehaviour/scripts/Filterable")]
public class Filterable : KMonoBehaviour
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06002669 RID: 9833 RVA: 0x000D0E10 File Offset: 0x000CF010
	// (remove) Token: 0x0600266A RID: 9834 RVA: 0x000D0E48 File Offset: 0x000CF048
	public event Action<Tag> onFilterChanged;

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x0600266B RID: 9835 RVA: 0x000D0E7D File Offset: 0x000CF07D
	// (set) Token: 0x0600266C RID: 9836 RVA: 0x000D0E85 File Offset: 0x000CF085
	public Tag SelectedTag
	{
		get
		{
			return this.selectedTag;
		}
		set
		{
			this.selectedTag = value;
			this.OnFilterChanged();
		}
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x000D0E94 File Offset: 0x000CF094
	public Dictionary<Tag, HashSet<Tag>> GetTagOptions()
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		if (this.filterElementState == Filterable.ElementState.Solid)
		{
			dictionary = DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(Filterable.filterableCategories);
		}
		else
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (!element.disabled && ((element.IsGas && this.filterElementState == Filterable.ElementState.Gas) || (element.IsLiquid && this.filterElementState == Filterable.ElementState.Liquid)))
				{
					Tag materialCategoryTag = element.GetMaterialCategoryTag();
					if (!dictionary.ContainsKey(materialCategoryTag))
					{
						dictionary[materialCategoryTag] = new HashSet<Tag>();
					}
					Tag item = GameTagExtensions.Create(element.id);
					dictionary[materialCategoryTag].Add(item);
				}
			}
		}
		dictionary.Add(GameTags.Void, new HashSet<Tag>
		{
			GameTags.Void
		});
		return dictionary;
	}

	// Token: 0x0600266E RID: 9838 RVA: 0x000D0F84 File Offset: 0x000CF184
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Filterable>(-905833192, Filterable.OnCopySettingsDelegate);
	}

	// Token: 0x0600266F RID: 9839 RVA: 0x000D0FA0 File Offset: 0x000CF1A0
	private void OnCopySettings(object data)
	{
		Filterable component = ((GameObject)data).GetComponent<Filterable>();
		if (component != null)
		{
			this.SelectedTag = component.SelectedTag;
		}
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000D0FCE File Offset: 0x000CF1CE
	protected override void OnSpawn()
	{
		this.OnFilterChanged();
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x000D0FD8 File Offset: 0x000CF1D8
	private void OnFilterChanged()
	{
		if (this.onFilterChanged != null)
		{
			this.onFilterChanged(this.selectedTag);
		}
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Filterable.filterSelected, this.selectedTag.IsValid);
		}
	}

	// Token: 0x04001603 RID: 5635
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001604 RID: 5636
	[Serialize]
	public Filterable.ElementState filterElementState;

	// Token: 0x04001605 RID: 5637
	[Serialize]
	private Tag selectedTag = GameTags.Void;

	// Token: 0x04001607 RID: 5639
	private static TagSet filterableCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements
	});

	// Token: 0x04001608 RID: 5640
	private static readonly Operational.Flag filterSelected = new Operational.Flag("filterSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04001609 RID: 5641
	private static readonly EventSystem.IntraObjectHandler<Filterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Filterable>(delegate(Filterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020012A3 RID: 4771
	public enum ElementState
	{
		// Token: 0x04006037 RID: 24631
		None,
		// Token: 0x04006038 RID: 24632
		Solid,
		// Token: 0x04006039 RID: 24633
		Liquid,
		// Token: 0x0400603A RID: 24634
		Gas
	}
}
