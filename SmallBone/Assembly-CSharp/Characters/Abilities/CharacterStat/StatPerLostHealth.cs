using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C89 RID: 3209
	[Serializable]
	public class StatPerLostHealth : Ability
	{
		// Token: 0x06004168 RID: 16744 RVA: 0x000BE464 File Offset: 0x000BC664
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatPerLostHealth.Instance(owner, this);
		}

		// Token: 0x0400322E RID: 12846
		[SerializeField]
		private Stat.Values _statPerLostHealth;

		// Token: 0x0400322F RID: 12847
		[SerializeField]
		[Range(0f, 1f)]
		private float _maxLostPercent;

		// Token: 0x02000C8A RID: 3210
		public class Instance : AbilityInstance<StatPerLostHealth>
		{
			// Token: 0x17000DC1 RID: 3521
			// (get) Token: 0x0600416A RID: 16746 RVA: 0x000BE46D File Offset: 0x000BC66D
			public override int iconStacks
			{
				get
				{
					return (int)(this._firstStatValue * 100.0);
				}
			}

			// Token: 0x0600416B RID: 16747 RVA: 0x000BE480 File Offset: 0x000BC680
			public Instance(Character owner, StatPerLostHealth ability) : base(owner, ability)
			{
				this._stat = ability._statPerLostHealth.Clone();
			}

			// Token: 0x0600416C RID: 16748 RVA: 0x000BE49C File Offset: 0x000BC69C
			protected override void OnAttach()
			{
				this.UpdateStat();
				this.owner.health.onTookDamage += new TookDamageDelegate(this.UpdateStat);
				this.owner.health.onHealed += this.UpdateStat;
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x0600416D RID: 16749 RVA: 0x000BE500 File Offset: 0x000BC700
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.UpdateStat);
				this.owner.health.onHealed -= this.UpdateStat;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600416E RID: 16750 RVA: 0x000BE55B File Offset: 0x000BC75B
			private void UpdateStat(double healed, double overHealed)
			{
				this.UpdateStat();
			}

			// Token: 0x0600416F RID: 16751 RVA: 0x000BE55B File Offset: 0x000BC75B
			private void UpdateStat(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				this.UpdateStat();
			}

			// Token: 0x06004170 RID: 16752 RVA: 0x000BE564 File Offset: 0x000BC764
			private void UpdateStat()
			{
				double num = Math.Min(1.0 - this.owner.health.percent, (double)this.ability._maxLostPercent) * 100.0;
				this._firstStatValue = num * this.ability._statPerLostHealth.values[0].value;
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					double num2 = num * this.ability._statPerLostHealth.values[i].value;
					if (this.ability._statPerLostHealth.values[i].categoryIndex == Stat.Category.Percent.index)
					{
						num2 += 1.0;
					}
					this._stat.values[i].value = num2;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003230 RID: 12848
			private Stat.Values _stat;

			// Token: 0x04003231 RID: 12849
			private double _firstStatValue;
		}
	}
}
