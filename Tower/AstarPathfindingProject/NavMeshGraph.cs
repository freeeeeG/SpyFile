using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000066 RID: 102
	[JsonOptIn]
	[Preserve]
	public class NavMeshGraph : NavmeshBase, IUpdatableGraph
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001DDB8 File Offset: 0x0001BFB8
		protected override bool RecalculateNormals
		{
			get
			{
				return this.recalculateNormals;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001DDC0 File Offset: 0x0001BFC0
		public override float TileWorldSizeX
		{
			get
			{
				return this.forcedBoundsSize.x;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001DDCD File Offset: 0x0001BFCD
		public override float TileWorldSizeZ
		{
			get
			{
				return this.forcedBoundsSize.z;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001DDDA File Offset: 0x0001BFDA
		protected override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001DDE4 File Offset: 0x0001BFE4
		public override GraphTransform CalculateTransform()
		{
			return new GraphTransform(Matrix4x4.TRS(this.offset, Quaternion.Euler(this.rotation), Vector3.one) * Matrix4x4.TRS((this.sourceMesh != null) ? (this.sourceMesh.bounds.min * this.scale) : (this.cachedSourceMeshBoundsMin * this.scale), Quaternion.identity, Vector3.one));
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001DE64 File Offset: 0x0001C064
		GraphUpdateThreading IUpdatableGraph.CanUpdateAsync(GraphUpdateObject o)
		{
			return GraphUpdateThreading.UnityThread;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001DE67 File Offset: 0x0001C067
		void IUpdatableGraph.UpdateAreaInit(GraphUpdateObject o)
		{
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001DE69 File Offset: 0x0001C069
		void IUpdatableGraph.UpdateAreaPost(GraphUpdateObject o)
		{
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001DE6B File Offset: 0x0001C06B
		void IUpdatableGraph.UpdateArea(GraphUpdateObject o)
		{
			NavMeshGraph.UpdateArea(o, this);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001DE74 File Offset: 0x0001C074
		public static void UpdateArea(GraphUpdateObject o, INavmeshHolder graph)
		{
			Bounds bounds = graph.transform.InverseTransform(o.bounds);
			IntRect irect = new IntRect(Mathf.FloorToInt(bounds.min.x * 1000f), Mathf.FloorToInt(bounds.min.z * 1000f), Mathf.CeilToInt(bounds.max.x * 1000f), Mathf.CeilToInt(bounds.max.z * 1000f));
			Int3 a = new Int3(irect.xmin, 0, irect.ymin);
			Int3 b = new Int3(irect.xmin, 0, irect.ymax);
			Int3 c = new Int3(irect.xmax, 0, irect.ymin);
			Int3 d = new Int3(irect.xmax, 0, irect.ymax);
			int ymin = ((Int3)bounds.min).y;
			int ymax = ((Int3)bounds.max).y;
			graph.GetNodes(delegate(GraphNode _node)
			{
				TriangleMeshNode triangleMeshNode = _node as TriangleMeshNode;
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < 3; i++)
				{
					Int3 vertexInGraphSpace = triangleMeshNode.GetVertexInGraphSpace(i);
					if (irect.Contains(vertexInGraphSpace.x, vertexInGraphSpace.z))
					{
						flag = true;
						break;
					}
					if (vertexInGraphSpace.x < irect.xmin)
					{
						num++;
					}
					if (vertexInGraphSpace.x > irect.xmax)
					{
						num2++;
					}
					if (vertexInGraphSpace.z < irect.ymin)
					{
						num3++;
					}
					if (vertexInGraphSpace.z > irect.ymax)
					{
						num4++;
					}
				}
				if (!flag && (num == 3 || num2 == 3 || num3 == 3 || num4 == 3))
				{
					return;
				}
				for (int j = 0; j < 3; j++)
				{
					int i2 = (j > 1) ? 0 : (j + 1);
					Int3 vertexInGraphSpace2 = triangleMeshNode.GetVertexInGraphSpace(j);
					Int3 vertexInGraphSpace3 = triangleMeshNode.GetVertexInGraphSpace(i2);
					if (VectorMath.SegmentsIntersectXZ(a, b, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(a, c, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(c, d, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
					if (VectorMath.SegmentsIntersectXZ(d, b, vertexInGraphSpace2, vertexInGraphSpace3))
					{
						flag = true;
						break;
					}
				}
				if (flag || triangleMeshNode.ContainsPointInGraphSpace(a) || triangleMeshNode.ContainsPointInGraphSpace(b) || triangleMeshNode.ContainsPointInGraphSpace(c) || triangleMeshNode.ContainsPointInGraphSpace(d))
				{
					flag = true;
				}
				if (!flag)
				{
					return;
				}
				int num5 = 0;
				int num6 = 0;
				for (int k = 0; k < 3; k++)
				{
					Int3 vertexInGraphSpace4 = triangleMeshNode.GetVertexInGraphSpace(k);
					if (vertexInGraphSpace4.y < ymin)
					{
						num6++;
					}
					if (vertexInGraphSpace4.y > ymax)
					{
						num5++;
					}
				}
				if (num6 == 3 || num5 == 3)
				{
					return;
				}
				o.WillUpdateNode(triangleMeshNode);
				o.Apply(triangleMeshNode);
			});
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001DFD4 File Offset: 0x0001C1D4
		protected override IEnumerable<Progress> ScanInternal()
		{
			this.cachedSourceMeshBoundsMin = ((this.sourceMesh != null) ? this.sourceMesh.bounds.min : Vector3.zero);
			this.transform = this.CalculateTransform();
			this.tileZCount = (this.tileXCount = 1);
			this.tiles = new NavmeshTile[this.tileZCount * this.tileXCount];
			TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this), this);
			if (this.sourceMesh == null)
			{
				base.FillWithEmptyTiles();
				yield break;
			}
			yield return new Progress(0f, "Transforming Vertices");
			this.forcedBoundsSize = this.sourceMesh.bounds.size * this.scale;
			Vector3[] vertices = this.sourceMesh.vertices;
			List<Int3> intVertices = ListPool<Int3>.Claim(vertices.Length);
			Matrix4x4 matrix4x = Matrix4x4.TRS(-this.sourceMesh.bounds.min * this.scale, Quaternion.identity, Vector3.one * this.scale);
			for (int i = 0; i < vertices.Length; i++)
			{
				intVertices.Add((Int3)matrix4x.MultiplyPoint3x4(vertices[i]));
			}
			yield return new Progress(0.1f, "Compressing Vertices");
			Int3[] compressedVertices = null;
			int[] compressedTriangles = null;
			Polygon.CompressMesh(intVertices, new List<int>(this.sourceMesh.triangles), out compressedVertices, out compressedTriangles);
			ListPool<Int3>.Release(ref intVertices);
			yield return new Progress(0.2f, "Building Nodes");
			base.ReplaceTile(0, 0, compressedVertices, compressedTriangles);
			this.navmeshUpdateData.OnRecalculatedTiles(this.tiles);
			if (this.OnRecalculatedTiles != null)
			{
				this.OnRecalculatedTiles(this.tiles.Clone() as NavmeshTile[]);
			}
			yield break;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001DFE4 File Offset: 0x0001C1E4
		protected override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			this.sourceMesh = (ctx.DeserializeUnityObject() as Mesh);
			this.offset = ctx.DeserializeVector3();
			this.rotation = ctx.DeserializeVector3();
			this.scale = ctx.reader.ReadSingle();
			this.nearestSearchOnlyXZ = !ctx.reader.ReadBoolean();
		}

		// Token: 0x04000304 RID: 772
		[JsonMember]
		public Mesh sourceMesh;

		// Token: 0x04000305 RID: 773
		[JsonMember]
		public Vector3 offset;

		// Token: 0x04000306 RID: 774
		[JsonMember]
		public Vector3 rotation;

		// Token: 0x04000307 RID: 775
		[JsonMember]
		public float scale = 1f;

		// Token: 0x04000308 RID: 776
		[JsonMember]
		public bool recalculateNormals = true;

		// Token: 0x04000309 RID: 777
		[JsonMember]
		private Vector3 cachedSourceMeshBoundsMin;
	}
}
