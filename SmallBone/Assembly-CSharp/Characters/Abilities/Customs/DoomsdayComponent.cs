using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D46 RID: 3398
	public class DoomsdayComponent : AbilityComponent<Doomsday>, IAttackDamage
	{
		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x000C6FD7 File Offset: 0x000C51D7
		// (set) Token: 0x0600447D RID: 17533 RVA: 0x000C6FDF File Offset: 0x000C51DF
		public float amount { get; set; }

		// Token: 0x0600447E RID: 17534 RVA: 0x000C6FE8 File Offset: 0x000C51E8
		public override void Initialize()
		{
			base.Initialize();
			base.baseAbility.component = this;
		}
	}
}
