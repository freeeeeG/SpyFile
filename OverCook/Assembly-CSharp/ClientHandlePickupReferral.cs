using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class ClientHandlePickupReferral : ClientSynchroniserBase
{
	// Token: 0x060015D6 RID: 5590 RVA: 0x00074F60 File Offset: 0x00073360
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_referral = (HandlePickupReferral)synchronisedObject;
		if (this.m_referral.m_pickupReferralObject != null)
		{
			this.m_iHandlePickup = this.m_referral.m_pickupReferralObject.RequireInterface<IClientHandlePickup>();
		}
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x00074FAC File Offset: 0x000733AC
	public void SetHandlePickupReferree(IClientHandlePickup _iHandlePickup)
	{
		this.m_iHandlePickup = _iHandlePickup;
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x00074FB5 File Offset: 0x000733B5
	public IClientHandlePickup GetHandlePickupReferree()
	{
		return this.m_iHandlePickup;
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x00074FBD File Offset: 0x000733BD
	public void RegisterAllowReferralBlock(Generic<bool, ICarrier> _callback)
	{
		this.m_allowBlockingCallbacks.Add(_callback);
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x00074FCB File Offset: 0x000733CB
	public void UnregisterAllowReferralBlock(Generic<bool, ICarrier> _callback)
	{
		this.m_allowBlockingCallbacks.Remove(_callback);
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x00074FDA File Offset: 0x000733DA
	public bool CanBeBlocked(ICarrier _carrier)
	{
		return !this.m_allowBlockingCallbacks.CallForResult(false, _carrier);
	}

	// Token: 0x04001084 RID: 4228
	private HandlePickupReferral m_referral;

	// Token: 0x04001085 RID: 4229
	private List<Generic<bool, ICarrier>> m_allowBlockingCallbacks = new List<Generic<bool, ICarrier>>();

	// Token: 0x04001086 RID: 4230
	private IClientHandlePickup m_iHandlePickup;
}
