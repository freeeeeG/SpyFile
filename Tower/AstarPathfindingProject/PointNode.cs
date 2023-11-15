using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000069 RID: 105
	public class PointNode : GraphNode
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x0001EF50 File Offset: 0x0001D150
		public void SetPosition(Int3 value)
		{
			this.position = value;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001EF59 File Offset: 0x0001D159
		public PointNode(AstarPath astar) : base(astar)
		{
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001EF62 File Offset: 0x0001D162
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			return (Vector3)this.position;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001EF70 File Offset: 0x0001D170
		public override void GetConnections(Action<GraphNode> action)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				action(this.connections[i].node);
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemoveConnection(this);
				}
			}
			this.connections = null;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001F00C File Offset: 0x0001D20C
		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			pathNode.UpdateG(path);
			handler.heap.Add(pathNode);
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

		// Token: 0x0600057E RID: 1406 RVA: 0x0001F07C File Offset: 0x0001D27C
		public override bool ContainsConnection(GraphNode node)
		{
			if (this.connections == null)
			{
				return false;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
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
			(base.Graph as PointGraph).RegisterConnectionLength((node.position - this.position).sqrMagnitudeLong);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001F1A8 File Offset: 0x0001D3A8
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

		// Token: 0x06000581 RID: 1409 RVA: 0x0001F260 File Offset: 0x0001D460
		public override void Open(Path path, PathNode pathNode, PathHandler handler)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				GraphNode node = this.connections[i].node;
				if (path.CanTraverse(node))
				{
					PathNode pathNode2 = handler.GetPathNode(node);
					if (pathNode2.pathID != handler.PathID)
					{
						pathNode2.parent = pathNode;
						pathNode2.pathID = handler.PathID;
						pathNode2.cost = this.connections[i].cost;
						pathNode2.H = path.CalculateHScore(node);
						pathNode2.UpdateG(path);
						handler.heap.Add(pathNode2);
					}
					else
					{
						uint cost = this.connections[i].cost;
						if (pathNode.G + cost + path.GetTraversalCost(node) < pathNode2.G)
						{
							pathNode2.cost = cost;
							pathNode2.parent = pathNode;
							node.UpdateRecursiveG(path, pathNode2, handler);
						}
					}
				}
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001F350 File Offset: 0x0001D550
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
			return num;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001F39D File Offset: 0x0001D59D
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001F3B2 File Offset: 0x0001D5B2
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
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

		// Token: 0x06000586 RID: 1414 RVA: 0x0001F444 File Offset: 0x0001D644
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
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

		// Token: 0x04000319 RID: 793
		public Connection[] connections;

		// Token: 0x0400031A RID: 794
		public GameObject gameObject;
	}
}
