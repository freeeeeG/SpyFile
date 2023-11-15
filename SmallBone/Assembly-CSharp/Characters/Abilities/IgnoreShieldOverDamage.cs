using System;

namespace Characters.Abilities
{
	// Token: 0x02000A4D RID: 2637
	[Serializable]
	public class IgnoreShieldOverDamage : Ability
	{
		// Token: 0x0600374C RID: 14156 RVA: 0x000A317C File Offset: 0x000A137C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new IgnoreShieldOverDamage.Instance(owner, this);
		}

		// Token: 0x02000A4E RID: 2638
		public class Instance : AbilityInstance<IgnoreShieldOverDamage>
		{
			// Token: 0x0600374E RID: 14158 RVA: 0x000A3185 File Offset: 0x000A1385
			public Instance(Character owner, IgnoreShieldOverDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x0600374F RID: 14159 RVA: 0x000A318F File Offset: 0x000A138F
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06003750 RID: 14160 RVA: 0x000A31B7 File Offset: 0x000A13B7
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06003751 RID: 14161 RVA: 0x000A31DC File Offset: 0x000A13DC
			private bool OnOwnerTakeDamage(ref Damage damage)
			{
				if (!this.owner.health.shield.hasAny)
				{
					return false;
				}
				if (this.owner.health.shield.amount > damage.amount)
				{
					return false;
				}
				this.owner.health.shield.Consume(damage.amount);
				return true;
			}
		}
	}
}
