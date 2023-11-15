using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000ABB RID: 2747
public class SubEntry
{
	// Token: 0x06005423 RID: 21539 RVA: 0x001E64F8 File Offset: 0x001E46F8
	public SubEntry()
	{
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001E650C File Offset: 0x001E470C
	public SubEntry(string id, string parentEntryID, List<ContentContainer> contentContainers, string name)
	{
		this.id = id;
		this.parentEntryID = parentEntryID;
		this.name = name;
		this.contentContainers = contentContainers;
		if (!string.IsNullOrEmpty(this.lockID))
		{
			foreach (ContentContainer contentContainer in contentContainers)
			{
				contentContainer.lockID = this.lockID;
			}
		}
		if (string.IsNullOrEmpty(this.sortString))
		{
			if (!string.IsNullOrEmpty(this.title))
			{
				this.sortString = UI.StripLinkFormatting(this.title);
				return;
			}
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06005425 RID: 21541 RVA: 0x001E65D4 File Offset: 0x001E47D4
	// (set) Token: 0x06005426 RID: 21542 RVA: 0x001E65DC File Offset: 0x001E47DC
	public List<ContentContainer> contentContainers { get; set; }

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06005427 RID: 21543 RVA: 0x001E65E5 File Offset: 0x001E47E5
	// (set) Token: 0x06005428 RID: 21544 RVA: 0x001E65ED File Offset: 0x001E47ED
	public string parentEntryID { get; set; }

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06005429 RID: 21545 RVA: 0x001E65F6 File Offset: 0x001E47F6
	// (set) Token: 0x0600542A RID: 21546 RVA: 0x001E65FE File Offset: 0x001E47FE
	public string id { get; set; }

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x0600542B RID: 21547 RVA: 0x001E6607 File Offset: 0x001E4807
	// (set) Token: 0x0600542C RID: 21548 RVA: 0x001E660F File Offset: 0x001E480F
	public string name { get; set; }

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x0600542D RID: 21549 RVA: 0x001E6618 File Offset: 0x001E4818
	// (set) Token: 0x0600542E RID: 21550 RVA: 0x001E6620 File Offset: 0x001E4820
	public string title { get; set; }

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x0600542F RID: 21551 RVA: 0x001E6629 File Offset: 0x001E4829
	// (set) Token: 0x06005430 RID: 21552 RVA: 0x001E6631 File Offset: 0x001E4831
	public string subtitle { get; set; }

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06005431 RID: 21553 RVA: 0x001E663A File Offset: 0x001E483A
	// (set) Token: 0x06005432 RID: 21554 RVA: 0x001E6642 File Offset: 0x001E4842
	public Sprite icon { get; set; }

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06005433 RID: 21555 RVA: 0x001E664B File Offset: 0x001E484B
	// (set) Token: 0x06005434 RID: 21556 RVA: 0x001E6653 File Offset: 0x001E4853
	public int layoutPriority { get; set; }

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06005435 RID: 21557 RVA: 0x001E665C File Offset: 0x001E485C
	// (set) Token: 0x06005436 RID: 21558 RVA: 0x001E6664 File Offset: 0x001E4864
	public bool disabled { get; set; }

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06005437 RID: 21559 RVA: 0x001E666D File Offset: 0x001E486D
	// (set) Token: 0x06005438 RID: 21560 RVA: 0x001E6675 File Offset: 0x001E4875
	public string lockID { get; set; }

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x06005439 RID: 21561 RVA: 0x001E667E File Offset: 0x001E487E
	// (set) Token: 0x0600543A RID: 21562 RVA: 0x001E6686 File Offset: 0x001E4886
	public string[] dlcIds { get; set; }

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x0600543B RID: 21563 RVA: 0x001E668F File Offset: 0x001E488F
	// (set) Token: 0x0600543C RID: 21564 RVA: 0x001E6697 File Offset: 0x001E4897
	public string[] forbiddenDLCIds { get; set; }

	// Token: 0x0600543D RID: 21565 RVA: 0x001E66A0 File Offset: 0x001E48A0
	public string[] GetDlcIds()
	{
		return this.dlcIds;
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001E66A8 File Offset: 0x001E48A8
	public string[] GetForbiddenDlCIds()
	{
		return this.forbiddenDLCIds;
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x0600543F RID: 21567 RVA: 0x001E66B0 File Offset: 0x001E48B0
	// (set) Token: 0x06005440 RID: 21568 RVA: 0x001E66B8 File Offset: 0x001E48B8
	public string sortString { get; set; }

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06005441 RID: 21569 RVA: 0x001E66C1 File Offset: 0x001E48C1
	// (set) Token: 0x06005442 RID: 21570 RVA: 0x001E66C9 File Offset: 0x001E48C9
	public bool showBeforeGeneratedCategoryLinks { get; set; }

	// Token: 0x04003848 RID: 14408
	public ContentContainer lockedContentContainer;

	// Token: 0x0400384F RID: 14415
	public Color iconColor = Color.white;
}
