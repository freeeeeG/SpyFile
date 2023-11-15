using System;
using System.Collections.Generic;
using KSerialization.Converters;
using UnityEngine;

// Token: 0x02000AC5 RID: 2757
public class ContentContainer
{
	// Token: 0x060054E7 RID: 21735 RVA: 0x001EEEFC File Offset: 0x001ED0FC
	public ContentContainer()
	{
		this.content = new List<ICodexWidget>();
	}

	// Token: 0x060054E8 RID: 21736 RVA: 0x001EEF0F File Offset: 0x001ED10F
	public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
	{
		this.content = content;
		this.contentLayout = contentLayout;
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x060054E9 RID: 21737 RVA: 0x001EEF25 File Offset: 0x001ED125
	// (set) Token: 0x060054EA RID: 21738 RVA: 0x001EEF2D File Offset: 0x001ED12D
	public List<ICodexWidget> content { get; set; }

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x060054EB RID: 21739 RVA: 0x001EEF36 File Offset: 0x001ED136
	// (set) Token: 0x060054EC RID: 21740 RVA: 0x001EEF3E File Offset: 0x001ED13E
	public string lockID { get; set; }

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x060054ED RID: 21741 RVA: 0x001EEF47 File Offset: 0x001ED147
	// (set) Token: 0x060054EE RID: 21742 RVA: 0x001EEF4F File Offset: 0x001ED14F
	[StringEnumConverter]
	public ContentContainer.ContentLayout contentLayout { get; set; }

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x060054EF RID: 21743 RVA: 0x001EEF58 File Offset: 0x001ED158
	// (set) Token: 0x060054F0 RID: 21744 RVA: 0x001EEF60 File Offset: 0x001ED160
	public bool showBeforeGeneratedContent { get; set; }

	// Token: 0x040038B9 RID: 14521
	public GameObject go;

	// Token: 0x020019E7 RID: 6631
	public enum ContentLayout
	{
		// Token: 0x040077C1 RID: 30657
		Vertical,
		// Token: 0x040077C2 RID: 30658
		Horizontal,
		// Token: 0x040077C3 RID: 30659
		Grid,
		// Token: 0x040077C4 RID: 30660
		GridTwoColumn,
		// Token: 0x040077C5 RID: 30661
		GridTwoColumnTall
	}
}
