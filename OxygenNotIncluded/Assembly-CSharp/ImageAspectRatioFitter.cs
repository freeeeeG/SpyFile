using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B1D RID: 2845
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x06005792 RID: 22418 RVA: 0x00200DD0 File Offset: 0x001FEFD0
	private void UpdateAspectRatio()
	{
		base.aspectRatio = this.targetImage.sprite.rect.width / this.targetImage.sprite.rect.height;
	}

	// Token: 0x06005793 RID: 22419 RVA: 0x00200E14 File Offset: 0x001FF014
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x06005794 RID: 22420 RVA: 0x00200E22 File Offset: 0x001FF022
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04003B34 RID: 15156
	[SerializeField]
	private Image targetImage;
}
