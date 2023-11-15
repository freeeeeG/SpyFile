using System;
using Characters.Abilities.Constraints;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D22 RID: 3362
	[Serializable]
	public class AlchemistPassive : Ability
	{
		// Token: 0x060043C1 RID: 17345 RVA: 0x000C52E3 File Offset: 0x000C34E3
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AlchemistPassive.Instance(owner, this);
		}

		// Token: 0x040033BB RID: 13243
		[SerializeField]
		[Header("Passive Settings")]
		private ValueGauge _valueGague;

		// Token: 0x040033BC RID: 13244
		[SerializeField]
		private float _gaugeAmountPerSecond;

		// Token: 0x040033BD RID: 13245
		[SerializeField]
		private bool _multiplyGaugeAmountByCooldown;

		// Token: 0x040033BE RID: 13246
		[SerializeField]
		[Space]
		private AlchemistGaugeBoostComponent _boost;

		// Token: 0x040033BF RID: 13247
		[SerializeField]
		[Space]
		private AlchemistGaugeDeactivateComponent _deactivate;

		// Token: 0x040033C0 RID: 13248
		[Space]
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x02000D23 RID: 3363
		public class Instance : AbilityInstance<AlchemistPassive>
		{
			// Token: 0x060043C3 RID: 17347 RVA: 0x000C52EC File Offset: 0x000C34EC
			public Instance(Character owner, AlchemistPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x060043C4 RID: 17348 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x060043C5 RID: 17349 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x060043C6 RID: 17350 RVA: 0x000C52F8 File Offset: 0x000C34F8
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (!this.ability._constraints.Pass())
				{
					return;
				}
				if (this.ability._deactivate.baseAbility.attached)
				{
					this.ability._valueGague.Clear();
					return;
				}
				float num = this.ability._gaugeAmountPerSecond * deltaTime;
				if (this.ability._multiplyGaugeAmountByCooldown)
				{
					num *= (float)this.owner.stat.GetFinal(Stat.Kind.SkillCooldownSpeed);
				}
				if (this.ability._boost.baseAbility.attached)
				{
					num *= (float)this.ability._boost.baseAbility.multiplier;
				}
				this.ability._valueGague.Add(num);
			}
		}
	}
}
