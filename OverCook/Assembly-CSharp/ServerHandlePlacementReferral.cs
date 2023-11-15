using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class ServerHandlePlacementReferral : ServerSynchroniserBase
{
	// Token: 0x060015DE RID: 5598 RVA: 0x00074FFC File Offset: 0x000733FC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_referral = (HandlePlacementReferral)synchronisedObject;
		if (this.m_referral.m_placementReferralObject != null)
		{
			this.m_iHandlePlacements = ComponentCache<IHandlePlacement>.GetComponents(this.m_referral.m_placementReferralObject);
		}
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x00075048 File Offset: 0x00073448
	public void SetHandlePlacementReferree(IHandlePlacement _iHandlePlacement)
	{
		this.m_iHandlePlacement = _iHandlePlacement;
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x00075051 File Offset: 0x00073451
	public IHandlePlacement GetHandlePlacementReferree()
	{
		if (this.m_iHandlePlacement == null && this.m_iHandlePlacements != null)
		{
			return HandlePlacementUtils.GetHighestPriority<IHandlePlacement>(this.m_iHandlePlacements);
		}
		return this.m_iHandlePlacement;
	}

	// Token: 0x04001088 RID: 4232
	private HandlePlacementReferral m_referral;

	// Token: 0x04001089 RID: 4233
	private IHandlePlacement[] m_iHandlePlacements;

	// Token: 0x0400108A RID: 4234
	private IHandlePlacement m_iHandlePlacement;
}
