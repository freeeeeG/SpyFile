using System;
using Data;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C5F RID: 3167
	[Serializable]
	public sealed class StatBonusByOutcome : Ability
	{
		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x000BC41C File Offset: 0x000BA61C
		// (set) Token: 0x060040CC RID: 16588 RVA: 0x000BC424 File Offset: 0x000BA624
		public int stack
		{
			get
			{
				return this._stack;
			}
			set
			{
				if (this._instance == null)
				{
					return;
				}
				this._stack = value;
				this._instance.TryUpdateStat();
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x000BC441 File Offset: 0x000BA641
		public void Load(Character owner, int stack)
		{
			owner.ability.Add(this);
			this.stack = stack;
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x000BC457 File Offset: 0x000BA657
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._instance = new StatBonusByOutcome.Instance(owner, this);
			return this._instance;
		}

		// Token: 0x040031C6 RID: 12742
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040031C7 RID: 12743
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031C8 RID: 12744
		[SerializeField]
		private int _goldPerStack;

		// Token: 0x040031C9 RID: 12745
		[SerializeField]
		private int _darkQuartzPerStack;

		// Token: 0x040031CA RID: 12746
		private StatBonusByOutcome.Instance _instance;

		// Token: 0x040031CB RID: 12747
		private int _stack;

		// Token: 0x02000C60 RID: 3168
		public class Instance : AbilityInstance<StatBonusByOutcome>
		{
			// Token: 0x17000DA9 RID: 3497
			// (get) Token: 0x060040D0 RID: 16592 RVA: 0x000BC46C File Offset: 0x000BA66C
			public override int iconStacks
			{
				get
				{
					return this.ability.stack;
				}
			}

			// Token: 0x060040D1 RID: 16593 RVA: 0x000BC479 File Offset: 0x000BA679
			public Instance(Character owner, StatBonusByOutcome ability) : base(owner, ability)
			{
			}

			// Token: 0x060040D2 RID: 16594 RVA: 0x000BC484 File Offset: 0x000BA684
			protected override void OnAttach()
			{
				GameData.Currency.gold.onConsume += this.OnGoldConsume;
				GameData.Currency.darkQuartz.onConsume += this.OnDarkQuartzConsume;
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.TryUpdateStat();
			}

			// Token: 0x060040D3 RID: 16595 RVA: 0x000BC4F0 File Offset: 0x000BA6F0
			protected override void OnDetach()
			{
				GameData.Currency.gold.onEarn -= this.OnGoldConsume;
				GameData.Currency.darkQuartz.onEarn -= this.OnDarkQuartzConsume;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x060040D4 RID: 16596 RVA: 0x000BC53F File Offset: 0x000BA73F
			private void OnGoldConsume(int amount)
			{
				if (this.ability._goldPerStack == 0)
				{
					return;
				}
				amount += this._remainGoldForStack;
				this.AddStack(amount / this.ability._goldPerStack);
				this._remainGoldForStack = amount % this.ability._goldPerStack;
			}

			// Token: 0x060040D5 RID: 16597 RVA: 0x000BC57F File Offset: 0x000BA77F
			private void OnDarkQuartzConsume(int amount)
			{
				if (this.ability._darkQuartzPerStack == 0)
				{
					return;
				}
				amount += this._remainDarkQuartzForStack;
				this.AddStack(amount / this.ability._darkQuartzPerStack);
				this._remainDarkQuartzForStack = amount % this.ability._darkQuartzPerStack;
			}

			// Token: 0x060040D6 RID: 16598 RVA: 0x000BC5BF File Offset: 0x000BA7BF
			private void AddStack(int amount)
			{
				this.ability.stack += amount;
				this.TryUpdateStat();
			}

			// Token: 0x060040D7 RID: 16599 RVA: 0x000BC5DC File Offset: 0x000BA7DC
			public void TryUpdateStat()
			{
				if (this.ability._maxStack > 0 && this.ability.stack > this.ability._maxStack)
				{
					this.ability.stack = this.ability._maxStack;
					GameData.Currency.gold.onEarn -= this.OnGoldConsume;
					GameData.Currency.darkQuartz.onEarn -= this.OnDarkQuartzConsume;
					this.UpdateStat();
					return;
				}
				this.UpdateStat();
			}

			// Token: 0x060040D8 RID: 16600 RVA: 0x000BC660 File Offset: 0x000BA860
			private void UpdateStat()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = (double)this.ability.stack * this.ability._statPerStack.values[i].value;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031CC RID: 12748
			private int _remainGoldForStack;

			// Token: 0x040031CD RID: 12749
			private int _remainDarkQuartzForStack;

			// Token: 0x040031CE RID: 12750
			private Stat.Values _stat;
		}
	}
}
