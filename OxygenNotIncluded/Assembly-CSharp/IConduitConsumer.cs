using System;

// Token: 0x020006F0 RID: 1776
public interface IConduitConsumer
{
	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060030AA RID: 12458
	Storage Storage { get; }

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060030AB RID: 12459
	ConduitType ConduitType { get; }
}
