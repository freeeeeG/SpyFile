using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000CEF RID: 3311
	public class FacadeInfo
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06006955 RID: 26965 RVA: 0x0027F7A3 File Offset: 0x0027D9A3
		// (set) Token: 0x06006956 RID: 26966 RVA: 0x0027F7AB File Offset: 0x0027D9AB
		public string prefabID { get; set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06006957 RID: 26967 RVA: 0x0027F7B4 File Offset: 0x0027D9B4
		// (set) Token: 0x06006958 RID: 26968 RVA: 0x0027F7BC File Offset: 0x0027D9BC
		public string id { get; set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06006959 RID: 26969 RVA: 0x0027F7C5 File Offset: 0x0027D9C5
		// (set) Token: 0x0600695A RID: 26970 RVA: 0x0027F7CD File Offset: 0x0027D9CD
		public string name { get; set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600695B RID: 26971 RVA: 0x0027F7D6 File Offset: 0x0027D9D6
		// (set) Token: 0x0600695C RID: 26972 RVA: 0x0027F7DE File Offset: 0x0027D9DE
		public string description { get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600695D RID: 26973 RVA: 0x0027F7E7 File Offset: 0x0027D9E7
		// (set) Token: 0x0600695E RID: 26974 RVA: 0x0027F7EF File Offset: 0x0027D9EF
		public string animFile { get; set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x0600695F RID: 26975 RVA: 0x0027F7F8 File Offset: 0x0027D9F8
		// (set) Token: 0x06006960 RID: 26976 RVA: 0x0027F800 File Offset: 0x0027DA00
		public List<FacadeInfo.workable> workables { get; set; }

		// Token: 0x02001C1D RID: 7197
		public class workable
		{
			// Token: 0x17000A5B RID: 2651
			// (get) Token: 0x06009BA8 RID: 39848 RVA: 0x00349EA9 File Offset: 0x003480A9
			// (set) Token: 0x06009BA9 RID: 39849 RVA: 0x00349EB1 File Offset: 0x003480B1
			public string workableName { get; set; }

			// Token: 0x17000A5C RID: 2652
			// (get) Token: 0x06009BAA RID: 39850 RVA: 0x00349EBA File Offset: 0x003480BA
			// (set) Token: 0x06009BAB RID: 39851 RVA: 0x00349EC2 File Offset: 0x003480C2
			public string workableAnim { get; set; }
		}
	}
}
