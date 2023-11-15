using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C4D RID: 3149
	[Serializable]
	public class StatBonusByIncome : Ability
	{
		// Token: 0x06004083 RID: 16515 RVA: 0x000BB705 File Offset: 0x000B9905
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByIncome.Instance(owner, this);
		}

		// Token: 0x04003193 RID: 12691
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x04003194 RID: 12692
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003195 RID: 12693
		[SerializeField]
		private int _goldPerStack;

		// Token: 0x04003196 RID: 12694
		[SerializeField]
		private int _darkQuartzPerStack;

		// Token: 0x02000C4E RID: 3150
		public class Instance : AbilityInstance<StatBonusByIncome>
		{
			// Token: 0x06004085 RID: 16517 RVA: 0x000BB70E File Offset: 0x000B990E
			public Instance(Character owner, StatBonusByIncome ability) : base(owner, ability)
			{
			}

			// Token: 0x06004086 RID: 16518 RVA: 0x000BB718 File Offset: 0x000B9918
			protected override void OnAttach()
			{
				GameData.Currency.gold.onEarn += this.OnGoldEarn;
				GameData.Currency.darkQuartz.onEarn += this.OnDarkQuartzEarn;
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x06004087 RID: 16519 RVA: 0x000BB780 File Offset: 0x000B9980
			protected override void OnDetach()
			{
				GameData.Currency.gold.onEarn -= this.OnGoldEarn;
				GameData.Currency.darkQuartz.onEarn -= this.OnDarkQuartzEarn;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004088 RID: 16520 RVA: 0x000BB7CF File Offset: 0x000B99CF
			private void OnGoldEarn(int amount)
			{
				amount += this._remainGoldForStack;
				this.AddStack(amount / this.ability._goldPerStack);
				this._remainGoldForStack = amount % this.ability._goldPerStack;
			}

			// Token: 0x06004089 RID: 16521 RVA: 0x000BB801 File Offset: 0x000B9A01
			private void OnDarkQuartzEarn(int amount)
			{
				amount += this._remainDarkQuartzForStack;
				this.AddStack(amount / this.ability._darkQuartzPerStack);
				this._remainDarkQuartzForStack = amount % this.ability._darkQuartzPerStack;
			}

			// Token: 0x0600408A RID: 16522 RVA: 0x000BB834 File Offset: 0x000B9A34
			private void AddStack(int amount)
			{
				this._stacks += amount;
				if (this.ability._maxStack > 0 && this._stacks > this.ability._maxStack)
				{
					this._stacks = this.ability._maxStack;
					GameData.Currency.gold.onEarn -= this.OnGoldEarn;
					GameData.Currency.darkQuartz.onEarn -= this.OnDarkQuartzEarn;
					this.UpdateStat();
					return;
				}
				this.UpdateStat();
			}

			// Token: 0x0600408B RID: 16523 RVA: 0x000BB8BC File Offset: 0x000B9ABC
			private void UpdateStat()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = (double)this._stacks * this.ability._statPerStack.values[i].value;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003197 RID: 12695
			private int _remainGoldForStack;

			// Token: 0x04003198 RID: 12696
			private int _remainDarkQuartzForStack;

			// Token: 0x04003199 RID: 12697
			private int _stacks;

			// Token: 0x0400319A RID: 12698
			private Stat.Values _stat;
		}
	}
}
