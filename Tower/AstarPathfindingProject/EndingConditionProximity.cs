using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000097 RID: 151
	public class EndingConditionProximity : ABPathEndingCondition
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x0002AE72 File Offset: 0x00029072
		public EndingConditionProximity(ABPath p, float maxDistance) : base(p)
		{
			this.maxDistance = maxDistance;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002AE90 File Offset: 0x00029090
		public override bool TargetFound(PathNode node)
		{
			return ((Vector3)node.node.position - this.abPath.originalEndPoint).sqrMagnitude <= this.maxDistance * this.maxDistance;
		}

		// Token: 0x04000417 RID: 1047
		public float maxDistance = 10f;
	}
}
