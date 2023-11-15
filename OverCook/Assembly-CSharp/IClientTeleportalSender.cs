using System;
using System.Collections;

// Token: 0x020004E1 RID: 1249
public interface IClientTeleportalSender
{
	// Token: 0x06001741 RID: 5953
	IEnumerator TeleportFromMe(ClientTeleportal _exitPortal, IClientTeleportalReceiver _receiver, IClientTeleportable _object);

	// Token: 0x06001742 RID: 5954
	bool IsSending();
}
