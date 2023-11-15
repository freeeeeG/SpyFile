using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A80 RID: 2688
public class TagFilterScreen : SideScreenContent
{
	// Token: 0x060051BF RID: 20927 RVA: 0x001D2BFF File Offset: 0x001D0DFF
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x060051C0 RID: 20928 RVA: 0x001D2C10 File Offset: 0x001D0E10
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		if (!this.targetFilterable.showUserMenu)
		{
			return;
		}
		this.Filter(this.targetFilterable.AcceptedTags);
		base.Activate();
	}

	// Token: 0x060051C1 RID: 20929 RVA: 0x001D2C7C File Offset: 0x001D0E7C
	protected override void OnActivate()
	{
		this.rootItem = this.BuildDisplay(this.rootTag);
		this.treeControl.SetUserItemRoot(this.rootItem);
		this.treeControl.root.opened = true;
		this.Filter(this.treeControl.root, this.acceptedTags, false);
	}

	// Token: 0x060051C2 RID: 20930 RVA: 0x001D2CD8 File Offset: 0x001D0ED8
	public static List<Tag> GetAllTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (TagFilterScreen.TagEntry tagEntry in TagFilterScreen.defaultRootTag.children)
		{
			if (tagEntry.tag.IsValid)
			{
				list.Add(tagEntry.tag);
			}
		}
		return list;
	}

	// Token: 0x060051C3 RID: 20931 RVA: 0x001D2D24 File Offset: 0x001D0F24
	private KTreeControl.UserItem BuildDisplay(TagFilterScreen.TagEntry root)
	{
		KTreeControl.UserItem userItem = null;
		if (root.name != null && root.name != "")
		{
			userItem = new KTreeControl.UserItem
			{
				text = root.name,
				userData = root.tag
			};
			List<KTreeControl.UserItem> list = new List<KTreeControl.UserItem>();
			if (root.children != null)
			{
				foreach (TagFilterScreen.TagEntry root2 in root.children)
				{
					list.Add(this.BuildDisplay(root2));
				}
			}
			userItem.children = list;
		}
		return userItem;
	}

	// Token: 0x060051C4 RID: 20932 RVA: 0x001D2DB0 File Offset: 0x001D0FB0
	private static KTreeControl.UserItem CreateTree(string tree_name, Tag tree_tag, IList<Element> items)
	{
		KTreeControl.UserItem userItem = new KTreeControl.UserItem
		{
			text = tree_name,
			userData = tree_tag,
			children = new List<KTreeControl.UserItem>()
		};
		foreach (Element element in items)
		{
			KTreeControl.UserItem item = new KTreeControl.UserItem
			{
				text = element.name,
				userData = GameTagExtensions.Create(element.id)
			};
			userItem.children.Add(item);
		}
		return userItem;
	}

	// Token: 0x060051C5 RID: 20933 RVA: 0x001D2E4C File Offset: 0x001D104C
	public void SetRootTag(TagFilterScreen.TagEntry root_tag)
	{
		this.rootTag = root_tag;
	}

	// Token: 0x060051C6 RID: 20934 RVA: 0x001D2E55 File Offset: 0x001D1055
	public void Filter(HashSet<Tag> acceptedTags)
	{
		this.acceptedTags = acceptedTags;
	}

	// Token: 0x060051C7 RID: 20935 RVA: 0x001D2E60 File Offset: 0x001D1060
	private void Filter(KTreeItem root, HashSet<Tag> acceptedTags, bool parentEnabled)
	{
		root.checkboxChecked = (parentEnabled || (root.userData != null && acceptedTags.Contains((Tag)root.userData)));
		foreach (KTreeItem root2 in root.children)
		{
			this.Filter(root2, acceptedTags, root.checkboxChecked);
		}
		if (!root.checkboxChecked && root.children.Count > 0)
		{
			bool checkboxChecked = true;
			using (IEnumerator<KTreeItem> enumerator = root.children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.checkboxChecked)
					{
						checkboxChecked = false;
						break;
					}
				}
			}
			root.checkboxChecked = checkboxChecked;
		}
	}

	// Token: 0x040035AF RID: 13743
	[SerializeField]
	private KTreeControl treeControl;

	// Token: 0x040035B0 RID: 13744
	private KTreeControl.UserItem rootItem;

	// Token: 0x040035B1 RID: 13745
	private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;

	// Token: 0x040035B2 RID: 13746
	private HashSet<Tag> acceptedTags = new HashSet<Tag>();

	// Token: 0x040035B3 RID: 13747
	private TreeFilterable targetFilterable;

	// Token: 0x040035B4 RID: 13748
	public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry
	{
		name = "All",
		tag = default(Tag),
		children = new TagFilterScreen.TagEntry[0]
	};

	// Token: 0x0200193C RID: 6460
	public class TagEntry
	{
		// Token: 0x040074CE RID: 29902
		public string name;

		// Token: 0x040074CF RID: 29903
		public Tag tag;

		// Token: 0x040074D0 RID: 29904
		public TagFilterScreen.TagEntry[] children;
	}
}
