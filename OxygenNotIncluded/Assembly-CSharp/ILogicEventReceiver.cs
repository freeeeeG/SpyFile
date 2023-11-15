using System;

// Token: 0x0200084B RID: 2123
public interface ILogicEventReceiver : ILogicNetworkConnection
{
	// Token: 0x06003DE6 RID: 15846
	void ReceiveLogicEvent(int value);

	// Token: 0x06003DE7 RID: 15847
	int GetLogicCell();
}
