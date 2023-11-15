using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200000E RID: 14
	public class RichSpecial : RichPathPart
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00007BDB File Offset: 0x00005DDB
		public override void OnEnterPool()
		{
			this.nodeLink = null;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public RichSpecial Initialize(NodeLink2 nodeLink, GraphNode first)
		{
			this.nodeLink = nodeLink;
			if (first == nodeLink.startNode)
			{
				this.first = nodeLink.StartTransform;
				this.second = nodeLink.EndTransform;
				this.reverse = false;
			}
			else
			{
				this.first = nodeLink.EndTransform;
				this.second = nodeLink.StartTransform;
				this.reverse = true;
			}
			return this;
		}

		// Token: 0x040000BD RID: 189
		public NodeLink2 nodeLink;

		// Token: 0x040000BE RID: 190
		public Transform first;

		// Token: 0x040000BF RID: 191
		public Transform second;

		// Token: 0x040000C0 RID: 192
		public bool reverse;
	}
}
