using System;
using UnityEngine;

// Token: 0x02000392 RID: 914
public static class RectTransformExtensions
{
	// Token: 0x0600130C RID: 4876 RVA: 0x00064C7C File Offset: 0x00062E7C
	public static RectTransform Fill(this RectTransform rectTransform)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		return rectTransform;
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x00064CE0 File Offset: 0x00062EE0
	public static RectTransform Fill(this RectTransform rectTransform, Padding padding)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(padding.left, padding.bottom);
		rectTransform.sizeDelta = new Vector2(-padding.right, -padding.top);
		return rectTransform;
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x00064D48 File Offset: 0x00062F48
	public static RectTransform Pivot(this RectTransform rectTransform, float x, float y)
	{
		rectTransform.pivot = new Vector2(x, y);
		return rectTransform;
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x00064D58 File Offset: 0x00062F58
	public static RectTransform Pivot(this RectTransform rectTransform, Vector2 pivot)
	{
		rectTransform.pivot = pivot;
		return rectTransform;
	}
}
