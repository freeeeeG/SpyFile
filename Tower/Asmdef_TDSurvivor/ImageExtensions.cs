using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F4 RID: 244
public static class ImageExtensions
{
	// Token: 0x0600061F RID: 1567 RVA: 0x000177DC File Offset: 0x000159DC
	public static void AdjustSizeToSprite(this Image image)
	{
		RectTransform component = image.GetComponent<RectTransform>();
		float width = image.sprite.rect.width;
		float height = image.sprite.rect.height;
		float width2 = component.rect.width;
		float height2 = component.rect.height;
		float num = width / height;
		float num2 = width2 / height2;
		if (num > num2)
		{
			float y = width2 / num;
			component.sizeDelta = new Vector2(width2, y);
			return;
		}
		float x = height2 * num;
		component.sizeDelta = new Vector2(x, height2);
	}
}
