using System;
using System.Collections.Generic;

// Token: 0x02000ABC RID: 2748
public class CategoryEntry : CodexEntry
{
	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06005443 RID: 21571 RVA: 0x001E66D2 File Offset: 0x001E48D2
	// (set) Token: 0x06005444 RID: 21572 RVA: 0x001E66DA File Offset: 0x001E48DA
	public bool largeFormat { get; set; }

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06005445 RID: 21573 RVA: 0x001E66E3 File Offset: 0x001E48E3
	// (set) Token: 0x06005446 RID: 21574 RVA: 0x001E66EB File Offset: 0x001E48EB
	public bool sort { get; set; }

	// Token: 0x06005447 RID: 21575 RVA: 0x001E66F4 File Offset: 0x001E48F4
	public CategoryEntry(string category, List<ContentContainer> contentContainers, string name, List<CodexEntry> entriesInCategory, bool largeFormat, bool sort) : base(category, contentContainers, name)
	{
		this.entriesInCategory = entriesInCategory;
		this.largeFormat = largeFormat;
		this.sort = sort;
	}

	// Token: 0x04003859 RID: 14425
	public List<CodexEntry> entriesInCategory = new List<CodexEntry>();
}
