using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000068 RID: 104
	public abstract class GridNodeBase : GraphNode
	{
		// Token: 0x0600055D RID: 1373 RVA: 0x0001E84A File Offset: 0x0001CA4A
		protected GridNodeBase(AstarPath astar) : base(astar)
		{
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001E853 File Offset: 0x0001CA53
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001E861 File Offset: 0x0001CA61
		public int NodeInGridIndex
		{
			get
			{
				return this.nodeInGridIndex & 16777215;
			}
			set
			{
				this.nodeInGridIndex = ((this.nodeInGridIndex & -16777216) | value);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001E877 File Offset: 0x0001CA77
		public int XCoordinateInGrid
		{
			get
			{
				return this.NodeInGridIndex % GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001E890 File Offset: 0x0001CA90
		public int ZCoordinateInGrid
		{
			get
			{
				return this.NodeInGridIndex / GridNode.GetGridGraph(base.GraphIndex).width;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001E8A9 File Offset: 0x0001CAA9
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x0001E8BA File Offset: 0x0001CABA
		public bool WalkableErosion
		{
			get
			{
				return (this.gridFlags & 256) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -257) | (value ? 256 : 0));
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001E8DB File Offset: 0x0001CADB
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0001E8EC File Offset: 0x0001CAEC
		public bool TmpWalkable
		{
			get
			{
				return (this.gridFlags & 512) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -513) | (value ? 512 : 0));
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000566 RID: 1382
		public abstract bool HasConnectionsToAllEightNeighbours { get; }

		// Token: 0x06000567 RID: 1383 RVA: 0x0001E910 File Offset: 0x0001CB10
		public override float SurfaceArea()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return gridGraph.nodeSize * gridGraph.nodeSize;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001E938 File Offset: 0x0001CB38
		public override Vector3 RandomPointOnSurface()
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			Vector3 a = gridGraph.transform.InverseTransform((Vector3)this.position);
			return gridGraph.transform.Transform(a + new Vector3(Random.value - 0.5f, 0f, Random.value - 0.5f));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001E998 File Offset: 0x0001CB98
		public Vector2 NormalizePoint(Vector3 worldPoint)
		{
			Vector3 vector = GridNode.GetGridGraph(base.GraphIndex).transform.InverseTransform(worldPoint);
			return new Vector2(vector.x - (float)this.XCoordinateInGrid, vector.z - (float)this.ZCoordinateInGrid);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001E9E0 File Offset: 0x0001CBE0
		public Vector3 UnNormalizePoint(Vector2 normalizedPointOnSurface)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			return (Vector3)this.position + gridGraph.transform.TransformVector(new Vector3(normalizedPointOnSurface.x - 0.5f, 0f, normalizedPointOnSurface.y - 0.5f));
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001EA38 File Offset: 0x0001CC38
		public override int GetGizmoHashCode()
		{
			int num = base.GetGizmoHashCode();
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					num ^= 17 * this.connections[i].GetHashCode();
				}
			}
			return num ^ (int)(109 * this.gridFlags);
		}

		// Token: 0x0600056C RID: 1388
		public abstract GridNodeBase GetNeighbourAlongDirection(int direction);

		// Token: 0x0600056D RID: 1389 RVA: 0x0001EA91 File Offset: 0x0001CC91
		public virtual bool HasConnectionInDirection(int direction)
		{
			return this.GetNeighbourAlongDirection(direction) != null;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001EAA0 File Offset: 0x0001CCA0
		public override bool ContainsConnection(GraphNode node)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == node)
					{
						return true;
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				if (node == this.GetNeighbourAlongDirection(j))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001EAF8 File Offset: 0x0001CCF8
		public void ClearCustomConnections(bool alsoReverse)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemoveConnection(this);
				}
			}
			this.connections = null;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001EB4E File Offset: 0x0001CD4E
		public override void ClearConnections(bool alsoReverse)
		{
			this.ClearCustomConnections(alsoReverse);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001EB58 File Offset: 0x0001CD58
		public override void GetConnections(Action<GraphNode> action)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					action(this.connections[i].node);
				}
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001EB98 File Offset: 0x0001CD98
		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			ushort pathID = handler.PathID;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					GraphNode node = this.connections[i].node;
					PathNode pathNode2 = handler.GetPathNode(node);
					if (pathNode2.parent == pathNode && pathNode2.pathID == pathID)
					{
						node.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001EBFC File Offset: 0x0001CDFC
		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			ushort pathID = handler.PathID;
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					GraphNode node = this.connections[i].node;
					if (path.CanTraverse(node))
					{
						PathNode pathNode2 = handler.GetPathNode(node);
						uint cost = this.connections[i].cost;
						if (pathNode2.pathID != pathID)
						{
							pathNode2.parent = pathNode;
							pathNode2.pathID = pathID;
							pathNode2.cost = cost;
							pathNode2.H = path.CalculateHScore(node);
							pathNode2.UpdateG(path);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + cost + path.GetTraversalCost(node) < pathNode2.G)
						{
							pathNode2.cost = cost;
							pathNode2.parent = pathNode;
							node.UpdateRecursiveG(path, pathNode2, handler);
						}
					}
				}
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001ECE0 File Offset: 0x0001CEE0
		public override void AddConnection(GraphNode node, uint cost)
		{
			if (node == null)
			{
				throw new ArgumentNullException();
			}
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == node)
					{
						this.connections[i].cost = cost;
						return;
					}
				}
			}
			int num = (this.connections != null) ? this.connections.Length : 0;
			Connection[] array = new Connection[num + 1];
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
			}
			array[num] = new Connection(node, cost, byte.MaxValue);
			this.connections = array;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
		public override void RemoveConnection(GraphNode node)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node)
				{
					int num = this.connections.Length;
					Connection[] array = new Connection[num - 1];
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
					}
					this.connections = array;
					AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
					return;
				}
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001EE58 File Offset: 0x0001D058
		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			if (this.connections == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(this.connections.Length);
			for (int i = 0; i < this.connections.Length; i++)
			{
				ctx.SerializeNodeReference(this.connections[i].node);
				ctx.writer.Write(this.connections[i].cost);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001EED4 File Offset: 0x0001D0D4
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			if (ctx.meta.version < AstarSerializer.V3_8_3)
			{
				return;
			}
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.connections = null;
				return;
			}
			this.connections = new Connection[num];
			for (int i = 0; i < num; i++)
			{
				this.connections[i] = new Connection(ctx.DeserializeNodeReference(), ctx.reader.ReadUInt32(), byte.MaxValue);
			}
		}

		// Token: 0x04000310 RID: 784
		private const int GridFlagsWalkableErosionOffset = 8;

		// Token: 0x04000311 RID: 785
		private const int GridFlagsWalkableErosionMask = 256;

		// Token: 0x04000312 RID: 786
		private const int GridFlagsWalkableTmpOffset = 9;

		// Token: 0x04000313 RID: 787
		private const int GridFlagsWalkableTmpMask = 512;

		// Token: 0x04000314 RID: 788
		protected const int NodeInGridIndexLayerOffset = 24;

		// Token: 0x04000315 RID: 789
		protected const int NodeInGridIndexMask = 16777215;

		// Token: 0x04000316 RID: 790
		protected int nodeInGridIndex;

		// Token: 0x04000317 RID: 791
		protected ushort gridFlags;

		// Token: 0x04000318 RID: 792
		public Connection[] connections;
	}
}
