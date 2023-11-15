using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000062 RID: 98
	public class LevelGridNode : GridNodeBase
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x0001ABA3 File Offset: 0x00018DA3
		public LevelGridNode(AstarPath astar) : base(astar)
		{
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001ABAC File Offset: 0x00018DAC
		public static LayerGridGraph GetGridGraph(uint graphIndex)
		{
			return LevelGridNode._gridGraphs[(int)graphIndex];
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001ABB8 File Offset: 0x00018DB8
		public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
		{
			GridNode.SetGridGraph(graphIndex, graph);
			if (LevelGridNode._gridGraphs.Length <= graphIndex)
			{
				LayerGridGraph[] array = new LayerGridGraph[graphIndex + 1];
				for (int i = 0; i < LevelGridNode._gridGraphs.Length; i++)
				{
					array[i] = LevelGridNode._gridGraphs[i];
				}
				LevelGridNode._gridGraphs = array;
			}
			LevelGridNode._gridGraphs[graphIndex] = graph;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001AC09 File Offset: 0x00018E09
		public void ResetAllGridConnections()
		{
			this.gridConnections = ulong.MaxValue;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001AC23 File Offset: 0x00018E23
		public bool HasAnyGridConnections()
		{
			return this.gridConnections != ulong.MaxValue;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001AC32 File Offset: 0x00018E32
		public override bool HasConnectionsToAllEightNeighbours
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0001AC35 File Offset: 0x00018E35
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x0001AC40 File Offset: 0x00018E40
		public int LayerCoordinateInGrid
		{
			get
			{
				return this.nodeInGridIndex >> 24;
			}
			set
			{
				this.nodeInGridIndex = ((this.nodeInGridIndex & 16777215) | value << 24);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001AC59 File Offset: 0x00018E59
		public void SetPosition(Int3 position)
		{
			this.position = position;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001AC62 File Offset: 0x00018E62
		public override int GetGizmoHashCode()
		{
			return base.GetGizmoHashCode() ^ (int)(805306457UL * this.gridConnections);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001AC7C File Offset: 0x00018E7C
		public override GridNodeBase GetNeighbourAlongDirection(int direction)
		{
			int connectionValue = this.GetConnectionValue(direction);
			if (connectionValue != 255)
			{
				LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
				return gridGraph.nodes[base.NodeInGridIndex + gridGraph.neighbourOffsets[direction] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
			}
			return null;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001ACD0 File Offset: 0x00018ED0
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse)
			{
				LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
				int[] neighbourOffsets = gridGraph.neighbourOffsets;
				GridNodeBase[] nodes = gridGraph.nodes;
				for (int i = 0; i < 4; i++)
				{
					int connectionValue = this.GetConnectionValue(i);
					if (connectionValue != 255)
					{
						LevelGridNode levelGridNode = nodes[base.NodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue] as LevelGridNode;
						if (levelGridNode != null)
						{
							levelGridNode.SetConnectionValue((i + 2) % 4, 255);
						}
					}
				}
			}
			this.ResetAllGridConnections();
			base.ClearConnections(alsoReverse);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001AD60 File Offset: 0x00018F60
		public override void GetConnections(Action<GraphNode> action)
		{
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					GraphNode graphNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (graphNode != null)
					{
						action(graphNode);
					}
				}
			}
			base.GetConnections(action);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001ADDF File Offset: 0x00018FDF
		[Obsolete("Use HasConnectionInDirection instead")]
		public bool GetConnection(int i)
		{
			return (this.gridConnections >> i * 8 & 255UL) != 255UL;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001AE00 File Offset: 0x00019000
		public override bool HasConnectionInDirection(int direction)
		{
			return (this.gridConnections >> direction * 8 & 255UL) != 255UL;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001AE21 File Offset: 0x00019021
		public void SetConnectionValue(int dir, int value)
		{
			this.gridConnections = ((this.gridConnections & ~(255UL << dir * 8)) | (ulong)((ulong)((long)value) << dir * 8));
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001AE58 File Offset: 0x00019058
		public int GetConnectionValue(int dir)
		{
			return (int)(this.gridConnections >> dir * 8 & 255UL);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001AE70 File Offset: 0x00019070
		public override void AddConnection(GraphNode node, uint cost)
		{
			LevelGridNode levelGridNode = node as LevelGridNode;
			if (levelGridNode != null && levelGridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(levelGridNode);
			}
			base.AddConnection(node, cost);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001AEA4 File Offset: 0x000190A4
		public override void RemoveConnection(GraphNode node)
		{
			base.RemoveConnection(node);
			LevelGridNode levelGridNode = node as LevelGridNode;
			if (levelGridNode != null && levelGridNode.GraphIndex == base.GraphIndex)
			{
				this.RemoveGridConnection(levelGridNode);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001AED8 File Offset: 0x000190D8
		protected void RemoveGridConnection(LevelGridNode node)
		{
			int nodeInGridIndex = base.NodeInGridIndex;
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			for (int i = 0; i < 8; i++)
			{
				if (nodeInGridIndex + gridGraph.neighbourOffsets[i] == node.NodeInGridIndex && this.GetNeighbourAlongDirection(i) == node)
				{
					this.SetConnectionValue(i, 255);
					return;
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001AF30 File Offset: 0x00019130
		public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			if (backwards)
			{
				return true;
			}
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255 && other == nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue])
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
			return false;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001B038 File Offset: 0x00019238
		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			handler.heap.Add(pathNode);
			pathNode.UpdateG(path);
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					GridNodeBase gridNodeBase = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					PathNode pathNode2 = handler.GetPathNode(gridNodeBase);
					if (pathNode2 != null && pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
					{
						gridNodeBase.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
			base.UpdateRecursiveG(path, pathNode, handler);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001B0F4 File Offset: 0x000192F4
		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			uint[] neighbourCosts = gridGraph.neighbourCosts;
			GridNodeBase[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = this.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					GraphNode graphNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					if (path.CanTraverse(graphNode))
					{
						PathNode pathNode2 = handler.GetPathNode(graphNode);
						if (pathNode2.pathID != handler.PathID)
						{
							pathNode2.parent = pathNode;
							pathNode2.pathID = handler.PathID;
							pathNode2.cost = neighbourCosts[i];
							pathNode2.H = path.CalculateHScore(graphNode);
							pathNode2.UpdateG(path);
							handler.heap.Add(pathNode2);
						}
						else
						{
							uint num = neighbourCosts[i];
							if (pathNode.G + num + path.GetTraversalCost(graphNode) < pathNode2.G)
							{
								pathNode2.cost = num;
								pathNode2.parent = pathNode;
								graphNode.UpdateRecursiveG(path, pathNode2, handler);
							}
						}
					}
				}
			}
			base.Open(path, pathNode, handler);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001B224 File Offset: 0x00019424
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			p = gridGraph.transform.InverseTransform(p);
			int xcoordinateInGrid = base.XCoordinateInGrid;
			int zcoordinateInGrid = base.ZCoordinateInGrid;
			float y = gridGraph.transform.InverseTransform((Vector3)this.position).y;
			Vector3 point = new Vector3(Mathf.Clamp(p.x, (float)xcoordinateInGrid, (float)xcoordinateInGrid + 1f), y, Mathf.Clamp(p.z, (float)zcoordinateInGrid, (float)zcoordinateInGrid + 1f));
			return gridGraph.transform.Transform(point);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001B2B1 File Offset: 0x000194B1
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
			ctx.writer.Write(this.gridFlags);
			ctx.writer.Write(this.gridConnections);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001B2E8 File Offset: 0x000194E8
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
			this.gridFlags = ctx.reader.ReadUInt16();
			if (ctx.meta.version < AstarSerializer.V3_9_0)
			{
				this.gridConnections = ((ulong)ctx.reader.ReadUInt32() | 18446744069414584320UL);
				return;
			}
			this.gridConnections = ctx.reader.ReadUInt64();
		}

		// Token: 0x040002E5 RID: 741
		private static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];

		// Token: 0x040002E6 RID: 742
		public ulong gridConnections;

		// Token: 0x040002E7 RID: 743
		protected static LayerGridGraph[] gridGraphs;

		// Token: 0x040002E8 RID: 744
		public const int NoConnection = 255;

		// Token: 0x040002E9 RID: 745
		public const int ConnectionMask = 255;

		// Token: 0x040002EA RID: 746
		private const int ConnectionStride = 8;

		// Token: 0x040002EB RID: 747
		public const int MaxLayerCount = 255;
	}
}
