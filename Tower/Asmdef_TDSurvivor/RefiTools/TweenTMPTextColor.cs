using System;
using TMPro;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001BE RID: 446
	[RequireComponent(typeof(TMP_Text))]
	public class TweenTMPTextColor : TweenBase
	{
		// Token: 0x06000BBF RID: 3007 RVA: 0x0002E33A File Offset: 0x0002C53A
		protected override void UpdateTween()
		{
			this.text.color = this.gradient.Evaluate(this.tweenT);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0002E358 File Offset: 0x0002C558
		protected override void Reset()
		{
			base.Reset();
			this.text = base.GetComponent<TMP_Text>();
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002E36C File Offset: 0x0002C56C
		protected override void OnValidate()
		{
			base.OnValidate();
			this.text = base.GetComponent<TMP_Text>();
		}

		// Token: 0x0400095F RID: 2399
		[SerializeField]
		[Header("Sprite Renderer物件")]
		private TMP_Text text;

		// Token: 0x04000960 RID: 2400
		[SerializeField]
		[Header("顏色設定")]
		private Gradient gradient;
	}
}
