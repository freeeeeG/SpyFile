using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public static class FixedAspectRatio
{
	// Token: 0x060007C8 RID: 1992 RVA: 0x000309CC File Offset: 0x0002EDCC
	public static Rect ComputeAspectRatioRect(float aspectX, float aspectY, float width, float height)
	{
		float num = aspectX / aspectY;
		float num2 = width / height;
		float num3 = num2 / num;
		float num4 = 1f / num3;
		Rect result;
		if (num3 < 1f)
		{
			result = new Rect(0f, (1f - num3) / 2f, 1f, num3);
		}
		else
		{
			result = new Rect((1f - num4) / 2f, 0f, num4, 1f);
		}
		return result;
	}

	// Token: 0x0400062E RID: 1582
	public static readonly Vector2 DefaultAspect = new Vector2(16f, 9f);
}
