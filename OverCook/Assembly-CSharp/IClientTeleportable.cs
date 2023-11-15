using System;

// Token: 0x020004DB RID: 1243
public interface IClientTeleportable
{
	// Token: 0x06001719 RID: 5913
	bool CanTeleport(ClientTeleportal _portal);

	// Token: 0x0600171A RID: 5914
	bool IsTeleporting();

	// Token: 0x0600171B RID: 5915
	bool IsTeleported();

	// Token: 0x0600171C RID: 5916
	void StartTeleportFrom(IClientTeleportalSender _sender, IClientTeleportalReceiver _receiver);

	// Token: 0x0600171D RID: 5917
	void EndTeleportFrom(IClientTeleportalSender _sender, IClientTeleportalReceiver _receiver);

	// Token: 0x0600171E RID: 5918
	void StartTeleportTo(IClientTeleportalReceiver _receiver, IClientTeleportalSender _sender);

	// Token: 0x0600171F RID: 5919
	void EndTeleportTo(IClientTeleportalReceiver _receiver, IClientTeleportalSender _sender);

	// Token: 0x06001720 RID: 5920
	void RegisterAllowTeleportCallback(Generic<bool> _callback);

	// Token: 0x06001721 RID: 5921
	void UnregisterAllowTeleportCallback(Generic<bool> _callback);
}
