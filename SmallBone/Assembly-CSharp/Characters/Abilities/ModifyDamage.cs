using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A5D RID: 2653
	[Serializable]
	public class ModifyDamage : Ability
	{
		// Token: 0x06003770 RID: 14192 RVA: 0x000A3656 File Offset: 0x000A1856
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamage.Instance(owner, this);
		}

		// Token: 0x04002C17 RID: 11287
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002C18 RID: 11288
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

		// Token: 0x04002C19 RID: 11289
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C1A RID: 11290
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C1B RID: 11291
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C1C RID: 11292
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C1D RID: 11293
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002C1E RID: 11294
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C1F RID: 11295
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002C20 RID: 11296
		[SerializeField]
		private int _applyCount;

		// Token: 0x04002C21 RID: 11297
		[Header("확정 크리티컬 설정")]
		[SerializeField]
		private bool _guaranteedCritical;

		// Token: 0x04002C22 RID: 11298
		[SerializeField]
		private int _guaranteedCriticalPriority;

		// Token: 0x02000A5E RID: 2654
		public class Instance : AbilityInstance<ModifyDamage>
		{
			// Token: 0x17000BB1 RID: 2993
			// (get) Token: 0x06003772 RID: 14194 RVA: 0x000A368F File Offset: 0x000A188F
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

			// Token: 0x06003773 RID: 14195 RVA: 0x000A36BC File Offset: 0x000A18BC
			internal Instance(Character owner, ModifyDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003774 RID: 14196 RVA: 0x000A36C8 File Offset: 0x000A18C8
			protected override void OnAttach()
			{
				this._remainCount = ((this.ability._applyCount == 0) ? int.MaxValue : this.ability._applyCount);
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x06003775 RID: 14197 RVA: 0x000A3717 File Offset: 0x000A1917
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x06003776 RID: 14198 RVA: 0x000A3736 File Offset: 0x000A1936
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06003777 RID: 14199 RVA: 0x000A3750 File Offset: 0x000A1950
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				if (this._remainCooldownTime > 0f)
				{
					return false;
				}
				if (target.character != null && !this.ability._characterTypes[target.character.type])
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
				if (this.ability._guaranteedCritical)
				{
					damage.SetGuaranteedCritical(this.ability._guaranteedCriticalPriority, true);
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
				return false;
			}

			// Token: 0x04002C23 RID: 11299
			private float _remainCooldownTime;

			// Token: 0x04002C24 RID: 11300
			private int _remainCount;
		}
	}
}
