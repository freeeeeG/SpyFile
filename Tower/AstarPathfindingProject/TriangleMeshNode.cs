using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200006B RID: 107
	public class TriangleMeshNode : MeshNode
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0001F4A8 File Offset: 0x0001D6A8
		public TriangleMeshNode(AstarPath astar) : base(astar)
		{
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001F4B1 File Offset: 0x0001D6B1
		public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
		{
			return TriangleMeshNode._navmeshHolders[(int)graphIndex];
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001F4BC File Offset: 0x0001D6BC
		public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
		{
			object obj = TriangleMeshNode.lockObject;
			lock (obj)
			{
				if (graphIndex >= TriangleMeshNode._navmeshHolders.Length)
				{
					INavmeshHolder[] array = new INavmeshHolder[graphIndex + 1];
					TriangleMeshNode._navmeshHolders.CopyTo(array, 0);
					TriangleMeshNode._navmeshHolders = array;
				}
				TriangleMeshNode._navmeshHolders[graphIndex] = graph;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001F524 File Offset: 0x0001D724
		public void UpdatePositionFromVertices()
		{
			Int3 lhs;
			Int3 rhs;
			Int3 rhs2;
			this.GetVertices(out lhs, out rhs, out rhs2);
			this.position = (lhs + rhs + rhs2) * 0.333333f;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001F55A File Offset: 0x0001D75A
		public int GetVertexIndex(int i)
		{
			if (i == 0)
			{
				return this.v0;
			}
			if (i != 1)
			{
				return this.v2;
			}
			return this.v1;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001F577 File Offset: 0x0001D777
		public int GetVertexArrayIndex(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexArrayIndex((i == 0) ? this.v0 : ((i == 1) ? this.v1 : this.v2));
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
		public void GetVertices(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertex(this.v0);
			v1 = navmeshHolder.GetVertex(this.v1);
			v2 = navmeshHolder.GetVertex(this.v2);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001F5F8 File Offset: 0x0001D7F8
		public void GetVerticesInGraphSpace(out Int3 v0, out Int3 v1, out Int3 v2)
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			v0 = navmeshHolder.GetVertexInGraphSpace(this.v0);
			v1 = navmeshHolder.GetVertexInGraphSpace(this.v1);
			v2 = navmeshHolder.GetVertexInGraphSpace(this.v2);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001F647 File Offset: 0x0001D847
		public override Int3 GetVertex(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertex(this.GetVertexIndex(i));
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001F660 File Offset: 0x0001D860
		public Int3 GetVertexInGraphSpace(int i)
		{
			return TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).GetVertexInGraphSpace(this.GetVertexIndex(i));
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001F679 File Offset: 0x0001D879
		public override int GetVertexCount()
		{
			return 3;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001F67C File Offset: 0x0001D87C
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			Int3 ob;
			Int3 ob2;
			Int3 ob3;
			this.GetVertices(out ob, out ob2, out ob3);
			return Polygon.ClosestPointOnTriangle((Vector3)ob, (Vector3)ob2, (Vector3)ob3, p);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001F6B0 File Offset: 0x0001D8B0
		internal Int3 ClosestPointOnNodeXZInGraphSpace(Vector3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVerticesInGraphSpace(out @int, out int2, out int3);
			p = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p);
			Int3 int4 = (Int3)Polygon.ClosestPointOnTriangleXZ((Vector3)@int, (Vector3)int2, (Vector3)int3, p);
			if (this.ContainsPointInGraphSpace(int4))
			{
				return int4;
			}
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (i != 0 || j != 0)
					{
						Int3 int5 = new Int3(int4.x + i, int4.y, int4.z + j);
						if (this.ContainsPointInGraphSpace(int5))
						{
							return int5;
						}
					}
				}
			}
			long sqrMagnitudeLong = (@int - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong2 = (int2 - int4).sqrMagnitudeLong;
			long sqrMagnitudeLong3 = (int3 - int4).sqrMagnitudeLong;
			if (sqrMagnitudeLong >= sqrMagnitudeLong2)
			{
				if (sqrMagnitudeLong2 >= sqrMagnitudeLong3)
				{
					return int3;
				}
				return int2;
			}
			else
			{
				if (sqrMagnitudeLong >= sqrMagnitudeLong3)
				{
					return int3;
				}
				return @int;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001F7B0 File Offset: 0x0001D9B0
		public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
		{
			Int3 ob;
			Int3 ob2;
			Int3 ob3;
			this.GetVertices(out ob, out ob2, out ob3);
			return Polygon.ClosestPointOnTriangleXZ((Vector3)ob, (Vector3)ob2, (Vector3)ob3, p);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001F7E1 File Offset: 0x0001D9E1
		public override bool ContainsPoint(Vector3 p)
		{
			return this.ContainsPointInGraphSpace((Int3)TriangleMeshNode.GetNavmeshHolder(base.GraphIndex).transform.InverseTransform(p));
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001F804 File Offset: 0x0001DA04
		public override bool ContainsPointInGraphSpace(Int3 p)
		{
			Int3 @int;
			Int3 int2;
			Int3 int3;
			this.GetVerticesInGraphSpace(out @int, out int2, out int3);
			return (long)(int2.x - @int.x) * (long)(p.z - @int.z) - (long)(p.x - @int.x) * (long)(int2.z - @int.z) <= 0L && (long)(int3.x - int2.x) * (long)(p.z - int2.z) - (long)(p.x - int2.x) * (long)(int3.z - int2.z) <= 0L && (long)(@int.x - int3.x) * (long)(p.z - int3.z) - (long)(p.x - int3.x) * (long)(@int.z - int3.z) <= 0L;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001F8E4 File Offset: 0x0001DAE4
		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				GraphNode node = this.connections[i].node;
				PathNode pathNode2 = handler.GetPathNode(node);
				if (pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
				{
					node.UpdateRecursiveG(path, pathNode2, handler);
				}
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001F95C File Offset: 0x0001DB5C
		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			if (this.connections == null)
			{
				return;
			}
			bool flag = pathNode.flag2;
			for (int i = this.connections.Length - 1; i >= 0; i--)
			{
				Connection connection = this.connections[i];
				GraphNode node = connection.node;
				if (path.CanTraverse(connection.node))
				{
					PathNode pathNode2 = handler.GetPathNode(connection.node);
					if (pathNode2 != pathNode.parent)
					{
						uint num = connection.cost;
						if (flag || pathNode2.flag2)
						{
							num = path.GetConnectionSpecialCost(this, connection.node, num);
						}
						if (pathNode2.pathID != handler.PathID)
						{
							pathNode2.node = connection.node;
							pathNode2.parent = pathNode;
							pathNode2.pathID = handler.PathID;
							pathNode2.cost = num;
							pathNode2.H = path.CalculateHScore(node);
							pathNode2.UpdateG(path);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + num + path.GetTraversalCost(node) < pathNode2.G)
						{
							pathNode2.cost = num;
							pathNode2.parent = pathNode;
							node.UpdateRecursiveG(path, pathNode2, handler);
						}
					}
				}
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001FA90 File Offset: 0x0001DC90
		public int SharedEdge(GraphNode other)
		{
			int result = -1;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == other)
					{
						result = (int)this.connections[i].shapeEdge;
					}
				}
			}
			return result;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
		public override bool GetPortal(GraphNode toNode, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			int num;
			int num2;
			return this.GetPortal(toNode, left, right, backwards, out num, out num2);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001FB00 File Offset: 0x0001DD00
		public bool GetPortal(GraphNode toNode, List<Vector3> left, List<Vector3> right, bool backwards, out int aIndex, out int bIndex)
		{
			aIndex = -1;
			bIndex = -1;
			if (backwards || toNode.GraphIndex != base.GraphIndex)
			{
				return false;
			}
			TriangleMeshNode triangleMeshNode = toNode as TriangleMeshNode;
			int num = this.SharedEdge(triangleMeshNode);
			if (num == 255)
			{
				return false;
			}
			if (num == -1)
			{
				if (this.connections != null)
				{
					for (int i = 0; i < this.connections.Length; i++)
					{
						if (this.connections[i].node.GraphIndex != base.GraphIndex)
						{
							NodeLink3Node nodeLink3Node = this.connections[i].node as NodeLink3Node;
							if (nodeLink3Node != null && nodeLink3Node.GetOther(this) == triangleMeshNode)
							{
								nodeLink3Node.GetPortal(triangleMeshNode, left, right, false);
								return true;
							}
						}
					}
				}
				return false;
			}
			aIndex = num;
			bIndex = (num + 1) % this.GetVertexCount();
			Int3 vertex = this.GetVertex(num);
			Int3 vertex2 = this.GetVertex((num + 1) % this.GetVertexCount());
			int num2 = this.GetVertexIndex(0) >> 12 & 524287;
			int num3 = triangleMeshNode.GetVertexIndex(0) >> 12 & 524287;
			if (num2 != num3)
			{
				INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
				int num4;
				int num5;
				navmeshHolder.GetTileCoordinates(num2, out num4, out num5);
				int num6;
				int num7;
				navmeshHolder.GetTileCoordinates(num3, out num6, out num7);
				int i2;
				if (Math.Abs(num4 - num6) == 1)
				{
					i2 = 2;
				}
				else
				{
					if (Math.Abs(num5 - num7) != 1)
					{
						return false;
					}
					i2 = 0;
				}
				int num8 = triangleMeshNode.SharedEdge(this);
				if (num8 == 255)
				{
					throw new Exception("Connection used edge in one direction, but not in the other direction. Has the wrong overload of AddConnection been used?");
				}
				if (num8 != -1)
				{
					int num9 = Math.Min(vertex[i2], vertex2[i2]);
					int num10 = Math.Max(vertex[i2], vertex2[i2]);
					Int3 vertex3 = triangleMeshNode.GetVertex(num8);
					Int3 vertex4 = triangleMeshNode.GetVertex((num8 + 1) % triangleMeshNode.GetVertexCount());
					num9 = Math.Max(num9, Math.Min(vertex3[i2], vertex4[i2]));
					num10 = Math.Min(num10, Math.Max(vertex3[i2], vertex4[i2]));
					if (vertex[i2] < vertex2[i2])
					{
						vertex[i2] = num9;
						vertex2[i2] = num10;
					}
					else
					{
						vertex[i2] = num10;
						vertex2[i2] = num9;
					}
				}
			}
			if (left != null)
			{
				left.Add((Vector3)vertex);
				right.Add((Vector3)vertex2);
			}
			return true;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001FD70 File Offset: 0x0001DF70
		public override float SurfaceArea()
		{
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return (float)Math.Abs(VectorMath.SignedTriangleAreaTimes2XZ(navmeshHolder.GetVertex(this.v0), navmeshHolder.GetVertex(this.v1), navmeshHolder.GetVertex(this.v2))) * 0.5f;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
		public override Vector3 RandomPointOnSurface()
		{
			float value;
			float value2;
			do
			{
				value = Random.value;
				value2 = Random.value;
			}
			while (value + value2 > 1f);
			INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(base.GraphIndex);
			return (Vector3)(navmeshHolder.GetVertex(this.v1) - navmeshHolder.GetVertex(this.v0)) * value + (Vector3)(navmeshHolder.GetVertex(this.v2) - navmeshHolder.GetVertex(this.v0)) * value2 + (Vector3)navmeshHolder.GetVertex(this.v0);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001FE5A File Offset: 0x0001E05A
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.writer.Write(this.v0);
			ctx.writer.Write(this.v1);
			ctx.writer.Write(this.v2);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001FE96 File Offset: 0x0001E096
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.v0 = ctx.reader.ReadInt32();
			this.v1 = ctx.reader.ReadInt32();
			this.v2 = ctx.reader.ReadInt32();
		}

		// Token: 0x0400031B RID: 795
		public int v0;

		// Token: 0x0400031C RID: 796
		public int v1;

		// Token: 0x0400031D RID: 797
		public int v2;

		// Token: 0x0400031E RID: 798
		protected static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];

		// Token: 0x0400031F RID: 799
		protected static readonly object lockObject = new object();
	}
}
