using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009A3 RID: 2467
	[Serializable]
	public class AddGaugeValueOnGaveDamage : Ability
	{
		// Token: 0x060034EF RID: 13551 RVA: 0x0009CB06 File Offset: 0x0009AD06
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AddGaugeValueOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04002A94 RID: 10900
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04002A95 RID: 10901
		[Range(1f, 100f)]
		[SerializeField]
		private int _chance = 100;

		// Token: 0x04002A96 RID: 10902
		[SerializeField]
		private int _amount = 1;

		// Token: 0x04002A97 RID: 10903
		[SerializeField]
		private int _amountOnCritical = 1;

		// Token: 0x04002A98 RID: 10904
		[SerializeField]
		private bool _multiplierByDamageDealt;

		// Token: 0x04002A99 RID: 10905
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002A9A RID: 10906
		[SerializeField]
		private AddGaugeValueOnGaveDamage.AttackTypeBoolArray _attackTypes;

		// Token: 0x04002A9B RID: 10907
		[SerializeField]
		private AddGaugeValueOnGaveDamage.DamageTypeBoolArray _types;

		// Token: 0x020009A4 RID: 2468
		public class Instance : AbilityInstance<AddGaugeValueOnGaveDamage>
		{
			// Token: 0x060034F1 RID: 13553 RVA: 0x0009CB2D File Offset: 0x0009AD2D
			internal Instance(Character owner, AddGaugeValueOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x060034F2 RID: 13554 RVA: 0x0009CB37 File Offset: 0x0009AD37
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x060034F3 RID: 13555 RVA: 0x0009CB60 File Offset: 0x0009AD60
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x060034F4 RID: 13556 RVA: 0x0009CB8C File Offset: 0x0009AD8C
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (target.character == null || !this.ability._attackTypes[tookDamage.motionType] || !this.ability._types[tookDamage.attackType] || !MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !tookDamage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				float num = (float)(tookDamage.critical ? this.ability._amountOnCritical : this.ability._amount);
				if (this.ability._multiplierByDamageDealt)
				{
					num *= (float)damageDealt;
				}
				this.ability._gauge.Add(num);
			}
		}

		// Token: 0x020009A5 RID: 2469
		[Serializable]
		private class AttackTypeBoolArray : EnumArray<Damage.MotionType, bool>
		{
		}

		// Token: 0x020009A6 RID: 2470
		[Serializable]
		private class DamageTypeBoolArray : EnumArray<Damage.AttackType, bool>
		{
		}
	}
}
