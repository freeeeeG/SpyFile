using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class Easing
{
	// Token: 0x06000AF1 RID: 2801 RVA: 0x0002A291 File Offset: 0x00028491
	public static float Linear(float k)
	{
		return k;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0002A294 File Offset: 0x00028494
	public static float GetEasingFunction(Easing.Type easingFunction, float t)
	{
		if (easingFunction == Easing.Type.EaseInQuad)
		{
			return Easing.Quadratic.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutQuad)
		{
			return Easing.Quadratic.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutQuad)
		{
			return Easing.Quadratic.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInCubic)
		{
			return Easing.Cubic.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutCubic)
		{
			return Easing.Cubic.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutCubic)
		{
			return Easing.Cubic.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInQuart)
		{
			return Easing.Quartic.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutQuart)
		{
			return Easing.Quartic.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutQuart)
		{
			return Easing.Quartic.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInQuint)
		{
			return Easing.Quintic.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutQuint)
		{
			return Easing.Quintic.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutQuint)
		{
			return Easing.Quintic.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInSine)
		{
			return Easing.Sinusoidal.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutSine)
		{
			return Easing.Sinusoidal.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutSine)
		{
			return Easing.Sinusoidal.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInExpo)
		{
			return Easing.Exponential.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutExpo)
		{
			return Easing.Exponential.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutExpo)
		{
			return Easing.Exponential.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInCirc)
		{
			return Easing.Circular.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutCirc)
		{
			return Easing.Circular.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutCirc)
		{
			return Easing.Circular.InOut(t);
		}
		if (easingFunction == Easing.Type.Linear)
		{
			return Easing.Linear(t);
		}
		if (easingFunction == Easing.Type.EaseInBounce)
		{
			return Easing.Bounce.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutBounce)
		{
			return Easing.Bounce.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutBounce)
		{
			return Easing.Bounce.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInBack)
		{
			return Easing.Back.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutBack)
		{
			return Easing.Back.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutBack)
		{
			return Easing.Back.InOut(t);
		}
		if (easingFunction == Easing.Type.EaseInElastic)
		{
			return Easing.Elastic.In(t);
		}
		if (easingFunction == Easing.Type.EaseOutElastic)
		{
			return Easing.Elastic.Out(t);
		}
		if (easingFunction == Easing.Type.EaseInOutElastic)
		{
			return Easing.Elastic.InOut(t);
		}
		if (easingFunction == Easing.Type.Spring)
		{
			return Easing.Bounce.Spring(t);
		}
		return 0f;
	}

	// Token: 0x020002B2 RID: 690
	public enum Type
	{
		// Token: 0x04000CA9 RID: 3241
		EaseInQuad,
		// Token: 0x04000CAA RID: 3242
		EaseOutQuad,
		// Token: 0x04000CAB RID: 3243
		EaseInOutQuad,
		// Token: 0x04000CAC RID: 3244
		EaseInCubic,
		// Token: 0x04000CAD RID: 3245
		EaseOutCubic,
		// Token: 0x04000CAE RID: 3246
		EaseInOutCubic,
		// Token: 0x04000CAF RID: 3247
		EaseInQuart,
		// Token: 0x04000CB0 RID: 3248
		EaseOutQuart,
		// Token: 0x04000CB1 RID: 3249
		EaseInOutQuart,
		// Token: 0x04000CB2 RID: 3250
		EaseInQuint,
		// Token: 0x04000CB3 RID: 3251
		EaseOutQuint,
		// Token: 0x04000CB4 RID: 3252
		EaseInOutQuint,
		// Token: 0x04000CB5 RID: 3253
		EaseInSine,
		// Token: 0x04000CB6 RID: 3254
		EaseOutSine,
		// Token: 0x04000CB7 RID: 3255
		EaseInOutSine,
		// Token: 0x04000CB8 RID: 3256
		EaseInExpo,
		// Token: 0x04000CB9 RID: 3257
		EaseOutExpo,
		// Token: 0x04000CBA RID: 3258
		EaseInOutExpo,
		// Token: 0x04000CBB RID: 3259
		EaseInCirc,
		// Token: 0x04000CBC RID: 3260
		EaseOutCirc,
		// Token: 0x04000CBD RID: 3261
		EaseInOutCirc,
		// Token: 0x04000CBE RID: 3262
		Linear,
		// Token: 0x04000CBF RID: 3263
		EaseInBounce,
		// Token: 0x04000CC0 RID: 3264
		EaseOutBounce,
		// Token: 0x04000CC1 RID: 3265
		EaseInOutBounce,
		// Token: 0x04000CC2 RID: 3266
		EaseInBack,
		// Token: 0x04000CC3 RID: 3267
		EaseOutBack,
		// Token: 0x04000CC4 RID: 3268
		EaseInOutBack,
		// Token: 0x04000CC5 RID: 3269
		EaseInElastic,
		// Token: 0x04000CC6 RID: 3270
		EaseOutElastic,
		// Token: 0x04000CC7 RID: 3271
		EaseInOutElastic,
		// Token: 0x04000CC8 RID: 3272
		Spring
	}

	// Token: 0x020002B3 RID: 691
	public class Quadratic
	{
		// Token: 0x06000F77 RID: 3959 RVA: 0x00038EEE File Offset: 0x000370EE
		public static float In(float k)
		{
			return k * k;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00038EF3 File Offset: 0x000370F3
		public static float Out(float k)
		{
			return k * (2f - k);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00038EFE File Offset: 0x000370FE
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return 0.5f * k * k;
			}
			return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
		}
	}

	// Token: 0x020002B4 RID: 692
	public class Cubic
	{
		// Token: 0x06000F7B RID: 3963 RVA: 0x00038F41 File Offset: 0x00037141
		public static float In(float k)
		{
			return k * k * k;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00038F48 File Offset: 0x00037148
		public static float Out(float k)
		{
			return 1f + (k -= 1f) * k * k;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00038F5E File Offset: 0x0003715E
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return 0.5f * k * k * k;
			}
			return 0.5f * ((k -= 2f) * k * k + 2f);
		}
	}

	// Token: 0x020002B5 RID: 693
	public class Quartic
	{
		// Token: 0x06000F7F RID: 3967 RVA: 0x00038F9F File Offset: 0x0003719F
		public static float In(float k)
		{
			return k * k * k * k;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00038FA8 File Offset: 0x000371A8
		public static float Out(float k)
		{
			return 1f - (k -= 1f) * k * k * k;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00038FC0 File Offset: 0x000371C0
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return 0.5f * k * k * k * k;
			}
			return -0.5f * ((k -= 2f) * k * k * k - 2f);
		}
	}

	// Token: 0x020002B6 RID: 694
	public class Quintic
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x00039005 File Offset: 0x00037205
		public static float In(float k)
		{
			return k * k * k * k * k;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00039010 File Offset: 0x00037210
		public static float Out(float k)
		{
			return 1f + (k -= 1f) * k * k * k * k;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0003902C File Offset: 0x0003722C
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return 0.5f * k * k * k * k * k;
			}
			return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
		}
	}

	// Token: 0x020002B7 RID: 695
	public class Sinusoidal
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x00039080 File Offset: 0x00037280
		public static float In(float k)
		{
			return 1f - Mathf.Cos(k * 3.1415927f / 2f);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0003909A File Offset: 0x0003729A
		public static float Out(float k)
		{
			return Mathf.Sin(k * 3.1415927f / 2f);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000390AE File Offset: 0x000372AE
		public static float InOut(float k)
		{
			return 0.5f * (1f - Mathf.Cos(3.1415927f * k));
		}
	}

	// Token: 0x020002B8 RID: 696
	public class Exponential
	{
		// Token: 0x06000F8B RID: 3979 RVA: 0x000390D0 File Offset: 0x000372D0
		public static float In(float k)
		{
			if (k != 0f)
			{
				return Mathf.Pow(1024f, k - 1f);
			}
			return 0f;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x000390F1 File Offset: 0x000372F1
		public static float Out(float k)
		{
			if (k != 1f)
			{
				return 1f - Mathf.Pow(2f, -10f * k);
			}
			return 1f;
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00039118 File Offset: 0x00037318
		public static float InOut(float k)
		{
			if (k == 0f)
			{
				return 0f;
			}
			if (k == 1f)
			{
				return 1f;
			}
			if ((k *= 2f) < 1f)
			{
				return 0.5f * Mathf.Pow(1024f, k - 1f);
			}
			return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
		}
	}

	// Token: 0x020002B9 RID: 697
	public class Circular
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x00039196 File Offset: 0x00037396
		public static float In(float k)
		{
			return 1f - Mathf.Sqrt(1f - k * k);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000391AC File Offset: 0x000373AC
		public static float Out(float k)
		{
			return Mathf.Sqrt(1f - (k -= 1f) * k);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x000391C8 File Offset: 0x000373C8
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return -0.5f * (Mathf.Sqrt(1f - k * k) - 1f);
			}
			return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
		}
	}

	// Token: 0x020002BA RID: 698
	public class Elastic
	{
		// Token: 0x06000F93 RID: 3987 RVA: 0x0003922C File Offset: 0x0003742C
		public static float In(float k)
		{
			if (k == 0f)
			{
				return 0f;
			}
			if (k == 1f)
			{
				return 1f;
			}
			return -Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * 6.2831855f / 0.4f);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0003928C File Offset: 0x0003748C
		public static float Out(float k)
		{
			if (k == 0f)
			{
				return 0f;
			}
			if (k == 1f)
			{
				return 1f;
			}
			return Mathf.Pow(2f, -10f * k) * Mathf.Sin((k - 0.1f) * 6.2831855f / 0.4f) + 1f;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000392E8 File Offset: 0x000374E8
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * 6.2831855f / 0.4f);
			}
			return Mathf.Pow(2f, -10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * 6.2831855f / 0.4f) * 0.5f + 1f;
		}
	}

	// Token: 0x020002BB RID: 699
	public class Back
	{
		// Token: 0x06000F97 RID: 3991 RVA: 0x00039387 File Offset: 0x00037587
		public static float In(float k)
		{
			return k * k * ((Easing.Back.s + 1f) * k - Easing.Back.s);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x000393A0 File Offset: 0x000375A0
		public static float Out(float k)
		{
			return (k -= 1f) * k * ((Easing.Back.s + 1f) * k + Easing.Back.s) + 1f;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000393C8 File Offset: 0x000375C8
		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f)
			{
				return 0.5f * (k * k * ((Easing.Back.s2 + 1f) * k - Easing.Back.s2));
			}
			return 0.5f * ((k -= 2f) * k * ((Easing.Back.s2 + 1f) * k + Easing.Back.s2) + 2f);
		}

		// Token: 0x04000CC9 RID: 3273
		private static float s = 1.70158f;

		// Token: 0x04000CCA RID: 3274
		private static float s2 = 2.5949094f;
	}

	// Token: 0x020002BC RID: 700
	public class Bounce
	{
		// Token: 0x06000F9C RID: 3996 RVA: 0x0003944E File Offset: 0x0003764E
		public static float In(float k)
		{
			return 1f - Easing.Bounce.Out(1f - k);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00039464 File Offset: 0x00037664
		public static float Out(float k)
		{
			if (k < 0.36363637f)
			{
				return 7.5625f * k * k;
			}
			if (k < 0.72727275f)
			{
				return 7.5625f * (k -= 0.54545456f) * k + 0.75f;
			}
			if (k < 0.90909094f)
			{
				return 7.5625f * (k -= 0.8181818f) * k + 0.9375f;
			}
			return 7.5625f * (k -= 0.95454544f) * k + 0.984375f;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x000394DD File Offset: 0x000376DD
		public static float InOut(float k)
		{
			if (k < 0.5f)
			{
				return Easing.Bounce.In(k * 2f) * 0.5f;
			}
			return Easing.Bounce.Out(k * 2f - 1f) * 0.5f + 0.5f;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00039518 File Offset: 0x00037718
		public static float Spring(float t)
		{
			t = Mathf.Clamp01(t);
			t = (Mathf.Sin(t * 3.1415927f * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + 1.2f * (1f - t));
			return t;
		}
	}
}
