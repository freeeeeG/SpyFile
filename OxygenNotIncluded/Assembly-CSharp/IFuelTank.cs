using System;

// Token: 0x020007FF RID: 2047
public interface IFuelTank
{
	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06003A66 RID: 14950
	IStorage Storage { get; }

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06003A67 RID: 14951
	bool ConsumeFuelOnLand { get; }

	// Token: 0x06003A68 RID: 14952
	void DEBUG_FillTank();
}
