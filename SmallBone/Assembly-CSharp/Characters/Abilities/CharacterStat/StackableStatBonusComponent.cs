using System;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C3B RID: 3131
	public class StackableStatBonusComponent : AbilityComponent<StackableStatBonus>, IStackable
	{
		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x0600403D RID: 16445 RVA: 0x000BA7AF File Offset: 0x000B89AF
		// (set) Token: 0x0600403E RID: 16446 RVA: 0x000BA7BD File Offset: 0x000B89BD
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

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x000BA7CC File Offset: 0x000B89CC
		public bool isMax
		{
			get
			{
				return base.baseAbility.stack >= base.baseAbility.maxStack;
			}
		}
	}
}
