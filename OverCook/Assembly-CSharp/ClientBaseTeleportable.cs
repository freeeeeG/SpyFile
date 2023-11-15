using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200043B RID: 1083
public abstract class ClientBaseTeleportable : ClientSynchroniserBase, IClientTeleportable
{
	// Token: 0x060013E8 RID: 5096 RVA: 0x0006D871 File Offset: 0x0006BC71
	public virtual bool CanTeleport(ClientTeleportal _portal)
	{
		return !this.m_allowTeleportCallback.CallForResult(false);
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x0006D882 File Offset: 0x0006BC82
	public bool IsTeleporting()
	{
		return this.m_teleportState != TeleportState.None;
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x0006D890 File Offset: 0x0006BC90
	public bool IsTeleported()
	{
		return this.m_teleportState == TeleportState.Teleported;
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x0006D89B File Offset: 0x0006BC9B
	public void StartTeleportFrom(IClientTeleportalSender _sender, IClientTeleportalReceiver _receiver)
	{
		this.m_teleportState = TeleportState.Entering;
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x0006D8A4 File Offset: 0x0006BCA4
	public void EndTeleportFrom(IClientTeleportalSender _sender, IClientTeleportalReceiver _receiver)
	{
		this.m_teleportState = TeleportState.Teleported;
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x0006D8AD File Offset: 0x0006BCAD
	public void StartTeleportTo(IClientTeleportalReceiver _receiver, IClientTeleportalSender _sender)
	{
		this.m_teleportState = TeleportState.Exiting;
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x0006D8B6 File Offset: 0x0006BCB6
	public void EndTeleportTo(IClientTeleportalReceiver _receiver, IClientTeleportalSender _sender)
	{
		this.m_teleportState = TeleportState.None;
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x0006D8BF File Offset: 0x0006BCBF
	public void RegisterAllowTeleportCallback(Generic<bool> _callback)
	{
		this.m_allowTeleportCallback.Add(_callback);
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0006D8CD File Offset: 0x0006BCCD
	public void UnregisterAllowTeleportCallback(Generic<bool> _callback)
	{
		this.m_allowTeleportCallback.Remove(_callback);
	}

	// Token: 0x04000F71 RID: 3953
	private List<Generic<bool>> m_allowTeleportCallback = new List<Generic<bool>>();

	// Token: 0x04000F72 RID: 3954
	private TeleportState m_teleportState;
}
