using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009C RID: 156
public interface IPlaceable
{
	// Token: 0x06000338 RID: 824
	void SwitchToPlacementMode();

	// Token: 0x06000339 RID: 825
	ePlaceableType GetPlaceableType();

	// Token: 0x0600033A RID: 826
	List<Collider> GetCollisionColliders();

	// Token: 0x0600033B RID: 827
	List<Collider> GetPlacementColliders();

	// Token: 0x0600033C RID: 828
	Vector3 GetPlacementOffset();

	// Token: 0x0600033D RID: 829
	void OnPlacementProc();
}
