using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D9E RID: 3486
	public class UnknownSeedComponent : AbilityComponent<UnknownSeed>, IStackable
	{
		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x000CB085 File Offset: 0x000C9285
		// (set) Token: 0x06004622 RID: 17954 RVA: 0x000CB08D File Offset: 0x000C928D
		public float healed { get; set; }

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x000CB096 File Offset: 0x000C9296
		// (set) Token: 0x06004624 RID: 17956 RVA: 0x000CB09E File Offset: 0x000C929E
		public float healedBefore { get; set; }

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x000CB0A7 File Offset: 0x000C92A7
		// (set) Token: 0x06004626 RID: 17958 RVA: 0x000CB0AF File Offset: 0x000C92AF
		public float stack
		{
			get
			{
				return this.healed;
			}
			set
			{
				this.healed = value;
				base.baseAbility.UpdateStat();
			}
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x000CB0C3 File Offset: 0x000C92C3
		public override void Initialize()
		{
			base.Initialize();
			base.baseAbility.component = this;
		}
	}
}
