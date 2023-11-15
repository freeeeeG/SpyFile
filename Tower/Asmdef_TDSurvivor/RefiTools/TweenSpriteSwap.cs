using System;
using System.Collections.Generic;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001BD RID: 445
	[RequireComponent(typeof(SpriteRenderer))]
	public class TweenSpriteSwap : TweenBase
	{
		// Token: 0x06000BBB RID: 3003 RVA: 0x0002E274 File Offset: 0x0002C474
		protected override void UpdateTween()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(this.tweenT * (float)this.list_Sprites.Count), 0, this.list_Sprites.Count - 1);
			if (num != this.lastIndex)
			{
				if (this.isReverse)
				{
					this.spriteRenderer.sprite = this.list_Sprites[this.list_Sprites.Count - (num + 1)];
				}
				else
				{
					this.spriteRenderer.sprite = this.list_Sprites[num];
				}
			}
			this.lastIndex = num;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0002E303 File Offset: 0x0002C503
		protected override void Reset()
		{
			base.Reset();
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002E317 File Offset: 0x0002C517
		protected override void OnValidate()
		{
			base.OnValidate();
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x0400095B RID: 2395
		[SerializeField]
		[Header("Sprite Renderer物件")]
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400095C RID: 2396
		[SerializeField]
		[Header("反向播放")]
		private bool isReverse;

		// Token: 0x0400095D RID: 2397
		[SerializeField]
		[Header("圖片清單")]
		private List<Sprite> list_Sprites;

		// Token: 0x0400095E RID: 2398
		private int lastIndex = -1;
	}
}
