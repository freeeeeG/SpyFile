using System;
using Characters.Gear.Weapons;
using Characters.Player;
using FX;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D2F RID: 3375
	[Serializable]
	public class BoneOfBrave : Ability
	{
		// Token: 0x0600441E RID: 17438 RVA: 0x000C5E54 File Offset: 0x000C4054
		public override void Initialize()
		{
			base.Initialize();
			this.damagePercents = new double[]
			{
				this._damagePercent0,
				this._damagePercent1,
				this._damagePercent2
			};
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x000C5E83 File Offset: 0x000C4083
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BoneOfBrave.Instance(owner, this);
		}

		// Token: 0x040033E6 RID: 13286
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x040033E7 RID: 13287
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x040033E8 RID: 13288
		[SerializeField]
		private DamageAttributeBoolArray _attributeFilter;

		// Token: 0x040033E9 RID: 13289
		[SerializeField]
		private EffectInfo _effectOnCanUse;

		// Token: 0x040033EA RID: 13290
		[SerializeField]
		private EffectInfo _effectOnStart;

		// Token: 0x040033EB RID: 13291
		[SerializeField]
		private float _abilityTime;

		// Token: 0x040033EC RID: 13292
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x040033ED RID: 13293
		[SerializeField]
		[Tooltip("파워 타입 스컬 개수가 0개일 때 피해량 증폭")]
		private double _damagePercent0;

		// Token: 0x040033EE RID: 13294
		[Tooltip("파워 타입 스컬 개수가 1개일 때 피해량 증폭")]
		[SerializeField]
		private double _damagePercent1;

		// Token: 0x040033EF RID: 13295
		[Tooltip("파워 타입 스컬 개수가 2개일 때 피해량 증폭")]
		[SerializeField]
		private double _damagePercent2;

		// Token: 0x040033F0 RID: 13296
		private double[] damagePercents;

		// Token: 0x02000D30 RID: 3376
		public class Instance : AbilityInstance<BoneOfBrave>
		{
			// Token: 0x17000E26 RID: 3622
			// (get) Token: 0x06004421 RID: 17441 RVA: 0x000C5E8C File Offset: 0x000C408C
			public override float iconFillAmount
			{
				get
				{
					return this._remainCooldownTime / this.ability._cooldownTime;
				}
			}

			// Token: 0x06004422 RID: 17442 RVA: 0x000C5EA0 File Offset: 0x000C40A0
			public Instance(Character owner, BoneOfBrave ability) : base(owner, ability)
			{
				this._weaponInventory = owner.playerComponents.inventory.weapon;
				this.UpdatePowerHeadCount();
			}

			// Token: 0x06004423 RID: 17443 RVA: 0x000C5EC6 File Offset: 0x000C40C6
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
				this._weaponInventory.onChanged += this.OnWeaponChanged;
			}

			// Token: 0x06004424 RID: 17444 RVA: 0x000C5EFC File Offset: 0x000C40FC
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
				this._weaponInventory.onChanged -= this.OnWeaponChanged;
			}

			// Token: 0x06004425 RID: 17445 RVA: 0x000C5F34 File Offset: 0x000C4134
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
				if (this._attached)
				{
					this._remainAbilityTime -= deltaTime;
					if (this._remainAbilityTime <= 0f)
					{
						this._attached = false;
					}
				}
				if (!this._canUse && this._remainCooldownTime < 0f)
				{
					this.ability._effectOnCanUse.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
					this._canUse = true;
				}
			}

			// Token: 0x06004426 RID: 17446 RVA: 0x000C5FD0 File Offset: 0x000C41D0
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this.ability._motionTypeFilter[damage.motionType])
				{
					return false;
				}
				if (!this.ability._damageTypeFilter[damage.attackType])
				{
					return false;
				}
				if (!this.ability._attributeFilter[damage.attribute])
				{
					return false;
				}
				if (this._attached)
				{
					if (this._remainAbilityTime < 0f)
					{
						return false;
					}
				}
				else
				{
					if (this._remainCooldownTime > 0f)
					{
						return false;
					}
					this.ability._effectOnStart.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
					this._attached = true;
					this._canUse = false;
					this._remainAbilityTime = this.ability._abilityTime;
					this._remainCooldownTime = this.ability._cooldownTime;
				}
				damage.percentMultiplier *= this.ability.damagePercents[this._powerHeadCount];
				return false;
			}

			// Token: 0x06004427 RID: 17447 RVA: 0x000C60CF File Offset: 0x000C42CF
			private void OnWeaponChanged(Weapon old, Weapon @new)
			{
				this.UpdatePowerHeadCount();
			}

			// Token: 0x06004428 RID: 17448 RVA: 0x000C60D7 File Offset: 0x000C42D7
			private void UpdatePowerHeadCount()
			{
				this._powerHeadCount = this._weaponInventory.GetCountByCategory(Weapon.Category.Power);
			}

			// Token: 0x040033F1 RID: 13297
			private WeaponInventory _weaponInventory;

			// Token: 0x040033F2 RID: 13298
			private float _remainCooldownTime;

			// Token: 0x040033F3 RID: 13299
			private float _remainAbilityTime;

			// Token: 0x040033F4 RID: 13300
			private int _powerHeadCount;

			// Token: 0x040033F5 RID: 13301
			private bool _canUse;

			// Token: 0x040033F6 RID: 13302
			private bool _attached;
		}
	}
}
