using System;

namespace Characters.Abilities
{
	// Token: 0x02000A57 RID: 2647
	[Serializable]
	public class IgnoreTrapDamage : Ability
	{
		// Token: 0x06003761 RID: 14177 RVA: 0x000A3426 File Offset: 0x000A1626
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new IgnoreTrapDamage.Instance(owner, this);
		}

		// Token: 0x02000A58 RID: 2648
		public class Instance : AbilityInstance<IgnoreTrapDamage>
		{
			// Token: 0x06003763 RID: 14179 RVA: 0x000A342F File Offset: 0x000A162F
			public Instance(Character owner, IgnoreTrapDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003764 RID: 14180 RVA: 0x000A3439 File Offset: 0x000A1639
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06003765 RID: 14181 RVA: 0x000A3461 File Offset: 0x000A1661
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06003766 RID: 14182 RVA: 0x000A3485 File Offset: 0x000A1685
			private bool OnOwnerTakeDamage(ref Damage damage)
			{
				return damage.attacker.character != null && damage.attacker.character.type == Character.Type.Trap;
			}

			// Token: 0x04002C12 RID: 11282
			private float _remainCooldownTime;
		}
	}
}
