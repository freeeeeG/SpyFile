using System;
using Characters.Gear.Weapons;
using Characters.Operations;
using Characters.Player;
using FX;
using FX.BoundsAttackVisualEffect;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D35 RID: 3381
	[Serializable]
	public class BoneOfSpeed : Ability
	{
		// Token: 0x06004436 RID: 17462 RVA: 0x000C62A8 File Offset: 0x000C44A8
		public BoneOfSpeed()
		{
			this._damagePercents = new double[]
			{
				this._damagePercent0,
				this._damagePercent1,
				this._damagePercent2
			};
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x000C6329 File Offset: 0x000C4529
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new BoneOfSpeed.Instance(owner, this);
		}

		// Token: 0x040033FE RID: 13310
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x040033FF RID: 13311
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x04003400 RID: 13312
		[SerializeField]
		private HitInfo _hitInfo;

		// Token: 0x04003401 RID: 13313
		[SerializeField]
		private double _baseDamage = 10.0;

		// Token: 0x04003402 RID: 13314
		[SerializeField]
		private float _cooldownTime = 0.5f;

		// Token: 0x04003403 RID: 13315
		[Tooltip("스피드 타입 스컬 개수가 0개일 때 피해량 증폭")]
		[SerializeField]
		private double _damagePercent0 = 0.35;

		// Token: 0x04003404 RID: 13316
		[Tooltip("스피드 타입 스컬 개수가 1개일 때 피해량 증폭")]
		[SerializeField]
		private double _damagePercent1 = 0.42;

		// Token: 0x04003405 RID: 13317
		[SerializeField]
		[Tooltip("스피드 타입 스컬 개수가 2개일 때 피해량 증폭")]
		private double _damagePercent2 = 0.56;

		// Token: 0x04003406 RID: 13318
		[SerializeField]
		[BoundsAttackVisualEffect.SubcomponentAttribute]
		private BoundsAttackVisualEffect.Subcomponents _hitEffect;

		// Token: 0x04003407 RID: 13319
		[SerializeField]
		private SoundInfo _attackSoundInfo;

		// Token: 0x04003408 RID: 13320
		private double[] _damagePercents;

		// Token: 0x02000D36 RID: 3382
		public class Instance : AbilityInstance<BoneOfSpeed>
		{
			// Token: 0x17000E28 RID: 3624
			// (get) Token: 0x06004438 RID: 17464 RVA: 0x000C6332 File Offset: 0x000C4532
			public override int iconStacks
			{
				get
				{
					return this._speedHeadCount;
				}
			}

			// Token: 0x06004439 RID: 17465 RVA: 0x000C633A File Offset: 0x000C453A
			public Instance(Character owner, BoneOfSpeed ability) : base(owner, ability)
			{
				this._weaponInventory = owner.playerComponents.inventory.weapon;
				this.UpdateSpeedHeadCount();
			}

			// Token: 0x0600443A RID: 17466 RVA: 0x000C6360 File Offset: 0x000C4560
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this._weaponInventory.onChanged += this.OnWeaponChanged;
				this._remainCooldown = this.ability._cooldownTime;
			}

			// Token: 0x0600443B RID: 17467 RVA: 0x000C63BC File Offset: 0x000C45BC
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this._weaponInventory.onChanged -= this.OnWeaponChanged;
			}

			// Token: 0x0600443C RID: 17468 RVA: 0x000C63FC File Offset: 0x000C45FC
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldown -= deltaTime;
			}

			// Token: 0x0600443D RID: 17469 RVA: 0x000C6414 File Offset: 0x000C4614
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (!this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._damageTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				if (this._remainCooldown > 0f)
				{
					return;
				}
				this._remainCooldown = this.ability._cooldownTime;
				this.ability._hitInfo.ChangeAdaptiveDamageAttribute(this.owner);
				Damage damage = this.owner.stat.GetDamage(this.ability._baseDamage * this.ability._damagePercents[this._speedHeadCount], MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._hitInfo);
				this.owner.Attack(target, ref damage);
				this.ability._hitEffect.Spawn(this.owner, target.collider.bounds, damage, target);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attackSoundInfo, target.transform.position);
			}

			// Token: 0x0600443E RID: 17470 RVA: 0x000C6524 File Offset: 0x000C4724
			private void OnWeaponChanged(Weapon old, Weapon @new)
			{
				this.UpdateSpeedHeadCount();
			}

			// Token: 0x0600443F RID: 17471 RVA: 0x000C652C File Offset: 0x000C472C
			private void UpdateSpeedHeadCount()
			{
				this._speedHeadCount = this._weaponInventory.GetCountByCategory(Weapon.Category.Speed);
			}

			// Token: 0x04003409 RID: 13321
			private WeaponInventory _weaponInventory;

			// Token: 0x0400340A RID: 13322
			private int _speedHeadCount;

			// Token: 0x0400340B RID: 13323
			private float _remainCooldown;
		}
	}
}
