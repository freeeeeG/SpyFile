using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D68 RID: 3432
	public sealed class MagicWandComponent : AbilityComponent<MagicWand>, IStackable
	{
		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06004531 RID: 17713 RVA: 0x000C8E48 File Offset: 0x000C7048
		// (set) Token: 0x06004532 RID: 17714 RVA: 0x000C8E56 File Offset: 0x000C7056
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
