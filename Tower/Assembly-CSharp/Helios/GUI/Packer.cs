﻿using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000D8 RID: 216
	public static class Packer
	{
		// Token: 0x0600031E RID: 798 RVA: 0x0000E034 File Offset: 0x0000C234
		public static float ToFloat(float x, float y, float z, float w)
		{
			x = ((x < 0f) ? 0f : ((1f < x) ? 1f : x));
			y = ((y < 0f) ? 0f : ((1f < y) ? 1f : y));
			z = ((z < 0f) ? 0f : ((1f < z) ? 1f : z));
			w = ((w < 0f) ? 0f : ((1f < w) ? 1f : w));
			return (float)((Mathf.FloorToInt(w * 63f) << 18) + (Mathf.FloorToInt(z * 63f) << 12) + (Mathf.FloorToInt(y * 63f) << 6) + Mathf.FloorToInt(x * 63f));
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E101 File Offset: 0x0000C301
		public static float ToFloat(Vector4 factor)
		{
			return Packer.ToFloat(Mathf.Clamp01(factor.x), Mathf.Clamp01(factor.y), Mathf.Clamp01(factor.z), Mathf.Clamp01(factor.w));
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E134 File Offset: 0x0000C334
		public static float ToFloat(float x, float y, float z)
		{
			x = ((x < 0f) ? 0f : ((1f < x) ? 1f : x));
			y = ((y < 0f) ? 0f : ((1f < y) ? 1f : y));
			z = ((z < 0f) ? 0f : ((1f < z) ? 1f : z));
			return (float)((Mathf.FloorToInt(z * 255f) << 16) + (Mathf.FloorToInt(y * 255f) << 8) + Mathf.FloorToInt(x * 255f));
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E1D0 File Offset: 0x0000C3D0
		public static float ToFloat(float x, float y)
		{
			x = ((x < 0f) ? 0f : ((1f < x) ? 1f : x));
			y = ((y < 0f) ? 0f : ((1f < y) ? 1f : y));
			return (float)((Mathf.FloorToInt(y * 4095f) << 12) + Mathf.FloorToInt(x * 4095f));
		}
	}
}
