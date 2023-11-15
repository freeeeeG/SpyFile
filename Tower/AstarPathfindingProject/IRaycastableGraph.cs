using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200001B RID: 27
	public interface IRaycastableGraph
	{
		// Token: 0x060001B3 RID: 435
		bool Linecast(Vector3 start, Vector3 end);

		// Token: 0x060001B4 RID: 436
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode hint);

		// Token: 0x060001B5 RID: 437
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit);

		// Token: 0x060001B6 RID: 438
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace);

		// Token: 0x060001B7 RID: 439
		bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, List<GraphNode> trace = null, Func<GraphNode, bool> filter = null);
	}
}
