using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000075 RID: 117
	public class RecastBBTreeBox
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x000245BC File Offset: 0x000227BC
		public RecastBBTreeBox(RecastMeshObj mesh)
		{
			this.mesh = mesh;
			Vector3 min = mesh.bounds.min;
			Vector3 max = mesh.bounds.max;
			this.rect = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00024611 File Offset: 0x00022811
		public bool Contains(Vector3 p)
		{
			return this.rect.Contains(p);
		}

		// Token: 0x04000376 RID: 886
		public Rect rect;

		// Token: 0x04000377 RID: 887
		public RecastMeshObj mesh;

		// Token: 0x04000378 RID: 888
		public RecastBBTreeBox c1;

		// Token: 0x04000379 RID: 889
		public RecastBBTreeBox c2;
	}
}
