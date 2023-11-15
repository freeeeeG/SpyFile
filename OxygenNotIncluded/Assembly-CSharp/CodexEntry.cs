using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000ABD RID: 2749
public class CodexEntry
{
	// Token: 0x06005448 RID: 21576 RVA: 0x001E6724 File Offset: 0x001E4924
	public CodexEntry()
	{
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06005449 RID: 21577 RVA: 0x001E677C File Offset: 0x001E497C
	public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
	{
		this.category = category;
		this.name = name;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(name);
		}
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600544A RID: 21578 RVA: 0x001E6800 File Offset: 0x001E4A00
	public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
	{
		this.category = category;
		this.title = titleKey;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(this.title);
		}
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x0600544B RID: 21579 RVA: 0x001E6889 File Offset: 0x001E4A89
	// (set) Token: 0x0600544C RID: 21580 RVA: 0x001E6891 File Offset: 0x001E4A91
	public List<ContentContainer> contentContainers
	{
		get
		{
			return this._contentContainers;
		}
		private set
		{
			this._contentContainers = value;
		}
	}

	// Token: 0x0600544D RID: 21581 RVA: 0x001E689C File Offset: 0x001E4A9C
	public static List<string> ContentContainerDebug(List<ContentContainer> _contentContainers)
	{
		List<string> list = new List<string>();
		foreach (ContentContainer contentContainer in _contentContainers)
		{
			if (contentContainer != null)
			{
				string text = string.Concat(new string[]
				{
					"<b>",
					contentContainer.contentLayout.ToString(),
					" container: ",
					((contentContainer.content == null) ? 0 : contentContainer.content.Count).ToString(),
					" items</b>"
				});
				if (contentContainer.content != null)
				{
					text += "\n";
					for (int i = 0; i < contentContainer.content.Count; i++)
					{
						text = string.Concat(new string[]
						{
							text,
							"    • ",
							contentContainer.content[i].ToString(),
							": ",
							CodexEntry.GetContentWidgetDebugString(contentContainer.content[i]),
							"\n"
						});
					}
				}
				list.Add(text);
			}
			else
			{
				list.Add("null container");
			}
		}
		return list;
	}

	// Token: 0x0600544E RID: 21582 RVA: 0x001E69F4 File Offset: 0x001E4BF4
	private static string GetContentWidgetDebugString(ICodexWidget widget)
	{
		CodexText codexText = widget as CodexText;
		if (codexText != null)
		{
			return codexText.text;
		}
		CodexLabelWithIcon codexLabelWithIcon = widget as CodexLabelWithIcon;
		if (codexLabelWithIcon != null)
		{
			return codexLabelWithIcon.label.text + " / " + codexLabelWithIcon.icon.spriteName;
		}
		CodexImage codexImage = widget as CodexImage;
		if (codexImage != null)
		{
			return codexImage.spriteName;
		}
		CodexVideo codexVideo = widget as CodexVideo;
		if (codexVideo != null)
		{
			return codexVideo.name;
		}
		CodexIndentedLabelWithIcon codexIndentedLabelWithIcon = widget as CodexIndentedLabelWithIcon;
		if (codexIndentedLabelWithIcon != null)
		{
			return codexIndentedLabelWithIcon.label.text + " / " + codexIndentedLabelWithIcon.icon.spriteName;
		}
		return "";
	}

	// Token: 0x0600544F RID: 21583 RVA: 0x001E6A93 File Offset: 0x001E4C93
	public void CreateContentContainerCollection()
	{
		this.contentContainers = new List<ContentContainer>();
	}

	// Token: 0x06005450 RID: 21584 RVA: 0x001E6AA0 File Offset: 0x001E4CA0
	public void InsertContentContainer(int index, ContentContainer container)
	{
		this.contentContainers.Insert(index, container);
	}

	// Token: 0x06005451 RID: 21585 RVA: 0x001E6AAF File Offset: 0x001E4CAF
	public void RemoveContentContainerAt(int index)
	{
		this.contentContainers.RemoveAt(index);
	}

	// Token: 0x06005452 RID: 21586 RVA: 0x001E6ABD File Offset: 0x001E4CBD
	public void AddContentContainer(ContentContainer container)
	{
		this.contentContainers.Add(container);
	}

	// Token: 0x06005453 RID: 21587 RVA: 0x001E6ACB File Offset: 0x001E4CCB
	public void AddContentContainerRange(IEnumerable<ContentContainer> containers)
	{
		this.contentContainers.AddRange(containers);
	}

	// Token: 0x06005454 RID: 21588 RVA: 0x001E6AD9 File Offset: 0x001E4CD9
	public void RemoveContentContainer(ContentContainer container)
	{
		this.contentContainers.Remove(container);
	}

	// Token: 0x06005455 RID: 21589 RVA: 0x001E6AE8 File Offset: 0x001E4CE8
	public ICodexWidget GetFirstWidget()
	{
		for (int i = 0; i < this.contentContainers.Count; i++)
		{
			if (this.contentContainers[i].content != null)
			{
				for (int j = 0; j < this.contentContainers[i].content.Count; j++)
				{
					if (this.contentContainers[i].content[j] != null)
					{
						return this.contentContainers[i].content[j];
					}
				}
			}
		}
		return null;
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06005456 RID: 21590 RVA: 0x001E6B71 File Offset: 0x001E4D71
	// (set) Token: 0x06005457 RID: 21591 RVA: 0x001E6B7C File Offset: 0x001E4D7C
	public string[] dlcIds
	{
		get
		{
			return this._dlcIds;
		}
		set
		{
			this._dlcIds = value;
			string str = "";
			for (int i = 0; i < value.Length; i++)
			{
				str += value[i];
				if (i != value.Length - 1)
				{
					str += "\n";
				}
			}
		}
	}

	// Token: 0x06005458 RID: 21592 RVA: 0x001E6BC2 File Offset: 0x001E4DC2
	public string[] GetDlcIds()
	{
		if (this._dlcIds == null)
		{
			this._dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
		}
		return this._dlcIds;
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06005459 RID: 21593 RVA: 0x001E6BDD File Offset: 0x001E4DDD
	// (set) Token: 0x0600545A RID: 21594 RVA: 0x001E6BE8 File Offset: 0x001E4DE8
	public string[] forbiddenDLCIds
	{
		get
		{
			return this._forbiddenDLCIds;
		}
		set
		{
			this._forbiddenDLCIds = value;
			string str = "";
			for (int i = 0; i < value.Length; i++)
			{
				str += value[i];
				if (i != value.Length - 1)
				{
					str += "\n";
				}
			}
		}
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001E6C2E File Offset: 0x001E4E2E
	public string[] GetForbiddenDLCs()
	{
		if (this._forbiddenDLCIds == null)
		{
			this._forbiddenDLCIds = this.NONE;
		}
		return this._forbiddenDLCIds;
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x0600545C RID: 21596 RVA: 0x001E6C4A File Offset: 0x001E4E4A
	// (set) Token: 0x0600545D RID: 21597 RVA: 0x001E6C52 File Offset: 0x001E4E52
	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x0600545E RID: 21598 RVA: 0x001E6C5B File Offset: 0x001E4E5B
	// (set) Token: 0x0600545F RID: 21599 RVA: 0x001E6C63 File Offset: 0x001E4E63
	public string parentId
	{
		get
		{
			return this._parentId;
		}
		set
		{
			this._parentId = value;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06005460 RID: 21600 RVA: 0x001E6C6C File Offset: 0x001E4E6C
	// (set) Token: 0x06005461 RID: 21601 RVA: 0x001E6C74 File Offset: 0x001E4E74
	public string category
	{
		get
		{
			return this._category;
		}
		set
		{
			this._category = value;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06005462 RID: 21602 RVA: 0x001E6C7D File Offset: 0x001E4E7D
	// (set) Token: 0x06005463 RID: 21603 RVA: 0x001E6C85 File Offset: 0x001E4E85
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			this._title = value;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06005464 RID: 21604 RVA: 0x001E6C8E File Offset: 0x001E4E8E
	// (set) Token: 0x06005465 RID: 21605 RVA: 0x001E6C96 File Offset: 0x001E4E96
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			this._name = value;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06005466 RID: 21606 RVA: 0x001E6C9F File Offset: 0x001E4E9F
	// (set) Token: 0x06005467 RID: 21607 RVA: 0x001E6CA7 File Offset: 0x001E4EA7
	public string subtitle
	{
		get
		{
			return this._subtitle;
		}
		set
		{
			this._subtitle = value;
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06005468 RID: 21608 RVA: 0x001E6CB0 File Offset: 0x001E4EB0
	// (set) Token: 0x06005469 RID: 21609 RVA: 0x001E6CB8 File Offset: 0x001E4EB8
	public List<SubEntry> subEntries
	{
		get
		{
			return this._subEntries;
		}
		set
		{
			this._subEntries = value;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x0600546A RID: 21610 RVA: 0x001E6CC1 File Offset: 0x001E4EC1
	// (set) Token: 0x0600546B RID: 21611 RVA: 0x001E6CC9 File Offset: 0x001E4EC9
	public Sprite icon
	{
		get
		{
			return this._icon;
		}
		set
		{
			this._icon = value;
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x0600546C RID: 21612 RVA: 0x001E6CD2 File Offset: 0x001E4ED2
	// (set) Token: 0x0600546D RID: 21613 RVA: 0x001E6CDA File Offset: 0x001E4EDA
	public Color iconColor
	{
		get
		{
			return this._iconColor;
		}
		set
		{
			this._iconColor = value;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x0600546E RID: 21614 RVA: 0x001E6CE3 File Offset: 0x001E4EE3
	// (set) Token: 0x0600546F RID: 21615 RVA: 0x001E6CEB File Offset: 0x001E4EEB
	public string iconPrefabID
	{
		get
		{
			return this._iconPrefabID;
		}
		set
		{
			this._iconPrefabID = value;
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06005470 RID: 21616 RVA: 0x001E6CF4 File Offset: 0x001E4EF4
	// (set) Token: 0x06005471 RID: 21617 RVA: 0x001E6CFC File Offset: 0x001E4EFC
	public string iconLockID
	{
		get
		{
			return this._iconLockID;
		}
		set
		{
			this._iconLockID = value;
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06005472 RID: 21618 RVA: 0x001E6D05 File Offset: 0x001E4F05
	// (set) Token: 0x06005473 RID: 21619 RVA: 0x001E6D0D File Offset: 0x001E4F0D
	public string iconAssetName
	{
		get
		{
			return this._iconAssetName;
		}
		set
		{
			this._iconAssetName = value;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06005474 RID: 21620 RVA: 0x001E6D16 File Offset: 0x001E4F16
	// (set) Token: 0x06005475 RID: 21621 RVA: 0x001E6D1E File Offset: 0x001E4F1E
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			this._disabled = value;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06005476 RID: 21622 RVA: 0x001E6D27 File Offset: 0x001E4F27
	// (set) Token: 0x06005477 RID: 21623 RVA: 0x001E6D2F File Offset: 0x001E4F2F
	public bool searchOnly
	{
		get
		{
			return this._searchOnly;
		}
		set
		{
			this._searchOnly = value;
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06005478 RID: 21624 RVA: 0x001E6D38 File Offset: 0x001E4F38
	// (set) Token: 0x06005479 RID: 21625 RVA: 0x001E6D40 File Offset: 0x001E4F40
	public int customContentLength
	{
		get
		{
			return this._customContentLength;
		}
		set
		{
			this._customContentLength = value;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x0600547A RID: 21626 RVA: 0x001E6D49 File Offset: 0x001E4F49
	// (set) Token: 0x0600547B RID: 21627 RVA: 0x001E6D51 File Offset: 0x001E4F51
	public string sortString
	{
		get
		{
			return this._sortString;
		}
		set
		{
			this._sortString = value;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x0600547C RID: 21628 RVA: 0x001E6D5A File Offset: 0x001E4F5A
	// (set) Token: 0x0600547D RID: 21629 RVA: 0x001E6D62 File Offset: 0x001E4F62
	public bool showBeforeGeneratedCategoryLinks
	{
		get
		{
			return this._showBeforeGeneratedCategoryLinks;
		}
		set
		{
			this._showBeforeGeneratedCategoryLinks = value;
		}
	}

	// Token: 0x0400385A RID: 14426
	public EntryDevLog log = new EntryDevLog();

	// Token: 0x0400385B RID: 14427
	private List<ContentContainer> _contentContainers = new List<ContentContainer>();

	// Token: 0x0400385C RID: 14428
	private string[] _dlcIds;

	// Token: 0x0400385D RID: 14429
	private string[] _forbiddenDLCIds;

	// Token: 0x0400385E RID: 14430
	private string[] NONE = new string[0];

	// Token: 0x0400385F RID: 14431
	private string _id;

	// Token: 0x04003860 RID: 14432
	private string _parentId;

	// Token: 0x04003861 RID: 14433
	private string _category;

	// Token: 0x04003862 RID: 14434
	private string _title;

	// Token: 0x04003863 RID: 14435
	private string _name;

	// Token: 0x04003864 RID: 14436
	private string _subtitle;

	// Token: 0x04003865 RID: 14437
	private List<SubEntry> _subEntries = new List<SubEntry>();

	// Token: 0x04003866 RID: 14438
	private Sprite _icon;

	// Token: 0x04003867 RID: 14439
	private Color _iconColor = Color.white;

	// Token: 0x04003868 RID: 14440
	private string _iconPrefabID;

	// Token: 0x04003869 RID: 14441
	private string _iconLockID;

	// Token: 0x0400386A RID: 14442
	private string _iconAssetName;

	// Token: 0x0400386B RID: 14443
	private bool _disabled;

	// Token: 0x0400386C RID: 14444
	private bool _searchOnly;

	// Token: 0x0400386D RID: 14445
	private int _customContentLength;

	// Token: 0x0400386E RID: 14446
	private string _sortString;

	// Token: 0x0400386F RID: 14447
	private bool _showBeforeGeneratedCategoryLinks;
}
