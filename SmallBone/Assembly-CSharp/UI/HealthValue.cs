using System;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003A7 RID: 935
	public class HealthValue : MonoBehaviour
	{
		// Token: 0x0600113B RID: 4411 RVA: 0x00033084 File Offset: 0x00031284
		public void Initialize(Health health, Shield shield)
		{
			this._health = health;
			this._shield = shield;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00033094 File Offset: 0x00031294
		private void Update()
		{
			if (this._health == null)
			{
				return;
			}
			string text;
			if (this._shield != null && this._shield.amount > 0.0)
			{
				text = string.Format("<color={0}>(+{1})</color>", "#53DEFF", this._shield.amount);
			}
			else
			{
				text = string.Empty;
			}
			string text2;
			if (this._health.percent < 0.30000001192092896)
			{
				text2 = "#ff0000";
				this._healthImage.color = Color.red;
			}
			else
			{
				text2 = "#ffffff";
				this._healthImage.color = Color.white;
			}
			this._text.text = string.Format("<color={0}>{1} / {2}</color> {3}", new object[]
			{
				text2,
				Math.Ceiling(this._health.currentHealth),
				Math.Ceiling(this._health.maximumHealth),
				text
			});
		}

		// Token: 0x04000E2E RID: 3630
		[SerializeField]
		private TextMeshProUGUI _text;

		// Token: 0x04000E2F RID: 3631
		[SerializeField]
		private Image _healthImage;

		// Token: 0x04000E30 RID: 3632
		private Health _health;

		// Token: 0x04000E31 RID: 3633
		private Shield _shield;

		// Token: 0x04000E32 RID: 3634
		private const string shieldColor = "#53DEFF";

		// Token: 0x04000E33 RID: 3635
		private const string canHealColor = "#D3D3D3";

		// Token: 0x04000E34 RID: 3636
		private const string highHealthColor = "#ffffff";

		// Token: 0x04000E35 RID: 3637
		private const string lowHealthColor = "#ff0000";
	}
}
