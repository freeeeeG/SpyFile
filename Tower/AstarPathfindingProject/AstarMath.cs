using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000031 RID: 49
	public static class AstarMath
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000B37B File Offset: 0x0000957B
		public static float MapTo(float startMin, float startMax, float targetMin, float targetMax, float value)
		{
			return Mathf.Lerp(targetMin, targetMax, Mathf.InverseLerp(startMin, startMax, value));
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000B390 File Offset: 0x00009590
		public static string FormatBytesBinary(int bytes)
		{
			double num = (bytes >= 0) ? 1.0 : -1.0;
			bytes = Mathf.Abs(bytes);
			if (bytes < 1024)
			{
				return ((double)bytes * num).ToString() + " bytes";
			}
			if (bytes < 1048576)
			{
				return ((double)bytes / 1024.0 * num).ToString("0.0") + " KiB";
			}
			if (bytes < 1073741824)
			{
				return ((double)bytes / 1048576.0 * num).ToString("0.0") + " MiB";
			}
			return ((double)bytes / 1073741824.0 * num).ToString("0.0") + " GiB";
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000B45E File Offset: 0x0000965E
		private static int Bit(int a, int b)
		{
			return a >> b & 1;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000B468 File Offset: 0x00009668
		public static Color IntToColor(int i, float a)
		{
			float num = (float)(AstarMath.Bit(i, 2) + AstarMath.Bit(i, 3) * 2 + 1);
			int num2 = AstarMath.Bit(i, 1) + AstarMath.Bit(i, 4) * 2 + 1;
			int num3 = AstarMath.Bit(i, 0) + AstarMath.Bit(i, 5) * 2 + 1;
			return new Color(num * 0.25f, (float)num2 * 0.25f, (float)num3 * 0.25f, a);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000B4D0 File Offset: 0x000096D0
		public static Color HSVToRGB(float h, float s, float v)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = s * v;
			float num5 = h / 60f;
			float num6 = num4 * (1f - Math.Abs(num5 % 2f - 1f));
			if (num5 < 1f)
			{
				num = num4;
				num2 = num6;
			}
			else if (num5 < 2f)
			{
				num = num6;
				num2 = num4;
			}
			else if (num5 < 3f)
			{
				num2 = num4;
				num3 = num6;
			}
			else if (num5 < 4f)
			{
				num2 = num6;
				num3 = num4;
			}
			else if (num5 < 5f)
			{
				num = num6;
				num3 = num4;
			}
			else if (num5 < 6f)
			{
				num = num4;
				num3 = num6;
			}
			float num7 = v - num4;
			num += num7;
			num2 += num7;
			num3 += num7;
			return new Color(num, num2, num3);
		}
	}
}
