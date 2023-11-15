using System;
using UnityEngine;

// Token: 0x020005C5 RID: 1477
public class ClientTeleportablePlayer : ClientBaseTeleportable
{
	// Token: 0x06001BFD RID: 7165 RVA: 0x000888C3 File Offset: 0x00086CC3
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_playerControls = base.gameObject.RequireComponent<PlayerControls>();
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x000888DD File Offset: 0x00086CDD
	public override bool CanTeleport(ClientTeleportal _portal)
	{
		return this.m_playerControls != null && base.CanTeleport(_portal);
	}

	// Token: 0x040015EE RID: 5614
	private PlayerControls m_playerControls;
}
