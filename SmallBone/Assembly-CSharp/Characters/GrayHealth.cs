using System;

namespace Characters
{
	// Token: 0x020006FB RID: 1787
	public sealed class GrayHealth
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002404 RID: 9220 RVA: 0x0006C92F File Offset: 0x0006AB2F
		// (set) Token: 0x06002405 RID: 9221 RVA: 0x0006C938 File Offset: 0x0006AB38
		public double maximum
		{
			get
			{
				return this._maximum;
			}
			set
			{
				this._maximum = value;
				if (this._health.currentHealth + this._maximum > this._health.maximumHealth)
				{
					this._maximum = this._health.maximumHealth - this._health.currentHealth;
				}
				if (this._maximum < this._canHeal)
				{
					this.canHeal = this._maximum;
				}
				if (this._maximum < 0.0)
				{
					this._maximum = 0.0;
				}
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x0006C9C2 File Offset: 0x0006ABC2
		// (set) Token: 0x06002407 RID: 9223 RVA: 0x0006C9CA File Offset: 0x0006ABCA
		public double canHeal
		{
			get
			{
				return this._canHeal;
			}
			set
			{
				if (value > this._maximum)
				{
					this._canHeal = this._maximum;
				}
				else
				{
					this._canHeal = value;
				}
				if (this._canHeal < 0.0)
				{
					this._canHeal = 0.0;
				}
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0006CA0A File Offset: 0x0006AC0A
		public GrayHealth(Health health)
		{
			this._health = health;
		}

		// Token: 0x04001ECF RID: 7887
		private double _maximum;

		// Token: 0x04001ED0 RID: 7888
		private double _canHeal;

		// Token: 0x04001ED1 RID: 7889
		private Health _health;
	}
}
