using System;

// Token: 0x0200084A RID: 2122
public interface ILogicEventSender : ILogicNetworkConnection
{
	// Token: 0x06003DE3 RID: 15843
	void LogicTick();

	// Token: 0x06003DE4 RID: 15844
	int GetLogicCell();

	// Token: 0x06003DE5 RID: 15845
	int GetLogicValue();
}
