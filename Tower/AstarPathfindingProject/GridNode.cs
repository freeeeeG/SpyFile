using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000067 RID: 103
	public class GridNode : GridNodeBase
	{
		// Token: 0x06000542 RID: 1346 RVA: 0x0001E060 File Offset: 0x0001C260
		public GridNode(AstarPath astar) : base(astar)
		{
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001E069 File Offset: 0x0001C269
		public static GridGraph GetGridGraph(uint graphIndex)
		{
			return GridNode._gridGraphs[(int)graphIndex];
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001E074 File Offset: 0x0001C274
		public static void SetGridGraph(int graphIndex, GridGraph graph)
		{
			if (GridNode._gridGraphs.Length <= graphIndex)
			{
				GridGraph[] array = new GridGraph[graphIndex + 1];
				for (int i = 0; i < GridNode._gridGraphs.Length; i++)
				{
					array[i] = GridNode._gridGraphs[i];
				}
				GridNode._gridGraphs = array;
			}
			GridNode._gridGraphs[graphIndex] = graph;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001E0BE File Offset: 0x0001C2BE
		public static void ClearGridGraph(int graphIndex, GridGraph graph)
		{
			if (graphIndex < GridNode._gridGraphs.Length && GridNode._gridGraphs[graphIndex] == graph)
			{
				GridNode._gridGraphs[graphIndex] = null;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001E0DC File Offset: 0x0001C2DC
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001E0E4 File Offset: 0x0001C2E4
		internal ushort InternalGridFlags
		{
			get
			{
				return this.gridFlags;
			}
			set
			{
				this.gridFlags = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001E0ED File Offset: 0x0001C2ED
		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				return (this.InternalGridFlags & 255) == 255;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001E102 File Offset: 0x0001C302
		public override bool HasConnectionInDirection(int dir)
		{
			return (this.gridFlags >> dir & 1) != 0;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001E114 File Offset: 0x0001C314
		[Obsolete("Use HasConnectionInDirection")]
		public bool GetConnectionInternal(int dir)
		{
			return this.HasConnectionInDirection(dir);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001E11D File Offset: 0x0001C31D
		public void SetConnectionInternal(int dir, bool value)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & ~(1 << dir)) | (value ? 1 : 0) << (dir & 31));
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001E151 File Offset: 0x0001C351
		public void SetAllConnectionInternal(int connections)
		{
			this.gridFlags = (ushort)(((int)this.gridFlags & -256) | connections);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001E178 File Offset: 0x0001C378
		public void ResetConnectionsInternal()
		{
			this.gridFlags = (ushort)((int)this.gridFlags & -256);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001E19D File Offset: 0x0001C39D
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x0001E1AE File Offset: 0x0001C3AE
		public bool EdgeNode
		{
			get
			{
				return (this.gridFlags & 1024) > 0;
			}
			set
			{
				this.gridFlags = (ushort)(((int)this.gridFlags & -1025) | (value ? 1024 : 0));
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001E1D0 File Offset: 0x0001C3D0
		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			if (this.HasConnectionInDirection(direction))
			{
				GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction]];
			}
			return null;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001E20C File Offset: 0x0001C40C
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				for (int i = 0; i < 8; i++)
				{
					GridNode gridNode = this.GetNeighbourAlongDirection(i) as GridNode;
					if (gridNode != null)
					{
						gridNode.SetConnectionInternal((i < 4) ? ((i + 2) % 4) : ((i - 2) % 4 + 4), false);
					}
				}
			}
			this.ResetConnectionsInternal();
			base.ClearConnections(alsoReverse);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001E260 File Offset: 0x0001C460
		public override void GetConnections(Action<GraphNode> action)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			for (int i = 0; i < 8; i++)
			{
				if (this.HasConnectionInDirection(i))
				{
					GridNodeBase gridNodeBase = nodes[base.NodeInGridIndex + neighbourOffsets[i]];
					if (gridNodeBase != null)
					{
						action(gridNodeBase);
					}
				}
			}
			base.GetConnections(action);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			p = gridGraph.transform.InverseTransform(p);
			int num = base.NodeInGridIndex % gridGraph.width;
			int num2 = base.NodeInGridIndex / gridGraph.width;
			float y = gridGraph.transform.InverseTransform((Vector3)this.position).y;
			Vector3 point = new Vector3(Mathf.Clamp(p.x, (float)num, (float)num + 1f), y, Mathf.Clamp(p.z, (float)num2, (float)num2 + 1f));
			return gridGraph.transform.Transform(point);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001E358 File Offset: 0x0001C558
		public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			if (backwards)
			{
				return true;
			}
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			for (int i = 0; i < 4; i++)
			{
				if (this.HasConnectionInDirection(i) && other == nodes[base.NodeInGridIndex + neighbourOffsets[i]])
				{
					Vector3 a = (Vector3)(this.position + other.position) * 0.5f;
					Vector3 vector = Vector3.Cross(gridGraph.collision.up, (Vector3)(other.position - this.position));
					vector.Normalize();
					vector *= gridGraph.nodeSize * 0.5f;
					left.Add(a - vector);
					right.Add(a + vector);
					return true;
				}
			}
			for (int j = 4; j < 8; j++)
			{
				if (this.HasConnectionInDirection(j) && other == nodes[base.NodeInGridIndex + neighbourOffsets[j]])
				{
					bool flag = false;
					bool flag2 = false;
					if (this.HasConnectionInDirection(j - 4))
					{
						GridNodeBase gridNodeBase = nodes[base.NodeInGridIndex + neighbourOffsets[j - 4]];
						if (gridNodeBase.Walkable && gridNodeBase.HasConnectionInDirection((j - 4 + 1) % 4))
						{
							flag = true;
						}
					}
					if (this.HasConnectionInDirection((j - 4 + 1) % 4))
					{
						GridNodeBase gridNodeBase2 = nodes[base.NodeInGridIndex + neighbourOffsets[(j - 4 + 1) % 4]];
						if (gridNodeBase2.Walkable && gridNodeBase2.HasConnectionInDirection(j - 4))
						{
							flag2 = true;
						}
					}
					Vector3 a2 = (Vector3)(this.position + other.position) * 0.5f;
					Vector3 vector2 = Vector3.Cross(gridGraph.collision.up, (Vector3)(other.position - this.position));
					vector2.Normalize();
					vector2 *= gridGraph.nodeSize * 1.4142f;
					left.Add(a2 - (flag2 ? vector2 : Vector3.zero));
					right.Add(a2 + (flag ? vector2 : Vector3.zero));
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001E58C File Offset: 0x0001C78C
		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
			ushort pathID = handler.PathID;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				if (this.HasConnectionInDirection(i))
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i]];
					PathNode pathNode2 = handler.GetPathNode(gridNodeBase);
					if (pathNode2.parent == pathNode && pathNode2.pathID == pathID)
					{
						gridNodeBase.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
			base.UpdateRecursiveG(path, pathNode, handler);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001E628 File Offset: 0x0001C828
		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			ushort pathID = handler.PathID;
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 8; i++)
			{
				if (this.HasConnectionInDirection(i))
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i]];
					if (path.CanTraverse(gridNodeBase))
					{
						PathNode pathNode2 = handler.GetPathNode(gridNodeBase);
						uint num = neighbourCosts[i];
						if (pathNode2.pathID != pathID)
						{
							pathNode2.parent = pathNode;
							pathNode2.pathID = pathID;
							pathNode2.cost = num;
							pathNode2.H = path.CalculateHScore(gridNodeBase);
							pathNode2.UpdateG(path);
							handler.heap.Add(pathNode2);
						}
						else if (pathNode.G + num + path.GetTraversalCost(gridNodeBase) < pathNode2.G)
						{
							pathNode2.cost = num;
							pathNode2.parent = pathNode;
							gridNodeBase.UpdateRecursiveG(path, pathNode2, handler);
						}
					}
				}
			}
			base.Open(path, pathNode, handler);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001E737 File Offset: 0x0001C937
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001E75D File Offset: 0x0001C95D
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001E784 File Offset: 0x0001C984
		public override void AddConnection(GraphNode node, uint cost)
		{
			GridNode gridNode = node as GridNode;
			if (gridNode != null && gridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(gridNode);
			}
			base.AddConnection(node, cost);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001E7B8 File Offset: 0x0001C9B8
		public override void RemoveConnection(GraphNode node)
		{
			base.RemoveConnection(node);
			GridNode gridNode = node as GridNode;
			if (gridNode != null && gridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(gridNode);
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001E7EC File Offset: 0x0001C9EC
		protected void RemoveGridConnection(GridNode node)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
			for (int i = 0; i < 8; i++)
			{
				if (nodeInGridIndex + gridGraph.neighbourOffsets[i] == node.NodeInGridIndex && this.GetNeighbourAlongDirection(i) == node)
				{
					this.SetConnectionInternal(i, false);
					return;
				}
			}
		}

		// Token: 0x0400030A RID: 778
		private static GridGraph[] _gridGraphs = new GridGraph[0];

		// Token: 0x0400030B RID: 779
		private const int GridFlagsConnectionOffset = 0;

		// Token: 0x0400030C RID: 780
		private const int GridFlagsConnectionBit0 = 1;

		// Token: 0x0400030D RID: 781
		private const int GridFlagsConnectionMask = 255;

		// Token: 0x0400030E RID: 782
		private const int GridFlagsEdgeNodeOffset = 10;

		// Token: 0x0400030F RID: 783
		private const int GridFlagsEdgeNodeMask = 1024;
	}
}
