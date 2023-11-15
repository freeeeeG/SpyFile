using System;
using UnityEngine;

// Token: 0x0200061D RID: 1565
public class ClientUsableItem : ClientInteractable
{
	// Token: 0x06001DA7 RID: 7591 RVA: 0x000905D8 File Offset: 0x0008E9D8
	public override bool CanInteract(GameObject _interacter)
	{
		IPlayerCarrier playerCarrier = _interacter.RequireInterface<IPlayerCarrier>();
		GameObject x = playerCarrier.InspectCarriedItem();
		return base.CanInteract(_interacter) && x == base.gameObject;
	}
}
