using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class ServerHandlePickupReferral : ServerSynchroniserBase
{
	// Token: 0x060015CF RID: 5583 RVA: 0x00074EC0 File Offset: 0x000732C0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_referral = (HandlePickupReferral)synchronisedObject;
		if (this.m_referral.m_pickupReferralObject != null)
		{
			this.m_iHandlePickup = this.m_referral.m_pickupReferralObject.RequireInterface<IHandlePickup>();
		}
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x00074F0C File Offset: 0x0007330C
	public void SetHandlePickupReferree(IHandlePickup _iHandlePickup)
	{
		this.m_iHandlePickup = _iHandlePickup;
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x00074F15 File Offset: 0x00073315
	public IHandlePickup GetHandlePickupReferree()
	{
		return this.m_iHandlePickup;
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x00074F1D File Offset: 0x0007331D
	public void RegisterAllowReferralBlock(Generic<bool, ICarrier> _callback)
	{
		this.m_allowBlockingCallbacks.Add(_callback);
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x00074F2B File Offset: 0x0007332B
	public void UnregisterAllowReferralBlock(Generic<bool, ICarrier> _callback)
	{
		this.m_allowBlockingCallbacks.Remove(_callback);
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x00074F3A File Offset: 0x0007333A
	public bool CanBeBlocked(ICarrier _carrier)
	{
		return !this.m_allowBlockingCallbacks.CallForResult(false, _carrier);
	}

	// Token: 0x04001081 RID: 4225
	private HandlePickupReferral m_referral;

	// Token: 0x04001082 RID: 4226
	private List<Generic<bool, ICarrier>> m_allowBlockingCallbacks = new List<Generic<bool, ICarrier>>();

	// Token: 0x04001083 RID: 4227
	private IHandlePickup m_iHandlePickup;
}
