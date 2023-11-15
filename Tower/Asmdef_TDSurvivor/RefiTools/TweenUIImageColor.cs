using System;
using UnityEngine;
using UnityEngine.UI;

namespace RefiTools
{
	// Token: 0x020001BF RID: 447
	[RequireComponent(typeof(Image))]
	public class TweenUIImageColor : TweenBase
	{
		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002E388 File Offset: 0x0002C588
		protected override void UpdateTween()
		{
			this.image.color = this.gradient.Evaluate(this.curve.Evaluate(this.tweenT));
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002E3B1 File Offset: 0x0002C5B1
		protected override void Reset()
		{
			base.Reset();
			this.image = base.GetComponent<Image>();
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002E3C5 File Offset: 0x0002C5C5
		protected override void OnValidate()
		{
			base.OnValidate();
			this.image = base.GetComponent<Image>();
		}

		// Token: 0x04000961 RID: 2401
		[SerializeField]
		[Header("Image物件")]
		private Image image;

		// Token: 0x04000962 RID: 2402
		[SerializeField]
		[Header("顏色設定")]
		private Gradient gradient;
	}
}
