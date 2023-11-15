using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000011 RID: 17
	public struct GraphHitInfo
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00008598 File Offset: 0x00006798
		public float distance
		{
			get
			{
				return (this.point - this.origin).magnitude;
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000085BE File Offset: 0x000067BE
		public GraphHitInfo(Vector3 point)
		{
			this.tangentOrigin = Vector3.zero;
			this.origin = Vector3.zero;
			this.point = point;
			this.node = null;
			this.tangent = Vector3.zero;
		}

		// Token: 0x040000E2 RID: 226
		public Vector3 origin;

		// Token: 0x040000E3 RID: 227
		public Vector3 point;

		// Token: 0x040000E4 RID: 228
		public GraphNode node;

		// Token: 0x040000E5 RID: 229
		public Vector3 tangentOrigin;

		// Token: 0x040000E6 RID: 230
		public Vector3 tangent;
	}
}
