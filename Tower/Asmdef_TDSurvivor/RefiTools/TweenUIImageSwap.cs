using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RefiTools
{
	// Token: 0x020001C0 RID: 448
	[RequireComponent(typeof(Image))]
	public class TweenUIImageSwap : TweenBase
	{
		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002E3E4 File Offset: 0x0002C5E4
		protected override void UpdateTween()
		{
			int num = Mathf.FloorToInt(this.tweenT * (float)this.list_Sprites.Count);
			num = Mathf.Clamp(num, 0, this.list_Sprites.Count - 1);
			if (this.image.enabled && num != this.lastIndex)
			{
				this.image.sprite = this.list_Sprites[num];
			}
			this.lastIndex = num;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002E453 File Offset: 0x0002C653
		protected override void Reset()
		{
			base.Reset();
			this.image = base.GetComponent<Image>();
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002E467 File Offset: 0x0002C667
		protected override void OnValidate()
		{
			base.OnValidate();
			this.image = base.GetComponent<Image>();
		}

		// Token: 0x04000963 RID: 2403
		[SerializeField]
		[Header("Image物件")]
		private Image image;

		// Token: 0x04000964 RID: 2404
		[SerializeField]
		[Header("要替換的圖片清單")]
		private List<Sprite> list_Sprites;

		// Token: 0x04000965 RID: 2405
		private int lastIndex = -1;
	}
}
