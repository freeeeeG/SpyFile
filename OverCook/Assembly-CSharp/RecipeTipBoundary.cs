using System;
using UnityEngine;

// Token: 0x0200068C RID: 1676
[Serializable]
public class RecipeTipBoundary
{
	// Token: 0x06002019 RID: 8217 RVA: 0x0009C726 File Offset: 0x0009AB26
	public RecipeTipBoundary(float _percent, int _score)
	{
		this.PercentageTimeRemaining = _percent;
		this.ScoreValue = _score;
	}

	// Token: 0x04001884 RID: 6276
	[Range(0f, 1f)]
	public float PercentageTimeRemaining;

	// Token: 0x04001885 RID: 6277
	public int ScoreValue;
}
