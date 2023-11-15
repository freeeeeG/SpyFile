using System;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class ServerTeleportableItem : ServerBaseTeleportable
{
	// Token: 0x06001BF3 RID: 7155 RVA: 0x000887CE File Offset: 0x00086BCE
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachment = base.gameObject.RequireComponent<ServerPhysicalAttachment>();
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000887E8 File Offset: 0x00086BE8
	public override bool CanTeleport(ServerTeleportal _portal)
	{
		return this.m_attachment != null && this.m_attachment.enabled && !this.m_attachment.IsAttached() && base.CanTeleport(_portal);
	}

	// Token: 0x040015EB RID: 5611
	private ServerPhysicalAttachment m_attachment;
}
