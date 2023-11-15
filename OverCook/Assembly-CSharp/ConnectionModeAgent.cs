using System;
using Team17.Online.Multiplayer;

// Token: 0x02000840 RID: 2112
public interface ConnectionModeAgent
{
	// Token: 0x060028BB RID: 10427
	bool Start(Server server, Client client, object data, GenericVoid<IConnectionModeSwitchStatus> callback);

	// Token: 0x060028BC RID: 10428
	void Stop();

	// Token: 0x060028BD RID: 10429
	void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback);

	// Token: 0x060028BE RID: 10430
	IConnectionModeSwitchStatus GetStatus();

	// Token: 0x060028BF RID: 10431
	object GetAgentData();

	// Token: 0x060028C0 RID: 10432
	void Update();
}
