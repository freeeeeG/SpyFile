using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000014 RID: 20
	public struct NNInfoInternal
	{
		// Token: 0x0600019E RID: 414 RVA: 0x0000870A File Offset: 0x0000690A
		public NNInfoInternal(GraphNode node)
		{
			this.node = node;
			this.constrainedNode = null;
			this.clampedPosition = Vector3.zero;
			this.constClampedPosition = Vector3.zero;
			this.UpdateInfo();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008738 File Offset: 0x00006938
		public void UpdateInfo()
		{
			this.clampedPosition = ((this.node != null) ? ((Vector3)this.node.position) : Vector3.zero);
			this.constClampedPosition = ((this.constrainedNode != null) ? ((Vector3)this.constrainedNode.position) : Vector3.zero);
		}

		// Token: 0x040000F0 RID: 240
		public GraphNode node;

		// Token: 0x040000F1 RID: 241
		public GraphNode constrainedNode;

		// Token: 0x040000F2 RID: 242
		public Vector3 clampedPosition;

		// Token: 0x040000F3 RID: 243
		public Vector3 constClampedPosition;
	}
}
