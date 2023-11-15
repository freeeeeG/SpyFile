using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Weapons.Wizard
{
	// Token: 0x02000C02 RID: 3074
	[Serializable]
	public sealed class WizardPassive : Ability
	{
		// Token: 0x06003F0D RID: 16141 RVA: 0x000B72EF File Offset: 0x000B54EF
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new WizardPassive.Instance(owner, this);
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x000B72F8 File Offset: 0x000B54F8
		public bool IsMaxGauge()
		{
			return this._gauge.maxValue <= this._gauge.currentValue;
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x000B7315 File Offset: 0x000B5515
		public bool TryReduceMana(float value)
		{
			if (this.transcendence || this.초월상태)
			{
				return false;
			}
			this._gauge.Consume(value);
			return true;
		}

		// Token: 0x040030A0 RID: 12448
		[Header("For Test")]
		[SerializeField]
		private bool 정신집중영향받기;

		// Token: 0x040030A1 RID: 12449
		[SerializeField]
		private bool 초월상태;

		// Token: 0x040030A2 RID: 12450
		[Header("마나 관련")]
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x040030A3 RID: 12451
		[Tooltip("초당 마나 획득량")]
		[SerializeField]
		private float _baseManaChargingSpeed;

		// Token: 0x040030A4 RID: 12452
		[SerializeField]
		private float _maxManaChargingSpeed;

		// Token: 0x040030A5 RID: 12453
		[Tooltip("스킬 쿨다운 속도 1%p당 추가 증가량")]
		[SerializeField]
		private float _manaChargingSpeedBonusBySkillCooldown;

		// Token: 0x040030A6 RID: 12454
		[Header("레전더리 초월")]
		[SerializeField]
		private Characters.Actions.Action _transcendenceAction;

		// Token: 0x040030A7 RID: 12455
		[SerializeField]
		private Weapon _polymorphWeapon;

		// Token: 0x040030A8 RID: 12456
		[NonSerialized]
		public float manaChargingSpeedMultiplier = 1f;

		// Token: 0x040030A9 RID: 12457
		[NonSerialized]
		public bool transcendence;

		// Token: 0x02000C03 RID: 3075
		public class Instance : AbilityInstance<WizardPassive>
		{
			// Token: 0x17000D52 RID: 3410
			// (get) Token: 0x06003F11 RID: 16145 RVA: 0x000B734A File Offset: 0x000B554A
			public override Sprite icon
			{
				get
				{
					if (!(this.ability._transcendenceAction == null))
					{
						return base.icon;
					}
					return null;
				}
			}

			// Token: 0x17000D53 RID: 3411
			// (get) Token: 0x06003F12 RID: 16146 RVA: 0x000B7368 File Offset: 0x000B5568
			public override float iconFillAmount
			{
				get
				{
					if (this.ability._transcendenceAction == null)
					{
						return 0f;
					}
					if (this.ability._transcendenceAction.cooldown.canUse)
					{
						return 0f;
					}
					return this.ability._transcendenceAction.cooldown.time.remainTime / this.ability._transcendenceAction.cooldown.time.cooldownTime;
				}
			}

			// Token: 0x06003F13 RID: 16147 RVA: 0x000B73E0 File Offset: 0x000B55E0
			public Instance(Character owner, WizardPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003F14 RID: 16148 RVA: 0x000B73EC File Offset: 0x000B55EC
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this.owner.playerComponents.inventory.weapon.polymorphWeapon != null && this.ability._polymorphWeapon != null && this.owner.playerComponents.inventory.weapon.polymorphWeapon.name.Equals(this.ability._polymorphWeapon.name))
				{
					return;
				}
				float num = (this.owner.stat.GetSkillCooldownSpeed() - 1f) * 100f * this.ability._manaChargingSpeedBonusBySkillCooldown;
				this._finalManaChargingSpeed = this.ability._baseManaChargingSpeed * this.ability.manaChargingSpeedMultiplier + num;
				if (this.ability.정신집중영향받기)
				{
					this._finalManaChargingSpeed *= (float)this.owner.stat.GetFinal(Stat.Kind.ChargingSpeed);
				}
				float amount = this._finalManaChargingSpeed * deltaTime;
				this.ability._gauge.Add(amount);
			}

			// Token: 0x06003F15 RID: 16149 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06003F16 RID: 16150 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x06003F17 RID: 16151 RVA: 0x000B74FE File Offset: 0x000B56FE
			public void AddGauge(float value)
			{
				this.ability._gauge.Add(value);
			}

			// Token: 0x06003F18 RID: 16152 RVA: 0x000B7511 File Offset: 0x000B5711
			public void FillUp()
			{
				this.ability._gauge.FillUp();
			}

			// Token: 0x040030AA RID: 12458
			private float _finalManaChargingSpeed;
		}
	}
}
