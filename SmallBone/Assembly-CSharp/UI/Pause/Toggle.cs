using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x0200042C RID: 1068
	public class Toggle : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001455 RID: 5205 RVA: 0x0003E824 File Offset: 0x0003CA24
		// (remove) Token: 0x06001456 RID: 5206 RVA: 0x0003E85C File Offset: 0x0003CA5C
		public event Action<int> onValueChanged;

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0003E891 File Offset: 0x0003CA91
		// (set) Token: 0x06001458 RID: 5208 RVA: 0x0003E899 File Offset: 0x0003CA99
		public int value
		{
			get
			{
				return this._value;
			}
			set
			{
				this.SetValueWithoutNotify(value);
				Action<int> action = this.onValueChanged;
				if (action == null)
				{
					return;
				}
				action(this._value);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x0003E8B8 File Offset: 0x0003CAB8
		public string text
		{
			get
			{
				return this._texts[this.value];
			}
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0003E8CC File Offset: 0x0003CACC
		private void ValidateValue()
		{
			if (this._value < 0)
			{
				this._value = this._texts.Count - 1;
				return;
			}
			if (this._value >= this._texts.Count)
			{
				this._value %= this._texts.Count;
			}
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0003E921 File Offset: 0x0003CB21
		public void SetTexts(IList<string> texts)
		{
			this._texts = texts;
			this.ValidateValue();
			this._text.text = this._texts[this._value];
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0003E94C File Offset: 0x0003CB4C
		public void SetValueWithoutNotify(int value)
		{
			this._value = value;
			this.ValidateValue();
			this._text.text = this._texts[this._value];
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0003E978 File Offset: 0x0003CB78
		public void OnPointerClick(PointerEventData eventData)
		{
			int value = this.value;
			this.value = value + 1;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0003E998 File Offset: 0x0003CB98
		public void OnSubmit(BaseEventData eventData)
		{
			int value = this.value;
			this.value = value + 1;
		}

		// Token: 0x0400114D RID: 4429
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x0400114E RID: 4430
		private IList<string> _texts;

		// Token: 0x0400114F RID: 4431
		private int _value;
	}
}
