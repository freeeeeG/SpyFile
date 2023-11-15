using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[Serializable]
public struct PathPoint
{
	// Token: 0x0600008D RID: 141 RVA: 0x000048CF File Offset: 0x00002ACF
	public PathPoint(Vector2 pos, Direction dir, Vector2 exit)
	{
		this.PathPos = pos;
		this.PathDirection = dir;
		this.ExitPoint = exit;
	}

	// Token: 0x040000CC RID: 204
	public Vector2 PathPos;

	// Token: 0x040000CD RID: 205
	public Direction PathDirection;

	// Token: 0x040000CE RID: 206
	public Vector2 ExitPoint;
}
