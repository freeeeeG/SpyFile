using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000043 RID: 67
	[AddComponentMenu("Pathfinding/Link3")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_node_link3.php")]
	public class NodeLink3 : GraphModifier
	{
		// Token: 0x06000326 RID: 806 RVA: 0x000116F0 File Offset: 0x0000F8F0
		public static NodeLink3 GetNodeLink(GraphNode node)
		{
			NodeLink3 result;
			NodeLink3.reference.TryGetValue(node, out result);
			return result;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0001170C File Offset: 0x0000F90C
		public Transform StartTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00011714 File Offset: 0x0000F914
		public Transform EndTransform
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0001171C File Offset: 0x0000F91C
		public GraphNode StartNode
		{
			get
			{
				return this.startNode;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00011724 File Offset: 0x0000F924
		public GraphNode EndNode
		{
			get
			{
				return this.endNode;
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001172C File Offset: 0x0000F92C
		public override void OnPostScan()
		{
			if (AstarPath.active.isScanning)
			{
				this.InternalOnPostScan();
				return;
			}
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(bool force)
			{
				this.InternalOnPostScan();
				return true;
			}));
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001175C File Offset: 0x0000F95C
		public void InternalOnPostScan()
		{
			if (AstarPath.active.data.pointGraph == null)
			{
				AstarPath.active.data.AddGraph(typeof(PointGraph));
			}
			this.startNode = AstarPath.active.data.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3)this.StartTransform.position);
			this.startNode.link = this;
			this.endNode = AstarPath.active.data.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3)this.EndTransform.position);
			this.endNode.link = this;
			this.connectedNode1 = null;
			this.connectedNode2 = null;
			if (this.startNode == null || this.endNode == null)
			{
				this.startNode = null;
				this.endNode = null;
				return;
			}
			this.postScanCalled = true;
			NodeLink3.reference[this.startNode] = this;
			NodeLink3.reference[this.endNode] = this;
			this.Apply(true);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00011874 File Offset: 0x0000FA74
		public override void OnGraphsPostUpdate()
		{
			if (!AstarPath.active.isScanning)
			{
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
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000118DB File Offset: 0x0000FADB
		protected override void OnEnable()
		{
			base.OnEnable();
			if (Application.isPlaying && AstarPath.active != null && AstarPath.active.data != null && AstarPath.active.data.pointGraph != null)
			{
				this.OnGraphsPostUpdate();
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001191C File Offset: 0x0000FB1C
		protected override void OnDisable()
		{
			base.OnDisable();
			this.postScanCalled = false;
			if (this.startNode != null)
			{
				NodeLink3.reference.Remove(this.startNode);
			}
			if (this.endNode != null)
			{
				NodeLink3.reference.Remove(this.endNode);
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

		// Token: 0x06000330 RID: 816 RVA: 0x000119EE File Offset: 0x0000FBEE
		private void RemoveConnections(GraphNode node)
		{
			node.ClearConnections(true);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000119F7 File Offset: 0x0000FBF7
		[ContextMenu("Recalculate neighbours")]
		private void ContextApplyForce()
		{
			if (Application.isPlaying)
			{
				this.Apply(true);
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00011A08 File Offset: 0x0000FC08
		public void Apply(bool forceNewCheck)
		{
			NNConstraint none = NNConstraint.None;
			none.distanceXZ = true;
			int graphIndex = (int)this.startNode.GraphIndex;
			none.graphMask = ~(1 << graphIndex);
			bool flag = true;
			NNInfo nearest = AstarPath.active.GetNearest(this.StartTransform.position, none);
			flag &= (nearest.node == this.connectedNode1 && nearest.node != null);
			this.connectedNode1 = (nearest.node as MeshNode);
			this.clamped1 = nearest.position;
			if (this.connectedNode1 != null)
			{
				Debug.DrawRay((Vector3)this.connectedNode1.position, Vector3.up * 5f, Color.red);
			}
			NNInfo nearest2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
			flag &= (nearest2.node == this.connectedNode2 && nearest2.node != null);
			this.connectedNode2 = (nearest2.node as MeshNode);
			this.clamped2 = nearest2.position;
			if (this.connectedNode2 != null)
			{
				Debug.DrawRay((Vector3)this.connectedNode2.position, Vector3.up * 5f, Color.cyan);
			}
			if (this.connectedNode2 == null || this.connectedNode1 == null)
			{
				return;
			}
			this.startNode.SetPosition((Int3)this.StartTransform.position);
			this.endNode.SetPosition((Int3)this.EndTransform.position);
			if (flag && !forceNewCheck)
			{
				return;
			}
			this.RemoveConnections(this.startNode);
			this.RemoveConnections(this.endNode);
			uint cost = (uint)Mathf.RoundToInt((float)((Int3)(this.StartTransform.position - this.EndTransform.position)).costMagnitude * this.costFactor);
			this.startNode.AddConnection(this.endNode, cost);
			this.endNode.AddConnection(this.startNode, cost);
			Int3 rhs = this.connectedNode2.position - this.connectedNode1.position;
			for (int i = 0; i < this.connectedNode1.GetVertexCount(); i++)
			{
				Int3 vertex = this.connectedNode1.GetVertex(i);
				Int3 vertex2 = this.connectedNode1.GetVertex((i + 1) % this.connectedNode1.GetVertexCount());
				if (Int3.DotLong((vertex2 - vertex).Normal2D(), rhs) <= 0L)
				{
					for (int j = 0; j < this.connectedNode2.GetVertexCount(); j++)
					{
						Int3 vertex3 = this.connectedNode2.GetVertex(j);
						Int3 vertex4 = this.connectedNode2.GetVertex((j + 1) % this.connectedNode2.GetVertexCount());
						if (Int3.DotLong((vertex4 - vertex3).Normal2D(), rhs) >= 0L && (double)Int3.Angle(vertex4 - vertex3, vertex2 - vertex) > 2.967059810956319)
						{
							float num = 0f;
							float num2 = 1f;
							num2 = Math.Min(num2, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex3));
							num = Math.Max(num, VectorMath.ClosestPointOnLineFactor(vertex, vertex2, vertex4));
							if (num2 >= num)
							{
								Vector3 vector = (Vector3)(vertex2 - vertex) * num + (Vector3)vertex;
								Vector3 vector2 = (Vector3)(vertex2 - vertex) * num2 + (Vector3)vertex;
								this.startNode.portalA = vector;
								this.startNode.portalB = vector2;
								this.endNode.portalA = vector2;
								this.endNode.portalB = vector;
								this.connectedNode1.AddConnection(this.startNode, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
								this.connectedNode2.AddConnection(this.endNode, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
								this.startNode.AddConnection(this.connectedNode1, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped1 - this.StartTransform.position)).costMagnitude * this.costFactor));
								this.endNode.AddConnection(this.connectedNode2, (uint)Mathf.RoundToInt((float)((Int3)(this.clamped2 - this.EndTransform.position)).costMagnitude * this.costFactor));
								return;
							}
							Debug.LogError(string.Concat(new string[]
							{
								"Something went wrong! ",
								num.ToString(),
								" ",
								num2.ToString(),
								" ",
								vertex,
								" ",
								vertex2,
								" ",
								vertex3,
								" ",
								vertex4,
								"\nTODO, how can this happen?"
							}));
						}
					}
				}
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00011F7C File Offset: 0x0001017C
		public virtual void OnDrawGizmosSelected()
		{
			this.OnDrawGizmos(true);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00011F85 File Offset: 0x00010185
		public void OnDrawGizmos()
		{
			this.OnDrawGizmos(false);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00011F90 File Offset: 0x00010190
		public void OnDrawGizmos(bool selected)
		{
			Color color = selected ? NodeLink3.GizmosColorSelected : NodeLink3.GizmosColor;
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

		// Token: 0x040001FC RID: 508
		protected static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();

		// Token: 0x040001FD RID: 509
		public Transform end;

		// Token: 0x040001FE RID: 510
		public float costFactor = 1f;

		// Token: 0x040001FF RID: 511
		public bool oneWay;

		// Token: 0x04000200 RID: 512
		private NodeLink3Node startNode;

		// Token: 0x04000201 RID: 513
		private NodeLink3Node endNode;

		// Token: 0x04000202 RID: 514
		private MeshNode connectedNode1;

		// Token: 0x04000203 RID: 515
		private MeshNode connectedNode2;

		// Token: 0x04000204 RID: 516
		private Vector3 clamped1;

		// Token: 0x04000205 RID: 517
		private Vector3 clamped2;

		// Token: 0x04000206 RID: 518
		private bool postScanCalled;

		// Token: 0x04000207 RID: 519
		private static readonly Color GizmosColor = new Color(0.80784315f, 0.53333336f, 0.1882353f, 0.5f);

		// Token: 0x04000208 RID: 520
		private static readonly Color GizmosColorSelected = new Color(0.92156863f, 0.48235294f, 0.1254902f, 1f);
	}
}
