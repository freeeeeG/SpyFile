using System;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
	// Token: 0x0400107C RID: 4220
	public Vector2I OriginOffset;

	// Token: 0x0400107D RID: 4221
	public Vector2I RangeMin;

	// Token: 0x0400107E RID: 4222
	public Vector2I RangeMax;

	// Token: 0x0400107F RID: 4223
	public bool TestLineOfSight = true;

	// Token: 0x04001080 RID: 4224
	public bool BlockingTileVisible;

	// Token: 0x04001081 RID: 4225
	public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);

	// Token: 0x04001082 RID: 4226
	public bool AllowLineOfSightInvalidCells;
}
