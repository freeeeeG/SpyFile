using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000D3 RID: 211
	public class ObstacleVertex
	{
		// Token: 0x04000524 RID: 1316
		public bool ignore;

		// Token: 0x04000525 RID: 1317
		public Vector3 position;

		// Token: 0x04000526 RID: 1318
		public Vector2 dir;

		// Token: 0x04000527 RID: 1319
		public float height;

		// Token: 0x04000528 RID: 1320
		public RVOLayer layer = RVOLayer.DefaultObstacle;

		// Token: 0x04000529 RID: 1321
		public ObstacleVertex next;

		// Token: 0x0400052A RID: 1322
		public ObstacleVertex prev;
	}
}
