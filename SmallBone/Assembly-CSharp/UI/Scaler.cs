using System;
using UnityEngine;

namespace UI
{
	// Token: 0x020003CA RID: 970
	public class Scaler : MonoBehaviour
	{
		// Token: 0x06001212 RID: 4626 RVA: 0x00035496 File Offset: 0x00033696
		private void Awake()
		{
			this._contentSize = this._content.sizeDelta;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x000354AC File Offset: 0x000336AC
		public void SetVerticalLetterBox(bool verticalLetterBox)
		{
			this._verticalLetterBox = verticalLetterBox;
			this._letterBox.SetVerticalLetterBox(verticalLetterBox);
			if (this._verticalLetterBox)
			{
				this._content.sizeDelta = this._contentSize;
				return;
			}
			this._content.sizeDelta = new Vector2(this._canvas.sizeDelta.x, this._contentSize.y);
		}

		// Token: 0x04000EEC RID: 3820
		[SerializeField]
		private RectTransform _canvas;

		// Token: 0x04000EED RID: 3821
		[SerializeField]
		private RectTransform _content;

		// Token: 0x04000EEE RID: 3822
		[SerializeField]
		private ScreenLetterBox _letterBox;

		// Token: 0x04000EEF RID: 3823
		private bool _verticalLetterBox;

		// Token: 0x04000EF0 RID: 3824
		private Vector2 _contentSize;
	}
}
