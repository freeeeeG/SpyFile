using System;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200004D RID: 77
	public abstract class MeshNode : GraphNode
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x000138B6 File Offset: 0x00011AB6
		protected MeshNode(AstarPath astar) : base(astar)
		{
		}

		// Token: 0x060003A6 RID: 934
		public abstract Int3 GetVertex(int i);

		// Token: 0x060003A7 RID: 935
		public abstract int GetVertexCount();

		// Token: 0x060003A8 RID: 936
		public abstract Vector3 ClosestPointOnNodeXZ(Vector3 p);

		// Token: 0x060003A9 RID: 937 RVA: 0x000138C0 File Offset: 0x00011AC0
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node != null)
					{
						this.connections[i].node.RemoveConnection(this);
					}
				}
			}
			ArrayPool<Connection>.Release(ref this.connections, true);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00013934 File Offset: 0x00011B34
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

		// Token: 0x060003AB RID: 939 RVA: 0x00013974 File Offset: 0x00011B74
		public override bool ContainsConnection(GraphNode node)
		{
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000139AC File Offset: 0x00011BAC
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

		// Token: 0x060003AD RID: 941 RVA: 0x00013A19 File Offset: 0x00011C19
		public override void AddConnection(GraphNode node, uint cost)
		{
			this.AddConnection(node, cost, byte.MaxValue);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00013A28 File Offset: 0x00011C28
		public void AddConnection(GraphNode node, uint cost, byte shapeEdge)
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
						this.connections[i].shapeEdge = ((shapeEdge != byte.MaxValue) ? shapeEdge : this.connections[i].shapeEdge);
						return;
					}
				}
			}
			int num = (this.connections != null) ? this.connections.Length : 0;
			Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num + 1);
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
			}
			array[num] = new Connection(node, cost, shapeEdge);
			if (this.connections != null)
			{
				ArrayPool<Connection>.Release(ref this.connections, true);
			}
			this.connections = array;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00013B24 File Offset: 0x00011D24
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
					Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num - 1);
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
					}
					if (this.connections != null)
					{
						ArrayPool<Connection>.Release(ref this.connections, true);
					}
					this.connections = array;
					AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
					return;
				}
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013BF0 File Offset: 0x00011DF0
		public virtual bool ContainsPoint(Int3 point)
		{
			return this.ContainsPoint((Vector3)point);
		}

		// Token: 0x060003B1 RID: 945
		public abstract bool ContainsPoint(Vector3 point);

		// Token: 0x060003B2 RID: 946
		public abstract bool ContainsPointInGraphSpace(Int3 point);

		// Token: 0x060003B3 RID: 947 RVA: 0x00013C00 File Offset: 0x00011E00
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

		// Token: 0x060003B4 RID: 948 RVA: 0x00013C50 File Offset: 0x00011E50
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
				ctx.writer.Write(this.connections[i].shapeEdge);
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00013CE8 File Offset: 0x00011EE8
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.connections = null;
				return;
			}
			this.connections = ArrayPool<Connection>.ClaimWithExactLength(num);
			for (int i = 0; i < num; i++)
			{
				this.connections[i] = new Connection(ctx.DeserializeNodeReference(), ctx.reader.ReadUInt32(), (ctx.meta.version < AstarSerializer.V4_1_0) ? byte.MaxValue : ctx.reader.ReadByte());
			}
		}

		// Token: 0x04000246 RID: 582
		public Connection[] connections;
	}
}
