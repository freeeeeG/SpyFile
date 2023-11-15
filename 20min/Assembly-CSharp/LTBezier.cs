using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class LTBezier
{
	// Token: 0x060002FE RID: 766 RVA: 0x000126EC File Offset: 0x000108EC
	public LTBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float precision)
	{
		this.a = a;
		this.aa = -a + 3f * (b - c) + d;
		this.bb = 3f * (a + c) - 6f * b;
		this.cc = 3f * (b - a);
		this.len = 1f / precision;
		this.arcLengths = new float[(int)this.len + 1];
		this.arcLengths[0] = 0f;
		Vector3 vector = a;
		float num = 0f;
		int num2 = 1;
		while ((float)num2 <= this.len)
		{
			Vector3 vector2 = this.bezierPoint((float)num2 * precision);
			num += (vector - vector2).magnitude;
			this.arcLengths[num2] = num;
			vector = vector2;
			num2++;
		}
		this.length = num;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000127E8 File Offset: 0x000109E8
	private float map(float u)
	{
		float num = u * this.arcLengths[(int)this.len];
		int i = 0;
		int num2 = (int)this.len;
		int num3 = 0;
		while (i < num2)
		{
			num3 = i + ((int)((float)(num2 - i) / 2f) | 0);
			if (this.arcLengths[num3] < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		if (this.arcLengths[num3] > num)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		return ((float)num3 + (num - this.arcLengths[num3]) / (this.arcLengths[num3 + 1] - this.arcLengths[num3])) / this.len;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00012878 File Offset: 0x00010A78
	private Vector3 bezierPoint(float t)
	{
		return ((this.aa * t + this.bb) * t + this.cc) * t + this.a;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000128B3 File Offset: 0x00010AB3
	public Vector3 point(float t)
	{
		return this.bezierPoint(this.map(t));
	}

	// Token: 0x0400018D RID: 397
	public float length;

	// Token: 0x0400018E RID: 398
	private Vector3 a;

	// Token: 0x0400018F RID: 399
	private Vector3 aa;

	// Token: 0x04000190 RID: 400
	private Vector3 bb;

	// Token: 0x04000191 RID: 401
	private Vector3 cc;

	// Token: 0x04000192 RID: 402
	private float len;

	// Token: 0x04000193 RID: 403
	private float[] arcLengths;
}
