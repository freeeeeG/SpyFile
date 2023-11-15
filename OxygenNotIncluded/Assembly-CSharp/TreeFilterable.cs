using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020006B0 RID: 1712
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06002E7D RID: 11901 RVA: 0x000F576D File Offset: 0x000F396D
	public HashSet<Tag> AcceptedTags
	{
		get
		{
			return this.acceptedTagSet;
		}
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x000F5778 File Offset: 0x000F3978
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.filterByStorageCategoriesOnSpawn = false;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.acceptedTagSet.UnionWith(this.acceptedTags);
			this.acceptedTags = null;
		}
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x000F57D4 File Offset: 0x000F39D4
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		if (this.storage.storageFilters.Contains(category_tag))
		{
			bool flag = false;
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
			{
				foreach (Tag tag2 in this.storage.storageFilters)
				{
					if (!(tag2 == category_tag) && DiscoveredResources.Instance.IsDiscovered(tag2))
					{
						flag = true;
						foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2))
						{
							if (!this.acceptedTagSet.Contains(item))
							{
								return;
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
			}
			foreach (Tag tag3 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
			{
				if (!(tag3 == tag) && !this.acceptedTagSet.Contains(tag3))
				{
					return;
				}
			}
			this.AddTagToFilter(tag);
		}
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x000F5928 File Offset: 0x000F3B28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x000F5944 File Offset: 0x000F3B44
	protected override void OnSpawn()
	{
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
		if (this.autoSelectStoredOnLoad && this.storage != null)
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
			hashSet.UnionWith(this.storage.GetAllIDsInStorage());
			this.UpdateFilters(hashSet);
		}
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (this.filterByStorageCategoriesOnSpawn)
		{
			this.RemoveIncorrectAcceptedTags();
		}
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x000F59D0 File Offset: 0x000F3BD0
	private void RemoveIncorrectAcceptedTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (Tag item in this.acceptedTagSet)
		{
			bool flag = false;
			foreach (Tag tag in this.storage.storageFilters)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item);
			}
		}
		foreach (Tag t in list)
		{
			this.RemoveTagFromFilter(t);
		}
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x000F5AC8 File Offset: 0x000F3CC8
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
		base.OnCleanUp();
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x000F5AE8 File Offset: 0x000F3CE8
	private void OnCopySettings(object data)
	{
		TreeFilterable component = ((GameObject)data).GetComponent<TreeFilterable>();
		if (component != null)
		{
			this.UpdateFilters(component.GetTags());
		}
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x000F5B16 File Offset: 0x000F3D16
	public HashSet<Tag> GetTags()
	{
		return this.acceptedTagSet;
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x000F5B1E File Offset: 0x000F3D1E
	public bool ContainsTag(Tag t)
	{
		return this.acceptedTagSet.Contains(t);
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x000F5B2C File Offset: 0x000F3D2C
	public void AddTagToFilter(Tag t)
	{
		if (this.ContainsTag(t))
		{
			return;
		}
		this.UpdateFilters(new HashSet<Tag>(this.acceptedTagSet)
		{
			t
		});
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x000F5B60 File Offset: 0x000F3D60
	public void RemoveTagFromFilter(Tag t)
	{
		if (!this.ContainsTag(t))
		{
			return;
		}
		HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
		hashSet.Remove(t);
		this.UpdateFilters(hashSet);
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x000F5B94 File Offset: 0x000F3D94
	public void UpdateFilters(HashSet<Tag> filters)
	{
		this.acceptedTagSet.Clear();
		this.acceptedTagSet.UnionWith(filters);
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (!this.dropIncorrectOnFilterChange || this.storage == null || this.storage.items == null)
		{
			return;
		}
		if (!this.filterAllStoragesOnBuilding)
		{
			this.DropFilteredItemsFromTargetStorage(this.storage);
			return;
		}
		foreach (Storage targetStorage in base.GetComponents<Storage>())
		{
			this.DropFilteredItemsFromTargetStorage(targetStorage);
		}
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x000F5C30 File Offset: 0x000F3E30
	private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
	{
		for (int i = targetStorage.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = targetStorage.items[i];
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!this.acceptedTagSet.Contains(component.PrefabTag))
				{
					targetStorage.Drop(gameObject, true);
				}
			}
		}
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x000F5C90 File Offset: 0x000F3E90
	public string GetTagsAsStatus(int maxDisplays = 6)
	{
		string text = "Tags:\n";
		List<Tag> list = new List<Tag>(this.storage.storageFilters);
		list.Intersect(this.acceptedTagSet);
		for (int i = 0; i < Mathf.Min(list.Count, maxDisplays); i++)
		{
			text += list[i].ProperName();
			if (i < Mathf.Min(list.Count, maxDisplays) - 1)
			{
				text += "\n";
			}
			if (i == maxDisplays - 1 && list.Count > maxDisplays)
			{
				text += "\n...";
				break;
			}
		}
		if (base.tag.Length == 0)
		{
			text = "No tags selected";
		}
		return text;
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x000F5D3C File Offset: 0x000F3F3C
	private void RefreshTint()
	{
		bool flag = this.acceptedTagSet != null && this.acceptedTagSet.Count != 0;
		base.GetComponent<KBatchedAnimController>().TintColour = (flag ? this.filterTint : this.noFilterTint);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, this);
	}

	// Token: 0x04001B69 RID: 7017
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001B6A RID: 7018
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001B6B RID: 7019
	public static readonly Color32 FILTER_TINT = Color.white;

	// Token: 0x04001B6C RID: 7020
	public static readonly Color32 NO_FILTER_TINT = new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);

	// Token: 0x04001B6D RID: 7021
	public Color32 filterTint = TreeFilterable.FILTER_TINT;

	// Token: 0x04001B6E RID: 7022
	public Color32 noFilterTint = TreeFilterable.NO_FILTER_TINT;

	// Token: 0x04001B6F RID: 7023
	[SerializeField]
	public bool dropIncorrectOnFilterChange = true;

	// Token: 0x04001B70 RID: 7024
	[SerializeField]
	public bool autoSelectStoredOnLoad = true;

	// Token: 0x04001B71 RID: 7025
	public bool showUserMenu = true;

	// Token: 0x04001B72 RID: 7026
	public bool filterAllStoragesOnBuilding;

	// Token: 0x04001B73 RID: 7027
	public TreeFilterable.UISideScreenHeight uiHeight = TreeFilterable.UISideScreenHeight.Tall;

	// Token: 0x04001B74 RID: 7028
	public bool filterByStorageCategoriesOnSpawn = true;

	// Token: 0x04001B75 RID: 7029
	[SerializeField]
	[Serialize]
	[Obsolete("Deprecated, use acceptedTagSet")]
	private List<Tag> acceptedTags = new List<Tag>();

	// Token: 0x04001B76 RID: 7030
	[SerializeField]
	[Serialize]
	private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();

	// Token: 0x04001B77 RID: 7031
	public Action<HashSet<Tag>> OnFilterChanged;

	// Token: 0x04001B78 RID: 7032
	private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>(delegate(TreeFilterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020013EA RID: 5098
	public enum UISideScreenHeight
	{
		// Token: 0x040063C5 RID: 25541
		Short,
		// Token: 0x040063C6 RID: 25542
		Tall
	}
}
