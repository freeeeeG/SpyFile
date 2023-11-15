﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000D2 RID: 210
	public class RetainedGizmos
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x0003B700 File Offset: 0x00039900
		public GraphGizmoHelper GetSingleFrameGizmoHelper(AstarPath active)
		{
			RetainedGizmos.Hasher hasher = default(RetainedGizmos.Hasher);
			hasher.AddHash(Time.realtimeSinceStartup.GetHashCode());
			this.Draw(hasher);
			return this.GetGizmoHelper(active, hasher);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0003B739 File Offset: 0x00039939
		public GraphGizmoHelper GetGizmoHelper(AstarPath active, RetainedGizmos.Hasher hasher)
		{
			GraphGizmoHelper graphGizmoHelper = ObjectPool<GraphGizmoHelper>.Claim();
			graphGizmoHelper.Init(active, hasher, this);
			return graphGizmoHelper;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0003B749 File Offset: 0x00039949
		private void PoolMesh(Mesh mesh)
		{
			mesh.Clear();
			this.cachedMeshes.Push(mesh);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0003B75D File Offset: 0x0003995D
		private Mesh GetMesh()
		{
			if (this.cachedMeshes.Count > 0)
			{
				return this.cachedMeshes.Pop();
			}
			return new Mesh
			{
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0003B786 File Offset: 0x00039986
		public bool HasCachedMesh(RetainedGizmos.Hasher hasher)
		{
			return this.existingHashes.Contains(hasher.Hash);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0003B79A File Offset: 0x0003999A
		public bool Draw(RetainedGizmos.Hasher hasher)
		{
			this.usedHashes.Add(hasher.Hash);
			return this.HasCachedMesh(hasher);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0003B7B8 File Offset: 0x000399B8
		public void DrawExisting()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.usedHashes.Add(this.meshes[i].hash);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0003B7F8 File Offset: 0x000399F8
		public void FinalizeDraw()
		{
			this.RemoveUnusedMeshes(this.meshes);
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.current);
			if (this.surfaceMaterial == null || this.lineMaterial == null)
			{
				return;
			}
			for (int i = 0; i <= 1; i++)
			{
				Material material = (i == 0) ? this.surfaceMaterial : this.lineMaterial;
				for (int j = 0; j < material.passCount; j++)
				{
					material.SetPass(j);
					for (int k = 0; k < this.meshes.Count; k++)
					{
						if (this.meshes[k].lines == (material == this.lineMaterial) && GeometryUtility.TestPlanesAABB(planes, this.meshes[k].mesh.bounds))
						{
							Graphics.DrawMeshNow(this.meshes[k].mesh, Matrix4x4.identity);
						}
					}
				}
			}
			this.usedHashes.Clear();
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0003B8FC File Offset: 0x00039AFC
		public void ClearCache()
		{
			this.usedHashes.Clear();
			this.RemoveUnusedMeshes(this.meshes);
			while (this.cachedMeshes.Count > 0)
			{
				Object.DestroyImmediate(this.cachedMeshes.Pop());
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0003B938 File Offset: 0x00039B38
		private void RemoveUnusedMeshes(List<RetainedGizmos.MeshWithHash> meshList)
		{
			int i = 0;
			int num = 0;
			while (i < meshList.Count)
			{
				if (num == meshList.Count)
				{
					num--;
					meshList.RemoveAt(num);
				}
				else if (this.usedHashes.Contains(meshList[num].hash))
				{
					meshList[i] = meshList[num];
					i++;
					num++;
				}
				else
				{
					this.PoolMesh(meshList[num].mesh);
					this.existingHashes.Remove(meshList[num].hash);
					num++;
				}
			}
		}

		// Token: 0x0400051E RID: 1310
		private List<RetainedGizmos.MeshWithHash> meshes = new List<RetainedGizmos.MeshWithHash>();

		// Token: 0x0400051F RID: 1311
		private HashSet<ulong> usedHashes = new HashSet<ulong>();

		// Token: 0x04000520 RID: 1312
		private HashSet<ulong> existingHashes = new HashSet<ulong>();

		// Token: 0x04000521 RID: 1313
		private Stack<Mesh> cachedMeshes = new Stack<Mesh>();

		// Token: 0x04000522 RID: 1314
		public Material surfaceMaterial;

		// Token: 0x04000523 RID: 1315
		public Material lineMaterial;

		// Token: 0x0200016F RID: 367
		public struct Hasher
		{
			// Token: 0x06000B8D RID: 2957 RVA: 0x000491E8 File Offset: 0x000473E8
			public Hasher(AstarPath active)
			{
				this.hash = 0UL;
				this.debugData = active.debugPathData;
				this.includePathSearchInfo = (this.debugData != null && (active.debugMode == GraphDebugMode.F || active.debugMode == GraphDebugMode.G || active.debugMode == GraphDebugMode.H || active.showSearchTree));
				this.includeAreaInfo = (active.debugMode == GraphDebugMode.Areas);
				this.AddHash((int)active.debugMode);
				this.AddHash(active.debugFloor.GetHashCode());
				this.AddHash(active.debugRoof.GetHashCode());
				this.AddHash(AstarColor.ColorHash());
			}

			// Token: 0x06000B8E RID: 2958 RVA: 0x00049286 File Offset: 0x00047486
			public void AddHash(int hash)
			{
				this.hash = (1572869UL * this.hash ^ (ulong)((long)hash));
			}

			// Token: 0x06000B8F RID: 2959 RVA: 0x000492A0 File Offset: 0x000474A0
			public void HashNode(GraphNode node)
			{
				this.AddHash(node.GetGizmoHashCode());
				if (this.includeAreaInfo)
				{
					this.AddHash((int)node.Area);
				}
				if (this.includePathSearchInfo)
				{
					PathNode pathNode = this.debugData.GetPathNode(node.NodeIndex);
					this.AddHash((int)pathNode.pathID);
					this.AddHash((pathNode.pathID == this.debugData.PathID) ? 1 : 0);
					this.AddHash((int)pathNode.F);
				}
			}

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0004931C File Offset: 0x0004751C
			public ulong Hash
			{
				get
				{
					return this.hash;
				}
			}

			// Token: 0x04000832 RID: 2098
			private ulong hash;

			// Token: 0x04000833 RID: 2099
			private bool includePathSearchInfo;

			// Token: 0x04000834 RID: 2100
			private bool includeAreaInfo;

			// Token: 0x04000835 RID: 2101
			private PathHandler debugData;
		}

		// Token: 0x02000170 RID: 368
		public class Builder : IAstarPooledObject
		{
			// Token: 0x06000B91 RID: 2961 RVA: 0x00049324 File Offset: 0x00047524
			public void DrawMesh(RetainedGizmos gizmos, Vector3[] vertices, List<int> triangles, Color[] colors)
			{
				Mesh mesh = gizmos.GetMesh();
				mesh.vertices = vertices;
				mesh.SetTriangles(triangles, 0);
				mesh.colors = colors;
				mesh.UploadMeshData(false);
				this.meshes.Add(mesh);
			}

			// Token: 0x06000B92 RID: 2962 RVA: 0x00049364 File Offset: 0x00047564
			public void DrawWireCube(GraphTransform tr, Bounds bounds, Color color)
			{
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				this.DrawLine(tr.Transform(new Vector3(min.x, min.y, min.z)), tr.Transform(new Vector3(max.x, min.y, min.z)), color);
				this.DrawLine(tr.Transform(new Vector3(max.x, min.y, min.z)), tr.Transform(new Vector3(max.x, min.y, max.z)), color);
				this.DrawLine(tr.Transform(new Vector3(max.x, min.y, max.z)), tr.Transform(new Vector3(min.x, min.y, max.z)), color);
				this.DrawLine(tr.Transform(new Vector3(min.x, min.y, max.z)), tr.Transform(new Vector3(min.x, min.y, min.z)), color);
				this.DrawLine(tr.Transform(new Vector3(min.x, max.y, min.z)), tr.Transform(new Vector3(max.x, max.y, min.z)), color);
				this.DrawLine(tr.Transform(new Vector3(max.x, max.y, min.z)), tr.Transform(new Vector3(max.x, max.y, max.z)), color);
				this.DrawLine(tr.Transform(new Vector3(max.x, max.y, max.z)), tr.Transform(new Vector3(min.x, max.y, max.z)), color);
				this.DrawLine(tr.Transform(new Vector3(min.x, max.y, max.z)), tr.Transform(new Vector3(min.x, max.y, min.z)), color);
				this.DrawLine(tr.Transform(new Vector3(min.x, min.y, min.z)), tr.Transform(new Vector3(min.x, max.y, min.z)), color);
				this.DrawLine(tr.Transform(new Vector3(max.x, min.y, min.z)), tr.Transform(new Vector3(max.x, max.y, min.z)), color);
				this.DrawLine(tr.Transform(new Vector3(max.x, min.y, max.z)), tr.Transform(new Vector3(max.x, max.y, max.z)), color);
				this.DrawLine(tr.Transform(new Vector3(min.x, min.y, max.z)), tr.Transform(new Vector3(min.x, max.y, max.z)), color);
			}

			// Token: 0x06000B93 RID: 2963 RVA: 0x00049690 File Offset: 0x00047890
			public void DrawLine(Vector3 start, Vector3 end, Color color)
			{
				this.lines.Add(start);
				this.lines.Add(end);
				Color32 item = color;
				this.lineColors.Add(item);
				this.lineColors.Add(item);
			}

			// Token: 0x06000B94 RID: 2964 RVA: 0x000496D4 File Offset: 0x000478D4
			public void Submit(RetainedGizmos gizmos, RetainedGizmos.Hasher hasher)
			{
				this.SubmitLines(gizmos, hasher.Hash);
				this.SubmitMeshes(gizmos, hasher.Hash);
			}

			// Token: 0x06000B95 RID: 2965 RVA: 0x000496F4 File Offset: 0x000478F4
			private void SubmitMeshes(RetainedGizmos gizmos, ulong hash)
			{
				for (int i = 0; i < this.meshes.Count; i++)
				{
					gizmos.meshes.Add(new RetainedGizmos.MeshWithHash
					{
						hash = hash,
						mesh = this.meshes[i],
						lines = false
					});
					gizmos.existingHashes.Add(hash);
				}
			}

			// Token: 0x06000B96 RID: 2966 RVA: 0x0004975C File Offset: 0x0004795C
			private void SubmitLines(RetainedGizmos gizmos, ulong hash)
			{
				int num = (this.lines.Count + 32766 - 1) / 32766;
				for (int i = 0; i < num; i++)
				{
					int num2 = 32766 * i;
					int num3 = Mathf.Min(num2 + 32766, this.lines.Count);
					int num4 = num3 - num2;
					List<Vector3> list = ListPool<Vector3>.Claim(num4 * 2);
					List<Color32> list2 = ListPool<Color32>.Claim(num4 * 2);
					List<Vector3> list3 = ListPool<Vector3>.Claim(num4 * 2);
					List<Vector2> list4 = ListPool<Vector2>.Claim(num4 * 2);
					List<int> list5 = ListPool<int>.Claim(num4 * 3);
					for (int j = num2; j < num3; j++)
					{
						Vector3 item = this.lines[j];
						list.Add(item);
						list.Add(item);
						Color32 item2 = this.lineColors[j];
						list2.Add(item2);
						list2.Add(item2);
						list4.Add(new Vector2(0f, 0f));
						list4.Add(new Vector2(1f, 0f));
					}
					for (int k = num2; k < num3; k += 2)
					{
						Vector3 item3 = this.lines[k + 1] - this.lines[k];
						list3.Add(item3);
						list3.Add(item3);
						list3.Add(item3);
						list3.Add(item3);
					}
					int l = 0;
					int num5 = 0;
					while (l < num4 * 3)
					{
						list5.Add(num5);
						list5.Add(num5 + 1);
						list5.Add(num5 + 2);
						list5.Add(num5 + 1);
						list5.Add(num5 + 3);
						list5.Add(num5 + 2);
						l += 6;
						num5 += 4;
					}
					Mesh mesh = gizmos.GetMesh();
					mesh.SetVertices(list);
					mesh.SetTriangles(list5, 0);
					mesh.SetColors(list2);
					mesh.SetNormals(list3);
					mesh.SetUVs(0, list4);
					mesh.UploadMeshData(false);
					ListPool<Vector3>.Release(ref list);
					ListPool<Color32>.Release(ref list2);
					ListPool<Vector3>.Release(ref list3);
					ListPool<Vector2>.Release(ref list4);
					ListPool<int>.Release(ref list5);
					gizmos.meshes.Add(new RetainedGizmos.MeshWithHash
					{
						hash = hash,
						mesh = mesh,
						lines = true
					});
					gizmos.existingHashes.Add(hash);
				}
			}

			// Token: 0x06000B97 RID: 2967 RVA: 0x000499BF File Offset: 0x00047BBF
			void IAstarPooledObject.OnEnterPool()
			{
				this.lines.Clear();
				this.lineColors.Clear();
				this.meshes.Clear();
			}

			// Token: 0x04000836 RID: 2102
			private List<Vector3> lines = new List<Vector3>();

			// Token: 0x04000837 RID: 2103
			private List<Color32> lineColors = new List<Color32>();

			// Token: 0x04000838 RID: 2104
			private List<Mesh> meshes = new List<Mesh>();
		}

		// Token: 0x02000171 RID: 369
		private struct MeshWithHash
		{
			// Token: 0x04000839 RID: 2105
			public ulong hash;

			// Token: 0x0400083A RID: 2106
			public Mesh mesh;

			// Token: 0x0400083B RID: 2107
			public bool lines;
		}
	}
}
