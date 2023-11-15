using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public static class MathfExtensions
{
	// Token: 0x06000621 RID: 1569 RVA: 0x000178A8 File Offset: 0x00015AA8
	public static float Map(this float value, float fromMin, float fromMax, float toMin, float toMax)
	{
		float t = Mathf.InverseLerp(fromMin, fromMax, value);
		return Mathf.Lerp(toMin, toMax, t);
	}
}
