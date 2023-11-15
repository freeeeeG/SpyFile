using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003DA RID: 986
public class ClientMovingPlatformCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001237 RID: 4663 RVA: 0x0006705D File Offset: 0x0006545D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_decisions = (MovingPlatformCosmeticDecisions)synchronisedObject;
		this.m_clientPilotMovement = base.gameObject.RequireComponent<ClientPilotMovement>();
		this.m_clientPilotMovement.OnPilotStatusChanged += this.OnPilotStatusChanged;
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x0006709A File Offset: 0x0006549A
	private void OnPilotStatusChanged(bool _hasPilot)
	{
		this.m_decisions.OnPilotStatusChanged(_hasPilot);
	}

	// Token: 0x04000E3F RID: 3647
	private MovingPlatformCosmeticDecisions m_decisions;

	// Token: 0x04000E40 RID: 3648
	private ClientPilotMovement m_clientPilotMovement;
}
