using System;

// Token: 0x020007B5 RID: 1973
public class Expectation
{
	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x060036AE RID: 13998 RVA: 0x001274FD File Offset: 0x001256FD
	// (set) Token: 0x060036AF RID: 13999 RVA: 0x00127505 File Offset: 0x00125705
	public string id { get; protected set; }

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x060036B0 RID: 14000 RVA: 0x0012750E File Offset: 0x0012570E
	// (set) Token: 0x060036B1 RID: 14001 RVA: 0x00127516 File Offset: 0x00125716
	public string name { get; protected set; }

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x060036B2 RID: 14002 RVA: 0x0012751F File Offset: 0x0012571F
	// (set) Token: 0x060036B3 RID: 14003 RVA: 0x00127527 File Offset: 0x00125727
	public string description { get; protected set; }

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x060036B4 RID: 14004 RVA: 0x00127530 File Offset: 0x00125730
	// (set) Token: 0x060036B5 RID: 14005 RVA: 0x00127538 File Offset: 0x00125738
	public Action<MinionResume> OnApply { get; protected set; }

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x060036B6 RID: 14006 RVA: 0x00127541 File Offset: 0x00125741
	// (set) Token: 0x060036B7 RID: 14007 RVA: 0x00127549 File Offset: 0x00125749
	public Action<MinionResume> OnRemove { get; protected set; }

	// Token: 0x060036B8 RID: 14008 RVA: 0x00127552 File Offset: 0x00125752
	public Expectation(string id, string name, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.OnApply = OnApply;
		this.OnRemove = OnRemove;
	}
}
