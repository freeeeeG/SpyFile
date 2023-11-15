using System;

// Token: 0x020006F8 RID: 1784
public interface IConduitFlow
{
	// Token: 0x060030E4 RID: 12516
	void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default);

	// Token: 0x060030E5 RID: 12517
	void RemoveConduitUpdater(Action<float> callback);

	// Token: 0x060030E6 RID: 12518
	bool IsConduitEmpty(int cell);
}
