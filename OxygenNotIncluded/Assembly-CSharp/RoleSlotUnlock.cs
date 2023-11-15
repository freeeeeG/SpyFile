using System;
using System.Collections.Generic;

// Token: 0x02000944 RID: 2372
public class RoleSlotUnlock
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06004514 RID: 17684 RVA: 0x00184FD1 File Offset: 0x001831D1
	// (set) Token: 0x06004515 RID: 17685 RVA: 0x00184FD9 File Offset: 0x001831D9
	public string id { get; protected set; }

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06004516 RID: 17686 RVA: 0x00184FE2 File Offset: 0x001831E2
	// (set) Token: 0x06004517 RID: 17687 RVA: 0x00184FEA File Offset: 0x001831EA
	public string name { get; protected set; }

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06004518 RID: 17688 RVA: 0x00184FF3 File Offset: 0x001831F3
	// (set) Token: 0x06004519 RID: 17689 RVA: 0x00184FFB File Offset: 0x001831FB
	public string description { get; protected set; }

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x0600451A RID: 17690 RVA: 0x00185004 File Offset: 0x00183204
	// (set) Token: 0x0600451B RID: 17691 RVA: 0x0018500C File Offset: 0x0018320C
	public List<global::Tuple<string, int>> slots { get; protected set; }

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x0600451C RID: 17692 RVA: 0x00185015 File Offset: 0x00183215
	// (set) Token: 0x0600451D RID: 17693 RVA: 0x0018501D File Offset: 0x0018321D
	public Func<bool> isSatisfied { get; protected set; }

	// Token: 0x0600451E RID: 17694 RVA: 0x00185026 File Offset: 0x00183226
	public RoleSlotUnlock(string id, string name, string description, List<global::Tuple<string, int>> slots, Func<bool> isSatisfied)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.slots = slots;
		this.isSatisfied = isSatisfied;
	}
}
