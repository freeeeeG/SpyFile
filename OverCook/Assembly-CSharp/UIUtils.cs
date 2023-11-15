using System;
using UnityEngine;

// Token: 0x02000374 RID: 884
public static class UIUtils
{
	// Token: 0x060010DF RID: 4319 RVA: 0x00060CC8 File Offset: 0x0005F0C8
	public static void SetupFillParentAreaRect(RectTransform _rect)
	{
		_rect.anchorMin = new Vector2(0f, 0f);
		_rect.anchorMax = new Vector2(1f, 1f);
		_rect.offsetMin = Vector2.zero;
		_rect.offsetMax = Vector2.zero;
		_rect.localScale = Vector3.one;
	}
}
