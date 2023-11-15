using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003CB RID: 971
	public class ScreenLetterBox : MonoBehaviour
	{
		// Token: 0x06001215 RID: 4629 RVA: 0x00035514 File Offset: 0x00033714
		private void Update()
		{
			bool force = this._screenWidth != Screen.width || this._screenHeight != Screen.height;
			this.UpdateLetterBox(force);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0003554C File Offset: 0x0003374C
		public void UpdateLetterBox(bool force)
		{
			float num = (this._canvas.sizeDelta.y - this._scaler.referenceResolution.y) / 2f;
			float num2;
			if (this._verticalLetterBox)
			{
				num2 = (this._canvas.sizeDelta.x - this._scaler.referenceResolution.x) / 2f;
			}
			else
			{
				num2 = 0f;
			}
			if (!force && this._heightCache == num && this._widthCache == num2)
			{
				return;
			}
			this._heightCache = num;
			this._widthCache = num2;
			Vector2 sizeDelta = this._top.sizeDelta;
			sizeDelta.y = num;
			this._top.sizeDelta = sizeDelta;
			Vector2 sizeDelta2 = this._top.sizeDelta;
			sizeDelta2.y = num;
			this._bottom.sizeDelta = sizeDelta2;
			Vector2 sizeDelta3 = this._left.sizeDelta;
			sizeDelta3.x = num2;
			this._left.sizeDelta = sizeDelta3;
			Vector2 sizeDelta4 = this._right.sizeDelta;
			sizeDelta4.x = num2;
			this._right.sizeDelta = sizeDelta4;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00035661 File Offset: 0x00033861
		public void SetVerticalLetterBox(bool verticalLetterBox)
		{
			this._verticalLetterBox = verticalLetterBox;
			this._pixelPerfectCamera.cropFrameY = this._verticalLetterBox;
			this.UpdateLetterBox(true);
		}

		// Token: 0x04000EF1 RID: 3825
		[SerializeField]
		private CanvasScaler _scaler;

		// Token: 0x04000EF2 RID: 3826
		[SerializeField]
		private PixelPerfectCamera _pixelPerfectCamera;

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		private RectTransform _canvas;

		// Token: 0x04000EF4 RID: 3828
		[SerializeField]
		private RectTransform _top;

		// Token: 0x04000EF5 RID: 3829
		[SerializeField]
		private RectTransform _bottom;

		// Token: 0x04000EF6 RID: 3830
		[SerializeField]
		private RectTransform _left;

		// Token: 0x04000EF7 RID: 3831
		[SerializeField]
		private RectTransform _right;

		// Token: 0x04000EF8 RID: 3832
		private float _heightCache;

		// Token: 0x04000EF9 RID: 3833
		private float _widthCache;

		// Token: 0x04000EFA RID: 3834
		private bool _verticalLetterBox;

		// Token: 0x04000EFB RID: 3835
		private int _screenWidth;

		// Token: 0x04000EFC RID: 3836
		private int _screenHeight;
	}
}
