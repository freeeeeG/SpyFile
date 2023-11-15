using System;

// Token: 0x020004DA RID: 1242
public interface ITeleportable
{
	// Token: 0x06001713 RID: 5907
	bool CanTeleport(ServerTeleportal _portal);

	// Token: 0x06001714 RID: 5908
	bool IsTeleporting();

	// Token: 0x06001715 RID: 5909
	void StartTeleport(ITeleportalSender _sender, ITeleportalReceiver _receiver);

	// Token: 0x06001716 RID: 5910
	void EndTeleport(ITeleportalReceiver _receiver, ITeleportalSender _sender);

	// Token: 0x06001717 RID: 5911
	void RegisterAllowTeleportCallback(Generic<bool> _callback);

	// Token: 0x06001718 RID: 5912
	void UnregisterAllowTeleportCallback(Generic<bool> _callback);
}
