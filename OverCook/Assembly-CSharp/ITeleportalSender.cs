using System;
using System.Collections;

// Token: 0x020004E0 RID: 1248
public interface ITeleportalSender
{
	// Token: 0x0600173D RID: 5949
	IEnumerator TeleportFromMe(ServerTeleportal _exitPortal, ITeleportalReceiver _receiver, ITeleportable _object);

	// Token: 0x0600173E RID: 5950
	bool CanHandleTeleport(ITeleportable _object);

	// Token: 0x0600173F RID: 5951
	bool CanTeleport(ITeleportable _object);

	// Token: 0x06001740 RID: 5952
	bool IsSending();
}
