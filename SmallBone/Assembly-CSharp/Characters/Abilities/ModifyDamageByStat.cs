using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A71 RID: 2673
	[Serializable]
	public sealed class ModifyDamageByStat : Ability
	{
		// Token: 0x060037B3 RID: 14259 RVA: 0x000A4531 File Offset: 0x000A2731
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyDamageByStat.Instance(owner, this);
		}

		// Token: 0x04002C5B RID: 11355
		[Header("발동 조건")]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002C5C RID: 11356
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x04002C5D RID: 11357
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002C5E RID: 11358
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002C5F RID: 11359
		[SerializeField]
		[Header("데미지 변화 설정")]
		private Stat.Value _sourceStat;

		// Token: 0x04002C60 RID: 11360
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002C61 RID: 11361
		[Header("스텟 입력 1 -> 0.01%p")]
		[SerializeField]
		private float _damagePercentByStatUnit;

		// Token: 0x04002C62 RID: 11362
		[SerializeField]
		private float _damagePercentPointByStatUnit;

		// Token: 0x04002C63 RID: 11363
		[SerializeField]
		private float _maxDamagePercent = 10f;

		// Token: 0x04002C64 RID: 11364
		[SerializeField]
		private float _maxDamagePercentPoint;

		// Token: 0x02000A72 RID: 2674
		public class Instance : AbilityInstance<ModifyDamageByStat>
		{
			// Token: 0x17000BB7 RID: 2999
			// (get) Token: 0x060037B5 RID: 14261 RVA: 0x000A454D File Offset: 0x000A274D
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

			// Token: 0x060037B6 RID: 14262 RVA: 0x000A457A File Offset: 0x000A277A
			internal Instance(Character owner, ModifyDamageByStat ability) : base(owner, ability)
			{
			}

			// Token: 0x060037B7 RID: 14263 RVA: 0x000A4584 File Offset: 0x000A2784
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x060037B8 RID: 14264 RVA: 0x000A45A3 File Offset: 0x000A27A3
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x060037B9 RID: 14265 RVA: 0x000A45C2 File Offset: 0x000A27C2
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060037BA RID: 14266 RVA: 0x000A45DC File Offset: 0x000A27DC
			private bool OnGiveDamage(ITarget target, ref Damage damage)
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
				double final = this.owner.stat.GetFinal(Stat.Kind.values[this.ability._sourceStat.kindIndex]);
				float num = Mathf.Min(this.ability._maxDamagePercent, (float)(final - 1.0) * this.ability._damagePercentByStatUnit);
				float num2 = Mathf.Min(this.ability._maxDamagePercentPoint, (float)(final - 1.0) * this.ability._damagePercentPointByStatUnit);
				damage.percentMultiplier *= (double)(1f + num);
				damage.multiplier += (double)num2;
				this._remainCooldownTime = this.ability._cooldownTime;
				return false;
			}

			// Token: 0x04002C65 RID: 11365
			private float _remainCooldownTime;
		}
	}
}
