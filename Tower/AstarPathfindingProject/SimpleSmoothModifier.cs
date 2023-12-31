﻿using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200007F RID: 127
	[AddComponentMenu("Pathfinding/Modifiers/Simple Smooth")]
	[RequireComponent(typeof(Seeker))]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_simple_smooth_modifier.php")]
	[Serializable]
	public class SimpleSmoothModifier : MonoModifier
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00025D18 File Offset: 0x00023F18
		public override int Order
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00025D1C File Offset: 0x00023F1C
		public override void Apply(Path p)
		{
			if (p.vectorPath == null)
			{
				Debug.LogWarning("Can't process NULL path (has another modifier logged an error?)");
				return;
			}
			List<Vector3> list = null;
			switch (this.smoothType)
			{
			case SimpleSmoothModifier.SmoothType.Simple:
				list = this.SmoothSimple(p.vectorPath);
				break;
			case SimpleSmoothModifier.SmoothType.Bezier:
				list = this.SmoothBezier(p.vectorPath);
				break;
			case SimpleSmoothModifier.SmoothType.OffsetSimple:
				list = this.SmoothOffsetSimple(p.vectorPath);
				break;
			case SimpleSmoothModifier.SmoothType.CurvedNonuniform:
				list = this.CurvedNonuniform(p.vectorPath);
				break;
			}
			if (list != p.vectorPath)
			{
				ListPool<Vector3>.Release(ref p.vectorPath);
				p.vectorPath = list;
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00025DB4 File Offset: 0x00023FB4
		public List<Vector3> CurvedNonuniform(List<Vector3> path)
		{
			if (this.maxSegmentLength <= 0f)
			{
				Debug.LogWarning("Max Segment Length is <= 0 which would cause DivByZero-exception or other nasty errors (avoid this)");
				return path;
			}
			int num = 0;
			for (int i = 0; i < path.Count - 1; i++)
			{
				float magnitude = (path[i] - path[i + 1]).magnitude;
				for (float num2 = 0f; num2 <= magnitude; num2 += this.maxSegmentLength)
				{
					num++;
				}
			}
			List<Vector3> list = ListPool<Vector3>.Claim(num);
			Vector3 vector = (path[1] - path[0]).normalized;
			for (int j = 0; j < path.Count - 1; j++)
			{
				float magnitude2 = (path[j] - path[j + 1]).magnitude;
				Vector3 a = vector;
				Vector3 vector2 = (j < path.Count - 2) ? ((path[j + 2] - path[j + 1]).normalized - (path[j] - path[j + 1]).normalized).normalized : (path[j + 1] - path[j]).normalized;
				Vector3 tan = a * magnitude2 * this.factor;
				Vector3 tan2 = vector2 * magnitude2 * this.factor;
				Vector3 a2 = path[j];
				Vector3 b = path[j + 1];
				float num3 = 1f / magnitude2;
				for (float num4 = 0f; num4 <= magnitude2; num4 += this.maxSegmentLength)
				{
					float t = num4 * num3;
					list.Add(SimpleSmoothModifier.GetPointOnCubic(a2, b, tan, tan2, t));
				}
				vector = vector2;
			}
			list[list.Count - 1] = path[path.Count - 1];
			return list;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00025FB8 File Offset: 0x000241B8
		public static Vector3 GetPointOnCubic(Vector3 a, Vector3 b, Vector3 tan1, Vector3 tan2, float t)
		{
			float num = t * t;
			float num2 = num * t;
			float d = 2f * num2 - 3f * num + 1f;
			float d2 = -2f * num2 + 3f * num;
			float d3 = num2 - 2f * num + t;
			float d4 = num2 - num;
			return d * a + d2 * b + d3 * tan1 + d4 * tan2;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00026034 File Offset: 0x00024234
		public List<Vector3> SmoothOffsetSimple(List<Vector3> path)
		{
			if (path.Count <= 2 || this.iterations <= 0)
			{
				return path;
			}
			if (this.iterations > 12)
			{
				Debug.LogWarning("A very high iteration count was passed, won't let this one through");
				return path;
			}
			int num = (path.Count - 2) * (int)Mathf.Pow(2f, (float)this.iterations) + 2;
			List<Vector3> list = ListPool<Vector3>.Claim(num);
			List<Vector3> list2 = ListPool<Vector3>.Claim(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(Vector3.zero);
				list2.Add(Vector3.zero);
			}
			for (int j = 0; j < path.Count; j++)
			{
				list[j] = path[j];
			}
			for (int k = 0; k < this.iterations; k++)
			{
				int num2 = (path.Count - 2) * (int)Mathf.Pow(2f, (float)k) + 2;
				List<Vector3> list3 = list;
				list = list2;
				list2 = list3;
				for (int l = 0; l < num2 - 1; l++)
				{
					Vector3 vector = list2[l];
					Vector3 vector2 = list2[l + 1];
					Vector3 normalized = Vector3.Cross(vector2 - vector, Vector3.up).normalized;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					if (l != 0 && !VectorMath.IsColinearXZ(vector, vector2, list2[l - 1]))
					{
						flag3 = true;
						flag = VectorMath.RightOrColinearXZ(vector, vector2, list2[l - 1]);
					}
					if (l < num2 - 1 && !VectorMath.IsColinearXZ(vector, vector2, list2[l + 2]))
					{
						flag4 = true;
						flag2 = VectorMath.RightOrColinearXZ(vector, vector2, list2[l + 2]);
					}
					if (flag3)
					{
						list[l * 2] = vector + (flag ? (normalized * this.offset * 1f) : (-normalized * this.offset * 1f));
					}
					else
					{
						list[l * 2] = vector;
					}
					if (flag4)
					{
						list[l * 2 + 1] = vector2 + (flag2 ? (normalized * this.offset * 1f) : (-normalized * this.offset * 1f));
					}
					else
					{
						list[l * 2 + 1] = vector2;
					}
				}
				list[(path.Count - 2) * (int)Mathf.Pow(2f, (float)(k + 1)) + 2 - 1] = list2[num2 - 1];
			}
			ListPool<Vector3>.Release(ref list2);
			return list;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x000262C8 File Offset: 0x000244C8
		public List<Vector3> SmoothSimple(List<Vector3> path)
		{
			if (path.Count < 2)
			{
				return path;
			}
			List<Vector3> list;
			if (this.uniformLength)
			{
				this.maxSegmentLength = Mathf.Max(this.maxSegmentLength, 0.005f);
				float num = 0f;
				for (int i = 0; i < path.Count - 1; i++)
				{
					num += Vector3.Distance(path[i], path[i + 1]);
				}
				list = ListPool<Vector3>.Claim(Mathf.FloorToInt(num / this.maxSegmentLength) + 2);
				float num2 = 0f;
				for (int j = 0; j < path.Count - 1; j++)
				{
					Vector3 a = path[j];
					Vector3 b = path[j + 1];
					float num3 = Vector3.Distance(a, b);
					while (num2 < num3)
					{
						list.Add(Vector3.Lerp(a, b, num2 / num3));
						num2 += this.maxSegmentLength;
					}
					num2 -= num3;
				}
				list.Add(path[path.Count - 1]);
			}
			else
			{
				this.subdivisions = Mathf.Max(this.subdivisions, 0);
				if (this.subdivisions > 10)
				{
					Debug.LogWarning("Very large number of subdivisions. Cowardly refusing to subdivide every segment into more than " + (1 << this.subdivisions).ToString() + " subsegments");
					this.subdivisions = 10;
				}
				int num4 = 1 << this.subdivisions;
				list = ListPool<Vector3>.Claim((path.Count - 1) * num4 + 1);
				Polygon.Subdivide(path, list, num4);
			}
			if (this.strength > 0f)
			{
				for (int k = 0; k < this.iterations; k++)
				{
					Vector3 a2 = list[0];
					for (int l = 1; l < list.Count - 1; l++)
					{
						Vector3 vector = list[l];
						list[l] = Vector3.Lerp(vector, (a2 + list[l + 1]) / 2f, this.strength);
						a2 = vector;
					}
				}
			}
			return list;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000264BC File Offset: 0x000246BC
		public List<Vector3> SmoothBezier(List<Vector3> path)
		{
			if (this.subdivisions < 0)
			{
				this.subdivisions = 0;
			}
			int num = 1 << this.subdivisions;
			List<Vector3> list = ListPool<Vector3>.Claim();
			for (int i = 0; i < path.Count - 1; i++)
			{
				Vector3 vector;
				if (i == 0)
				{
					vector = path[i + 1] - path[i];
				}
				else
				{
					vector = path[i + 1] - path[i - 1];
				}
				Vector3 vector2;
				if (i == path.Count - 2)
				{
					vector2 = path[i] - path[i + 1];
				}
				else
				{
					vector2 = path[i] - path[i + 2];
				}
				vector *= this.bezierTangentLength;
				vector2 *= this.bezierTangentLength;
				Vector3 vector3 = path[i];
				Vector3 p = vector3 + vector;
				Vector3 vector4 = path[i + 1];
				Vector3 p2 = vector4 + vector2;
				for (int j = 0; j < num; j++)
				{
					list.Add(AstarSplines.CubicBezier(vector3, p, p2, vector4, (float)j / (float)num));
				}
			}
			list.Add(path[path.Count - 1]);
			return list;
		}

		// Token: 0x040003A2 RID: 930
		public SimpleSmoothModifier.SmoothType smoothType;

		// Token: 0x040003A3 RID: 931
		[Tooltip("The number of times to subdivide (divide in half) the path segments. [0...inf] (recommended [1...10])")]
		public int subdivisions = 2;

		// Token: 0x040003A4 RID: 932
		[Tooltip("Number of times to apply smoothing")]
		public int iterations = 2;

		// Token: 0x040003A5 RID: 933
		[Tooltip("Determines how much smoothing to apply in each smooth iteration. 0.5 usually produces the nicest looking curves")]
		[Range(0f, 1f)]
		public float strength = 0.5f;

		// Token: 0x040003A6 RID: 934
		[Tooltip("Toggle to divide all lines in equal length segments")]
		public bool uniformLength = true;

		// Token: 0x040003A7 RID: 935
		[Tooltip("The length of each segment in the smoothed path. A high value yields rough paths and low value yields very smooth paths, but is slower")]
		public float maxSegmentLength = 2f;

		// Token: 0x040003A8 RID: 936
		[Tooltip("Length factor of the bezier curves' tangents")]
		public float bezierTangentLength = 0.4f;

		// Token: 0x040003A9 RID: 937
		[Tooltip("Offset to apply in each smoothing iteration when using Offset Simple")]
		public float offset = 0.2f;

		// Token: 0x040003AA RID: 938
		[Tooltip("How much to smooth the path. A higher value will give a smoother path, but might take the character far off the optimal path.")]
		public float factor = 0.1f;

		// Token: 0x0200014E RID: 334
		public enum SmoothType
		{
			// Token: 0x040007A9 RID: 1961
			Simple,
			// Token: 0x040007AA RID: 1962
			Bezier,
			// Token: 0x040007AB RID: 1963
			OffsetSimple,
			// Token: 0x040007AC RID: 1964
			CurvedNonuniform
		}
	}
}
