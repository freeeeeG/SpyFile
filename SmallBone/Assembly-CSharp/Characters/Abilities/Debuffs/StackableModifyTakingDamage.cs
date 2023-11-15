using System;
using UnityEngine;

namespace Characters.Abilities.Debuffs
{
	// Token: 0x02000BAC RID: 2988
	[Serializable]
	public sealed class StackableModifyTakingDamage : Ability
	{
		// Token: 0x06003DAE RID: 15790 RVA: 0x000B3387 File Offset: 0x000B1587
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StackableModifyTakingDamage.Instance(owner, this);
		}

		// Token: 0x04002FA7 RID: 12199
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002FA8 RID: 12200
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002FA9 RID: 12201
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

		// Token: 0x04002FAA RID: 12202
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002FAB RID: 12203
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002FAC RID: 12204
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002FAD RID: 12205
		[SerializeField]
		private int _applyCount;

		// Token: 0x04002FAE RID: 12206
		[SerializeField]
		private float _refreshCooldownTime;

		// Token: 0x04002FAF RID: 12207
		[Header("스택당 설정")]
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002FB0 RID: 12208
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002FB1 RID: 12209
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002FB2 RID: 12210
		[SerializeField]
		private float _extraCriticalDamagePercentMultiplier;

		// Token: 0x04002FB3 RID: 12211
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002FB4 RID: 12212
		private int _stack;

		// Token: 0x02000BAD RID: 2989
		public class Instance : AbilityInstance<StackableModifyTakingDamage>
		{
			// Token: 0x06003DB0 RID: 15792 RVA: 0x000B33C0 File Offset: 0x000B15C0
			public Instance(Character owner, StackableModifyTakingDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DB1 RID: 15793 RVA: 0x000B33CC File Offset: 0x000B15CC
			protected override void OnAttach()
			{
				this.ability._stack = 1;
				if (this.ability._applyCount == 0)
				{
					this._remainCount = int.MaxValue;
				}
				else
				{
					this._remainCount = this.ability._applyCount;
				}
				this.owner.health.onTakeDamage.Add(0, new TakeDamageDelegate(this.HandleOnTakeDamage));
				this._remainRefreshCooldownTime = this.ability._refreshCooldownTime;
			}

			// Token: 0x06003DB2 RID: 15794 RVA: 0x000B3444 File Offset: 0x000B1644
			public override void Refresh()
			{
				if (this._remainRefreshCooldownTime > 0f)
				{
					return;
				}
				base.Refresh();
				this.ability._stack = Mathf.Min(this.ability._stack + 1, this.ability._maxStack);
				this._remainRefreshCooldownTime = this.ability._refreshCooldownTime;
			}

			// Token: 0x06003DB3 RID: 15795 RVA: 0x000B34A0 File Offset: 0x000B16A0
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
				damage.percentMultiplier *= (double)(1f + this.ability._damagePercent * (float)this.ability._stack);
				damage.multiplier += (double)(this.ability._damagePercentPoint * (float)this.ability._stack);
				damage.criticalChance += (double)(this.ability._extraCriticalChance * (float)this.ability._stack);
				damage.criticalDamageMultiplier += (double)(this.ability._extraCriticalDamageMultiplier * (float)this.ability._stack);
				damage.criticalDamagePercentMultiplier += (double)(this.ability._extraCriticalDamagePercentMultiplier * (float)this.ability._stack);
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
				return false;
			}

			// Token: 0x06003DB4 RID: 15796 RVA: 0x000B3643 File Offset: 0x000B1843
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
			}

			// Token: 0x06003DB5 RID: 15797 RVA: 0x000B3667 File Offset: 0x000B1867
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
				this._remainRefreshCooldownTime -= deltaTime;
			}

			// Token: 0x04002FB5 RID: 12213
			private float _remainCooldownTime;

			// Token: 0x04002FB6 RID: 12214
			private float _remainRefreshCooldownTime;

			// Token: 0x04002FB7 RID: 12215
			private int _remainCount;
		}
	}
}
