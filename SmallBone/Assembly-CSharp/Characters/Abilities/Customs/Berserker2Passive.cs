using System;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D2A RID: 3370
	[Serializable]
	public class Berserker2Passive : Ability
	{
		// Token: 0x060043F2 RID: 17394 RVA: 0x000C570B File Offset: 0x000C390B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Berserker2Passive.Instance(owner, this);
		}

		// Token: 0x040033CB RID: 13259
		[Range(0f, 99f)]
		[SerializeField]
		[Space]
		private int _losingHealthPercentOnPolymorph;

		// Token: 0x040033CC RID: 13260
		[SerializeField]
		[Header("Gauge")]
		private ValueGauge _gauge;

		// Token: 0x040033CD RID: 13261
		[SerializeField]
		[Space]
		private float _gettingGaugeAmountByGaveDamage;

		// Token: 0x040033CE RID: 13262
		[SerializeField]
		private float _gettingGaugeAmountByTookDamage;

		// Token: 0x040033CF RID: 13263
		[SerializeField]
		[Space]
		private float _losingGaugeAmountPerSecond;

		// Token: 0x040033D0 RID: 13264
		[SerializeField]
		[Header("Filter")]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x040033D1 RID: 13265
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x040033D2 RID: 13266
		[NonSerialized]
		public Weapon _polymorphWeapon;

		// Token: 0x02000D2B RID: 3371
		public class Instance : AbilityInstance<Berserker2Passive>
		{
			// Token: 0x060043F3 RID: 17395 RVA: 0x000C5714 File Offset: 0x000C3914
			public Instance(Character owner, Berserker2Passive ability) : base(owner, ability)
			{
			}

			// Token: 0x060043F4 RID: 17396 RVA: 0x000C5720 File Offset: 0x000C3920
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this.owner.health.onTookDamage += new TookDamageDelegate(this.OnOwnerTookDamage);
			}

			// Token: 0x060043F5 RID: 17397 RVA: 0x000C5770 File Offset: 0x000C3970
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnOwnerTookDamage);
			}

			// Token: 0x060043F6 RID: 17398 RVA: 0x000C57C0 File Offset: 0x000C39C0
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (!this.owner.playerComponents.combatDetector.inCombat)
				{
					this.ability._gauge.Add(-this.ability._losingGaugeAmountPerSecond * deltaTime);
				}
				this.CheckGaugeAndPolymorph();
			}

			// Token: 0x060043F7 RID: 17399 RVA: 0x000C5810 File Offset: 0x000C3A10
			private void CheckGaugeAndPolymorph()
			{
				if (this.ability._gauge.currentValue < this.ability._gauge.maxValue)
				{
					return;
				}
				if (this.owner.motion != null && this.owner.motion.running)
				{
					return;
				}
				this.ability._gauge.Clear();
				double amount = this.owner.health.currentHealth * (double)this.ability._losingHealthPercentOnPolymorph * 0.01;
				this.owner.health.TakeHealth(amount);
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(amount, this.owner.transform.position);
				this.owner.playerComponents.inventory.weapon.Polymorph(this.ability._polymorphWeapon);
			}

			// Token: 0x060043F8 RID: 17400 RVA: 0x000C58FC File Offset: 0x000C3AFC
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._attackTypeFilter[tookDamage.attackType] || !this.ability._motionTypeFilter[tookDamage.motionType])
				{
					return;
				}
				this.ability._gauge.Add(this.ability._gettingGaugeAmountByGaveDamage);
			}

			// Token: 0x060043F9 RID: 17401 RVA: 0x000C5964 File Offset: 0x000C3B64
			private void OnOwnerTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this.ability._attackTypeFilter[tookDamage.attackType] || !this.ability._motionTypeFilter[tookDamage.motionType])
				{
					return;
				}
				this.ability._gauge.Add(this.ability._gettingGaugeAmountByTookDamage);
			}
		}
	}
}
