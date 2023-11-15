using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
public class ServerCarryableItem : ServerSynchroniserBase, IHandlePickup, IHandleAttachTarget, IBaseHandlePickup
{
	// Token: 0x1700031B RID: 795
	// (get) Token: 0x060026F4 RID: 9972 RVA: 0x0008DDB7 File Offset: 0x0008C1B7
	public virtual PlayerAttachTarget PlayerAttachTarget
	{
		get
		{
			return PlayerAttachTarget.Default;
		}
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x0008DDBA File Offset: 0x0008C1BA
	public virtual bool CanHandlePickup(ICarrier _carrier)
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x0008DDC4 File Offset: 0x0008C1C4
	public virtual void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		IAttachment component = base.gameObject.GetComponent<IAttachment>();
		if (component.IsAttached())
		{
			component.Detach();
		}
		_carrier.CarryItem(base.gameObject);
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x0008DDFA File Offset: 0x0008C1FA
	public int GetPickupPriority()
	{
		return 0;
	}
}
