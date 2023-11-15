using System;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003DC RID: 988
	public class TextHolderSizer : MonoBehaviour
	{
		// Token: 0x06001265 RID: 4709 RVA: 0x000372DA File Offset: 0x000354DA
		private void Update()
		{
			this.UpdateSize();
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000372E4 File Offset: 0x000354E4
		public void UpdateSize()
		{
			Vector2 vector = new Vector2(this._text.preferredWidth * this._multiplier.x, this._text.preferredHeight * this._multiplier.y) + this._padding;
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			if (this._mode.HasFlag(TextHolderSizer.Mode.Width))
			{
				sizeDelta.x = Mathf.Clamp(this._minSize.x, vector.x, this._maxSize.x);
			}
			if (this._mode.HasFlag(TextHolderSizer.Mode.Height))
			{
				sizeDelta.y = Mathf.Clamp(this._minSize.y, vector.y, this._maxSize.y);
			}
			this._rectTransform.sizeDelta = sizeDelta;
		}

		// Token: 0x04000F54 RID: 3924
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x04000F55 RID: 3925
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04000F56 RID: 3926
		[EnumFlag]
		[SerializeField]
		private TextHolderSizer.Mode _mode;

		// Token: 0x04000F57 RID: 3927
		[SerializeField]
		private Vector2 _minSize = Vector2.zero;

		// Token: 0x04000F58 RID: 3928
		[SerializeField]
		private Vector2 _maxSize = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x04000F59 RID: 3929
		[SerializeField]
		private Vector2 _padding = Vector2.zero;

		// Token: 0x04000F5A RID: 3930
		[SerializeField]
		private Vector2 _multiplier = Vector2.one;

		// Token: 0x020003DD RID: 989
		[Flags]
		public enum Mode
		{
			// Token: 0x04000F5C RID: 3932
			Width = 1,
			// Token: 0x04000F5D RID: 3933
			Height = 2,
			// Token: 0x04000F5E RID: 3934
			Both = 3
		}
	}
}
