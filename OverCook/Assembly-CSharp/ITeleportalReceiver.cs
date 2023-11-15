using System;
using System.Collections;

// Token: 0x020004DE RID: 1246
public interface ITeleportalReceiver
{
	// Token: 0x0600172A RID: 5930
	IEnumerator TeleportToMe(ServerTeleportal _entrancePortal, ITeleportalSender _sender, ITeleportable _object);

	// Token: 0x0600172B RID: 5931
	bool CanHandleTeleport(ITeleportable _object);

	// Token: 0x0600172C RID: 5932
	bool CanTeleportTo(ITeleportable _object);

	// Token: 0x0600172D RID: 5933
	bool IsReceiving();

	// Token: 0x0600172E RID: 5934
	void RegisterAllowTeleportCallback(Generic<bool> _callback);

	// Token: 0x0600172F RID: 5935
	void UnregisterAllowTeleportCallback(Generic<bool> _callback);

	// Token: 0x06001730 RID: 5936
	void RegisterStartedTeleportCallback(TeleportCallback _callback);

	// Token: 0x06001731 RID: 5937
	void UnregisterStartedTeleportCallback(TeleportCallback _callback);

	// Token: 0x06001732 RID: 5938
	void RegisterFinishedTeleportCallback(TeleportCallback _callback);

	// Token: 0x06001733 RID: 5939
	void UnregisterFinishedTeleportCallback(TeleportCallback _callback);
}
