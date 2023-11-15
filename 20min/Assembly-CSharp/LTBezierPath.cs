using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class LTBezierPath
{
	// Token: 0x06000302 RID: 770 RVA: 0x0000DD02 File Offset: 0x0000BF02
	public LTBezierPath()
	{
	}

	// Token: 0x06000303 RID: 771 RVA: 0x000128C2 File Offset: 0x00010AC2
	public LTBezierPath(Vector3[] pts_)
	{
		this.setPoints(pts_);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x000128D4 File Offset: 0x00010AD4
	public void setPoints(Vector3[] pts_)
	{
		if (pts_.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, you must pass four or more values!");
		}
		if (pts_.Length % 4 != 0)
		{
			LeanTween.logError("LeanTween - When passing values for a vector path, they must be in sets of four: controlPoint1, controlPoint2, endPoint2, controlPoint2, controlPoint2...");
		}
		this.pts = pts_;
		int num = 0;
		this.beziers = new LTBezier[this.pts.Length / 4];
		this.lengthRatio = new float[this.beziers.Length];
		this.length = 0f;
		for (int i = 0; i < this.pts.Length; i += 4)
		{
			this.beziers[num] = new LTBezier(this.pts[i], this.pts[i + 2], this.pts[i + 1], this.pts[i + 3], 0.05f);
			this.length += this.beziers[num].length;
			num++;
		}
		for (int i = 0; i < this.beziers.Length; i++)
		{
			this.lengthRatio[i] = this.beziers[i].length / this.length;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000305 RID: 773 RVA: 0x000129E8 File Offset: 0x00010BE8
	public float distance
	{
		get
		{
			return this.length;
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x000129F0 File Offset: 0x00010BF0
	public Vector3 point(float ratio)
	{
		float num = 0f;
		for (int i = 0; i < this.lengthRatio.Length; i++)
		{
			num += this.lengthRatio[i];
			if (num >= ratio)
			{
				return this.beziers[i].point((ratio - (num - this.lengthRatio[i])) / this.lengthRatio[i]);
			}
		}
		return this.beziers[this.lengthRatio.Length - 1].point(1f);
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00012A64 File Offset: 0x00010C64
	public void place2d(Transform transform, float ratio)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.eulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00012AD0 File Offset: 0x00010CD0
	public void placeLocal2d(Transform transform, float ratio)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.localPosition;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.localEulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00012B3C File Offset: 0x00010D3C
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00012B4B File Offset: 0x00010D4B
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00012B79 File Offset: 0x00010D79
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00012B88 File Offset: 0x00010D88
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		ratio = Mathf.Clamp01(ratio);
		transform.localPosition = this.point(ratio);
		ratio = Mathf.Clamp01(ratio + 0.001f);
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00012BDC File Offset: 0x00010DDC
	public void gizmoDraw(float t = -1f)
	{
		Vector3 to = this.point(0f);
		for (int i = 1; i <= 120; i++)
		{
			float ratio = (float)i / 120f;
			Vector3 vector = this.point(ratio);
			Gizmos.color = ((this.previousBezier == this.currentBezier) ? Color.magenta : Color.grey);
			Gizmos.DrawLine(vector, to);
			to = vector;
			this.previousBezier = this.currentBezier;
		}
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00012C48 File Offset: 0x00010E48
	public float ratioAtPoint(Vector3 pt, float precision = 0.01f)
	{
		float num = float.MaxValue;
		int num2 = 0;
		int num3 = Mathf.RoundToInt(1f / precision);
		for (int i = 0; i < num3; i++)
		{
			float ratio = (float)i / (float)num3;
			float num4 = Vector3.Distance(pt, this.point(ratio));
			if (num4 < num)
			{
				num = num4;
				num2 = i;
			}
		}
		return (float)num2 / (float)num3;
	}

	// Token: 0x04000194 RID: 404
	public Vector3[] pts;

	// Token: 0x04000195 RID: 405
	public float length;

	// Token: 0x04000196 RID: 406
	public bool orientToPath;

	// Token: 0x04000197 RID: 407
	public bool orientToPath2d;

	// Token: 0x04000198 RID: 408
	private LTBezier[] beziers;

	// Token: 0x04000199 RID: 409
	private float[] lengthRatio;

	// Token: 0x0400019A RID: 410
	private int currentBezier;

	// Token: 0x0400019B RID: 411
	private int previousBezier;
}
