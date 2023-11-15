using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001BC RID: 444
	[RequireComponent(typeof(SpriteRenderer))]
	public class TweenSpriteColor : TweenBase
	{
		// Token: 0x06000BB7 RID: 2999 RVA: 0x0002E223 File Offset: 0x0002C423
		protected override void UpdateTween()
		{
			this.spriteRenderer.color = this.gradient.Evaluate(this.tweenT);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0002E241 File Offset: 0x0002C441
		protected override void Reset()
		{
			base.Reset();
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002E255 File Offset: 0x0002C455
		protected override void OnValidate()
		{
			base.OnValidate();
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x04000959 RID: 2393
		[SerializeField]
		[Header("Sprite Renderer物件")]
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400095A RID: 2394
		[SerializeField]
		[Header("顏色設定")]
		private Gradient gradient;
	}
}
