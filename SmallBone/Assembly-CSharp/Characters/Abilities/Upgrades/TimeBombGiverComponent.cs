using System;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B15 RID: 2837
	public sealed class TimeBombGiverComponent : AbilityComponent<TimeBombGiver>
	{
		// Token: 0x060039A9 RID: 14761 RVA: 0x000AA4C4 File Offset: 0x000A86C4
		public void Attack(Character target)
		{
			base.baseAbility.Attack(target);
		}
	}
}
