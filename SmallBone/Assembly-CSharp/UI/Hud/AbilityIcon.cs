using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
	// Token: 0x02000463 RID: 1123
	public class AbilityIcon : MonoBehaviour
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00043484 File Offset: 0x00041684
		// (set) Token: 0x06001562 RID: 5474 RVA: 0x00043491 File Offset: 0x00041691
		public Sprite icon
		{
			get
			{
				return this._icon.sprite;
			}
			set
			{
				this._icon.sprite = value;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x0004349F File Offset: 0x0004169F
		// (set) Token: 0x06001564 RID: 5476 RVA: 0x000434AC File Offset: 0x000416AC
		public float fillAmount
		{
			get
			{
				return this._fill.fillAmount;
			}
			set
			{
				this._fill.fillAmount = value;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x000434BA File Offset: 0x000416BA
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x000434C7 File Offset: 0x000416C7
		public bool clockwise
		{
			get
			{
				return this._fill.fillClockwise;
			}
			set
			{
				this._fill.fillClockwise = value;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x000434D5 File Offset: 0x000416D5
		// (set) Token: 0x06001568 RID: 5480 RVA: 0x000434DD File Offset: 0x000416DD
		public int stacks
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = value;
				this._stackText.text = ((value > 0) ? value.ToString() : string.Empty);
			}
		}

		// Token: 0x040012B6 RID: 4790
		[SerializeField]
		private Image _icon;

		// Token: 0x040012B7 RID: 4791
		[SerializeField]
		private Image _fill;

		// Token: 0x040012B8 RID: 4792
		[SerializeField]
		private TMP_Text _stackText;

		// Token: 0x040012B9 RID: 4793
		private int _stack;
	}
}
