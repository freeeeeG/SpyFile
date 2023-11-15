using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003B4 RID: 948
	public class NpcConfirmText : MonoBehaviour
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x0003403C File Offset: 0x0003223C
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x00034049 File Offset: 0x00032249
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

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00034057 File Offset: 0x00032257
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x0003405F File Offset: 0x0003225F
		public bool focus
		{
			get
			{
				return this._focus;
			}
			set
			{
				this._focus = value;
				this._text.color = (this._focus ? Color.white : Color.gray);
			}
		}

		// Token: 0x04000E88 RID: 3720
		[SerializeField]
		[GetComponent]
		private Text _text;

		// Token: 0x04000E89 RID: 3721
		private bool _focus;
	}
}
