using System;
using Characters.Gear.Weapons.Gauges;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
	// Token: 0x02000466 RID: 1126
	public class GaugeBar : MonoBehaviour
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00043781 File Offset: 0x00041981
		// (set) Token: 0x06001575 RID: 5493 RVA: 0x00043789 File Offset: 0x00041989
		public Gauge gauge
		{
			get
			{
				return this._gauge;
			}
			set
			{
				this._gauge = value;
				this.Update();
				base.gameObject.SetActive(this._gauge != null);
			}
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000437AF File Offset: 0x000419AF
		private void OnEnable()
		{
			this.bar1.Reset();
			this.bar2.Reset();
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x000437C8 File Offset: 0x000419C8
		private void Update()
		{
			if (this.gauge == null)
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			float num = math.clamp(this.gauge.gaugePercent, 0f, 1f);
			if (!this.gauge.secondBar)
			{
				Gauge.GaugeInfo defaultBarGaugeColor = this.gauge.defaultBarGaugeColor;
				if (defaultBarGaugeColor.useChargedColor && num >= 1f)
				{
					this.bar1.SetColor(defaultBarGaugeColor.chargedColor);
				}
				else
				{
					this.bar1.SetColor(defaultBarGaugeColor.baseColor);
				}
				this.bar2.SetActive(false);
				this.bar1.Lerp(num);
			}
			else
			{
				Gauge.GaugeInfo defaultBarGaugeColor2 = this.gauge.defaultBarGaugeColor;
				float proportion = this.gauge.defaultBarGaugeColor.proportion;
				Gauge.GaugeInfo secondBarGaugeColor = this.gauge.secondBarGaugeColor;
				float proportion2 = this.gauge.secondBarGaugeColor.proportion;
				if (num < proportion)
				{
					this.bar2.SetActive(false);
					this.bar1.SetColor(defaultBarGaugeColor2.baseColor);
					this.bar1.Lerp(num / proportion);
				}
				else
				{
					if (defaultBarGaugeColor2.useChargedColor)
					{
						this.bar1.SetColor(defaultBarGaugeColor2.chargedColor);
					}
					else
					{
						this.bar1.SetColor(defaultBarGaugeColor2.baseColor);
					}
					if (secondBarGaugeColor.useChargedColor && num >= 1f)
					{
						this.bar2.SetColor(secondBarGaugeColor.chargedColor);
					}
					else
					{
						this.bar2.SetColor(secondBarGaugeColor.baseColor);
					}
					this.bar2.SetActive(true);
					this.bar1.Lerp(1f);
					this.bar2.Lerp(Mathf.Clamp((num - proportion) / proportion2, 0f, 1f));
				}
			}
			this._displayText.color = this.gauge.textColor;
			this._displayText.text = this.gauge.displayText;
		}

		// Token: 0x040012C4 RID: 4804
		[SerializeField]
		private RectTransform _container;

		// Token: 0x040012C5 RID: 4805
		[SerializeField]
		private TMP_Text _displayText;

		// Token: 0x040012C6 RID: 4806
		[SerializeField]
		private GaugeBar.Bar bar1;

		// Token: 0x040012C7 RID: 4807
		[SerializeField]
		private GaugeBar.Bar bar2;

		// Token: 0x040012C8 RID: 4808
		private Gauge _gauge;

		// Token: 0x02000467 RID: 1127
		[Serializable]
		private class Bar
		{
			// Token: 0x17000431 RID: 1073
			// (get) Token: 0x06001579 RID: 5497 RVA: 0x000439BD File Offset: 0x00041BBD
			public Image mainBar
			{
				get
				{
					return this._mainBar;
				}
			}

			// Token: 0x17000432 RID: 1074
			// (get) Token: 0x0600157A RID: 5498 RVA: 0x000439C5 File Offset: 0x00041BC5
			public Vector3 defaultMainBarScale
			{
				get
				{
					return this._defaultMainBarScale;
				}
			}

			// Token: 0x0600157B RID: 5499 RVA: 0x000439CD File Offset: 0x00041BCD
			public void Reset()
			{
				this._mainScale.x = 0f;
			}

			// Token: 0x0600157C RID: 5500 RVA: 0x000439DF File Offset: 0x00041BDF
			public void SetColor(Color color)
			{
				this._mainBar.color = color;
			}

			// Token: 0x0600157D RID: 5501 RVA: 0x000439F0 File Offset: 0x00041BF0
			public void Lerp(float targetPercent)
			{
				this._mainScale.x = Mathf.Lerp(this._mainScale.x, targetPercent, 0.25f);
				this._mainBar.transform.localScale = Vector3.Scale(this._mainScale, this._defaultMainBarScale);
			}

			// Token: 0x0600157E RID: 5502 RVA: 0x00043A3F File Offset: 0x00041C3F
			public void SetActive(bool value)
			{
				this._mainBar.enabled = value;
			}

			// Token: 0x040012C9 RID: 4809
			[SerializeField]
			private Image _mainBar;

			// Token: 0x040012CA RID: 4810
			[SerializeField]
			private Vector3 _defaultMainBarScale = Vector3.one;

			// Token: 0x040012CB RID: 4811
			private Vector3 _mainScale = Vector3.one;
		}
	}
}
