using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A85 RID: 2693
	[Serializable]
	public class ModifyTakingDamage : Ability
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x000A4FB4 File Offset: 0x000A31B4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyTakingDamage.Instance(owner, this);
		}

		// Token: 0x04002C95 RID: 11413
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002C96 RID: 11414
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		});

		// Token: 0x04002C97 RID: 11415
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C98 RID: 11416
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C99 RID: 11417
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C9A RID: 11418
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C9B RID: 11419
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002C9C RID: 11420
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C9D RID: 11421
		[SerializeField]
		private float _extraCriticalDamagePercentMultiplier;

		// Token: 0x04002C9E RID: 11422
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002C9F RID: 11423
		[SerializeField]
		private int _applyCount;

		// Token: 0x02000A86 RID: 2694
		public class Instance : AbilityInstance<ModifyTakingDamage>
		{
			// Token: 0x17000BBC RID: 3004
			// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000A4FED File Offset: 0x000A31ED
			public override float iconFillAmount
			{
				get
				{
					if (this.ability._cooldownTime != 0f)
					{
						return this._remainCooldownTime / this.ability._cooldownTime;
					}
					return base.iconFillAmount;
				}
			}

			// Token: 0x060037EA RID: 14314 RVA: 0x000A501A File Offset: 0x000A321A
			internal Instance(Character owner, ModifyTakingDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x060037EB RID: 14315 RVA: 0x000A5024 File Offset: 0x000A3224
			protected override void OnAttach()
			{
				if (this.ability._applyCount == 0)
				{
					this._remainCount = int.MaxValue;
				}
				else
				{
					this._remainCount = this.ability._applyCount;
				}
				this.owner.health.onTakeDamage.Add(0, new TakeDamageDelegate(this.HandleOnTakeDamage));
			}

			// Token: 0x060037EC RID: 14316 RVA: 0x000A5080 File Offset: 0x000A3280
			private bool HandleOnTakeDamage(ref Damage damage)
			{
				if (this._remainCooldownTime > 0f)
				{
					return false;
				}
				if (damage.attacker.character != null && !this.ability._characterTypes[damage.attacker.character.type])
				{
					return false;
				}
				if (!this.ability._attackTypes[damage.motionType])
				{
					return false;
				}
				if (!this.ability._damageTypes[damage.attackType])
				{
					return false;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !damage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				damage.percentMultiplier *= (double)this.ability._damagePercent;
				damage.multiplier += (double)this.ability._damagePercentPoint;
				damage.criticalChance += (double)this.ability._extraCriticalChance;
				damage.criticalDamageMultiplier += (double)this.ability._extraCriticalDamageMultiplier;
				damage.criticalDamagePercentMultiplier += (double)this.ability._extraCriticalDamagePercentMultiplier;
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
				return false;
			}

			// Token: 0x060037ED RID: 14317 RVA: 0x000A51DC File Offset: 0x000A33DC
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
			}

			// Token: 0x060037EE RID: 14318 RVA: 0x000A5200 File Offset: 0x000A3400
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x04002CA0 RID: 11424
			private float _remainCooldownTime;

			// Token: 0x04002CA1 RID: 11425
			private int _remainCount;
		}
	}
}
