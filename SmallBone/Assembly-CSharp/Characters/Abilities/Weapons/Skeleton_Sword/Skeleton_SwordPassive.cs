using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.Skeleton_Sword
{
	// Token: 0x02000BF6 RID: 3062
	[Serializable]
	public class Skeleton_SwordPassive : Ability
	{
		// Token: 0x06003EEB RID: 16107 RVA: 0x000B6E8B File Offset: 0x000B508B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Skeleton_SwordPassive.Instance(owner, this);
		}

		// Token: 0x0400308E RID: 12430
		[Tooltip("기본 상처 부여 확률")]
		[SerializeField]
		[Range(0f, 100f)]
		private int _chance;

		// Token: 0x0400308F RID: 12431
		[Tooltip("교대 보너스 지속 시간")]
		[SerializeField]
		private float _bonusDuration;

		// Token: 0x04003090 RID: 12432
		[SerializeField]
		[Tooltip("교대 보너스가 있을 때 상처 부여 확률")]
		[Range(0f, 100f)]
		private int _chanceWithBonus;

		// Token: 0x04003091 RID: 12433
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04003092 RID: 12434
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x04003093 RID: 12435
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x02000BF7 RID: 3063
		public class Instance : AbilityInstance<Skeleton_SwordPassive>
		{
			// Token: 0x17000D4F RID: 3407
			// (get) Token: 0x06003EED RID: 16109 RVA: 0x000B6E94 File Offset: 0x000B5094
			public override float iconFillAmount
			{
				get
				{
					return 1f - this._remainBonusTime / this.ability._bonusDuration;
				}
			}

			// Token: 0x17000D50 RID: 3408
			// (get) Token: 0x06003EEE RID: 16110 RVA: 0x000B6EAE File Offset: 0x000B50AE
			public override Sprite icon
			{
				get
				{
					if (this._remainBonusTime <= 0f)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x06003EEF RID: 16111 RVA: 0x000B6EC5 File Offset: 0x000B50C5
			public Instance(Character owner, Skeleton_SwordPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003EF0 RID: 16112 RVA: 0x000B6ECF File Offset: 0x000B50CF
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
				this._remainBonusTime = this.ability._bonusDuration;
			}

			// Token: 0x06003EF1 RID: 16113 RVA: 0x000B6F09 File Offset: 0x000B5109
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003EF2 RID: 16114 RVA: 0x000B6F32 File Offset: 0x000B5132
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainBonusTime -= deltaTime;
			}

			// Token: 0x06003EF3 RID: 16115 RVA: 0x000B6F4C File Offset: 0x000B514C
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null || target.character.health.dead)
				{
					return;
				}
				if (!this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._damageTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				if (!MMMaths.PercentChance((this._remainBonusTime < 0f) ? this.ability._chance : this.ability._chanceWithBonus))
				{
					return;
				}
				this.owner.GiveStatus(target.character, this.ability._status);
			}

			// Token: 0x04003094 RID: 12436
			private float _remainBonusTime;
		}
	}
}
