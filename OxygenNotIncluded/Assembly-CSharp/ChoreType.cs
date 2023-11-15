using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000543 RID: 1347
[DebuggerDisplay("{IdHash}")]
public class ChoreType : Resource
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x060020AE RID: 8366 RVA: 0x000AF4B4 File Offset: 0x000AD6B4
	// (set) Token: 0x060020AF RID: 8367 RVA: 0x000AF4BC File Offset: 0x000AD6BC
	public Urge urge { get; private set; }

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060020B0 RID: 8368 RVA: 0x000AF4C5 File Offset: 0x000AD6C5
	// (set) Token: 0x060020B1 RID: 8369 RVA: 0x000AF4CD File Offset: 0x000AD6CD
	public ChoreGroup[] groups { get; private set; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x060020B2 RID: 8370 RVA: 0x000AF4D6 File Offset: 0x000AD6D6
	// (set) Token: 0x060020B3 RID: 8371 RVA: 0x000AF4DE File Offset: 0x000AD6DE
	public int priority { get; private set; }

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x060020B4 RID: 8372 RVA: 0x000AF4E7 File Offset: 0x000AD6E7
	// (set) Token: 0x060020B5 RID: 8373 RVA: 0x000AF4EF File Offset: 0x000AD6EF
	public int interruptPriority { get; set; }

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x060020B6 RID: 8374 RVA: 0x000AF4F8 File Offset: 0x000AD6F8
	// (set) Token: 0x060020B7 RID: 8375 RVA: 0x000AF500 File Offset: 0x000AD700
	public int explicitPriority { get; private set; }

	// Token: 0x060020B8 RID: 8376 RVA: 0x000AF509 File Offset: 0x000AD709
	private string ResolveStringCallback(string str, object data)
	{
		return ((Chore)data).ResolveString(str);
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000AF518 File Offset: 0x000AD718
	public ChoreType(string id, ResourceSet parent, string[] chore_groups, string urge, string name, string status_message, string tooltip, IEnumerable<Tag> interrupt_exclusion, int implicit_priority, int explicit_priority) : base(id, parent, name)
	{
		this.statusItem = new StatusItem(id, status_message, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveStringCallback);
		this.tags.Add(TagManager.Create(id));
		this.interruptExclusion = new HashSet<Tag>(interrupt_exclusion);
		Db.Get().DuplicantStatusItems.Add(this.statusItem);
		List<ChoreGroup> list = new List<ChoreGroup>();
		for (int i = 0; i < chore_groups.Length; i++)
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.TryGet(chore_groups[i]);
			if (choreGroup != null)
			{
				if (!choreGroup.choreTypes.Contains(this))
				{
					choreGroup.choreTypes.Add(this);
				}
				list.Add(choreGroup);
			}
		}
		this.groups = list.ToArray();
		if (!string.IsNullOrEmpty(urge))
		{
			this.urge = Db.Get().Urges.Get(urge);
		}
		this.priority = implicit_priority;
		this.explicitPriority = explicit_priority;
	}

	// Token: 0x0400126D RID: 4717
	public StatusItem statusItem;

	// Token: 0x04001272 RID: 4722
	public HashSet<Tag> tags = new HashSet<Tag>();

	// Token: 0x04001273 RID: 4723
	public HashSet<Tag> interruptExclusion;

	// Token: 0x04001275 RID: 4725
	public string reportName;
}
