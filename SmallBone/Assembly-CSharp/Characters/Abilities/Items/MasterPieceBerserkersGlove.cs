using System;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CDB RID: 3291
	[Serializable]
	public class MasterPieceBerserkersGlove : Ability
	{
		// Token: 0x0600429E RID: 17054 RVA: 0x000C1F6F File Offset: 0x000C016F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new MasterPieceBerserkersGlove.Instance(owner, this);
		}

		// Token: 0x040032FA RID: 13050
		[SerializeField]
		private Stat.Values _statPerLostHealth;

		// Token: 0x040032FB RID: 13051
		[Range(0f, 1f)]
		[SerializeField]
		private float _maxLostPercent;

		// Token: 0x040032FC RID: 13052
		[Range(0f, 1f)]
		[Header("Bonus Stat")]
		[SerializeField]
		private float _bonusHealthPercent;

		// Token: 0x040032FD RID: 13053
		[SerializeField]
		private Stat.Values _bonus;

		// Token: 0x02000CDC RID: 3292
		public class Instance : AbilityInstance<MasterPieceBerserkersGlove>
		{
			// Token: 0x17000DE1 RID: 3553
			// (get) Token: 0x060042A0 RID: 17056 RVA: 0x000C1F78 File Offset: 0x000C0178
			public override int iconStacks
			{
				get
				{
					return (int)(this._statValue * 100.0) + this._bonusStack;
				}
			}

			// Token: 0x060042A1 RID: 17057 RVA: 0x000C1F92 File Offset: 0x000C0192
			public Instance(Character owner, MasterPieceBerserkersGlove ability) : base(owner, ability)
			{
				this._stat = ability._statPerLostHealth.Clone();
			}

			// Token: 0x060042A2 RID: 17058 RVA: 0x000C1FB0 File Offset: 0x000C01B0
			protected override void OnAttach()
			{
				this._bonusStack = 0;
				this.owner.health.onTookDamage += new TookDamageDelegate(this.UpdateStat);
				this.owner.health.onHealed += this.UpdateStat;
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStat();
			}

			// Token: 0x060042A3 RID: 17059 RVA: 0x000C2018 File Offset: 0x000C0218
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.UpdateStat);
				this.owner.health.onHealed -= this.UpdateStat;
				this.owner.stat.DetachValues(this._stat);
				this.owner.stat.DetachValues(this.ability._bonus);
			}

			// Token: 0x060042A4 RID: 17060 RVA: 0x000C208E File Offset: 0x000C028E
			private void UpdateStat(double healed, double overHealed)
			{
				this.UpdateStat();
			}

			// Token: 0x060042A5 RID: 17061 RVA: 0x000C208E File Offset: 0x000C028E
			private void UpdateStat(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				this.UpdateStat();
			}

			// Token: 0x060042A6 RID: 17062 RVA: 0x000C2098 File Offset: 0x000C0298
			private void UpdateStat()
			{
				double num = Math.Min(1.0 - this.owner.health.percent, (double)this.ability._maxLostPercent) * 100.0;
				this._statValue = num * this.ability._statPerLostHealth.values[0].value;
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					double num2 = num * this.ability._statPerLostHealth.values[i].value;
					if (this.ability._statPerLostHealth.values[i].categoryIndex == Stat.Category.Percent.index)
					{
						num2 += 1.0;
					}
					this._stat.values[i].value = num2;
				}
				if (this.owner.health.percent <= (double)this.ability._bonusHealthPercent)
				{
					if (this._bonusStack <= 0)
					{
						this._bonusStack = (int)(this.ability._bonus.values[0].value * 100.0);
						this.owner.stat.AttachValues(this.ability._bonus);
					}
				}
				else
				{
					this._bonusStack = 0;
					this.owner.stat.DetachValues(this.ability._bonus);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040032FE RID: 13054
			private Stat.Values _stat;

			// Token: 0x040032FF RID: 13055
			private double _statValue;

			// Token: 0x04003300 RID: 13056
			private int _bonusStack;
		}
	}
}
