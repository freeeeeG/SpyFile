using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B19 RID: 2841
	[Serializable]
	public sealed class WhaleboneAmulet : Ability
	{
		// Token: 0x060039B6 RID: 14774 RVA: 0x000AA776 File Offset: 0x000A8976
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new WhaleboneAmulet.Instance(owner, this);
		}

		// Token: 0x04002DCD RID: 11725
		[SerializeField]
		private int _reduceDamagePerBone;

		// Token: 0x02000B1A RID: 2842
		public sealed class Instance : AbilityInstance<WhaleboneAmulet>
		{
			// Token: 0x060039B8 RID: 14776 RVA: 0x000AA77F File Offset: 0x000A897F
			public Instance(Character owner, WhaleboneAmulet ability) : base(owner, ability)
			{
			}

			// Token: 0x060039B9 RID: 14777 RVA: 0x000AA789 File Offset: 0x000A8989
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(-100, new TakeDamageDelegate(this.HandleOnTakeDamage));
			}

			// Token: 0x060039BA RID: 14778 RVA: 0x000AA7B0 File Offset: 0x000A89B0
			private bool HandleOnTakeDamage(ref Damage damage)
			{
				if (this.owner.invulnerable.value || this.owner.evasion.value)
				{
					return false;
				}
				if (damage.@null)
				{
					return false;
				}
				if (damage.amount < 1.0)
				{
					return false;
				}
				int num = (int)(damage.amount / (double)this.ability._reduceDamagePerBone);
				if (num > 0 && GameData.Currency.bone.Has(num))
				{
					GameData.Currency.bone.Consume(num);
					damage.@null = true;
					return false;
				}
				if (GameData.Currency.bone.Has(1))
				{
					if (num == 0)
					{
						GameData.Currency.bone.Consume(1);
						damage.@null = true;
						return false;
					}
					GameData.Currency.bone.Consume(GameData.Currency.bone.balance);
					int num2 = this.ability._reduceDamagePerBone * GameData.Currency.bone.balance;
					damage.extraFixedDamage = (double)(-(double)num2);
				}
				return false;
			}

			// Token: 0x060039BB RID: 14779 RVA: 0x000AA896 File Offset: 0x000A8A96
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
			}
		}
	}
}
