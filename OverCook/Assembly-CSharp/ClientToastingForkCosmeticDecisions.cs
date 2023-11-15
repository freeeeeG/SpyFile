using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000402 RID: 1026
public class ClientToastingForkCosmeticDecisions : ClientSynchroniserBase, IClientSurfacePlacementNotified
{
	// Token: 0x060012AA RID: 4778 RVA: 0x00068C10 File Offset: 0x00067010
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cosmeticDecisions = (ToastingForkCosmeticDecisions)synchronisedObject;
		this.m_cookingHandler = base.gameObject.RequireComponent<CookingHandler>();
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x00068C30 File Offset: 0x00067030
	public void OnSurfacePlacement(ClientAttachStation _station)
	{
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			return;
		}
		if (_station != null && _station.gameObject != null)
		{
			CookingStation cookingStation = _station.gameObject.RequestComponent<CookingStation>();
			if (cookingStation != null && cookingStation.m_stationType == this.m_cookingHandler.m_stationType)
			{
				this.m_cosmeticDecisions.ApplyPositionOffsetToChilden(new Vector3(this.m_cosmeticDecisions.m_surfaceAttachedOffset, 0f, 0f), ref this.m_originalPositions);
			}
		}
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x00068CC8 File Offset: 0x000670C8
	public void OnSurfaceDeplacement(ClientAttachStation _station)
	{
		this.m_cosmeticDecisions.RestorePositionsToChildren(ref this.m_originalPositions);
		this.m_originalPositions.Clear();
	}

	// Token: 0x04000EA5 RID: 3749
	private ToastingForkCosmeticDecisions m_cosmeticDecisions;

	// Token: 0x04000EA6 RID: 3750
	private CookingHandler m_cookingHandler;

	// Token: 0x04000EA7 RID: 3751
	private Dictionary<string, Vector3> m_originalPositions = new Dictionary<string, Vector3>();
}
