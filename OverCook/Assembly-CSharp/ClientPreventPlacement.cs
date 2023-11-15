using System;
using UnityEngine;

// Token: 0x02000592 RID: 1426
public class ClientPreventPlacement : MonoBehaviour, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x06001B0A RID: 6922 RVA: 0x00086AFF File Offset: 0x00084EFF
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		return false;
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x00086B02 File Offset: 0x00084F02
	public int GetPlacementPriority()
	{
		return 0;
	}
}
