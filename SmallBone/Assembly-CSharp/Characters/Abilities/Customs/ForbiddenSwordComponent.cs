using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D50 RID: 3408
	public class ForbiddenSwordComponent : AbilityComponent<ForbiddenSword>, IStackable
	{
		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060044B9 RID: 17593 RVA: 0x000C79E1 File Offset: 0x000C5BE1
		// (set) Token: 0x060044BA RID: 17594 RVA: 0x000C79E9 File Offset: 0x000C5BE9
		public int currentKillCount { get; set; }

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060044BB RID: 17595 RVA: 0x000C79F2 File Offset: 0x000C5BF2
		// (set) Token: 0x060044BC RID: 17596 RVA: 0x000C79FB File Offset: 0x000C5BFB
		public float stack
		{
			get
			{
				return (float)this.currentKillCount;
			}
			set
			{
				this.currentKillCount = (int)value;
			}
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x000C7A05 File Offset: 0x000C5C05
		public override void Initialize()
		{
			base.Initialize();
			base.baseAbility.component = this;
		}
	}
}
