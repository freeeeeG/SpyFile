using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class FlatTagFilterable : KMonoBehaviour
{
	// Token: 0x06002693 RID: 9875 RVA: 0x000D18DB File Offset: 0x000CFADB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.filterByStorageCategoriesOnSpawn = false;
		component.UpdateFilters(new HashSet<Tag>(this.selectedTags));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000D1918 File Offset: 0x000CFB18
	public void SelectTag(Tag tag, bool state)
	{
		global::Debug.Assert(this.tagOptions.Contains(tag), "The tag " + tag.Name + " is not valid for this filterable - it must be added to tagOptions");
		if (state)
		{
			if (!this.selectedTags.Contains(tag))
			{
				this.selectedTags.Add(tag);
			}
		}
		else if (this.selectedTags.Contains(tag))
		{
			this.selectedTags.Remove(tag);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x000D199C File Offset: 0x000CFB9C
	public void ToggleTag(Tag tag)
	{
		this.SelectTag(tag, !this.selectedTags.Contains(tag));
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x000D19B4 File Offset: 0x000CFBB4
	public string GetHeaderText()
	{
		return this.headerText;
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x000D19BC File Offset: 0x000CFBBC
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (base.GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
		{
			return;
		}
		this.selectedTags.Clear();
		foreach (Tag tag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
		{
			this.SelectTag(tag, true);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x0400161A RID: 5658
	[Serialize]
	public List<Tag> selectedTags = new List<Tag>();

	// Token: 0x0400161B RID: 5659
	public List<Tag> tagOptions = new List<Tag>();

	// Token: 0x0400161C RID: 5660
	public string headerText;

	// Token: 0x0400161D RID: 5661
	public bool displayOnlyDiscoveredTags = true;
}
