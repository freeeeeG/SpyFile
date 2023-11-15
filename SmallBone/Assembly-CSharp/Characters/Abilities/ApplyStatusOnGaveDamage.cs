using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009BF RID: 2495
	[Serializable]
	public class ApplyStatusOnGaveDamage : Ability
	{
		// Token: 0x06003542 RID: 13634 RVA: 0x0009DD65 File Offset: 0x0009BF65
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ApplyStatusOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04002AE7 RID: 10983
		[Tooltip("default는 0초")]
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002AE8 RID: 10984
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x04002AE9 RID: 10985
		[Range(1f, 100f)]
		[SerializeField]
		private int _chance = 100;

		// Token: 0x04002AEA RID: 10986
		[SerializeField]
		private bool _onCritical;

		// Token: 0x04002AEB RID: 10987
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x04002AEC RID: 10988
		[SerializeField]
		private ApplyStatusOnGaveDamage.AttackTypeBoolArray _attackTypes;

		// Token: 0x04002AED RID: 10989
		[SerializeField]
		private ApplyStatusOnGaveDamage.DamageTypeBoolArray _types;

		// Token: 0x020009C0 RID: 2496
		public class Instance : AbilityInstance<ApplyStatusOnGaveDamage>
		{
			// Token: 0x17000B88 RID: 2952
			// (get) Token: 0x06003544 RID: 13636 RVA: 0x0009DD7E File Offset: 0x0009BF7E
			public override float iconFillAmount
			{
				get
				{
					if (this.ability._cooldownTime != 0f)
					{
						return this._remainTime / this.ability._cooldownTime;
					}
					return 0f;
				}
			}

			// Token: 0x06003545 RID: 13637 RVA: 0x0009DDAA File Offset: 0x0009BFAA
			internal Instance(Character owner, ApplyStatusOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003546 RID: 13638 RVA: 0x0009DDB4 File Offset: 0x0009BFB4
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003547 RID: 13639 RVA: 0x0009DDDD File Offset: 0x0009BFDD
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003548 RID: 13640 RVA: 0x0009DE06 File Offset: 0x0009C006
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTime -= deltaTime;
			}

			// Token: 0x06003549 RID: 13641 RVA: 0x0009DE20 File Offset: 0x0009C020
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (this._remainTime > 0f)
				{
					return;
				}
				if (target.character == null || target.character == this.owner || (this.ability._onCritical && !tookDamage.critical) || !this.ability._attackTypes[tookDamage.motionType] || !this.ability._types[tookDamage.attackType] || !MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !tookDamage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (this.owner.GiveStatus(target.character, this.ability._status))
				{
					this._remainTime = this.ability._cooldownTime;
				}
			}

			// Token: 0x04002AEE RID: 10990
			private float _remainTime;
		}

		// Token: 0x020009C1 RID: 2497
		[Serializable]
		private class AttackTypeBoolArray : EnumArray<Damage.MotionType, bool>
		{
		}

		// Token: 0x020009C2 RID: 2498
		[Serializable]
		private class DamageTypeBoolArray : EnumArray<Damage.AttackType, bool>
		{
		}
	}
}
