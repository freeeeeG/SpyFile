using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000CB7 RID: 3255
	public class DynamicMesh
	{
		// Token: 0x06006829 RID: 26665 RVA: 0x002767B0 File Offset: 0x002749B0
		public DynamicMesh(string name, Bounds bounds)
		{
			this.Name = name;
			this.Bounds = bounds;
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x002767D4 File Offset: 0x002749D4
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.VertexCount)
			{
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.TriangleCount != triangle_count)
			{
				this.SetTriangles = true;
			}
			else
			{
				this.SetTriangles = false;
			}
			int num = (int)Mathf.Ceil((float)triangle_count / (float)DynamicMesh.TrianglesPerMesh);
			if (num != this.Meshes.Length)
			{
				this.Meshes = new DynamicSubMesh[num];
				for (int i = 0; i < this.Meshes.Length; i++)
				{
					int idx_offset = -i * DynamicMesh.VerticesPerMesh;
					this.Meshes[i] = new DynamicSubMesh(this.Name, this.Bounds, idx_offset);
				}
				this.SetUVs = true;
				this.SetTriangles = true;
			}
			for (int j = 0; j < this.Meshes.Length; j++)
			{
				if (j == this.Meshes.Length - 1)
				{
					this.Meshes[j].Reserve(vertex_count % DynamicMesh.VerticesPerMesh, triangle_count % DynamicMesh.TrianglesPerMesh);
				}
				else
				{
					this.Meshes[j].Reserve(DynamicMesh.VerticesPerMesh, DynamicMesh.TrianglesPerMesh);
				}
			}
			this.VertexCount = vertex_count;
			this.TriangleCount = triangle_count;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x002768E0 File Offset: 0x00274AE0
		public void Commit()
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Commit();
			}
			this.TriangleMeshIdx = 0;
			this.UVMeshIdx = 0;
			this.VertexMeshIdx = 0;
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x00276920 File Offset: 0x00274B20
		public void AddTriangle(int triangle)
		{
			if (this.Meshes[this.TriangleMeshIdx].AreTrianglesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.TriangleMeshIdx + 1;
				this.TriangleMeshIdx = num;
				object obj = meshes[num];
			}
			this.Meshes[this.TriangleMeshIdx].AddTriangle(triangle);
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x00276970 File Offset: 0x00274B70
		public void AddUV(Vector2 uv)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.UVMeshIdx];
			if (dynamicSubMesh.AreUVsFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.UVMeshIdx + 1;
				this.UVMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddUV(uv);
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x002769B4 File Offset: 0x00274BB4
		public void AddVertex(Vector3 vertex)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.VertexMeshIdx];
			if (dynamicSubMesh.AreVerticesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.VertexMeshIdx + 1;
				this.VertexMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddVertex(vertex);
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x002769F8 File Offset: 0x00274BF8
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Render(position, rotation, material, layer, property_block);
			}
		}

		// Token: 0x040047D8 RID: 18392
		private static int TrianglesPerMesh = 65004;

		// Token: 0x040047D9 RID: 18393
		private static int VerticesPerMesh = 4 * DynamicMesh.TrianglesPerMesh / 6;

		// Token: 0x040047DA RID: 18394
		public bool SetUVs;

		// Token: 0x040047DB RID: 18395
		public bool SetTriangles;

		// Token: 0x040047DC RID: 18396
		public string Name;

		// Token: 0x040047DD RID: 18397
		public Bounds Bounds;

		// Token: 0x040047DE RID: 18398
		public DynamicSubMesh[] Meshes = new DynamicSubMesh[0];

		// Token: 0x040047DF RID: 18399
		private int VertexCount;

		// Token: 0x040047E0 RID: 18400
		private int TriangleCount;

		// Token: 0x040047E1 RID: 18401
		private int VertexIdx;

		// Token: 0x040047E2 RID: 18402
		private int UVIdx;

		// Token: 0x040047E3 RID: 18403
		private int TriangleIdx;

		// Token: 0x040047E4 RID: 18404
		private int TriangleMeshIdx;

		// Token: 0x040047E5 RID: 18405
		private int VertexMeshIdx;

		// Token: 0x040047E6 RID: 18406
		private int UVMeshIdx;
	}
}
