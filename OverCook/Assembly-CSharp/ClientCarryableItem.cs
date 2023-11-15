using System;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020007EA RID: 2026
public class ClientCarryableItem : ClientSynchroniserBase, IClientHandlePickup, IHandleAttachTarget, IBaseHandlePickup
{
	// Token: 0x1700031C RID: 796
	// (get) Token: 0x060026F9 RID: 9977 RVA: 0x0008E041 File Offset: 0x0008C441
	public virtual PlayerAttachTarget PlayerAttachTarget
	{
		get
		{
			return PlayerAttachTarget.Default;
		}
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x0008E044 File Offset: 0x0008C444
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x0008E04C File Offset: 0x0008C44C
	public int GetPickupPriority()
	{
		return 0;
	}
}
