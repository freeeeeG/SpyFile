using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000090 RID: 144
	public class FloodPathConstraint : NNConstraint
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x00029689 File Offset: 0x00027889
		public FloodPathConstraint(FloodPath path)
		{
			if (path == null)
			{
				Debug.LogWarning("FloodPathConstraint should not be used with a NULL path");
			}
			this.path = path;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000296A5 File Offset: 0x000278A5
		public override bool Suitable(GraphNode node)
		{
			return base.Suitable(node) && this.path.HasPathTo(node);
		}

		// Token: 0x040003FC RID: 1020
		private readonly FloodPath path;
	}
}
