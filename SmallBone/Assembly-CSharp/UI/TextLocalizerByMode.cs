using System;
using GameResources;
using Hardmode;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003DF RID: 991
	public sealed class TextLocalizerByMode : MonoBehaviour
	{
		// Token: 0x0600126A RID: 4714 RVA: 0x0003742C File Offset: 0x0003562C
		private void OnEnable()
		{
			bool hardmode = Singleton<HardmodeManager>.Instance.hardmode;
			if (this._colorChange)
			{
				this._text.color = (hardmode ? this._hardColor : this._normalColor);
			}
			if (!hardmode && string.IsNullOrWhiteSpace(this._normalKey))
			{
				return;
			}
			if (hardmode && string.IsNullOrWhiteSpace(this._hardKey))
			{
				return;
			}
			this._text.text = Localization.GetLocalizedString(hardmode ? this._hardKey : this._normalKey);
		}

		// Token: 0x04000F61 RID: 3937
		[GetComponent]
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04000F62 RID: 3938
		[SerializeField]
		private string _normalKey;

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private string _hardKey;

		// Token: 0x04000F64 RID: 3940
		[SerializeField]
		private bool _colorChange;

		// Token: 0x04000F65 RID: 3941
		[SerializeField]
		private Color _normalColor;

		// Token: 0x04000F66 RID: 3942
		[SerializeField]
		private Color _hardColor;
	}
}
