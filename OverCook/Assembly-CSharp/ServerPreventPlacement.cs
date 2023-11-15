using System;
using UnityEngine;

// Token: 0x02000591 RID: 1425
public class ServerPreventPlacement : MonoBehaviour, IHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x06001B05 RID: 6917 RVA: 0x00086AED File Offset: 0x00084EED
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		return false;
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x00086AF0 File Offset: 0x00084EF0
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x00086AF2 File Offset: 0x00084EF2
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x00086AF4 File Offset: 0x00084EF4
	public int GetPlacementPriority()
	{
		return 0;
	}
}
