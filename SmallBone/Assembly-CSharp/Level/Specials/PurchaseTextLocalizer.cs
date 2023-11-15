using System;
using GameResources;
using TMPro;
using UnityEngine;

namespace Level.Specials
{
	// Token: 0x0200062A RID: 1578
	public class PurchaseTextLocalizer : MonoBehaviour
	{
		// Token: 0x06001FA2 RID: 8098 RVA: 0x000602E9 File Offset: 0x0005E4E9
		private void OnEnable()
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				return;
			}
			this._text.text = string.Format(Localization.GetLocalizedString(this._key), this._timeCostEvent.GetValue());
		}

		// Token: 0x04001ACF RID: 6863
		[SerializeField]
		private TimeCostEvent _timeCostEvent;

		// Token: 0x04001AD0 RID: 6864
		[SerializeField]
		[GetComponent]
		private TMP_Text _text;

		// Token: 0x04001AD1 RID: 6865
		[SerializeField]
		private string _key;
	}
}
