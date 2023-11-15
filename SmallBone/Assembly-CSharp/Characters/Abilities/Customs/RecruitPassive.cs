using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D7F RID: 3455
	[Serializable]
	public class RecruitPassive : Ability
	{
		// Token: 0x06004592 RID: 17810 RVA: 0x000C9A69 File Offset: 0x000C7C69
		public override void Initialize()
		{
			base.Initialize();
			this._summoningOperation.Initialize();
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x000C9A7C File Offset: 0x000C7C7C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RecruitPassive.Instance(owner, this);
		}

		// Token: 0x040034DA RID: 13530
		[SerializeField]
		[Header("Gauge")]
		private ValueGauge _gauge;

		// Token: 0x040034DB RID: 13531
		[SerializeField]
		private Color _fullGaugeColor;

		// Token: 0x040034DC RID: 13532
		[SerializeField]
		private RecruitPassive.GaugeAmountByAttackKey[] _attackKeyAndGaugeAmounts;

		// Token: 0x040034DD RID: 13533
		[SerializeField]
		[Header("Summon")]
		private float _summoningDuration;

		// Token: 0x040034DE RID: 13534
		[SerializeField]
		private float _summoningInterval;

		// Token: 0x040034DF RID: 13535
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _summoningOperation;

		// Token: 0x02000D80 RID: 3456
		public class Instance : AbilityInstance<RecruitPassive>
		{
			// Token: 0x06004595 RID: 17813 RVA: 0x000C9A85 File Offset: 0x000C7C85
			public Instance(Character owner, RecruitPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06004596 RID: 17814 RVA: 0x000C9A90 File Offset: 0x000C7C90
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				this.ability._gauge.onChanged += this.OnGaugeChanged;
			}

			// Token: 0x06004597 RID: 17815 RVA: 0x000C9AE0 File Offset: 0x000C7CE0
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				if (this._remainSummoningTime > 0f)
				{
					this.ability._gauge.defaultBarColor = this._defaultGaugeColor;
					this.ability._gauge.Clear();
				}
			}

			// Token: 0x06004598 RID: 17816 RVA: 0x000C9B48 File Offset: 0x000C7D48
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._remainSummoningTime <= 0f)
				{
					if (this.ability._gauge.currentValue == this.ability._gauge.maxValue)
					{
						this.ability._gauge.defaultBarColor = this._defaultGaugeColor;
						this.ability._gauge.Clear();
					}
					return;
				}
				this._remainSummoningInterval -= deltaTime;
				this._remainSummoningTime -= deltaTime;
				if (this._remainSummoningInterval < 0f)
				{
					this.ability._summoningOperation.Run(this.owner);
					this._remainSummoningInterval += this.ability._summoningInterval;
				}
			}

			// Token: 0x06004599 RID: 17817 RVA: 0x000C9C0C File Offset: 0x000C7E0C
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (string.IsNullOrEmpty(gaveDamage.key))
				{
					return;
				}
				for (int i = 0; i < this.ability._attackKeyAndGaugeAmounts.Length; i++)
				{
					RecruitPassive.GaugeAmountByAttackKey gaugeAmountByAttackKey = this.ability._attackKeyAndGaugeAmounts[i];
					if (string.Equals(gaugeAmountByAttackKey.attackKey, gaveDamage.key, StringComparison.OrdinalIgnoreCase))
					{
						this.ability._gauge.Add((float)gaugeAmountByAttackKey.gaugeAmountByAttack);
						return;
					}
				}
			}

			// Token: 0x0600459A RID: 17818 RVA: 0x000C9C7C File Offset: 0x000C7E7C
			private void OnGaugeChanged(float oldValue, float newValue)
			{
				if (oldValue >= newValue || newValue < this.ability._gauge.maxValue)
				{
					return;
				}
				this._defaultGaugeColor = this.ability._gauge.defaultBarColor;
				this.ability._gauge.defaultBarColor = this.ability._fullGaugeColor;
				this._remainSummoningTime = this.ability._summoningDuration;
				this._remainSummoningInterval = 0f;
			}

			// Token: 0x040034E0 RID: 13536
			private Color _defaultGaugeColor;

			// Token: 0x040034E1 RID: 13537
			private float _remainSummoningTime;

			// Token: 0x040034E2 RID: 13538
			private float _remainSummoningInterval;
		}

		// Token: 0x02000D81 RID: 3457
		[Serializable]
		private class GaugeAmountByAttackKey
		{
			// Token: 0x040034E3 RID: 13539
			public string attackKey;

			// Token: 0x040034E4 RID: 13540
			public int gaugeAmountByAttack;
		}
	}
}
