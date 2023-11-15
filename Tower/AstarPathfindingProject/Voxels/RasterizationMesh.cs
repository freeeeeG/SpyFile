using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Voxels
{
	// Token: 0x020000A7 RID: 167
	public class RasterizationMesh
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x0002E9D9 File Offset: 0x0002CBD9
		public RasterizationMesh()
		{
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002E9E4 File Offset: 0x0002CBE4
		public RasterizationMesh(Vector3[] vertices, int[] triangles, Bounds bounds)
		{
			this.matrix = Matrix4x4.identity;
			this.vertices = vertices;
			this.numVertices = vertices.Length;
			this.triangles = triangles;
			this.numTriangles = triangles.Length;
			this.bounds = bounds;
			this.original = null;
			this.area = 0;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002EA38 File Offset: 0x0002CC38
		public RasterizationMesh(Vector3[] vertices, int[] triangles, Bounds bounds, Matrix4x4 matrix)
		{
			this.matrix = matrix;
			this.vertices = vertices;
			this.numVertices = vertices.Length;
			this.triangles = triangles;
			this.numTriangles = triangles.Length;
			this.bounds = bounds;
			this.original = null;
			this.area = 0;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002EA88 File Offset: 0x0002CC88
		public void RecalculateBounds()
		{
			Bounds bounds = new Bounds(this.matrix.MultiplyPoint3x4(this.vertices[0]), Vector3.zero);
			for (int i = 1; i < this.numVertices; i++)
			{
				bounds.Encapsulate(this.matrix.MultiplyPoint3x4(this.vertices[i]));
			}
			this.bounds = bounds;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002EAEE File Offset: 0x0002CCEE
		public void Pool()
		{
			if (this.pool)
			{
				ArrayPool<int>.Release(ref this.triangles, false);
				ArrayPool<Vector3>.Release(ref this.vertices, false);
			}
		}

		// Token: 0x04000463 RID: 1123
		public MeshFilter original;

		// Token: 0x04000464 RID: 1124
		public int area;

		// Token: 0x04000465 RID: 1125
		public Vector3[] vertices;

		// Token: 0x04000466 RID: 1126
		public int[] triangles;

		// Token: 0x04000467 RID: 1127
		public int numVertices;

		// Token: 0x04000468 RID: 1128
		public int numTriangles;

		// Token: 0x04000469 RID: 1129
		public Bounds bounds;

		// Token: 0x0400046A RID: 1130
		public Matrix4x4 matrix;

		// Token: 0x0400046B RID: 1131
		public bool pool;
	}
}
