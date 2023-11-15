using System;
using UnityEngine;

namespace Characters.Gear.Weapons.Gauges
{
	// Token: 0x0200083E RID: 2110
	public class ValueGauge : Gauge
	{
		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06002BCF RID: 11215 RVA: 0x000867AC File Offset: 0x000849AC
		// (remove) Token: 0x06002BD0 RID: 11216 RVA: 0x000867E4 File Offset: 0x000849E4
		public event ValueGauge.onChangedDelegate onChanged;

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x0008681C File Offset: 0x00084A1C
		protected virtual string maxValueText
		{
			get
			{
				return this.maxValue.ToString();
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x00086837 File Offset: 0x00084A37
		public override float gaugePercent
		{
			get
			{
				return this._currentValue / this._maxValue;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x00086846 File Offset: 0x00084A46
		public override string displayText
		{
			get
			{
				return this._cachedDisplayText;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x0008684E File Offset: 0x00084A4E
		public float currentValue
		{
			get
			{
				return this._currentValue;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x00086856 File Offset: 0x00084A56
		// (set) Token: 0x06002BD6 RID: 11222 RVA: 0x0008685E File Offset: 0x00084A5E
		public float maxValue
		{
			get
			{
				return this._maxValue;
			}
			set
			{
				this._maxValue = value;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x00086867 File Offset: 0x00084A67
		// (set) Token: 0x06002BD8 RID: 11224 RVA: 0x00086874 File Offset: 0x00084A74
		public Color defaultBarColor
		{
			get
			{
				return this._baseBarGaugeColor.baseColor;
			}
			set
			{
				this._baseBarGaugeColor.baseColor = value;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x00086882 File Offset: 0x00084A82
		public override Color barColor
		{
			get
			{
				return this._defaultBarColor;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002BDA RID: 11226 RVA: 0x0008688A File Offset: 0x00084A8A
		public override bool secondBar
		{
			get
			{
				return this._secondBar;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x00086892 File Offset: 0x00084A92
		public override Color secondBarColor
		{
			get
			{
				return this._secondBarColor;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x0008689A File Offset: 0x00084A9A
		public override Color textColor
		{
			get
			{
				return this._textColor;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06002BDD RID: 11229 RVA: 0x000868A2 File Offset: 0x00084AA2
		public override Gauge.GaugeInfo defaultBarGaugeColor
		{
			get
			{
				return this._baseBarGaugeColor;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06002BDE RID: 11230 RVA: 0x000868AA File Offset: 0x00084AAA
		public override Gauge.GaugeInfo secondBarGaugeColor
		{
			get
			{
				return this._scoundBarGaugeColor;
			}
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000868B4 File Offset: 0x00084AB4
		private void Awake()
		{
			this._currentValue = this._initialValue;
			if (this._displayText)
			{
				this._cachedDisplayText = string.Format("{0} / {1}", (int)this._currentValue, this.maxValueText);
			}
			this._baseBarGaugeColor.baseColor = this._defaultBarColor;
			this._scoundBarGaugeColor.baseColor = this._secondBarColor;
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x00086919 File Offset: 0x00084B19
		public bool isMax()
		{
			return this._currentValue == this._maxValue;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x00086929 File Offset: 0x00084B29
		public bool Has(float amount)
		{
			return this._currentValue >= amount;
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x00086937 File Offset: 0x00084B37
		public void Clear()
		{
			this.Set(0f);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00086944 File Offset: 0x00084B44
		public bool Consume(float amount)
		{
			if (!this.Has(amount))
			{
				return false;
			}
			this.Set(this._currentValue - amount);
			return true;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x00086960 File Offset: 0x00084B60
		public void Add(float amount)
		{
			this.Set(this._currentValue + amount);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x00086970 File Offset: 0x00084B70
		public void FillUp()
		{
			this.Set(this._maxValue);
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x00086980 File Offset: 0x00084B80
		public void Set(float value)
		{
			value = Mathf.Clamp(value, 0f, this._maxValue);
			float currentValue = this._currentValue;
			this._currentValue = value;
			if (this._displayText)
			{
				this._cachedDisplayText = string.Format("{0} / {1}", (int)this._currentValue, this.maxValueText);
			}
			ValueGauge.onChangedDelegate onChangedDelegate = this.onChanged;
			if (onChangedDelegate == null)
			{
				return;
			}
			onChangedDelegate(currentValue, this._currentValue);
		}

		// Token: 0x0400251A RID: 9498
		[SerializeField]
		private float _initialValue;

		// Token: 0x0400251B RID: 9499
		[SerializeField]
		protected Color _defaultBarColor = Color.black;

		// Token: 0x0400251C RID: 9500
		[Space]
		[SerializeField]
		private bool _secondBar;

		// Token: 0x0400251D RID: 9501
		[SerializeField]
		private Color _secondBarColor;

		// Token: 0x0400251E RID: 9502
		[SerializeField]
		[Space]
		private Gauge.GaugeInfo _baseBarGaugeColor;

		// Token: 0x0400251F RID: 9503
		[SerializeField]
		private Gauge.GaugeInfo _scoundBarGaugeColor;

		// Token: 0x04002520 RID: 9504
		protected float _currentValue;

		// Token: 0x04002521 RID: 9505
		[SerializeField]
		[Space]
		protected float _maxValue;

		// Token: 0x04002522 RID: 9506
		[SerializeField]
		protected bool _displayText = true;

		// Token: 0x04002523 RID: 9507
		[SerializeField]
		protected Color _textColor = Color.white;

		// Token: 0x04002524 RID: 9508
		private string _cachedDisplayText;

		// Token: 0x0200083F RID: 2111
		// (Invoke) Token: 0x06002BE9 RID: 11241
		public delegate void onChangedDelegate(float oldValue, float newValue);
	}
}
