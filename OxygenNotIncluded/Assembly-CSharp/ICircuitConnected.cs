using System;

// Token: 0x020006CE RID: 1742
public interface ICircuitConnected
{
	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06002F58 RID: 12120
	bool IsVirtual { get; }

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06002F59 RID: 12121
	int PowerCell { get; }

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06002F5A RID: 12122
	object VirtualCircuitKey { get; }
}
