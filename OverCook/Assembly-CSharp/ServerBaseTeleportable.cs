using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200043A RID: 1082
public abstract class ServerBaseTeleportable : ServerSynchroniserBase, ITeleportable
{
	// Token: 0x060013E1 RID: 5089 RVA: 0x0006D810 File Offset: 0x0006BC10
	public virtual bool CanTeleport(ServerTeleportal _portal)
	{
		return !this.m_allowTeleportCallback.CallForResult(false);
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x0006D821 File Offset: 0x0006BC21
	public bool IsTeleporting()
	{
		return this.m_teleportState != TeleportState.None;
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x0006D82F File Offset: 0x0006BC2F
	public void StartTeleport(ITeleportalSender _sender, ITeleportalReceiver _receiver)
	{
		this.m_teleportState = TeleportState.Teleported;
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x0006D838 File Offset: 0x0006BC38
	public void EndTeleport(ITeleportalReceiver _receiver, ITeleportalSender _sender)
	{
		this.m_teleportState = TeleportState.None;
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x0006D841 File Offset: 0x0006BC41
	public void RegisterAllowTeleportCallback(Generic<bool> _callback)
	{
		this.m_allowTeleportCallback.Add(_callback);
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0006D84F File Offset: 0x0006BC4F
	public void UnregisterAllowTeleportCallback(Generic<bool> _callback)
	{
		this.m_allowTeleportCallback.Remove(_callback);
	}

	// Token: 0x04000F6F RID: 3951
	private List<Generic<bool>> m_allowTeleportCallback = new List<Generic<bool>>();

	// Token: 0x04000F70 RID: 3952
	private TeleportState m_teleportState;
}
