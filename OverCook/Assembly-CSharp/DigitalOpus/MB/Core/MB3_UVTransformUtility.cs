using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200004B RID: 75
	public class MB3_UVTransformUtility
	{
		// Token: 0x0600021B RID: 539 RVA: 0x00018C48 File Offset: 0x00017048
		public static void Test()
		{
			DRect drect = new DRect(0.5, 0.5, 2.0, 2.0);
			DRect drect2 = new DRect(0.25, 0.25, 3.0, 3.0);
			DRect drect3 = MB3_UVTransformUtility.InverseTransform(ref drect);
			DRect drect4 = MB3_UVTransformUtility.InverseTransform(ref drect2);
			DRect drect5 = MB3_UVTransformUtility.CombineTransforms(ref drect, ref drect4);
			Debug.Log(drect3);
			Debug.Log(drect5);
			Debug.Log("one mat trans " + MB3_UVTransformUtility.TransformPoint(ref drect, new Vector2(1f, 1f)));
			Debug.Log("one inv mat trans " + MB3_UVTransformUtility.TransformPoint(ref drect3, new Vector2(1f, 1f)).ToString("f4"));
			Debug.Log("zero " + MB3_UVTransformUtility.TransformPoint(ref drect5, new Vector2(0f, 0f)).ToString("f4"));
			Debug.Log("one " + MB3_UVTransformUtility.TransformPoint(ref drect5, new Vector2(1f, 1f)).ToString("f4"));
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00018DA0 File Offset: 0x000171A0
		public static float TransformX(DRect r, double x)
		{
			return (float)(r.width * x + r.x);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00018DB4 File Offset: 0x000171B4
		public static DRect CombineTransforms(ref DRect r1, ref DRect r2)
		{
			DRect result = new DRect(r1.x * r2.width + r2.x, r1.y * r2.height + r2.y, r1.width * r2.width, r1.height * r2.height);
			return result;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00018E0C File Offset: 0x0001720C
		public static Rect CombineTransforms(ref Rect r1, ref Rect r2)
		{
			Rect result = new Rect(r1.x * r2.width + r2.x, r1.y * r2.height + r2.y, r1.width * r2.width, r1.height * r2.height);
			return result;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00018E64 File Offset: 0x00017264
		public static void Canonicalize(ref DRect r, double minX, double minY)
		{
			r.x -= (double)Mathf.FloorToInt((float)r.x);
			if (r.x < minX)
			{
				r.x += (double)Mathf.CeilToInt((float)minX);
			}
			r.y -= (double)Mathf.FloorToInt((float)r.y);
			if (r.y < minY)
			{
				r.y += (double)Mathf.CeilToInt((float)minY);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00018EE8 File Offset: 0x000172E8
		public static void Canonicalize(ref Rect r, float minX, float minY)
		{
			r.x -= (float)Mathf.FloorToInt(r.x);
			if (r.x < minX)
			{
				r.x += (float)Mathf.CeilToInt(minX);
			}
			r.y -= (float)Mathf.FloorToInt(r.y);
			if (r.y < minY)
			{
				r.y += (float)Mathf.CeilToInt(minY);
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00018F68 File Offset: 0x00017368
		public static DRect InverseTransform(ref DRect t)
		{
			return new DRect
			{
				x = -t.x / t.width,
				y = -t.y / t.height,
				width = 1.0 / t.width,
				height = 1.0 / t.height
			};
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00018FD8 File Offset: 0x000173D8
		public static DRect GetEncapsulatingRect(ref DRect uvRect1, ref DRect uvRect2)
		{
			double x = uvRect1.x;
			double y = uvRect1.y;
			double num = uvRect1.x + uvRect1.width;
			double num2 = uvRect1.y + uvRect1.height;
			double x2 = uvRect2.x;
			double y2 = uvRect2.y;
			double num3 = uvRect2.x + uvRect2.width;
			double num4 = uvRect2.y + uvRect2.height;
			double num6;
			double num5 = num6 = x;
			double num8;
			double num7 = num8 = y;
			if (x2 < num6)
			{
				num6 = x2;
			}
			if (x < num6)
			{
				num6 = x;
			}
			if (y2 < num8)
			{
				num8 = y2;
			}
			if (y < num8)
			{
				num8 = y;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			if (num > num5)
			{
				num5 = num;
			}
			if (num4 > num7)
			{
				num7 = num4;
			}
			if (num2 > num7)
			{
				num7 = num2;
			}
			DRect result = new DRect(num6, num8, num5 - num6, num7 - num8);
			return result;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000190C0 File Offset: 0x000174C0
		public static bool RectContains(ref DRect bigRect, ref DRect smallToTestIfFits)
		{
			double x = smallToTestIfFits.x;
			double y = smallToTestIfFits.y;
			double num = smallToTestIfFits.x + smallToTestIfFits.width;
			double num2 = smallToTestIfFits.y + smallToTestIfFits.height;
			double num3 = bigRect.x - 0.009999999776482582;
			double num4 = bigRect.y - 0.009999999776482582;
			double num5 = bigRect.x + bigRect.width + 0.019999999552965164;
			double num6 = bigRect.y + bigRect.height + 0.019999999552965164;
			return num3 <= x && x <= num5 && num3 <= num && num <= num5 && num4 <= y && y <= num6 && num4 <= num2 && num2 <= num6;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00019190 File Offset: 0x00017590
		public static bool RectContains(ref Rect bigRect, ref Rect smallToTestIfFits)
		{
			float x = smallToTestIfFits.x;
			float y = smallToTestIfFits.y;
			float num = smallToTestIfFits.x + smallToTestIfFits.width;
			float num2 = smallToTestIfFits.y + smallToTestIfFits.height;
			float num3 = bigRect.x - 0.01f;
			float num4 = bigRect.y - 0.01f;
			float num5 = bigRect.x + bigRect.width + 0.02f;
			float num6 = bigRect.y + bigRect.height + 0.02f;
			return num3 <= x && x <= num5 && num3 <= num && num <= num5 && num4 <= y && y <= num6 && num4 <= num2 && num2 <= num6;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00019250 File Offset: 0x00017650
		internal static Vector2 TransformPoint(ref DRect r, Vector2 p)
		{
			return new Vector2((float)(r.width * (double)p.x + r.x), (float)(r.height * (double)p.y + r.y));
		}
	}
}
