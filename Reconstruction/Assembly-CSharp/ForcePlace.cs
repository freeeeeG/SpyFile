using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000117 RID: 279
[Serializable]
public class ForcePlace
{
	// Token: 0x060006E9 RID: 1769 RVA: 0x00012F38 File Offset: 0x00011138
	public ForcePlace(Vector2 forcePos, Vector2 forceDir, List<Vector2> guidePoss)
	{
		this.ForcePos = forcePos;
		this.ForceDir = forceDir;
		this.GuidePos = guidePoss;
	}

	// Token: 0x04000341 RID: 833
	public Vector2 ForcePos;

	// Token: 0x04000342 RID: 834
	public Vector2 ForceDir;

	// Token: 0x04000343 RID: 835
	public List<Vector2> GuidePos = new List<Vector2>();
}
