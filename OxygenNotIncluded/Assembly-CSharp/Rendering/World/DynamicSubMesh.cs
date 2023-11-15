using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000CB8 RID: 3256
	public class DynamicSubMesh
	{
		// Token: 0x06006831 RID: 26673 RVA: 0x00276A44 File Offset: 0x00274C44
		public DynamicSubMesh(string name, Bounds bounds, int idx_offset)
		{
			this.IdxOffset = idx_offset;
			this.Mesh = new Mesh();
			this.Mesh.name = name;
			this.Mesh.bounds = bounds;
			this.Mesh.MarkDynamic();
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x00276AB0 File Offset: 0x00274CB0
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.Vertices.Length)
			{
				this.Vertices = new Vector3[vertex_count];
				this.UVs = new Vector2[vertex_count];
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.Triangles.Length != triangle_count)
			{
				this.Triangles = new int[triangle_count];
				this.SetTriangles = true;
				return;
			}
			this.SetTriangles = false;
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x00276B16 File Offset: 0x00274D16
		public bool AreTrianglesFull()
		{
			return this.Triangles.Length == this.TriangleIdx;
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x00276B28 File Offset: 0x00274D28
		public bool AreVerticesFull()
		{
			return this.Vertices.Length == this.VertexIdx;
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x00276B3A File Offset: 0x00274D3A
		public bool AreUVsFull()
		{
			return this.UVs.Length == this.UVIdx;
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x00276B4C File Offset: 0x00274D4C
		public void Commit()
		{
			if (this.SetTriangles)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.SetUVs || this.SetTriangles)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.SetTriangles)
			{
				this.Mesh.triangles = this.Triangles;
			}
			this.VertexIdx = 0;
			this.UVIdx = 0;
			this.TriangleIdx = 0;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x00276BCC File Offset: 0x00274DCC
		public void AddTriangle(int triangle)
		{
			int[] triangles = this.Triangles;
			int triangleIdx = this.TriangleIdx;
			this.TriangleIdx = triangleIdx + 1;
			triangles[triangleIdx] = triangle + this.IdxOffset;
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x00276BFC File Offset: 0x00274DFC
		public void AddUV(Vector2 uv)
		{
			Vector2[] uvs = this.UVs;
			int uvidx = this.UVIdx;
			this.UVIdx = uvidx + 1;
			uvs[uvidx] = uv;
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x00276C28 File Offset: 0x00274E28
		public void AddVertex(Vector3 vertex)
		{
			Vector3[] vertices = this.Vertices;
			int vertexIdx = this.VertexIdx;
			this.VertexIdx = vertexIdx + 1;
			vertices[vertexIdx] = vertex;
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x00276C54 File Offset: 0x00274E54
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			Graphics.DrawMesh(this.Mesh, position, rotation, material, layer, null, 0, property_block, false, false);
		}

		// Token: 0x040047E7 RID: 18407
		public Vector3[] Vertices = new Vector3[0];

		// Token: 0x040047E8 RID: 18408
		public Vector2[] UVs = new Vector2[0];

		// Token: 0x040047E9 RID: 18409
		public int[] Triangles = new int[0];

		// Token: 0x040047EA RID: 18410
		public Mesh Mesh;

		// Token: 0x040047EB RID: 18411
		public bool SetUVs;

		// Token: 0x040047EC RID: 18412
		public bool SetTriangles;

		// Token: 0x040047ED RID: 18413
		private int VertexIdx;

		// Token: 0x040047EE RID: 18414
		private int UVIdx;

		// Token: 0x040047EF RID: 18415
		private int TriangleIdx;

		// Token: 0x040047F0 RID: 18416
		private int IdxOffset;
	}
}
