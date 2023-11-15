using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D90 RID: 3472
	public class Skeleton_ShieldExplosionPassiveComponent : AbilityComponent<Skeleton_ShieldExplosionPassive>, IAttackDamage
	{
		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x060045E6 RID: 17894 RVA: 0x000CA846 File Offset: 0x000C8A46
		public float amount
		{
			get
			{
				return this.attackDamage;
			}
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x000CA84E File Offset: 0x000C8A4E
		public override void Initialize()
		{
			base.Initialize();
			this._ability.component = this;
		}

		// Token: 0x04003511 RID: 13585
		[NonSerialized]
		public float attackDamage;
	}
}
