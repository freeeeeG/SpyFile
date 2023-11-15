using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C4A RID: 3146
	[Serializable]
	public class StatBonusByGaveDamage : Ability
	{
		// Token: 0x06004077 RID: 16503 RVA: 0x000BB40D File Offset: 0x000B960D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByGaveDamage.Instance(owner, this);
		}

		// Token: 0x04003187 RID: 12679
		[SerializeField]
		private float _timeToReset;

		// Token: 0x04003188 RID: 12680
		[SerializeField]
		private float _damageToMax;

		// Token: 0x04003189 RID: 12681
		[SerializeField]
		[Space]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x0400318A RID: 12682
		[SerializeField]
		private AttackTypeBoolArray _damageTypeFilter;

		// Token: 0x0400318B RID: 12683
		[SerializeField]
		private DamageAttributeBoolArray _attributeFilter;

		// Token: 0x0400318C RID: 12684
		[SerializeField]
		[Space]
		private Stat.Values _maxStat;

		// Token: 0x02000C4B RID: 3147
		public class Instance : AbilityInstance<StatBonusByGaveDamage>
		{
			// Token: 0x17000D99 RID: 3481
			// (get) Token: 0x06004079 RID: 16505 RVA: 0x000BB416 File Offset: 0x000B9616
			public override Sprite icon
			{
				get
				{
					if (this._gaveDamage <= 0f)
					{
						return null;
					}
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000D9A RID: 3482
			// (get) Token: 0x0600407A RID: 16506 RVA: 0x000BB432 File Offset: 0x000B9632
			public override float iconFillAmount
			{
				get
				{
					return 1f - this._remainTime / this.ability._timeToReset;
				}
			}

			// Token: 0x17000D9B RID: 3483
			// (get) Token: 0x0600407B RID: 16507 RVA: 0x000BB44C File Offset: 0x000B964C
			public override int iconStacks
			{
				get
				{
					return (int)(this._stat.values[0].value * 100.0);
				}
			}

			// Token: 0x0600407C RID: 16508 RVA: 0x000BB46B File Offset: 0x000B966B
			public Instance(Character owner, StatBonusByGaveDamage ability) : base(owner, ability)
			{
				this._stat = ability._maxStat.Clone();
			}

			// Token: 0x0600407D RID: 16509 RVA: 0x000BB488 File Offset: 0x000B9688
			protected override void OnAttach()
			{
				Stat.Value[] values = this._stat.values;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._maxStat.values[i].GetMultipliedValue(0f);
				}
				this.owner.stat.AttachValues(this._stat);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x0600407E RID: 16510 RVA: 0x000BB510 File Offset: 0x000B9710
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			}

			// Token: 0x0600407F RID: 16511 RVA: 0x000BB550 File Offset: 0x000B9750
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._motionTypeFilter[gaveDamage.motionType])
				{
					return;
				}
				if (!this.ability._damageTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				if (!this.ability._attributeFilter[gaveDamage.attribute])
				{
					return;
				}
				this._remainTime = this.ability._timeToReset;
				float gaveDamage2 = this._gaveDamage;
				Damage damage = gaveDamage;
				this._gaveDamage = gaveDamage2 + (float)damage.amount;
				if (this._gaveDamage > this.ability._damageToMax)
				{
					this._gaveDamage = this.ability._damageToMax;
				}
			}

			// Token: 0x06004080 RID: 16512 RVA: 0x000BB608 File Offset: 0x000B9808
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTime -= deltaTime;
				this._remainUpdateTime -= deltaTime;
				if (this._remainTime < 0f)
				{
					this._gaveDamage = 0f;
					this.UpdateStat();
					return;
				}
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime = 0.2f;
					this.UpdateStat();
				}
			}

			// Token: 0x06004081 RID: 16513 RVA: 0x000BB678 File Offset: 0x000B9878
			public void UpdateStat()
			{
				if (this._gaveDamage == this._gaveDamageBefore)
				{
					return;
				}
				Stat.Value[] values = this._stat.values;
				float multiplier = this._gaveDamage / this.ability._damageToMax;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._maxStat.values[i].GetMultipliedValue(multiplier);
				}
				this.owner.stat.SetNeedUpdate();
				this._gaveDamageBefore = this._gaveDamage;
			}

			// Token: 0x0400318D RID: 12685
			private const float _updateInterval = 0.2f;

			// Token: 0x0400318E RID: 12686
			private float _remainUpdateTime;

			// Token: 0x0400318F RID: 12687
			private Stat.Values _stat;

			// Token: 0x04003190 RID: 12688
			private float _gaveDamage;

			// Token: 0x04003191 RID: 12689
			private float _gaveDamageBefore;

			// Token: 0x04003192 RID: 12690
			private float _remainTime;
		}
	}
}
