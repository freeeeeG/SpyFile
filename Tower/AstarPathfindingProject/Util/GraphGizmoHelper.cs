using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x020000D1 RID: 209
	public class GraphGizmoHelper : IAstarPooledObject, IDisposable
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0003B2E3 File Offset: 0x000394E3
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0003B2EB File Offset: 0x000394EB
		public RetainedGizmos.Hasher hasher { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0003B2F4 File Offset: 0x000394F4
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0003B2FC File Offset: 0x000394FC
		public RetainedGizmos.Builder builder { get; private set; }

		// Token: 0x060008C9 RID: 2249 RVA: 0x0003B305 File Offset: 0x00039505
		public GraphGizmoHelper()
		{
			this.drawConnection = new Action<GraphNode>(this.DrawConnection);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0003B320 File Offset: 0x00039520
		public void Init(AstarPath active, RetainedGizmos.Hasher hasher, RetainedGizmos gizmos)
		{
			if (active != null)
			{
				this.debugData = active.debugPathData;
				this.debugPathID = active.debugPathID;
				this.debugMode = active.debugMode;
				this.debugFloor = active.debugFloor;
				this.debugRoof = active.debugRoof;
				this.showSearchTree = (active.showSearchTree && this.debugData != null);
			}
			this.gizmos = gizmos;
			this.hasher = hasher;
			this.builder = ObjectPool<RetainedGizmos.Builder>.Claim();
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0003B3A8 File Offset: 0x000395A8
		public void OnEnterPool()
		{
			RetainedGizmos.Builder builder = this.builder;
			ObjectPool<RetainedGizmos.Builder>.Release(ref builder);
			this.builder = null;
			this.debugData = null;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0003B3D4 File Offset: 0x000395D4
		public void DrawConnections(GraphNode node)
		{
			if (this.showSearchTree)
			{
				if (GraphGizmoHelper.InSearchTree(node, this.debugData, this.debugPathID) && this.debugData.GetPathNode(node).parent != null)
				{
					this.builder.DrawLine((Vector3)node.position, (Vector3)this.debugData.GetPathNode(node).parent.node.position, this.NodeColor(node));
					return;
				}
			}
			else
			{
				this.drawConnectionColor = this.NodeColor(node);
				this.drawConnectionStart = (Vector3)node.position;
				node.GetConnections(this.drawConnection);
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0003B478 File Offset: 0x00039678
		private void DrawConnection(GraphNode other)
		{
			this.builder.DrawLine(this.drawConnectionStart, Vector3.Lerp((Vector3)other.position, this.drawConnectionStart, 0.5f), this.drawConnectionColor);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0003B4AC File Offset: 0x000396AC
		public Color NodeColor(GraphNode node)
		{
			if (this.showSearchTree && !GraphGizmoHelper.InSearchTree(node, this.debugData, this.debugPathID))
			{
				return Color.clear;
			}
			Color result;
			if (node.Walkable)
			{
				switch (this.debugMode)
				{
				case GraphDebugMode.SolidColor:
					return AstarColor.SolidColor;
				case GraphDebugMode.Penalty:
					return Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (node.Penalty - this.debugFloor) / (this.debugRoof - this.debugFloor));
				case GraphDebugMode.Areas:
					return AstarColor.GetAreaColor(node.Area);
				case GraphDebugMode.Tags:
					return AstarColor.GetTagColor(node.Tag);
				case GraphDebugMode.HierarchicalNode:
					return AstarColor.GetTagColor((uint)node.HierarchicalNodeIndex);
				}
				if (this.debugData == null)
				{
					result = AstarColor.SolidColor;
				}
				else
				{
					PathNode pathNode = this.debugData.GetPathNode(node);
					float num;
					if (this.debugMode == GraphDebugMode.G)
					{
						num = pathNode.G;
					}
					else if (this.debugMode == GraphDebugMode.H)
					{
						num = pathNode.H;
					}
					else
					{
						num = pathNode.F;
					}
					result = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (num - this.debugFloor) / (this.debugRoof - this.debugFloor));
				}
			}
			else
			{
				result = AstarColor.UnwalkableNode;
			}
			return result;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0003B5FF File Offset: 0x000397FF
		public static bool InSearchTree(GraphNode node, PathHandler handler, ushort pathID)
		{
			return handler.GetPathNode(node).pathID == pathID;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0003B610 File Offset: 0x00039810
		public void DrawWireTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			this.builder.DrawLine(a, b, color);
			this.builder.DrawLine(b, c, color);
			this.builder.DrawLine(c, a, color);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0003B640 File Offset: 0x00039840
		public void DrawTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			List<int> list = ListPool<int>.Claim(numTriangles);
			for (int i = 0; i < numTriangles * 3; i++)
			{
				list.Add(i);
			}
			this.builder.DrawMesh(this.gizmos, vertices, list, colors);
			ListPool<int>.Release(ref list);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0003B684 File Offset: 0x00039884
		public void DrawWireTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			for (int i = 0; i < numTriangles; i++)
			{
				this.DrawWireTriangle(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2], colors[i * 3]);
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0003B6CB File Offset: 0x000398CB
		public void Submit()
		{
			this.builder.Submit(this.gizmos, this.hasher);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0003B6E4 File Offset: 0x000398E4
		void IDisposable.Dispose()
		{
			GraphGizmoHelper graphGizmoHelper = this;
			this.Submit();
			ObjectPool<GraphGizmoHelper>.Release(ref graphGizmoHelper);
		}

		// Token: 0x04000513 RID: 1299
		private RetainedGizmos gizmos;

		// Token: 0x04000514 RID: 1300
		private PathHandler debugData;

		// Token: 0x04000515 RID: 1301
		private ushort debugPathID;

		// Token: 0x04000516 RID: 1302
		private GraphDebugMode debugMode;

		// Token: 0x04000517 RID: 1303
		private bool showSearchTree;

		// Token: 0x04000518 RID: 1304
		private float debugFloor;

		// Token: 0x04000519 RID: 1305
		private float debugRoof;

		// Token: 0x0400051B RID: 1307
		private Vector3 drawConnectionStart;

		// Token: 0x0400051C RID: 1308
		private Color drawConnectionColor;

		// Token: 0x0400051D RID: 1309
		private readonly Action<GraphNode> drawConnection;
	}
}
