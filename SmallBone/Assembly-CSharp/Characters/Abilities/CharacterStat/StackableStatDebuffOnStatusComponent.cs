using System;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C3E RID: 3134
	public sealed class StackableStatDebuffOnStatusComponent : AbilityComponent<StackableStatDebuffOnStatus>, IStackable
	{
		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x000BAD42 File Offset: 0x000B8F42
		// (set) Token: 0x0600404F RID: 16463 RVA: 0x000BAD50 File Offset: 0x000B8F50
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
