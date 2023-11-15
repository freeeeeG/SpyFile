using System;
using UnityEngine;
using UnityEngine.UI;

namespace FX
{
	// Token: 0x0200025E RID: 606
	public class StackVignette : MonoBehaviour
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x00020B8F File Offset: 0x0001ED8F
		public void UpdateStack(float stack)
		{
			if (stack <= (float)this._stackRange.x)
			{
				this.UpdateAlpha(0f);
				return;
			}
			this.UpdateAlpha(stack / (float)this._stackRange.y);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00020BC0 File Offset: 0x0001EDC0
		private void UpdateAlpha(float a)
		{
			Color color = this._image.color;
			color.a = a;
			this._image.color = color;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00020BED File Offset: 0x0001EDED
		public void Hide()
		{
			this.UpdateAlpha(0f);
		}

		// Token: 0x040009F0 RID: 2544
		[SerializeField]
		[GetComponent]
		private Image _image;

		// Token: 0x040009F1 RID: 2545
		[SerializeField]
		[MinMaxSlider(0f, 100f)]
		private Vector2Int _stackRange;
	}
}
