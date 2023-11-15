using System;
using PathCreation;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class CreatePath : MonoBehaviour
{
	// Token: 0x06000B9D RID: 2973 RVA: 0x0001E418 File Offset: 0x0001C618
	private void Start()
	{
		this.pathCreator = base.GetComponent<PathCreator>();
		if (this.waypoints.Length != 0)
		{
			BezierPath bezierPath = new BezierPath(this.waypoints, false, PathSpace.xyz);
			base.GetComponent<PathCreator>().bezierPath = bezierPath;
		}
	}

	// Token: 0x040005C8 RID: 1480
	public PathCreator pathCreator;

	// Token: 0x040005C9 RID: 1481
	public Transform[] waypoints;
}
