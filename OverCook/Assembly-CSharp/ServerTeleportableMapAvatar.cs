using System;
using UnityEngine;

// Token: 0x02000BCA RID: 3018
public class ServerTeleportableMapAvatar : ServerBaseTeleportable
{
	// Token: 0x06003DBC RID: 15804 RVA: 0x0012664F File Offset: 0x00124A4F
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_controls = base.gameObject.RequireComponent<MapAvatarControls>();
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x00126669 File Offset: 0x00124A69
	public override bool CanTeleport(ServerTeleportal _portal)
	{
		return this.m_controls != null && this.m_controls.enabled && base.CanTeleport(_portal);
	}

	// Token: 0x04003189 RID: 12681
	private MapAvatarControls m_controls;
}
