using System;

namespace Database
{
	// Token: 0x02000D67 RID: 3431
	public class SkillPerk : Resource
	{
		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06006B2D RID: 27437 RVA: 0x0029CDB9 File Offset: 0x0029AFB9
		// (set) Token: 0x06006B2E RID: 27438 RVA: 0x0029CDC1 File Offset: 0x0029AFC1
		public Action<MinionResume> OnApply { get; protected set; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06006B2F RID: 27439 RVA: 0x0029CDCA File Offset: 0x0029AFCA
		// (set) Token: 0x06006B30 RID: 27440 RVA: 0x0029CDD2 File Offset: 0x0029AFD2
		public Action<MinionResume> OnRemove { get; protected set; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06006B31 RID: 27441 RVA: 0x0029CDDB File Offset: 0x0029AFDB
		// (set) Token: 0x06006B32 RID: 27442 RVA: 0x0029CDE3 File Offset: 0x0029AFE3
		public Action<MinionResume> OnMinionsChanged { get; protected set; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06006B33 RID: 27443 RVA: 0x0029CDEC File Offset: 0x0029AFEC
		// (set) Token: 0x06006B34 RID: 27444 RVA: 0x0029CDF4 File Offset: 0x0029AFF4
		public bool affectAll { get; protected set; }

		// Token: 0x06006B35 RID: 27445 RVA: 0x0029CDFD File Offset: 0x0029AFFD
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
		}
	}
}
