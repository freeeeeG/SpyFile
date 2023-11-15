using System;
using UnityEngine;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x02000968 RID: 2408
	public class Gauge : Cooldown
	{
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x00099F05 File Offset: 0x00098105
		// (set) Token: 0x060033ED RID: 13293 RVA: 0x00099F0D File Offset: 0x0009810D
		public float gauge
		{
			get
			{
				return this._gauge;
			}
			set
			{
				if (value > this._maxGauge)
				{
					this._gauge = this._maxGauge;
				}
				this._gauge = value;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x00099F2B File Offset: 0x0009812B
		public override float remainPercent
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x00099F32 File Offset: 0x00098132
		public override bool canUse
		{
			get
			{
				return this.gauge >= this._minGaugeToUse;
			}
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x00099F45 File Offset: 0x00098145
		internal override bool Consume()
		{
			if (this.canUse)
			{
				this._gauge -= this._minGaugeToUse;
				return true;
			}
			return false;
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x00099F65 File Offset: 0x00098165
		private void Update()
		{
			this.gauge += this._recoveryPerSecond * this._character.chronometer.master.deltaTime;
		}

		// Token: 0x04002A0E RID: 10766
		[SerializeField]
		protected float _maxGauge;

		// Token: 0x04002A0F RID: 10767
		[SerializeField]
		protected float _minGaugeToUse;

		// Token: 0x04002A10 RID: 10768
		[SerializeField]
		protected float _recoveryPerSecond;

		// Token: 0x04002A11 RID: 10769
		protected float _gauge;
	}
}
