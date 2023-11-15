using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000538 RID: 1336
public class ClientPlacementInteractable : ClientSynchroniserBase, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x06001906 RID: 6406 RVA: 0x0007F449 File Offset: 0x0007D849
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		return true;
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x0007F44C File Offset: 0x0007D84C
	public int GetPlacementPriority()
	{
		return 1;
	}
}
