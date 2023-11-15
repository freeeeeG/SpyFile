using System;
using UnityEngine;

namespace PathCreation.Examples
{
	// Token: 0x020002B4 RID: 692
	[RequireComponent(typeof(PathCreator))]
	public class GeneratePathExample : MonoBehaviour
	{
		// Token: 0x06001106 RID: 4358 RVA: 0x000300C0 File Offset: 0x0002E2C0
		private void Start()
		{
			if (this.waypoints.Length != 0)
			{
				BezierPath bezierPath = new BezierPath(this.waypoints, this.closedLoop, PathSpace.xyz);
				base.GetComponent<PathCreator>().bezierPath = bezierPath;
			}
		}

		// Token: 0x04000942 RID: 2370
		public bool closedLoop = true;

		// Token: 0x04000943 RID: 2371
		public Transform[] waypoints;
	}
}
