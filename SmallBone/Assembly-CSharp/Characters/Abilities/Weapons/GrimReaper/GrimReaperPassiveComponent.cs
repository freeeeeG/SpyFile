using System;

namespace Characters.Abilities.Weapons.GrimReaper
{
	// Token: 0x02000C0D RID: 3085
	public class GrimReaperPassiveComponent : AbilityComponent<GrimReaperPassive>, IStackable
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x000B7F28 File Offset: 0x000B6128
		// (set) Token: 0x06003F6D RID: 16237 RVA: 0x000B7F36 File Offset: 0x000B6136
		public float stack
		{
			get
			{
				return (float)base.baseAbility.stack;
			}
			set
			{
				base.baseAbility.stack = (int)value;
				base.baseAbility.SetEnhanceSkill();
			}
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x000B7F50 File Offset: 0x000B6150
		public void AddStack()
		{
			base.baseAbility.AddStack();
		}
	}
}
