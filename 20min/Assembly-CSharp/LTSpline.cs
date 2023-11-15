using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000029 RID: 41
[Serializable]
public class LTSpline
{
	// Token: 0x0600030F RID: 783 RVA: 0x00012C9C File Offset: 0x00010E9C
	public LTSpline(Vector3[] pts)
	{
		this.init(pts, true);
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00012CB3 File Offset: 0x00010EB3
	public LTSpline(Vector3[] pts, bool constantSpeed)
	{
		this.constantSpeed = constantSpeed;
		this.init(pts, constantSpeed);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00012CD4 File Offset: 0x00010ED4
	private void init(Vector3[] pts, bool constantSpeed)
	{
		if (pts.Length < 4)
		{
			LeanTween.logError("LeanTween - When passing values for a spline path, you must pass four or more values!");
			return;
		}
		this.pts = new Vector3[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
		this.numSections = pts.Length - 3;
		float num = float.PositiveInfinity;
		Vector3 vector = this.pts[1];
		float num2 = 0f;
		for (int i = 1; i < this.pts.Length - 1; i++)
		{
			float num3 = Vector3.Distance(this.pts[i], vector);
			if (num3 < num)
			{
				num = num3;
			}
			num2 += num3;
		}
		if (constantSpeed)
		{
			num = num2 / (float)(this.numSections * LTSpline.SUBLINE_COUNT);
			float num4 = num / (float)LTSpline.SUBLINE_COUNT;
			int num5 = (int)Mathf.Ceil(num2 / num4) * LTSpline.DISTANCE_COUNT;
			if (num5 <= 1)
			{
				num5 = 2;
			}
			this.ptsAdj = new Vector3[num5];
			vector = this.interp(0f);
			int num6 = 1;
			this.ptsAdj[0] = vector;
			this.distance = 0f;
			for (int j = 0; j < num5 + 1; j++)
			{
				float num7 = (float)j / (float)num5;
				Vector3 vector2 = this.interp(num7);
				float num8 = Vector3.Distance(vector2, vector);
				if (num8 >= num4 || num7 >= 1f)
				{
					this.ptsAdj[num6] = vector2;
					this.distance += num8;
					vector = vector2;
					num6++;
				}
			}
			this.ptsAdjLength = num6;
		}
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00012E48 File Offset: 0x00011048
	public Vector3 map(float u)
	{
		if (u >= 1f)
		{
			return this.pts[this.pts.Length - 2];
		}
		float num = u * (float)(this.ptsAdjLength - 1);
		int num2 = (int)Mathf.Floor(num);
		int num3 = (int)Mathf.Ceil(num);
		if (num2 < 0)
		{
			num2 = 0;
		}
		Vector3 vector = this.ptsAdj[num2];
		Vector3 a = this.ptsAdj[num3];
		float d = num - (float)num2;
		return vector + (a - vector) * d;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00012ECC File Offset: 0x000110CC
	public Vector3 interp(float t)
	{
		this.currPt = Mathf.Min(Mathf.FloorToInt(t * (float)this.numSections), this.numSections - 1);
		float num = t * (float)this.numSections - (float)this.currPt;
		Vector3 a = this.pts[this.currPt];
		Vector3 a2 = this.pts[this.currPt + 1];
		Vector3 vector = this.pts[this.currPt + 2];
		Vector3 b = this.pts[this.currPt + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num * num * num) + (2f * a - 5f * a2 + 4f * vector - b) * (num * num) + (-a + vector) * num + 2f * a2);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00013004 File Offset: 0x00011204
	public float ratioAtPoint(Vector3 pt)
	{
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < this.ptsAdjLength; i++)
		{
			float num3 = Vector3.Distance(pt, this.ptsAdj[i]);
			if (num3 < num)
			{
				num = num3;
				num2 = i;
			}
		}
		return (float)num2 / (float)(this.ptsAdjLength - 1);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00013054 File Offset: 0x00011254
	public Vector3 point(float ratio)
	{
		float num = (ratio > 1f) ? 1f : ratio;
		if (!this.constantSpeed)
		{
			return this.interp(num);
		}
		return this.map(num);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0001308C File Offset: 0x0001128C
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

	// Token: 0x06000317 RID: 791 RVA: 0x000130F8 File Offset: 0x000112F8
	public void placeLocal2d(Transform transform, float ratio)
	{
		if (transform.parent == null)
		{
			this.place2d(transform, ratio);
			return;
		}
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			Vector3 vector = this.point(ratio) - transform.localPosition;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			transform.localEulerAngles = new Vector3(0f, 0f, z);
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0001317B File Offset: 0x0001137B
	public void place(Transform transform, float ratio)
	{
		this.place(transform, ratio, Vector3.up);
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0001318A File Offset: 0x0001138A
	public void place(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.position = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(this.point(ratio), worldUp);
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x000131B8 File Offset: 0x000113B8
	public void placeLocal(Transform transform, float ratio)
	{
		this.placeLocal(transform, ratio, Vector3.up);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000131C7 File Offset: 0x000113C7
	public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
	{
		transform.localPosition = this.point(ratio);
		ratio += 0.001f;
		if (ratio <= 1f)
		{
			transform.LookAt(transform.parent.TransformPoint(this.point(ratio)), worldUp);
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00013200 File Offset: 0x00011400
	public void gizmoDraw(float t = -1f)
	{
		if (this.ptsAdj == null || this.ptsAdj.Length == 0)
		{
			return;
		}
		Vector3 from = this.ptsAdj[0];
		for (int i = 0; i < this.ptsAdjLength; i++)
		{
			Vector3 vector = this.ptsAdj[i];
			Gizmos.DrawLine(from, vector);
			from = vector;
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00013254 File Offset: 0x00011454
	public void drawGizmo(Color color)
	{
		if (this.ptsAdjLength >= 4)
		{
			Vector3 from = this.ptsAdj[0];
			Color color2 = Gizmos.color;
			Gizmos.color = color;
			for (int i = 0; i < this.ptsAdjLength; i++)
			{
				Vector3 vector = this.ptsAdj[i];
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
			Gizmos.color = color2;
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x000132B0 File Offset: 0x000114B0
	public static void drawGizmo(Transform[] arr, Color color)
	{
		if (arr.Length >= 4)
		{
			Vector3[] array = new Vector3[arr.Length];
			for (int i = 0; i < arr.Length; i++)
			{
				array[i] = arr[i].position;
			}
			LTSpline ltspline = new LTSpline(array);
			Vector3 from = ltspline.ptsAdj[0];
			Color color2 = Gizmos.color;
			Gizmos.color = color;
			for (int j = 0; j < ltspline.ptsAdjLength; j++)
			{
				Vector3 vector = ltspline.ptsAdj[j];
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
			Gizmos.color = color2;
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00013347 File Offset: 0x00011547
	public static void drawLine(Transform[] arr, float width, Color color)
	{
		int num = arr.Length;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00013350 File Offset: 0x00011550
	public void drawLinesGLLines(Material outlineMaterial, Color color, float width)
	{
		GL.PushMatrix();
		outlineMaterial.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Begin(1);
		GL.Color(color);
		if (this.constantSpeed)
		{
			if (this.ptsAdjLength >= 4)
			{
				Vector3 v = this.ptsAdj[0];
				for (int i = 0; i < this.ptsAdjLength; i++)
				{
					Vector3 vector = this.ptsAdj[i];
					GL.Vertex(v);
					GL.Vertex(vector);
					v = vector;
				}
			}
		}
		else if (this.pts.Length >= 4)
		{
			Vector3 v2 = this.pts[0];
			float num = 1f / ((float)this.pts.Length * 10f);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float t = num2 / 1f;
				Vector3 vector2 = this.interp(t);
				GL.Vertex(v2);
				GL.Vertex(vector2);
				v2 = vector2;
			}
		}
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00013438 File Offset: 0x00011638
	public Vector3[] generateVectors()
	{
		if (this.pts.Length >= 4)
		{
			List<Vector3> list = new List<Vector3>();
			Vector3 item = this.pts[0];
			list.Add(item);
			float num = 1f / ((float)this.pts.Length * 10f);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float t = num2 / 1f;
				Vector3 item2 = this.interp(t);
				list.Add(item2);
			}
			list.ToArray();
		}
		return null;
	}

	// Token: 0x0400019C RID: 412
	public static int DISTANCE_COUNT = 3;

	// Token: 0x0400019D RID: 413
	public static int SUBLINE_COUNT = 20;

	// Token: 0x0400019E RID: 414
	public float distance;

	// Token: 0x0400019F RID: 415
	public bool constantSpeed = true;

	// Token: 0x040001A0 RID: 416
	public Vector3[] pts;

	// Token: 0x040001A1 RID: 417
	[NonSerialized]
	public Vector3[] ptsAdj;

	// Token: 0x040001A2 RID: 418
	public int ptsAdjLength;

	// Token: 0x040001A3 RID: 419
	public bool orientToPath;

	// Token: 0x040001A4 RID: 420
	public bool orientToPath2d;

	// Token: 0x040001A5 RID: 421
	private int numSections;

	// Token: 0x040001A6 RID: 422
	private int currPt;
}
