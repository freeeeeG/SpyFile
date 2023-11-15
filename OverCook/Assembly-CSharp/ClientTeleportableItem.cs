using System;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
public class ClientTeleportableItem : ClientBaseTeleportable
{
	// Token: 0x06001BF6 RID: 7158 RVA: 0x0008882D File Offset: 0x00086C2D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachment = base.gameObject.RequireComponent<ClientPhysicalAttachment>();
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x00088847 File Offset: 0x00086C47
	public override bool CanTeleport(ClientTeleportal _portal)
	{
		return this.m_attachment != null && base.CanTeleport(_portal);
	}

	// Token: 0x040015EC RID: 5612
	private ClientPhysicalAttachment m_attachment;
}
