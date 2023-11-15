using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public static class RefiUtilities
{
	// Token: 0x06000B2B RID: 2859 RVA: 0x0002B3FC File Offset: 0x000295FC
	public static void AdjustSizeToSprite(Image image)
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

	// Token: 0x06000B2C RID: 2860 RVA: 0x0002B498 File Offset: 0x00029698
	public static bool IsMouseInsideWindow()
	{
		Vector3 mousePosition = Input.mousePosition;
		return mousePosition.x >= 0f && mousePosition.x <= (float)Screen.width && mousePosition.y >= 0f && mousePosition.y <= (float)Screen.height;
	}
}
