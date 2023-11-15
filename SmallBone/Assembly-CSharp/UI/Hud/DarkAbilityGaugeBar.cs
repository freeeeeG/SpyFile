using System;
using Characters.Abilities.Darks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
	// Token: 0x02000460 RID: 1120
	public sealed class DarkAbilityGaugeBar : MonoBehaviour
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x00042DB4 File Offset: 0x00040FB4
		// (set) Token: 0x06001543 RID: 5443 RVA: 0x00042DBC File Offset: 0x00040FBC
		public DarkAbilityGauge gauge
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

		// Token: 0x06001544 RID: 5444 RVA: 0x00042DE2 File Offset: 0x00040FE2
		public void Initialize(DarkAbilityAttacher abilityAttacher)
		{
			this._attacher = abilityAttacher;
			this._gauge = this._attacher.gauge;
			if (this._gauge == null)
			{
				this._container.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00042E1B File Offset: 0x0004101B
		private void OnEnable()
		{
			this._mainScale.x = 0f;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00042E30 File Offset: 0x00041030
		private void Update()
		{
			if (this.gauge == null)
			{
				return;
			}
			float targetPercent = math.clamp(this.gauge.gaugePercent, 0f, 1f);
			this.Lerp(targetPercent);
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00042E70 File Offset: 0x00041070
		private void Lerp(float targetPercent)
		{
			this._mainScale.x = Mathf.Lerp(this._mainScale.x, targetPercent, 0.25f);
			this._mainBar.transform.localScale = Vector3.Scale(this._mainScale, this._defaultMainBarScale);
		}

		// Token: 0x04001295 RID: 4757
		[SerializeField]
		private RectTransform _container;

		// Token: 0x04001296 RID: 4758
		[SerializeField]
		private Image _mainBar;

		// Token: 0x04001297 RID: 4759
		[SerializeField]
		private Vector3 _defaultMainBarScale = Vector3.one;

		// Token: 0x04001298 RID: 4760
		private Vector3 _mainScale = Vector3.one;

		// Token: 0x04001299 RID: 4761
		private DarkAbilityAttacher _attacher;

		// Token: 0x0400129A RID: 4762
		private DarkAbilityGauge _gauge;
	}
}
