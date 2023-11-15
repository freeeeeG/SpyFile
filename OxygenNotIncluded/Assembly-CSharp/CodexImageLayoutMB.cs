using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000AC0 RID: 2752
public class CodexImageLayoutMB : UIBehaviour
{
	// Token: 0x060054BC RID: 21692 RVA: 0x001ECEC0 File Offset: 0x001EB0C0
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (this.image.preserveAspect && this.image.sprite != null && this.image.sprite)
		{
			float num = this.image.sprite.rect.height / this.image.sprite.rect.width;
			this.layoutElement.preferredHeight = num * this.rectTransform.sizeDelta.x;
			this.layoutElement.minHeight = this.layoutElement.preferredHeight;
			return;
		}
		this.layoutElement.preferredHeight = -1f;
		this.layoutElement.preferredWidth = -1f;
		this.layoutElement.minHeight = -1f;
		this.layoutElement.minWidth = -1f;
		this.layoutElement.flexibleHeight = -1f;
		this.layoutElement.flexibleWidth = -1f;
		this.layoutElement.ignoreLayout = false;
	}

	// Token: 0x0400387C RID: 14460
	public RectTransform rectTransform;

	// Token: 0x0400387D RID: 14461
	public LayoutElement layoutElement;

	// Token: 0x0400387E RID: 14462
	public Image image;
}
