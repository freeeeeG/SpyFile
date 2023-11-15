using System;
using UnityEngine;

// Token: 0x020004C2 RID: 1218
public interface IHandlePlacement : IBaseHandlePlacement
{
	// Token: 0x06001689 RID: 5769
	void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context);

	// Token: 0x0600168A RID: 5770
	void OnFailedToPlace(GameObject _item);
}
