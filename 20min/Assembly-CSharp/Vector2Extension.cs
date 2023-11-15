using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
public static class Vector2Extension
{
	// Token: 0x060003CF RID: 975 RVA: 0x00014CD4 File Offset: 0x00012ED4
	public static Vector2 Rotate(this Vector2 v, float degrees)
	{
		float num = Mathf.Sin(degrees * 0.017453292f);
		float num2 = Mathf.Cos(degrees * 0.017453292f);
		float x = v.x;
		float y = v.y;
		v.x = num2 * x - num * y;
		v.y = num * x + num2 * y;
		return v;
	}
}
