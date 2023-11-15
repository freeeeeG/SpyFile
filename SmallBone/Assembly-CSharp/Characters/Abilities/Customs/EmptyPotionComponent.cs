using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D49 RID: 3401
	public class EmptyPotionComponent : AbilityComponent<EmptyPotion>, IStackable
	{
		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x0600448B RID: 17547 RVA: 0x000C7162 File Offset: 0x000C5362
		// (set) Token: 0x0600448C RID: 17548 RVA: 0x000C7170 File Offset: 0x000C5370
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
