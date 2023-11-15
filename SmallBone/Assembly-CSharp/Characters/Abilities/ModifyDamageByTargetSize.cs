using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A77 RID: 2679
	[Serializable]
	public class ModifyDamageByTargetSize : Ability
	{
		// Token: 0x060037C3 RID: 14275 RVA: 0x000A4840 File Offset: 0x000A2A40
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByTargetSize.Instance(owner, this);
		}

		// Token: 0x04002C6B RID: 11371
		[SerializeField]
		private ModifyDamageByTargetSize.TargetSize _targetSize;

		// Token: 0x04002C6C RID: 11372
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002C6D RID: 11373
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C6E RID: 11374
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C6F RID: 11375
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C70 RID: 11376
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002C71 RID: 11377
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002C72 RID: 11378
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C73 RID: 11379
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002C74 RID: 11380
		[SerializeField]
		private int _applyCount;

		// Token: 0x02000A78 RID: 2680
		public class Instance : AbilityInstance<ModifyDamageByTargetSize>
		{
			// Token: 0x17000BB8 RID: 3000
			// (get) Token: 0x060037C5 RID: 14277 RVA: 0x000A485C File Offset: 0x000A2A5C
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

			// Token: 0x060037C6 RID: 14278 RVA: 0x000A4889 File Offset: 0x000A2A89
			internal Instance(Character owner, ModifyDamageByTargetSize ability) : base(owner, ability)
			{
			}

			// Token: 0x060037C7 RID: 14279 RVA: 0x000A4893 File Offset: 0x000A2A93
			protected override void OnAttach()
			{
				this._remainCount = this.ability._applyCount;
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037C8 RID: 14280 RVA: 0x000A48C3 File Offset: 0x000A2AC3
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037C9 RID: 14281 RVA: 0x000A48E2 File Offset: 0x000A2AE2
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060037CA RID: 14282 RVA: 0x000A48FC File Offset: 0x000A2AFC
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				if (target.character == null)
				{
					return false;
				}
				ModifyDamageByTargetSize.TargetSize targetSize = this.ability._targetSize;
				if (targetSize != ModifyDamageByTargetSize.TargetSize.Bigger)
				{
					if (targetSize == ModifyDamageByTargetSize.TargetSize.Smaller)
					{
						if (target.collider.bounds.size.sqrMagnitude > this.owner.collider.bounds.size.sqrMagnitude)
						{
							return false;
						}
					}
				}
				else if (target.collider.bounds.size.sqrMagnitude < this.owner.collider.bounds.size.sqrMagnitude)
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

			// Token: 0x04002C75 RID: 11381
			private float _remainCooldownTime;

			// Token: 0x04002C76 RID: 11382
			private int _remainCount;
		}

		// Token: 0x02000A79 RID: 2681
		private enum TargetSize
		{
			// Token: 0x04002C78 RID: 11384
			Bigger,
			// Token: 0x04002C79 RID: 11385
			Smaller
		}
	}
}
