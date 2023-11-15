using System;
using UnityEngine;

// Token: 0x0200068B RID: 1675
[Serializable]
public class GameConfig : ScriptableObject
{
	// Token: 0x04001880 RID: 6272
	public RecipeTipBoundary[] TipBoundaries = new RecipeTipBoundary[]
	{
		new RecipeTipBoundary(0f, 0),
		new RecipeTipBoundary(0.25f, 2),
		new RecipeTipBoundary(0.5f, 4),
		new RecipeTipBoundary(0.75f, 6)
	};

	// Token: 0x04001881 RID: 6273
	public int DefaultDeliveryAward = 20;

	// Token: 0x04001882 RID: 6274
	public int RecipeTimeOutPointLoss = 10;

	// Token: 0x04001883 RID: 6275
	public int SingleplayerChopTimeMultiplier = 5;
}
