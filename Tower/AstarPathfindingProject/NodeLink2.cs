using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000041 RID: 65
	[AddComponentMenu("Pathfinding/Link2")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_node_link2.php")]
	public class NodeLink2 : GraphModifier
	{
		// Token: 0x0600030A RID: 778 RVA: 0x00010B68 File Offset: 0x0000ED68
		public static NodeLink2 GetNodeLink(GraphNode node)
		{
			NodeLink2 result;
			NodeLink2.reference.TryGetValue(node, out result);
			return result;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00010B84 File Offset: 0x0000ED84
		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00010B8C File Offset: 0x0000ED8C
		public Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00010B94 File Offset: 0x0000ED94
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00010B9C File Offset: 0x0000ED9C
		public PointNode startNode { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00010BA5 File Offset: 0x0000EDA5
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00010BAD File Offset: 0x0000EDAD
		public PointNode endNode { get; private set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00010BB6 File Offset: 0x0000EDB6
		[Obsolete("Use startNode instead (lowercase s)")]
		public GraphNode StartNode
		{
			get
			{
				return this.startNode;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00010BBE File Offset: 0x0000EDBE
		[Obsolete("Use endNode instead (lowercase e)")]
		public GraphNode EndNode
		{
			get
			{
				return this.endNode;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00010BC6 File Offset: 0x0000EDC6
		public override void OnPostScan()
		{
			this.InternalOnPostScan();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00010BD0 File Offset: 0x0000EDD0
		public void InternalOnPostScan()
		{
			if (this.EndTransform == null || this.StartTransform == null)
			{
				return;
			}
			if (AstarPath.active.data.pointGraph == null)
			{
				(AstarPath.active.data.AddGraph(typeof(PointGraph)) as PointGraph).name = "PointGraph (used for node links)";
			}
			if (this.startNode != null && this.startNode.Destroyed)
			{
				NodeLink2.reference.Remove(this.startNode);
				this.startNode = null;
			}
			if (this.endNode != null && this.endNode.Destroyed)
			{
				NodeLink2.reference.Remove(this.endNode);
				this.endNode = null;
			}
			if (this.startNode == null)
			{
				this.startNode = AstarPath.active.data.pointGraph.AddNode((Int3)this.StartTransform.position);
			}
			if (this.endNode == null)
			{
				this.endNode = AstarPath.active.data.pointGraph.AddNode((Int3)this.EndTransform.position);
			}
			this.connectedNode1 = null;
			this.connectedNode2 = null;
			if (this.startNode == null || this.endNode == null)
			{
				this.startNode = null;
				this.endNode = null;
				return;
			}
			this.postScanCalled = true;
			NodeLink2.reference[this.startNode] = this;
			NodeLink2.reference[this.endNode] = this;
			this.Apply(true);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00010D50 File Offset: 0x0000EF50
		public override void OnGraphsPostUpdate()
		{
			if (AstarPath.active.isScanning)
			{
				return;
			}
			if (this.connectedNode1 != null && this.connectedNode1.Destroyed)
			{
				this.connectedNode1 = null;
			}
			if (this.connectedNode2 != null && this.connectedNode2.Destroyed)
			{
				this.connectedNode2 = null;
			}
			if (!this.postScanCalled)
			{
				this.OnPostScan();
				return;
			}
			this.Apply(false);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00010DB8 File Offset: 0x0000EFB8
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && AstarPath.active != null && AstarPath.active.data != null && AstarPath.active.data.pointGraph != null && !AstarPath.active.isScanning)
			{
				AstarPath.active.AddWorkItem(new Action(this.OnGraphsPostUpdate));
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00010E20 File Offset: 0x0000F020
		protected override void OnDisable()
		{
			base.OnDisable();
			this.postScanCalled = false;
			if (this.startNode != null)
			{
				NodeLink2.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				NodeLink2.reference.Remove(this.endNode);
			}
			if (this.startNode != null && this.endNode != null)
			{
				this.startNode.RemoveConnection(this.endNode);
				this.endNode.RemoveConnection(this.startNode);
				if (this.connectedNode1 != null && this.connectedNode2 != null)
				{
					this.startNode.RemoveConnection(this.connectedNode1);
					this.connectedNode1.RemoveConnection(this.startNode);
					this.endNode.RemoveConnection(this.connectedNode2);
					this.connectedNode2.RemoveConnection(this.endNode);
				}
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00010EF2 File Offset: 0x0000F0F2
		private void RemoveConnections(GraphNode node)
		{
			node.ClearConnections(true);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00010EFB File Offset: 0x0000F0FB
		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			if (Application.isPlaying)
			{
				this.Apply(true);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00010F0C File Offset: 0x0000F10C
		public void Apply(bool forceNewCheck)
		{
			NNConstraint none = NNConstraint.None;
			int graphIndex = (int)this.startNode.GraphIndex;
			none.graphMask = ~(1 << graphIndex);
			this.startNode.SetPosition((Int3)this.StartTransform.position);
			this.endNode.SetPosition((Int3)this.EndTransform.position);
			this.RemoveConnections(this.startNode);
			this.RemoveConnections(this.endNode);
			uint cost = (uint)Mathf.RoundToInt((float)((Int3)(this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
			this.startNode.AddConnection(this.endNode, cost);
			this.endNode.AddConnection(this.startNode, cost);
			if (this.connectedNode1 == null || forceNewCheck)
			{
				NNInfo nearest = AstarPath.active.GetNearest(this.StartTransform.position, none);
				this.connectedNode1 = nearest.node;
				this.clamped1 = nearest.position;
			}
			if (this.connectedNode2 == null || forceNewCheck)
			{
				NNInfo nearest2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
				this.connectedNode2 = nearest2.node;
				this.clamped2 = nearest2.position;
			}
			if (this.connectedNode2 == null || this.connectedNode1 == null)
			{
				return;
			}
			this.connectedNode1.AddConnection(this.startNode, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
			if (!this.oneWay)
			{
				this.connectedNode2.AddConnection(this.endNode, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
			}
			if (!this.oneWay)
			{
				this.startNode.AddConnection(this.connectedNode1, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
			}
			this.endNode.AddConnection(this.connectedNode2, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00011188 File Offset: 0x0000F388
		public virtual void OnDrawGizmosSelected()
		{
			this.OnDrawGizmos(true);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00011191 File Offset: 0x0000F391
		public void OnDrawGizmos()
		{
			this.OnDrawGizmos(false);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0001119C File Offset: 0x0000F39C
		public void OnDrawGizmos(bool selected)
		{
			Color color = selected ? NodeLink2.GizmosColorSelected : NodeLink2.GizmosColor;
			if (this.StartTransform != null)
			{
				Draw.Gizmos.CircleXZ(this.StartTransform.position, 0.4f, color, 0f, 6.2831855f);
			}
			if (this.EndTransform != null)
			{
				Draw.Gizmos.CircleXZ(this.EndTransform.position, 0.4f, color, 0f, 6.2831855f);
			}
			if (this.StartTransform != null && this.EndTransform != null)
			{
				Draw.Gizmos.Bezier(this.StartTransform.position, this.EndTransform.position, color);
				if (selected)
				{
					Vector3 normalized = Vector3.Cross(Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
					Draw.Gizmos.Bezier(this.StartTransform.position + normalized * 0.1f, this.EndTransform.position + normalized * 0.1f, color);
					Draw.Gizmos.Bezier(this.StartTransform.position - normalized * 0.1f, this.EndTransform.position - normalized * 0.1f, color);
				}
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00011318 File Offset: 0x0000F518
		internal static void SerializeReferences(GraphSerializationContext ctx)
		{
			List<NodeLink2> modifiersOfType = GraphModifier.GetModifiersOfType<NodeLink2>();
			ctx.writer.Write(modifiersOfType.Count);
			foreach (NodeLink2 nodeLink in modifiersOfType)
			{
				ctx.writer.Write(nodeLink.uniqueID);
				ctx.SerializeNodeReference(nodeLink.startNode);
				ctx.SerializeNodeReference(nodeLink.endNode);
				ctx.SerializeNodeReference(nodeLink.connectedNode1);
				ctx.SerializeNodeReference(nodeLink.connectedNode2);
				ctx.SerializeVector3(nodeLink.clamped1);
				ctx.SerializeVector3(nodeLink.clamped2);
				ctx.writer.Write(nodeLink.postScanCalled);
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000113E0 File Offset: 0x0000F5E0
		internal static void DeserializeReferences(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ulong key = ctx.reader.ReadUInt64();
				GraphNode graphNode = ctx.DeserializeNodeReference();
				GraphNode graphNode2 = ctx.DeserializeNodeReference();
				GraphNode graphNode3 = ctx.DeserializeNodeReference();
				GraphNode graphNode4 = ctx.DeserializeNodeReference();
				Vector3 vector = ctx.DeserializeVector3();
				Vector3 vector2 = ctx.DeserializeVector3();
				bool flag = ctx.reader.ReadBoolean();
				GraphModifier graphModifier;
				if (!GraphModifier.usedIDs.TryGetValue(key, out graphModifier))
				{
					throw new Exception("Tried to deserialize a NodeLink2 reference, but the link could not be found in the scene.\nIf a NodeLink2 is included in serialized graph data, the same NodeLink2 component must be present in the scene when loading the graph data.");
				}
				NodeLink2 nodeLink = graphModifier as NodeLink2;
				if (!(nodeLink != null))
				{
					throw new Exception("Tried to deserialize a NodeLink2 reference, but the link was not of the correct type or it has been destroyed.\nIf a NodeLink2 is included in serialized graph data, the same NodeLink2 component must be present in the scene when loading the graph data.");
				}
				if (graphNode != null)
				{
					NodeLink2.reference[graphNode] = nodeLink;
				}
				if (graphNode2 != null)
				{
					NodeLink2.reference[graphNode2] = nodeLink;
				}
				if (nodeLink.startNode != null)
				{
					NodeLink2.reference.Remove(nodeLink.startNode);
				}
				if (nodeLink.endNode != null)
				{
					NodeLink2.reference.Remove(nodeLink.endNode);
				}
				nodeLink.startNode = (graphNode as PointNode);
				nodeLink.endNode = (graphNode2 as PointNode);
				nodeLink.connectedNode1 = graphNode3;
				nodeLink.connectedNode2 = graphNode4;
				nodeLink.postScanCalled = flag;
				nodeLink.clamped1 = vector;
				nodeLink.clamped2 = vector2;
			}
		}

		// Token: 0x040001EC RID: 492
		protected static Dictionary<GraphNode, NodeLink2> reference = new Dictionary<GraphNode, NodeLink2>();

		// Token: 0x040001ED RID: 493
		public Transform end;

		// Token: 0x040001EE RID: 494
		public float costFactor = 1f;

		// Token: 0x040001EF RID: 495
		public bool oneWay;

		// Token: 0x040001F2 RID: 498
		private GraphNode connectedNode1;

		// Token: 0x040001F3 RID: 499
		private GraphNode connectedNode2;

		// Token: 0x040001F4 RID: 500
		private Vector3 clamped1;

		// Token: 0x040001F5 RID: 501
		private Vector3 clamped2;

		// Token: 0x040001F6 RID: 502
		private bool postScanCalled;

		// Token: 0x040001F7 RID: 503
		private static readonly Color GizmosColor = new Color(0.80784315f, 0.53333336f, 0.1882353f, 0.5f);

		// Token: 0x040001F8 RID: 504
		private static readonly Color GizmosColorSelected = new Color(0.92156863f, 0.48235294f, 0.1254902f, 1f);
	}
}
