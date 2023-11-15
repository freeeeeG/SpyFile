using System;
using UnityEngine;

// Token: 0x0200021E RID: 542
public static class FloatUtils
{
	// Token: 0x06000922 RID: 2338 RVA: 0x00035FA2 File Offset: 0x000343A2
	public static int ToUnorm(float f, int bitCount)
	{
		f = Mathf.Clamp01(f);
		return (int)(f * ((float)(1 << bitCount - 1 - 1) + 0.5f));
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00035FC0 File Offset: 0x000343C0
	public static float FromUnorm(int i, int bitCount)
	{
		return Mathf.Clamp01((float)i / (float)(1 << bitCount - 1 - 1));
	}
}
