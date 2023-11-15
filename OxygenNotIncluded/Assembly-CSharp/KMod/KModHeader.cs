using System;

namespace KMod
{
	// Token: 0x02000D74 RID: 3444
	public class KModHeader
	{
		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06006B57 RID: 27479 RVA: 0x002A04E8 File Offset: 0x0029E6E8
		// (set) Token: 0x06006B58 RID: 27480 RVA: 0x002A04F0 File Offset: 0x0029E6F0
		public string staticID { get; set; }

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06006B59 RID: 27481 RVA: 0x002A04F9 File Offset: 0x0029E6F9
		// (set) Token: 0x06006B5A RID: 27482 RVA: 0x002A0501 File Offset: 0x0029E701
		public string title { get; set; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06006B5B RID: 27483 RVA: 0x002A050A File Offset: 0x0029E70A
		// (set) Token: 0x06006B5C RID: 27484 RVA: 0x002A0512 File Offset: 0x0029E712
		public string description { get; set; }
	}
}
