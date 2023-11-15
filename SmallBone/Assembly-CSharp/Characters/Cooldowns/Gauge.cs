using System;
using Characters.Cooldowns.Streaks;
using Characters.Gear.Weapons.Gauges;

namespace Characters.Cooldowns
{
	// Token: 0x02000907 RID: 2311
	public class Gauge : ICooldown
	{
		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003161 RID: 12641 RVA: 0x000076D4 File Offset: 0x000058D4
		public int maxStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x000939FF File Offset: 0x00091BFF
		// (set) Token: 0x06003163 RID: 12643 RVA: 0x00002191 File Offset: 0x00000391
		public int stacks
		{
			get
			{
				if (!this._gauge.Has(this._requiredAmount))
				{
					return 0;
				}
				return 1;
			}
			set
			{
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x00093A17 File Offset: 0x00091C17
		public bool canUse
		{
			get
			{
				return this.streak.remains > 0 || this._gauge.Has(this._requiredAmount);
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x00093A3A File Offset: 0x00091C3A
		// (set) Token: 0x06003166 RID: 12646 RVA: 0x00093A42 File Offset: 0x00091C42
		public IStreak streak { get; private set; }

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x00071719 File Offset: 0x0006F919
		public float remainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06003168 RID: 12648 RVA: 0x00093A4C File Offset: 0x00091C4C
		// (remove) Token: 0x06003169 RID: 12649 RVA: 0x00093A84 File Offset: 0x00091C84
		public event Action onReady;

		// Token: 0x0600316A RID: 12650 RVA: 0x00093AB9 File Offset: 0x00091CB9
		public Gauge(ValueGauge gauge, int requiredAmount, int streakCount, float streakTimeout)
		{
			this._gauge = gauge;
			this._gauge.onChanged += this.OnChanged;
			this._requiredAmount = (float)requiredAmount;
			this.streak = new Streak(streakCount, streakTimeout);
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x00093AF5 File Offset: 0x00091CF5
		private void OnChanged(float oldValue, float newValue)
		{
			if (oldValue < this._requiredAmount && newValue >= this._requiredAmount)
			{
				Action action = this.onReady;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x00093B1C File Offset: 0x00091D1C
		public bool Consume()
		{
			if (this.streak.Consume())
			{
				return true;
			}
			if (this.stacks > 0)
			{
				int stacks = this.stacks;
				this.stacks = stacks - 1;
				this.streak.Start();
				return true;
			}
			return this._gauge.Consume(this._requiredAmount);
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x00002191 File Offset: 0x00000391
		public void ExpireStreak()
		{
		}

		// Token: 0x040028A3 RID: 10403
		private ValueGauge _gauge;

		// Token: 0x040028A4 RID: 10404
		private float _requiredAmount;
	}
}
