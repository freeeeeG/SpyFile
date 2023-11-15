using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003C1 RID: 961
	public class NpcName : MonoBehaviour
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x00034DC7 File Offset: 0x00032FC7
		// (set) Token: 0x060011E6 RID: 4582 RVA: 0x00034DD4 File Offset: 0x00032FD4
		public string text
		{
			get
			{
				return this._text.text;
			}
			set
			{
				this._text.text = value;
				if (string.IsNullOrWhiteSpace(this.text))
				{
					base.gameObject.SetActive(false);
					return;
				}
				base.gameObject.SetActive(true);
				this._sizer.UpdateSize();
			}
		}

		// Token: 0x04000ECF RID: 3791
		[SerializeField]
		private TextHolderSizer _sizer;

		// Token: 0x04000ED0 RID: 3792
		[SerializeField]
		private Image _textField;

		// Token: 0x04000ED1 RID: 3793
		[SerializeField]
		private TextMeshProUGUI _text;
	}
}
