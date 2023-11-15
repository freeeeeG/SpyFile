using System;

namespace Characters.Abilities
{
	// Token: 0x02000A04 RID: 2564
	[Serializable]
	public class ChangeTakingDamageToOne : Ability
	{
		// Token: 0x06003680 RID: 13952 RVA: 0x000A1580 File Offset: 0x0009F780
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChangeTakingDamageToOne.Instance(owner, this);
		}

		// Token: 0x02000A05 RID: 2565
		public class Instance : AbilityInstance<ChangeTakingDamageToOne>
		{
			// Token: 0x06003682 RID: 13954 RVA: 0x000A1589 File Offset: 0x0009F789
			public Instance(Character owner, ChangeTakingDamageToOne ability) : base(owner, ability)
			{
			}

			// Token: 0x06003683 RID: 13955 RVA: 0x000A1593 File Offset: 0x0009F793
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06003684 RID: 13956 RVA: 0x000A15BB File Offset: 0x0009F7BB
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06003685 RID: 13957 RVA: 0x000A15E0 File Offset: 0x0009F7E0
			private bool OnOwnerTakeDamage(ref Damage damage)
			{
				if (damage.amount < 1.0)
				{
					return false;
				}
				damage.@base = 1.0;
				damage.percentMultiplier = 1.0;
				damage.multiplier = 1.0;
				damage.criticalDamageMultiplier = 1.0;
				return false;
			}

			// Token: 0x04002BAB RID: 11179
			private float _remainCooldownTime;
		}
	}
}
