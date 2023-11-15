using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;

// Token: 0x02000542 RID: 1346
[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
	// Token: 0x1700016D RID: 365
	// (get) Token: 0x060020AC RID: 8364 RVA: 0x000AF448 File Offset: 0x000AD648
	public int DefaultPersonalPriority
	{
		get
		{
			return this.defaultPersonalPriority;
		}
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x000AF450 File Offset: 0x000AD650
	public ChoreGroup(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true) : base(id, name)
	{
		this.attribute = attribute;
		this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
		this.sprite = sprite;
		this.defaultPersonalPriority = default_personal_priority;
		this.userPrioritizable = user_prioritizable;
	}

	// Token: 0x04001267 RID: 4711
	public List<ChoreType> choreTypes = new List<ChoreType>();

	// Token: 0x04001268 RID: 4712
	public Klei.AI.Attribute attribute;

	// Token: 0x04001269 RID: 4713
	public string description;

	// Token: 0x0400126A RID: 4714
	public string sprite;

	// Token: 0x0400126B RID: 4715
	private int defaultPersonalPriority;

	// Token: 0x0400126C RID: 4716
	public bool userPrioritizable;
}
