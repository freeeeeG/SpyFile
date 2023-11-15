using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A7C RID: 2684
	[Serializable]
	public class ModifyDamageOnStatus : Ability
	{
		// Token: 0x060037CD RID: 14285 RVA: 0x000A4ABE File Offset: 0x000A2CBE
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageOnStatus.Instance(owner, this);
		}

		// Token: 0x04002C7A RID: 11386
		[SerializeField]
		private CharacterStatusKindBoolArray _filter;

		// Token: 0x04002C7B RID: 11387
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002C7C RID: 11388
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C7D RID: 11389
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C7E RID: 11390
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C7F RID: 11391
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C80 RID: 11392
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002C81 RID: 11393
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C82 RID: 11394
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002C83 RID: 11395
		[SerializeField]
		private int _applyCount;

		// Token: 0x02000A7D RID: 2685
		public class Instance : AbilityInstance<ModifyDamageOnStatus>
		{
			// Token: 0x17000BB9 RID: 3001
			// (get) Token: 0x060037CF RID: 14287 RVA: 0x000A4ADA File Offset: 0x000A2CDA
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

			// Token: 0x060037D0 RID: 14288 RVA: 0x000A4B07 File Offset: 0x000A2D07
			internal Instance(Character owner, ModifyDamageOnStatus ability) : base(owner, ability)
			{
			}

			// Token: 0x060037D1 RID: 14289 RVA: 0x000A4B11 File Offset: 0x000A2D11
			protected override void OnAttach()
			{
				this._remainCount = this.ability._applyCount;
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037D2 RID: 14290 RVA: 0x000A4B41 File Offset: 0x000A2D41
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037D3 RID: 14291 RVA: 0x000A4B60 File Offset: 0x000A2D60
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060037D4 RID: 14292 RVA: 0x000A4B78 File Offset: 0x000A2D78
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				if (target == null || target.character == null || target.character.status == null)
				{
					return false;
				}
				if (this._remainCooldownTime > 0f)
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
				if (target.character.status == null || !target.character.status.IsApplying(this.ability._filter))
				{
					return false;
				}
				damage.percentMultiplier *= (double)this.ability._damagePercent;
				damage.multiplier += (double)this.ability._damagePercentPoint;
				damage.criticalChance += (double)this.ability._extraCriticalChance;
				damage.criticalDamageMultiplier += (double)this.ability._extraCriticalDamageMultiplier;
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
				return false;
			}

			// Token: 0x04002C84 RID: 11396
			private float _remainCooldownTime;

			// Token: 0x04002C85 RID: 11397
			private int _remainCount;
		}
	}
}
