using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000401 RID: 1025
public class ServerToastingForkCosmeticDecisions : ServerSynchroniserBase, ISurfacePlacementNotified
{
	// Token: 0x060012A6 RID: 4774 RVA: 0x00068B3D File Offset: 0x00066F3D
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cosmeticDecisions = (ToastingForkCosmeticDecisions)synchronisedObject;
		this.m_cookingHandler = base.gameObject.RequireComponent<CookingHandler>();
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x00068B5C File Offset: 0x00066F5C
	public void OnSurfacePlacement(ServerAttachStation _station)
	{
		if (_station != null && _station.gameObject != null)
		{
			CookingStation cookingStation = _station.gameObject.RequestComponent<CookingStation>();
			if (cookingStation != null && cookingStation.m_stationType == this.m_cookingHandler.m_stationType)
			{
				this.m_cosmeticDecisions.ApplyPositionOffsetToChilden(new Vector3(this.m_cosmeticDecisions.m_surfaceAttachedOffset, 0f, 0f), ref this.m_originalPositions);
			}
		}
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x00068BDF File Offset: 0x00066FDF
	public void OnSurfaceDeplacement(ServerAttachStation _station)
	{
		this.m_cosmeticDecisions.RestorePositionsToChildren(ref this.m_originalPositions);
		this.m_originalPositions.Clear();
	}

	// Token: 0x04000EA2 RID: 3746
	private ToastingForkCosmeticDecisions m_cosmeticDecisions;

	// Token: 0x04000EA3 RID: 3747
	private CookingHandler m_cookingHandler;

	// Token: 0x04000EA4 RID: 3748
	private Dictionary<string, Vector3> m_originalPositions = new Dictionary<string, Vector3>();
}
