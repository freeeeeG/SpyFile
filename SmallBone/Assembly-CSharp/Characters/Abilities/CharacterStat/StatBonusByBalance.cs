using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C47 RID: 3143
	[Serializable]
	public sealed class StatBonusByBalance : Ability
	{
		// Token: 0x0600406E RID: 16494 RVA: 0x000BB21A File Offset: 0x000B941A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByBalance.Instance(owner, this);
		}

		// Token: 0x04003181 RID: 12673
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04003182 RID: 12674
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x04003183 RID: 12675
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003184 RID: 12676
		[SerializeField]
		private int _balancePerStack;

		// Token: 0x02000C48 RID: 3144
		public sealed class Instance : AbilityInstance<StatBonusByBalance>
		{
			// Token: 0x06004070 RID: 16496 RVA: 0x000BB223 File Offset: 0x000B9423
			public Instance(Character owner, StatBonusByBalance ability) : base(owner, ability)
			{
			}

			// Token: 0x06004071 RID: 16497 RVA: 0x000BB230 File Offset: 0x000B9430
			protected override void OnAttach()
			{
				GameData.Currency.currencies[this.ability._type].onEarn += this.UpdateStackAndStat;
				GameData.Currency.currencies[this.ability._type].onConsume += this.UpdateStackAndStat;
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStackAndStat(0);
			}

			// Token: 0x06004072 RID: 16498 RVA: 0x000BB2BC File Offset: 0x000B94BC
			protected override void OnDetach()
			{
				this.DetachEvent();
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004073 RID: 16499 RVA: 0x000BB2DC File Offset: 0x000B94DC
			private void DetachEvent()
			{
				GameData.Currency.currencies[this.ability._type].onEarn -= this.UpdateStackAndStat;
				GameData.Currency.currencies[this.ability._type].onConsume -= this.UpdateStackAndStat;
			}

			// Token: 0x06004074 RID: 16500 RVA: 0x000BB338 File Offset: 0x000B9538
			private void UpdateStackAndStat(int amount)
			{
				int num = GameData.Currency.currencies[this.ability._type].balance / this.ability._balancePerStack;
				this._stacks = num;
				if (this.ability._maxStack > 0)
				{
					this._stacks = Mathf.Min(num, this.ability._maxStack);
				}
				this.UpdateStat();
			}

			// Token: 0x06004075 RID: 16501 RVA: 0x000BB3A0 File Offset: 0x000B95A0
			private void UpdateStat()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this._stacks);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003185 RID: 12677
			private int _stacks;

			// Token: 0x04003186 RID: 12678
			private Stat.Values _stat;
		}
	}
}
