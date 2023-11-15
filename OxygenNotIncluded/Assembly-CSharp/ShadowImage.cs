using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BED RID: 3053
public class ShadowImage : ShadowRect
{
	// Token: 0x0600608C RID: 24716 RVA: 0x0023ACFC File Offset: 0x00238EFC
	protected override void MatchRect()
	{
		base.MatchRect();
		if (this.RectMain == null || this.RectShadow == null)
		{
			return;
		}
		if (this.shadowImage == null)
		{
			this.shadowImage = this.RectShadow.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			this.mainImage = this.RectMain.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			if (this.shadowImage != null)
			{
				this.shadowImage.color = Color.clear;
			}
			return;
		}
		if (this.shadowImage == null)
		{
			return;
		}
		if (this.shadowImage.sprite != this.mainImage.sprite)
		{
			this.shadowImage.sprite = this.mainImage.sprite;
		}
		if (this.shadowImage.color != this.shadowColor)
		{
			if (this.shadowImage.sprite != null)
			{
				this.shadowImage.color = this.shadowColor;
				return;
			}
			this.shadowImage.color = Color.clear;
		}
	}

	// Token: 0x040041C3 RID: 16835
	private Image shadowImage;

	// Token: 0x040041C4 RID: 16836
	private Image mainImage;
}
