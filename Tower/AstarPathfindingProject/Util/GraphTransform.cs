using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000CA RID: 202
	public class GraphTransform : IMovementPlane, ITransform
	{
		// Token: 0x0600087B RID: 2171 RVA: 0x000383EC File Offset: 0x000365EC
		public GraphTransform(Matrix4x4 matrix)
		{
			this.matrix = matrix;
			this.inverseMatrix = matrix.inverse;
			this.identity = matrix.isIdentity;
			this.onlyTranslational = GraphTransform.MatrixIsTranslational(matrix);
			this.up = matrix.MultiplyVector(Vector3.up).normalized;
			this.translation = matrix.MultiplyPoint3x4(Vector3.zero);
			this.i3translation = (Int3)this.translation;
			this.rotation = Quaternion.LookRotation(this.TransformVector(Vector3.forward), this.TransformVector(Vector3.up));
			this.inverseRotation = Quaternion.Inverse(this.rotation);
			this.isXY = (this.rotation == Quaternion.Euler(-90f, 0f, 0f));
			this.isXZ = (this.rotation == Quaternion.Euler(0f, 0f, 0f));
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000384E5 File Offset: 0x000366E5
		public Vector3 WorldUpAtGraphPosition(Vector3 point)
		{
			return this.up;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000384F0 File Offset: 0x000366F0
		private static bool MatrixIsTranslational(Matrix4x4 matrix)
		{
			return matrix.GetColumn(0) == new Vector4(1f, 0f, 0f, 0f) && matrix.GetColumn(1) == new Vector4(0f, 1f, 0f, 0f) && matrix.GetColumn(2) == new Vector4(0f, 0f, 1f, 0f) && matrix.m33 == 1f;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00038584 File Offset: 0x00036784
		public Vector3 Transform(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point + this.translation;
			}
			return this.matrix.MultiplyPoint3x4(point);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000385B8 File Offset: 0x000367B8
		public Vector3 TransformVector(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point;
			}
			return this.matrix.MultiplyVector(point);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000385E0 File Offset: 0x000367E0
		public void Transform(Int3[] arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					arr[i] += this.i3translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				arr[j] = (Int3)this.matrix.MultiplyPoint3x4((Vector3)arr[j]);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0003865C File Offset: 0x0003685C
		public void Transform(Vector3[] arr)
		{
			if (this.onlyTranslational)
			{
				for (int i = arr.Length - 1; i >= 0; i--)
				{
					arr[i] += this.translation;
				}
				return;
			}
			for (int j = arr.Length - 1; j >= 0; j--)
			{
				arr[j] = this.matrix.MultiplyPoint3x4(arr[j]);
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000386CC File Offset: 0x000368CC
		public Vector3 InverseTransform(Vector3 point)
		{
			if (this.onlyTranslational)
			{
				return point - this.translation;
			}
			return this.inverseMatrix.MultiplyPoint3x4(point);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00038700 File Offset: 0x00036900
		public Int3 InverseTransform(Int3 point)
		{
			if (this.onlyTranslational)
			{
				return point - this.i3translation;
			}
			return (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)point);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0003873C File Offset: 0x0003693C
		public void InverseTransform(Int3[] arr)
		{
			for (int i = arr.Length - 1; i >= 0; i--)
			{
				arr[i] = (Int3)this.inverseMatrix.MultiplyPoint3x4((Vector3)arr[i]);
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0003877F File Offset: 0x0003697F
		public static GraphTransform operator *(GraphTransform lhs, Matrix4x4 rhs)
		{
			return new GraphTransform(lhs.matrix * rhs);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00038792 File Offset: 0x00036992
		public static GraphTransform operator *(Matrix4x4 lhs, GraphTransform rhs)
		{
			return new GraphTransform(lhs * rhs.matrix);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000387A8 File Offset: 0x000369A8
		public Bounds Transform(Bounds bounds)
		{
			if (this.onlyTranslational)
			{
				return new Bounds(bounds.center + this.translation, bounds.size);
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(8);
			Vector3 extents = bounds.extents;
			array[0] = this.Transform(bounds.center + new Vector3(extents.x, extents.y, extents.z));
			array[1] = this.Transform(bounds.center + new Vector3(extents.x, extents.y, -extents.z));
			array[2] = this.Transform(bounds.center + new Vector3(extents.x, -extents.y, extents.z));
			array[3] = this.Transform(bounds.center + new Vector3(extents.x, -extents.y, -extents.z));
			array[4] = this.Transform(bounds.center + new Vector3(-extents.x, extents.y, extents.z));
			array[5] = this.Transform(bounds.center + new Vector3(-extents.x, extents.y, -extents.z));
			array[6] = this.Transform(bounds.center + new Vector3(-extents.x, -extents.y, extents.z));
			array[7] = this.Transform(bounds.center + new Vector3(-extents.x, -extents.y, -extents.z));
			Vector3 vector = array[0];
			Vector3 vector2 = array[0];
			for (int i = 1; i < 8; i++)
			{
				vector = Vector3.Min(vector, array[i]);
				vector2 = Vector3.Max(vector2, array[i]);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000389DC File Offset: 0x00036BDC
		public Bounds InverseTransform(Bounds bounds)
		{
			if (this.onlyTranslational)
			{
				return new Bounds(bounds.center - this.translation, bounds.size);
			}
			Vector3[] array = ArrayPool<Vector3>.Claim(8);
			Vector3 extents = bounds.extents;
			array[0] = this.InverseTransform(bounds.center + new Vector3(extents.x, extents.y, extents.z));
			array[1] = this.InverseTransform(bounds.center + new Vector3(extents.x, extents.y, -extents.z));
			array[2] = this.InverseTransform(bounds.center + new Vector3(extents.x, -extents.y, extents.z));
			array[3] = this.InverseTransform(bounds.center + new Vector3(extents.x, -extents.y, -extents.z));
			array[4] = this.InverseTransform(bounds.center + new Vector3(-extents.x, extents.y, extents.z));
			array[5] = this.InverseTransform(bounds.center + new Vector3(-extents.x, extents.y, -extents.z));
			array[6] = this.InverseTransform(bounds.center + new Vector3(-extents.x, -extents.y, extents.z));
			array[7] = this.InverseTransform(bounds.center + new Vector3(-extents.x, -extents.y, -extents.z));
			Vector3 vector = array[0];
			Vector3 vector2 = array[0];
			for (int i = 1; i < 8; i++)
			{
				vector = Vector3.Min(vector, array[i]);
				vector2 = Vector3.Max(vector2, array[i]);
			}
			ArrayPool<Vector3>.Release(ref array, false);
			return new Bounds((vector + vector2) * 0.5f, vector2 - vector);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00038C10 File Offset: 0x00036E10
		Vector2 IMovementPlane.ToPlane(Vector3 point)
		{
			if (this.isXY)
			{
				return new Vector2(point.x, point.y);
			}
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			return new Vector2(point.x, point.z);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00038C5E File Offset: 0x00036E5E
		Vector2 IMovementPlane.ToPlane(Vector3 point, out float elevation)
		{
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			elevation = point.y;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00038C8F File Offset: 0x00036E8F
		Vector3 IMovementPlane.ToWorld(Vector2 point, float elevation)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x040004E8 RID: 1256
		public readonly bool identity;

		// Token: 0x040004E9 RID: 1257
		public readonly bool onlyTranslational;

		// Token: 0x040004EA RID: 1258
		private readonly bool isXY;

		// Token: 0x040004EB RID: 1259
		private readonly bool isXZ;

		// Token: 0x040004EC RID: 1260
		private readonly Matrix4x4 matrix;

		// Token: 0x040004ED RID: 1261
		private readonly Matrix4x4 inverseMatrix;

		// Token: 0x040004EE RID: 1262
		private readonly Vector3 up;

		// Token: 0x040004EF RID: 1263
		private readonly Vector3 translation;

		// Token: 0x040004F0 RID: 1264
		private readonly Int3 i3translation;

		// Token: 0x040004F1 RID: 1265
		private readonly Quaternion rotation;

		// Token: 0x040004F2 RID: 1266
		private readonly Quaternion inverseRotation;

		// Token: 0x040004F3 RID: 1267
		public static readonly GraphTransform identityTransform = new GraphTransform(Matrix4x4.identity);
	}
}
