using System;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public interface IBaseHandlePlacement
{
	// Token: 0x06001687 RID: 5767
	bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context);

	// Token: 0x06001688 RID: 5768
	int GetPlacementPriority();
}
