using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000042 RID: 66
	public class NodeLink3Node : PointNode
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0001159B File Offset: 0x0000F79B
		public NodeLink3Node(AstarPath active) : base(active)
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000115A4 File Offset: 0x0000F7A4
		public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			if (this.connections.Length < 2)
			{
				return false;
			}
			if (this.connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
			}
			if (left != null)
			{
				left.Add(this.portalA);
				right.Add(this.portalB);
			}
			return true;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00011608 File Offset: 0x0000F808
		public GraphNode GetOther(GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (this.connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
			}
			if (a != this.connections[0].node)
			{
				return (this.connections[0].node as NodeLink3Node).GetOtherInternal(this);
			}
			return (this.connections[1].node as NodeLink3Node).GetOtherInternal(this);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001169C File Offset: 0x0000F89C
		private GraphNode GetOtherInternal(GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (a != this.connections[0].node)
			{
				return this.connections[0].node;
			}
			return this.connections[1].node;
		}

		// Token: 0x040001F9 RID: 505
		public NodeLink3 link;

		// Token: 0x040001FA RID: 506
		public Vector3 portalA;

		// Token: 0x040001FB RID: 507
		public Vector3 portalB;
	}
}
