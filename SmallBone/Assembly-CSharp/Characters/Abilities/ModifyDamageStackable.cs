using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A7F RID: 2687
	[Serializable]
	public class ModifyDamageStackable : Ability
	{
		// Token: 0x060037D6 RID: 14294 RVA: 0x000A4CE7 File Offset: 0x000A2EE7
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageStackable.Instance(owner, this);
		}

		// Token: 0x04002C86 RID: 11398
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002C87 RID: 11399
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C88 RID: 11400
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C89 RID: 11401
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C8A RID: 11402
		[SerializeField]
		[Information("base *= (1 + damagePercent * stack)", InformationAttribute.InformationType.Info, false)]
		private float _damagePercentByStack = 0.1f;

		// Token: 0x04002C8B RID: 11403
		[SerializeField]
		[Information(" multiplier += damagePercentPoint * stack", InformationAttribute.InformationType.Info, false)]
		private float _damagePercentPoint;

		// Token: 0x04002C8C RID: 11404
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002C8D RID: 11405
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002C8E RID: 11406
		[SerializeField]
		private int _applyCount;

		// Token: 0x04002C8F RID: 11407
		[SerializeField]
		private int _maxStack;

		// Token: 0x02000A80 RID: 2688
		public class Instance : AbilityInstance<ModifyDamageStackable>
		{
			// Token: 0x17000BBA RID: 3002
			// (get) Token: 0x060037D8 RID: 14296 RVA: 0x000A4D03 File Offset: 0x000A2F03
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

			// Token: 0x17000BBB RID: 3003
			// (get) Token: 0x060037D9 RID: 14297 RVA: 0x000A4D30 File Offset: 0x000A2F30
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x060037DA RID: 14298 RVA: 0x000A4D38 File Offset: 0x000A2F38
			internal Instance(Character owner, ModifyDamageStackable ability) : base(owner, ability)
			{
			}

			// Token: 0x060037DB RID: 14299 RVA: 0x000A4D44 File Offset: 0x000A2F44
			protected override void OnAttach()
			{
				this._remainCount = this.ability._applyCount;
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
				if (this.ability._maxStack == 0)
				{
					this.ability._maxStack = int.MaxValue;
				}
				this._stack = 1;
			}

			// Token: 0x060037DC RID: 14300 RVA: 0x000A4DA3 File Offset: 0x000A2FA3
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060037DD RID: 14301 RVA: 0x000A4DC2 File Offset: 0x000A2FC2
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060037DE RID: 14302 RVA: 0x000A4DD9 File Offset: 0x000A2FD9
			public override void Refresh()
			{
				base.Refresh();
				this._stack = Mathf.Clamp(this._stack + 1, 0, this.ability._maxStack);
			}

			// Token: 0x060037DF RID: 14303 RVA: 0x000A4E00 File Offset: 0x000A3000
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
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
				damage.percentMultiplier *= (double)(1f + this.ability._damagePercentByStack * (float)this._stack);
				damage.multiplier += (double)(this.ability._damagePercentPoint * (float)this._stack);
				damage.criticalChance += (double)(this.ability._extraCriticalChance * (float)this._stack);
				damage.criticalDamageMultiplier += (double)(this.ability._extraCriticalDamageMultiplier * (float)this._stack);
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
				return false;
			}

			// Token: 0x04002C90 RID: 11408
			private float _remainCooldownTime;

			// Token: 0x04002C91 RID: 11409
			private int _remainCount;

			// Token: 0x04002C92 RID: 11410
			private int _stack;
		}
	}
}
