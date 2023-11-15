using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class ClientHandlePlacementReferral : ClientSynchroniserBase
{
	// Token: 0x060015E2 RID: 5602 RVA: 0x00075084 File Offset: 0x00073484
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_referral = (HandlePlacementReferral)synchronisedObject;
		if (this.m_referral.m_placementReferralObject != null)
		{
			this.m_iHandlePlacements = ComponentCache<IClientHandlePlacement>.GetComponents(this.m_referral.m_placementReferralObject);
		}
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x000750D0 File Offset: 0x000734D0
	public void SetHandlePlacementReferree(IClientHandlePlacement _iHandlePlacement)
	{
		this.m_iHandlePlacement = _iHandlePlacement;
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x000750D9 File Offset: 0x000734D9
	public IClientHandlePlacement GetHandlePlacementReferree()
	{
		if (this.m_iHandlePlacement == null && this.m_iHandlePlacements != null)
		{
			return HandlePlacementUtils.GetHighestPriority<IClientHandlePlacement>(this.m_iHandlePlacements);
		}
		return this.m_iHandlePlacement;
	}

	// Token: 0x0400108B RID: 4235
	private HandlePlacementReferral m_referral;

	// Token: 0x0400108C RID: 4236
	private IClientHandlePlacement[] m_iHandlePlacements;

	// Token: 0x0400108D RID: 4237
	private IClientHandlePlacement m_iHandlePlacement;
}
