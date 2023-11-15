using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000040 RID: 64
	[AddComponentMenu("Pathfinding/Link")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_node_link.php")]
	public class NodeLink : GraphModifier
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0001099C File Offset: 0x0000EB9C
		public Transform Start
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000109A4 File Offset: 0x0000EBA4
		public Transform End
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000109AC File Offset: 0x0000EBAC
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

		// Token: 0x06000303 RID: 771 RVA: 0x000109DC File Offset: 0x0000EBDC
		public void InternalOnPostScan()
		{
			this.Apply();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000109E4 File Offset: 0x0000EBE4
		public override void OnGraphsPostUpdate()
		{
			if (!AstarPath.active.isScanning)
			{
				AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(bool force)
				{
					this.InternalOnPostScan();
					return true;
				}));
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010A10 File Offset: 0x0000EC10
		public virtual void Apply()
		{
			if (this.Start == null || this.End == null || AstarPath.active == null)
			{
				return;
			}
			GraphNode node = AstarPath.active.GetNearest(this.Start.position).node;
			GraphNode node2 = AstarPath.active.GetNearest(this.End.position).node;
			if (node == null || node2 == null)
			{
				return;
			}
			if (this.deleteConnection)
			{
				node.RemoveConnection(node2);
				if (!this.oneWay)
				{
					node2.RemoveConnection(node);
					return;
				}
			}
			else
			{
				uint cost = (uint)Math.Round((double)((float)(node.position - node2.position).costMagnitude * this.costFactor));
				node.AddConnection(node2, cost);
				if (!this.oneWay)
				{
					node2.AddConnection(node, cost);
				}
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00010AE4 File Offset: 0x0000ECE4
		public void OnDrawGizmos()
		{
			if (this.Start == null || this.End == null)
			{
				return;
			}
			Draw.Gizmos.Bezier(this.Start.position, this.End.position, this.deleteConnection ? Color.red : Color.green);
		}

		// Token: 0x040001E8 RID: 488
		public Transform end;

		// Token: 0x040001E9 RID: 489
		public float costFactor = 1f;

		// Token: 0x040001EA RID: 490
		public bool oneWay;

		// Token: 0x040001EB RID: 491
		public bool deleteConnection;
	}
}
