using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B3E RID: 2878
[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
	// Token: 0x060058E2 RID: 22754 RVA: 0x002093D4 File Offset: 0x002075D4
	protected override void OnRectTransformDimensionsChange()
	{
		this.Layout();
	}

	// Token: 0x060058E3 RID: 22755 RVA: 0x002093DC File Offset: 0x002075DC
	public void Layout()
	{
		KleiPermitDioramaVisScaler.Layout(this.root, this.scaleTarget, this.slot);
	}

	// Token: 0x060058E4 RID: 22756 RVA: 0x002093F8 File Offset: 0x002075F8
	public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot)
	{
		float aspectRatio = 2.125f;
		AspectRatioFitter aspectRatioFitter = slot.FindOrAddComponent<AspectRatioFitter>();
		aspectRatioFitter.aspectRatio = aspectRatio;
		aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
		float num = 1700f;
		float a = Mathf.Max(0.1f, root.rect.width) / num;
		float num2 = 800f;
		float b = Mathf.Max(0.1f, root.rect.height) / num2;
		float d = Mathf.Max(a, b);
		scaleTarget.localScale = Vector3.one * d;
		scaleTarget.sizeDelta = new Vector2(1700f, 800f);
		scaleTarget.anchorMin = Vector2.one * 0.5f;
		scaleTarget.anchorMax = Vector2.one * 0.5f;
		scaleTarget.pivot = Vector2.one * 0.5f;
		scaleTarget.anchoredPosition = Vector2.zero;
	}

	// Token: 0x04003C20 RID: 15392
	public const float REFERENCE_WIDTH = 1700f;

	// Token: 0x04003C21 RID: 15393
	public const float REFERENCE_HEIGHT = 800f;

	// Token: 0x04003C22 RID: 15394
	[SerializeField]
	private RectTransform root;

	// Token: 0x04003C23 RID: 15395
	[SerializeField]
	private RectTransform scaleTarget;

	// Token: 0x04003C24 RID: 15396
	[SerializeField]
	private RectTransform slot;
}
