using System;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public interface IPlacementSupression
{
	// Token: 0x06001710 RID: 5904
	void RegisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback);

	// Token: 0x06001711 RID: 5905
	void UnregisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback);
}
