using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class ClientTeleportalCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001293 RID: 4755 RVA: 0x000686BD File Offset: 0x00066ABD
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x000686C6 File Offset: 0x00066AC6
	private void Awake()
	{
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x000686C8 File Offset: 0x00066AC8
	private void OnCanTeleportStateChanged(bool _canTeleport)
	{
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x000686CA File Offset: 0x00066ACA
	private void OnTeleportStateChanged(bool _teleporting)
	{
	}

	// Token: 0x04000E8A RID: 3722
	private TeleportalCosmeticDecisions m_decisions;

	// Token: 0x04000E8B RID: 3723
	private ClientTeleportal m_portal;
}
