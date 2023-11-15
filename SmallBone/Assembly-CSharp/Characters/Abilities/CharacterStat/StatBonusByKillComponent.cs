using System;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C55 RID: 3157
	public class StatBonusByKillComponent : AbilityComponent<StatBonusByKill>, IStackable
	{
		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x000BBED6 File Offset: 0x000BA0D6
		// (set) Token: 0x060040A8 RID: 16552 RVA: 0x000BBEE4 File Offset: 0x000BA0E4
		public float stack
		{
			get
			{
				return (float)base.baseAbility.stack;
			}
			set
			{
				base.baseAbility.stack = (int)value;
			}
		}
	}
}
