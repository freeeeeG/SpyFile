using System;
using System.Collections;

// Token: 0x020004DF RID: 1247
public interface IClientTeleportalReceiver
{
	// Token: 0x06001734 RID: 5940
	IEnumerator TeleportToMe(ClientTeleportal _entrancePortal, IClientTeleportalSender _sender, IClientTeleportable _object);

	// Token: 0x06001735 RID: 5941
	bool CanTeleportTo(IClientTeleportable _object);

	// Token: 0x06001736 RID: 5942
	bool IsReceiving();

	// Token: 0x06001737 RID: 5943
	void RegisterCanTeleportToCallback(Generic<bool> _callback);

	// Token: 0x06001738 RID: 5944
	void UnregisterCanTeleportToCallback(Generic<bool> _callback);

	// Token: 0x06001739 RID: 5945
	void RegisterStartedTeleportCallback(ClientTeleportCallback _callback);

	// Token: 0x0600173A RID: 5946
	void UnregisterStartedTeleportCallback(ClientTeleportCallback _callback);

	// Token: 0x0600173B RID: 5947
	void RegisterFinishedTeleportCallback(ClientTeleportCallback _callback);

	// Token: 0x0600173C RID: 5948
	void UnregisterFinishedTeleportCallback(ClientTeleportCallback _callback);
}
