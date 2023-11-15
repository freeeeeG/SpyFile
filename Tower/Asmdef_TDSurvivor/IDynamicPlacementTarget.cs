using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public interface IDynamicPlacementTarget
{
	// Token: 0x06000336 RID: 822
	Transform GetPlacementTransform();

	// Token: 0x06000337 RID: 823
	void PlaceTower(ABaseTower tower);
}
