using System;
using GameResources;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003DE RID: 990
	public class TextLocalizer : MonoBehaviour
	{
		// Token: 0x06001268 RID: 4712 RVA: 0x00037406 File Offset: 0x00035606
		private void OnEnable()
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				return;
			}
			this._text.text = Localization.GetLocalizedString(this._key);
		}

		// Token: 0x04000F5F RID: 3935
		[SerializeField]
		[GetComponent]
		private TMP_Text _text;

		// Token: 0x04000F60 RID: 3936
		[SerializeField]
		private string _key;
	}
}
