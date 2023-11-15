using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CE2 RID: 3298
	public sealed class OldSpellBookCoverComponent : AbilityComponent<OldSpellBookCover>, IStackable
	{
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x000C25C2 File Offset: 0x000C07C2
		// (set) Token: 0x060042C1 RID: 17089 RVA: 0x000C25D0 File Offset: 0x000C07D0
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
