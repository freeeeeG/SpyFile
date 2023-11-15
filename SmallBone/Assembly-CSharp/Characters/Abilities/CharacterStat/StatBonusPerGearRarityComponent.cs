using System;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C72 RID: 3186
	public class StatBonusPerGearRarityComponent : AbilityComponent<StatBonusPerGearRarity>, IStackable
	{
		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x000BD122 File Offset: 0x000BB322
		// (set) Token: 0x06004116 RID: 16662 RVA: 0x000BD12A File Offset: 0x000BB32A
		public bool loaded { get; set; }

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x000BD133 File Offset: 0x000BB333
		// (set) Token: 0x06004118 RID: 16664 RVA: 0x000BD141 File Offset: 0x000BB341
		public float stack
		{
			get
			{
				return (float)base.baseAbility.count;
			}
			set
			{
				this.loaded = false;
				base.baseAbility.count = (int)value;
				this.loaded = true;
			}
		}
	}
}
