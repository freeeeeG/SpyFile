﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000C0 RID: 192
	public class Draw
	{
		// Token: 0x06000841 RID: 2113 RVA: 0x00036EB0 File Offset: 0x000350B0
		private void SetColor(Color color)
		{
			if (this.gizmos && UnityEngine.Gizmos.color != color)
			{
				UnityEngine.Gizmos.color = color;
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00036ED0 File Offset: 0x000350D0
		public void Polyline(List<Vector3> points, Color color, bool cycle = false)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1], color);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0], color);
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00036F30 File Offset: 0x00035130
		public void Line(Vector3 a, Vector3 b, Color color)
		{
			this.SetColor(color);
			if (this.gizmos)
			{
				UnityEngine.Gizmos.DrawLine(this.matrix.MultiplyPoint3x4(a), this.matrix.MultiplyPoint3x4(b));
				return;
			}
			UnityEngine.Debug.DrawLine(this.matrix.MultiplyPoint3x4(a), this.matrix.MultiplyPoint3x4(b), color);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00036F88 File Offset: 0x00035188
		public void CircleXZ(Vector3 center, float radius, Color color, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			int num = 40;
			while (startAngle > endAngle)
			{
				startAngle -= 6.2831855f;
			}
			Vector3 b = new Vector3(Mathf.Cos(startAngle) * radius, 0f, Mathf.Sin(startAngle) * radius);
			for (int i = 0; i <= num; i++)
			{
				Vector3 vector = new Vector3(Mathf.Cos(Mathf.Lerp(startAngle, endAngle, (float)i / (float)num)) * radius, 0f, Mathf.Sin(Mathf.Lerp(startAngle, endAngle, (float)i / (float)num)) * radius);
				this.Line(center + b, center + vector, color);
				b = vector;
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00037024 File Offset: 0x00035224
		public void Cylinder(Vector3 position, Vector3 up, float height, float radius, Color color)
		{
			Vector3 normalized = Vector3.Cross(up, Vector3.one).normalized;
			this.matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(normalized, up), new Vector3(radius, height, radius));
			this.CircleXZ(Vector3.zero, 1f, color, 0f, 6.2831855f);
			if (height > 0f)
			{
				this.CircleXZ(Vector3.up, 1f, color, 0f, 6.2831855f);
				this.Line(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), color);
				this.Line(new Vector3(-1f, 0f, 0f), new Vector3(-1f, 1f, 0f), color);
				this.Line(new Vector3(0f, 0f, 1f), new Vector3(0f, 1f, 1f), color);
				this.Line(new Vector3(0f, 0f, -1f), new Vector3(0f, 1f, -1f), color);
			}
			this.matrix = Matrix4x4.identity;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00037170 File Offset: 0x00035370
		public void CrossXZ(Vector3 position, Color color, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - Vector3.right * size, position + Vector3.right * size, color);
			this.Line(position - Vector3.forward * size, position + Vector3.forward * size, color);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000371D8 File Offset: 0x000353D8
		public void Bezier(Vector3 a, Vector3 b, Color color)
		{
			Vector3 vector = b - a;
			if (vector == Vector3.zero)
			{
				return;
			}
			Vector3 rhs = Vector3.Cross(Vector3.up, vector);
			Vector3 vector2 = Vector3.Cross(vector, rhs).normalized;
			vector2 *= vector.magnitude * 0.1f;
			Vector3 p = a + vector2;
			Vector3 p2 = b + vector2;
			Vector3 a2 = a;
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				Vector3 vector3 = AstarSplines.CubicBezier(a, p, p2, b, t);
				this.Line(a2, vector3, color);
				a2 = vector3;
			}
		}

		// Token: 0x040004D5 RID: 1237
		public static readonly Draw Debug = new Draw
		{
			gizmos = false
		};

		// Token: 0x040004D6 RID: 1238
		public static readonly Draw Gizmos = new Draw
		{
			gizmos = true
		};

		// Token: 0x040004D7 RID: 1239
		private bool gizmos;

		// Token: 0x040004D8 RID: 1240
		private Matrix4x4 matrix = Matrix4x4.identity;
	}
}
