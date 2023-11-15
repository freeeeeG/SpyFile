using System;
using Characters.Cooldowns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x0200042D RID: 1069
	public class Cooldown : MonoBehaviour
	{
		// Token: 0x06001460 RID: 5216 RVA: 0x0003E9B8 File Offset: 0x0003CBB8
		public void Set(CooldownSerializer cooldown)
		{
			float num;
			if (cooldown.type == CooldownSerializer.Type.Time)
			{
				num = cooldown.time.cooldownTime;
			}
			else
			{
				num = 0f;
			}
			if (this._icon != null)
			{
				this._icon.enabled = true;
			}
			this._text.enabled = true;
			this._text.text = num.ToString();
		}

		// Token: 0x04001150 RID: 4432
		[SerializeField]
		private Image _icon;

		// Token: 0x04001151 RID: 4433
		[SerializeField]
		private TextMeshProUGUI _text;
	}
}
