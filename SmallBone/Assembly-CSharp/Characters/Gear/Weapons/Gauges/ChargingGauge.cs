using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Weapons.Gauges
{
	// Token: 0x02000837 RID: 2103
	public class ChargingGauge : Gauge
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x00086559 File Offset: 0x00084759
		public override float gaugePercent
		{
			get
			{
				if (!(this._currentChargeAction == null))
				{
					return this._currentChargeAction.chargingPercent;
				}
				return 0f;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002BAA RID: 11178 RVA: 0x0008657A File Offset: 0x0008477A
		public override string displayText
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002BAB RID: 11179 RVA: 0x00086581 File Offset: 0x00084781
		public override Color barColor
		{
			get
			{
				return this._defaultBarColor;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x00086589 File Offset: 0x00084789
		public override bool secondBar
		{
			get
			{
				return this._secondBar;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002BAD RID: 11181 RVA: 0x00086591 File Offset: 0x00084791
		public override Color secondBarColor
		{
			get
			{
				return this._secondBarColor;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x00086599 File Offset: 0x00084799
		public override Color textColor
		{
			get
			{
				return this._textColor;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06002BAF RID: 11183 RVA: 0x000865A1 File Offset: 0x000847A1
		public override Gauge.GaugeInfo defaultBarGaugeColor
		{
			get
			{
				return this._baseBarGaugeColor;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x000865A9 File Offset: 0x000847A9
		public override Gauge.GaugeInfo secondBarGaugeColor
		{
			get
			{
				return this._scoundBarGaugeColor;
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000865B4 File Offset: 0x000847B4
		private void Awake()
		{
			this._baseBarGaugeColor.baseColor = this._defaultBarColor;
			this._scoundBarGaugeColor.baseColor = this._secondBarColor;
			ChargeAction[] chargeActions = this._chargeActions;
			for (int i = 0; i < chargeActions.Length; i++)
			{
				ChargeAction chargeAction = chargeActions[i];
				chargeAction.onStart += delegate()
				{
					this._currentChargeAction = chargeAction;
				};
			}
		}

		// Token: 0x04002503 RID: 9475
		[SerializeField]
		private Color _defaultBarColor = Color.black;

		// Token: 0x04002504 RID: 9476
		[Space]
		[SerializeField]
		private bool _secondBar;

		// Token: 0x04002505 RID: 9477
		[SerializeField]
		private Color _secondBarColor;

		// Token: 0x04002506 RID: 9478
		[SerializeField]
		[Space]
		private Gauge.GaugeInfo _baseBarGaugeColor;

		// Token: 0x04002507 RID: 9479
		[SerializeField]
		private Gauge.GaugeInfo _scoundBarGaugeColor;

		// Token: 0x04002508 RID: 9480
		[SerializeField]
		protected Color _textColor = Color.white;

		// Token: 0x04002509 RID: 9481
		private ChargeAction _currentChargeAction;

		// Token: 0x0400250A RID: 9482
		[SerializeField]
		[Space]
		private ChargeAction[] _chargeActions;
	}
}
