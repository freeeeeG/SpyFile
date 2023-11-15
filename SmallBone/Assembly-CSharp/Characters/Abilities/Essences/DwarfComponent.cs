using System;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BDE RID: 3038
	public sealed class DwarfComponent : AbilityComponent<Dwarf>, IStackable
	{
		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x000B5AAC File Offset: 0x000B3CAC
		// (set) Token: 0x06003E7F RID: 15999 RVA: 0x000B5ABA File Offset: 0x000B3CBA
		public float stack
		{
			get
			{
				return (float)base.baseAbility.attackCount;
			}
			set
			{
				base.baseAbility.attackCount = (int)value;
			}
		}
	}
}
