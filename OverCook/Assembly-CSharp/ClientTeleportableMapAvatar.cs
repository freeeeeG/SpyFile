using System;
using UnityEngine;

// Token: 0x02000BCB RID: 3019
public class ClientTeleportableMapAvatar : ClientBaseTeleportable
{
	// Token: 0x06003DBF RID: 15807 RVA: 0x0012669E File Offset: 0x00124A9E
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_controls = base.gameObject.RequireComponent<MapAvatarControls>();
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x001266B8 File Offset: 0x00124AB8
	public override bool CanTeleport(ClientTeleportal _portal)
	{
		return this.m_controls != null && base.CanTeleport(_portal);
	}

	// Token: 0x0400318A RID: 12682
	private MapAvatarControls m_controls;
}
