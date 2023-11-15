using System;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class ServerTeleportablePlayer : ServerBaseTeleportable
{
	// Token: 0x06001BFA RID: 7162 RVA: 0x00088874 File Offset: 0x00086C74
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_playerControls = base.gameObject.RequireComponent<PlayerControls>();
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x0008888E File Offset: 0x00086C8E
	public override bool CanTeleport(ServerTeleportal _portal)
	{
		return this.m_playerControls != null && this.m_playerControls.enabled && base.CanTeleport(_portal);
	}

	// Token: 0x040015ED RID: 5613
	private PlayerControls m_playerControls;
}
