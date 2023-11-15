using System;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003DB RID: 987
	public class TextAdaptiveFrame : MonoBehaviour
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00037285 File Offset: 0x00035485
		// (set) Token: 0x06001261 RID: 4705 RVA: 0x00037292 File Offset: 0x00035492
		public string text
		{
			get
			{
				return this._text.text;
			}
			set
			{
				this._text.text = value;
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000372A0 File Offset: 0x000354A0
		private void OnEnable()
		{
			this.UpdateSize();
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000372A8 File Offset: 0x000354A8
		public void UpdateSize()
		{
			float size = this._text.preferredWidth * 0.2f + 40f;
			this._rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
		}

		// Token: 0x04000F52 RID: 3922
		[GetComponent]
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		private TMP_Text _text;
	}
}
